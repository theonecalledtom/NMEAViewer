using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace NMEAViewer
{

    public partial class TackingWindow : DockableDrawable
    {
        public class TacksGraphOverlay : TimeBasedGraph.GraphOverlay
        {
            TackAnalysis m_TackAnalysis;
            static int counter = 0;

            public TacksGraphOverlay(TackAnalysis ta)
            {
                m_TackAnalysis = ta;
                counter++;
                OverlayName = "Tacks " + counter;
            }

            public override void Draw(Graphics g, double fLeftTime, double fRightTime)
            {
                if (fRightTime <= fLeftTime)
                    return;

                double height = (double)g.Clip.GetBounds(g).Height;
                double width = (double)g.Clip.GetBounds(g).Width;

                double fY = height * 0.8;

                Pen tackPen = new Pen(new SolidBrush(Color.Yellow));
                tackPen.Width = 3;

                Pen gybePen = new Pen(new SolidBrush(Color.Blue));
                gybePen.Width = 3;

                Pen counterPen = new Pen(new SolidBrush(Color.Red));
                counterPen.Width = 1;

                for (int i = 0; i < m_TackAnalysis.GetNumTacks(); i++)
                {
                    TackAnalysisData data = m_TackAnalysis.GetData(i);
                    Pen drawPen = data.IsTack() ? tackPen : gybePen;

                    double fStartOfTurn = data.GetValue(TackAnalysisData.eTackDataTypes.TimeOfStartOfTurn);

                    double startX = width * (fStartOfTurn - fLeftTime) / (fRightTime - fLeftTime);
                    double endX = width * (data.GetValue(TackAnalysisData.eTackDataTypes.TimeOfEndOfTurn) - fLeftTime) / (fRightTime - fLeftTime);

                    double depth = Math.Max(10.0, (endX - startX) * 0.1);


                    g.DrawLine(drawPen, (float)startX, (float)fY, (float)endX, (float)fY);
                    g.DrawLine(drawPen, (float)startX, (float)fY, (float)startX, (float)(fY + depth));
                    g.DrawLine(drawPen, (float)endX, (float)fY, (float)endX, (float)(fY + depth));

                    double headX = width * (fStartOfTurn + data.GetValue(TackAnalysisData.eTackDataTypes.TimeToHeadToWind) - fLeftTime) / (fRightTime - fLeftTime);
                    g.DrawLine(drawPen, (float)headX, (float)fY, (float)headX, (float)(fY + depth));

                    double timeToCounterSteerEnd = data.GetValue(TackAnalysisData.eTackDataTypes.TimeToCounterSteerEnd);
                    if (timeToCounterSteerEnd > 0.0)
                    {
                        double counterX = width * ((fStartOfTurn + timeToCounterSteerEnd) - fLeftTime) / (fRightTime - fLeftTime);
                        g.DrawLine(counterPen, (float)counterX, (float)fY, (float)counterX, (float)(fY + depth));
                    }
                }
            }
        }

        TackAnalysis m_TackAnalysis;    //TODO: Move central ownership of this someplace sharable? Or keep separate?
        TacksGraphOverlay m_GraphOverlay;
        NMEACruncher m_Data;
        List<bool> m_ActiveDataColumns = new List<bool>();
        List<int> m_SectionStarts;
        List<int> m_SectionEnds;
        double m_fStartSelection = -1.0;
        double m_fEndSelection = -1.0;
        bool m_bNeedsBuild;

        [JsonObject(MemberSerialization.OptOut)]
        private class SerializedData : DockableDrawable.SerializedDataBase
        {
            public SerializedData(DockableDrawable parent)
                : base(parent) { }
            public bool UseSelection;
            public bool RestrictToLegs;
            public bool UseSOG;
            public bool RestrictAWA;
            public double UpwindValue;
            public double DownwindValue;
            public List<string> VisibleColumns;
            public List<int> m_SectionStarts;
            public List<int> m_SectionEnds;
        };

        public override DockableDrawable.SerializedDataBase CreateSerializedData()
        {
            SerializedData data = new SerializedData(this);
            data.UseSelection = UseSelection.Checked;
            data.RestrictToLegs = RestrictToLegs.Checked;
            data.UseSOG = UseSOG.Checked;
            data.RestrictAWA = checkBox2_RestrictByAWA.Checked;
            data.UpwindValue = Convert.ToDouble(numericUpDown_Upwind.Value);
            data.DownwindValue = Convert.ToDouble(numericUpDown_Downwind.Value);
            data.VisibleColumns = new List<string>();
            for (int i=0 ; i<VisibleColumns.Items.Count ; i++)
            {
                if (VisibleColumns.GetItemChecked(i))
                {
                    data.VisibleColumns.Add(VisibleColumns.GetItemText(VisibleColumns.Items[i]));
                }
            }

            if (m_SectionStarts != null)
            {
                data.m_SectionStarts = new List<int>();
                data.m_SectionEnds = new List<int>();
                for (int i = 0; i < m_SectionStarts.Count; i++)
                {
                    data.m_SectionStarts.Add(m_SectionStarts[i]);
                    data.m_SectionEnds.Add(m_SectionEnds[i]);
                }
            }
            return data;
        }

        public override void InitFromSerializedData(SerializedDataBase data_base)
        {
            base.InitFromSerializedData(data_base);

            SerializedData data = (SerializedData)data_base;
            UseSelection.Checked = data.UseSelection;
            RestrictToLegs.Checked = data.RestrictToLegs;
            UseSOG.Checked = data.UseSOG;
            checkBox2_RestrictByAWA.Checked = data.RestrictAWA;
            numericUpDown_Upwind.Value = Convert.ToDecimal(data.UpwindValue);
            numericUpDown_Downwind.Value = Convert.ToDecimal(data.DownwindValue);

            if (data.VisibleColumns != null)
            {
                for (int i = 0; i < data.VisibleColumns.Count; i++)
                {
                    for (int ii = 0; ii < VisibleColumns.Items.Count; ii++)
                    {
                        if (VisibleColumns.GetItemText(VisibleColumns.Items[ii]) == data.VisibleColumns[i])
                        {
                            VisibleColumns.SetItemChecked(ii, true);
                        }
                    }
                }
            }
            BuildActiveColumnList();
        }

        public override void PostInitFromSerializedData(SerializedDataBase data_base)
        {
            SerializedData data = (SerializedData)data_base;

            //Rescan the sections we had before
            if (data.m_SectionStarts != null)
            {
                m_SectionStarts = new List<int>();
                m_SectionEnds = new List<int>();
                for (int i = 0; i < data.m_SectionStarts.Count; i++)
                {
                    m_TackAnalysis.AnalyseSection(data.m_SectionStarts[i], data.m_SectionEnds[i], i > 0, checkBox2_RestrictByAWA.Checked, Convert.ToDouble(numericUpDown_Upwind.Value), Convert.ToDouble(numericUpDown_Downwind.Value));
                    m_SectionStarts.Add(data.m_SectionStarts[i]);
                    m_SectionEnds.Add(data.m_SectionEnds[i]);
                }

                RebuildTable();
            }
        }

        protected override void OnDataReplaced(NMEACruncher newData)
        {
            m_Data = newData;
            //RebuildHistogram(m_fStartTime, m_fEndTime);

            m_TackAnalysis.UseSOG = UseSOG.Checked;
            m_TackAnalysis.OnDataReplaced(newData);
            //m_TackAnalysis.AnalyseSection(0, newData.GetDataCount(), false);
        }

        void VisibleColumns_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            m_bNeedsBuild = true;
        }

        void BuildActiveColumnList()
        {
            //Clear the data
            m_ActiveDataColumns.Clear();

            //Now run through the data columns and see which ones are ticked
            for (int i = 0; i < TackAnalysisData.GetNumValues(); i++)
            {
                m_ActiveDataColumns.Add(false);
                for (int ii = 0; ii < VisibleColumns.Items.Count; ii++)
                {
                    if (VisibleColumns.GetItemChecked(ii))
                    {
                        if (VisibleColumns.GetItemText(VisibleColumns.Items[ii]) == TackAnalysisData.GetValueName(i))
                        {
                            m_ActiveDataColumns[i] = true;
                            break;
                        }
                    }
                }
            }

            if (UpwindDataGrid.Columns.Count > 0)
            {
                for (int i = 0; i < m_ActiveDataColumns.Count; i++)
                {
                    UpwindDataGrid.Columns[i].Visible = m_ActiveDataColumns[i];
                    DownwindDataGrid.Columns[i].Visible = m_ActiveDataColumns[i];
                }
            }

            UpwindDataGrid.Refresh();
            DownwindDataGrid.Refresh();
        }

        public TackingWindow(NMEACruncher data)
        {
            InitializeComponent();

            m_Data = data;
            m_TackAnalysis = new TackAnalysis(m_Data);
            m_GraphOverlay = new TacksGraphOverlay(m_TackAnalysis);

            UpwindDataGrid.CellMouseUp += UpwindDataGrid_CellClick;
            DownwindDataGrid.CellClick += DownwindDataGrid_CellClick;

            VisibleColumns.ItemCheck += VisibleColumns_ItemCheck;
            VisibleColumns.CheckOnClick = true;

            DataSelectionTab.Leave += DataSelectionTab_Leave;

            for (int i = 0; i < TackAnalysisData.GetNumValues(); i++)
            {
                VisibleColumns.Items.Add(TackAnalysisData.GetValueName(i));
            }
        }

        ~TackingWindow()
        {
            m_GraphOverlay.Remove();
        }

        void DataSelectionTab_Leave(object sender, EventArgs e)
        {
            if (m_bNeedsBuild)
            {
                BuildActiveColumnList();
                m_bNeedsBuild = false;
            }
        }

        //void DownwindDataGrid_MouseUp(object sender, MouseEventArgs e)
        //{
        //    if (e.Button == MouseButtons.Right)
        //    {
        //        var relativeMousePosition = UpwindDataGrid.PointToClient(Cursor.Position);
        //        m_ContextMenu.Show(UpwindDataGrid, relativeMousePosition);
        //    }
        //}

        void DownwindDataGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.RowIndex >= 0) && (e.ColumnIndex >= 0))
            {
                TackAnalysisData data = (TackAnalysisData)DownwindDataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag;
                if (data != null)
                {
                    double fStartTime = data.GetValue(TackAnalysisData.eTackDataTypes.TimeOfStartOfTurn);
                    double fEndTime = data.GetValue(TackAnalysisData.eTackDataTypes.TimeOfEndOfTurn);
                    BroadcastOnTimeRangeSelected(fStartTime, fEndTime);
                }
            }
        }

        void UpwindDataGrid_CellClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if ((e.RowIndex >= 0) && (e.ColumnIndex >= 0))
            {
                TackAnalysisData data = (TackAnalysisData)UpwindDataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag;
                if (data != null)
                {
                    double fStartTime = data.GetValue(TackAnalysisData.eTackDataTypes.TimeOfStartOfTurn);
                    double fEndTime = data.GetValue(TackAnalysisData.eTackDataTypes.TimeOfEndOfTurn);
                    BroadcastOnTimeRangeSelected(fStartTime, fEndTime);
                }
            }
        }

        protected override void OnTimeRangeSelected(double fTimeA, double fTimeB)
        {
            m_fStartSelection = fTimeA > fTimeB ? fTimeB : fTimeA;
            m_fEndSelection = fTimeB > fTimeA ? fTimeB : fTimeA;
        }

        private void ScanForTacks()
        {
            m_SectionStarts = new List<int>();
            m_SectionEnds = new List<int>();
            m_TackAnalysis.UseSOG = UseSOG.Checked;
            if (UseSelection.Checked)
            {
                int iStartIndex = m_Data.GetIndexForTime(m_fStartSelection);
                int iEndIndex = m_Data.GetIndexForTime(m_fEndSelection);
                if (iEndIndex > iStartIndex)
                {
                    m_TackAnalysis.AnalyseSection(iStartIndex, iEndIndex, false, checkBox2_RestrictByAWA.Checked, Convert.ToDouble(numericUpDown_Upwind.Value), Convert.ToDouble(numericUpDown_Downwind.Value));
                    m_SectionStarts.Add(iStartIndex);
                    m_SectionEnds.Add(iEndIndex);
                }
            }
            else if (RestrictToLegs.Checked)
            {
                int iDoneIndex = 0;
                bool bFirstLeg = true;
                for (int iLeg = 0; iLeg < MetaDataWindow.GetNumLegs(); iLeg++)
                {
                    int iStartIndex = Math.Max(iDoneIndex, m_Data.GetIndexForTime(MetaDataWindow.GetLegStartTime(iLeg)));
                    int iEndIndex = m_Data.GetIndexForTime(MetaDataWindow.GetLegEndTime(iLeg));
                    if (iEndIndex > iStartIndex)
                    {
                        m_TackAnalysis.AnalyseSection(iStartIndex, iEndIndex, !bFirstLeg, checkBox2_RestrictByAWA.Checked, Convert.ToDouble(numericUpDown_Upwind.Value), Convert.ToDouble(numericUpDown_Downwind.Value));
                        m_SectionStarts.Add(iStartIndex);
                        m_SectionEnds.Add(iEndIndex);
                        bFirstLeg = false;
                    }
                    iDoneIndex = Math.Max(iEndIndex, iDoneIndex);
                }
            }
            else
            {
                m_TackAnalysis.AnalyseSection(0, m_Data.GetDataCount(), false, checkBox2_RestrictByAWA.Checked, Convert.ToDouble(numericUpDown_Upwind.Value), Convert.ToDouble(numericUpDown_Downwind.Value));
                m_SectionStarts.Add(0);
                m_SectionEnds.Add(m_Data.GetDataCount());
            }

            RebuildTable();
        }

        private void Scan_Click(object sender, EventArgs e)
        {
            ScanForTacks();
        }

        private void SetupTable(DataGridView grid)
        {
            grid.Rows.Clear();

			grid.ColumnCount = TackAnalysisData.GetNumValues();
			grid.ColumnHeadersVisible = true;

			for (int iColumn = 0; iColumn < grid.ColumnCount; iColumn++)
			{
                grid.Columns[iColumn].Name = TackAnalysisData.GetValueName(iColumn);
				grid.Columns[iColumn].DefaultCellStyle.Format = "N1";
			}
        }

        private void AddLineToTable(TackAnalysisData data, DataGridView grid)
        {
            int iRow = grid.Rows.Add();
            for (int iColumn = 0; iColumn < grid.ColumnCount; iColumn++)
            {
                grid.Rows[iRow].Cells[iColumn].Value = data.GetValue(iColumn);
                grid.Rows[iRow].Cells[iColumn].Tag = data;
            }
        }

        private void RebuildTable()
        {
            SetupTable(DownwindDataGrid);
            SetupTable(UpwindDataGrid);
            for (int i=0 ; i<m_TackAnalysis.GetNumTacks() ; i++)
            {
                TackAnalysisData data = m_TackAnalysis.GetData(i);
                if (data.IsTack())
                {
                    //Add to the unwind table
                    AddLineToTable(data, UpwindDataGrid);
                }
                else 
                {
                    //Add to the downwind table
                    AddLineToTable(data, DownwindDataGrid);
                }
            }

            BuildActiveColumnList();

            DownwindDataGrid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            UpwindDataGrid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            //Make sure the overlay is updated too
            m_GraphOverlay.NotifyDataChanged();
        }

        private void checkBox2_RestrictByAWA_CheckedChanged(object sender, EventArgs e)
        {
            numericUpDown_Upwind.Enabled = checkBox2_RestrictByAWA.Checked;
            numericUpDown_Downwind.Enabled = checkBox2_RestrictByAWA.Checked;
        }

        private void RestrictToLegs_CheckedChanged(object sender, EventArgs e)
        {
            if (RestrictToLegs.Checked)
            {
                UseSelection.Checked = false;
            }
        }

        private void UseSelection_CheckedChanged(object sender, EventArgs e)
        {
            if (UseSelection.Checked)
            {
                RestrictToLegs.Checked = false;
            }
        }
    }
}
