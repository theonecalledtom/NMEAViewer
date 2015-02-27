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
    public partial class MetaDataWindow : DockableDrawable
    {
        public class LegsGraphOverlay : TimeBasedGraph.GraphOverlay
        {
            public LegsGraphOverlay()
            {
                OverlayName = "Legs";
            }

            public override void Draw(Graphics g, double fLeftTime, double fRightTime)
            {
                if (fRightTime <= fLeftTime)
                    return;

                double height = (double)g.Clip.GetBounds(g).Height;
                double width = (double)g.Clip.GetBounds(g).Width;

                double fDY = height / (double)(StaticData.sm_Data.m_LegStartTimes.Count + 1);
                double fY = fDY;
                //if (fDY > 15)
                //{
                //    fDY = 15;
                //}
                Pen outlinePen = new Pen(new SolidBrush(Color.DarkSlateBlue));
                outlinePen.Width = 3;
                System.Drawing.Font legFont = new System.Drawing.Font("Arial", 10);

                for (int i = 0; i < StaticData.sm_Data.m_LegStartTimes.Count; i++)
                {
                    if ((StaticData.sm_Data.m_LegStartTimes[i] <= fRightTime) && (StaticData.sm_Data.m_LegEndTimes[i] >= fLeftTime))
                    {
                        double startX = width * (StaticData.sm_Data.m_LegStartTimes[i] - fLeftTime) / (fRightTime - fLeftTime);
                        double endX = width * (StaticData.sm_Data.m_LegEndTimes[i] - fLeftTime) / (fRightTime - fLeftTime);

                        g.DrawString(StaticData.sm_Data.m_LegNames[i], legFont, System.Drawing.Brushes.DarkSlateBlue, (float)(startX + 3.0), (float)(fY + 3.0));
                        g.DrawLine(outlinePen, (float)startX, (float)fY, (float)endX, (float)fY);
                        g.DrawLine(outlinePen, (float)startX, (float)fY, (float)startX, (float)(fY + 10.0));
                        g.DrawLine(outlinePen, (float)endX, (float)fY, (float)endX, (float)(fY + 10.0));
                    }
                    // fY += fDY;
                }
            }
        }

        public class EventsGraphOverlay : TimeBasedGraph.GraphOverlay
        {
            public EventsGraphOverlay()
            {
                OverlayName = "Events";
            }

            public override void Draw(Graphics g, double fLeftTime, double fRightTime)
            {
                double height = (double)g.Clip.GetBounds(g).Height;
                double width = (double)g.Clip.GetBounds(g).Width;

                Pen outlinePen = new Pen(new SolidBrush(Color.DarkSlateBlue));
                System.Drawing.Font legFont = new System.Drawing.Font("Arial", 10);

                for (int i = 0; i < StaticData.sm_Data.m_EventTimes.Count; i++)
                {
                    if ((StaticData.sm_Data.m_EventTimes[i] < fRightTime) && (StaticData.sm_Data.m_EventTimes[i] >= fLeftTime))
                    {
                        double startX = width * (StaticData.sm_Data.m_EventTimes[i] - fLeftTime) / (fRightTime - fLeftTime);

                        g.DrawString(StaticData.sm_Data.m_EventNames[i], legFont, System.Drawing.Brushes.DarkSlateBlue, (float)startX, (float)height * 0.85f);
                        g.DrawLine(outlinePen, (float)startX, (float)0, (float)startX, (float)height);
                    }
                }
            }
        }

        static LegsGraphOverlay sm_LegsGraphOverlay = null;
        static EventsGraphOverlay sm_EventsGraphOverlay = null;

        public static void StaticInit()
        {
            sm_LegsGraphOverlay = new LegsGraphOverlay();
            sm_EventsGraphOverlay = new EventsGraphOverlay();
        }

        public static void StaticDeInit()
        {
            sm_LegsGraphOverlay.Remove();
            sm_LegsGraphOverlay = null;
            sm_EventsGraphOverlay.Remove();
            sm_EventsGraphOverlay = null;
        }

        double m_fSelectionStartTime;
        double m_fSelectionEndTime;
        double m_fSelectedTime;

        [JsonObject(MemberSerialization.OptOut)]
        public class StaticData : DockableDrawable.StaticSerializedDataBase
        {
            public List<string> m_LegNames = new List<string>();
            public List<string> m_LegDescriptions = new List<string>();
            public List<double> m_LegStartTimes = new List<double>();
            public List<double> m_LegEndTimes = new List<double>();
            public List<string> m_EventNames = new List<string>();
            public List<string> m_EventDecriptions = new List<string>();
            public List<double> m_EventTimes = new List<double>();
            public static StaticData sm_Data = new StaticData();
        };

        public void CreateStaticSerializedData()
        {
            StaticData.sm_Data.m_LegNames.Clear();
            StaticData.sm_Data.m_LegDescriptions.Clear();
            StaticData.sm_Data.m_LegStartTimes.Clear();
            StaticData.sm_Data.m_LegEndTimes.Clear();
            StaticData.sm_Data.m_EventNames.Clear();
            StaticData.sm_Data.m_EventDecriptions.Clear();
            StaticData.sm_Data.m_EventTimes.Clear();
            for (int i = 0; i < LegsDataGrid.Rows.Count; i++)
            {
                StaticData.sm_Data.m_LegNames.Add(Convert.ToString(LegsDataGrid.Rows[i].Cells[LegName.Index].Value));
                StaticData.sm_Data.m_LegDescriptions.Add(Convert.ToString(LegsDataGrid.Rows[i].Cells[LegDescription.Index].Value));
                StaticData.sm_Data.m_LegStartTimes.Add(Convert.ToDouble(LegsDataGrid.Rows[i].Cells[StartTime.Index].Value));
                StaticData.sm_Data.m_LegEndTimes.Add(Convert.ToDouble(LegsDataGrid.Rows[i].Cells[EndTime.Index].Value));
            }

            for (int i = 0; i < EventsDataGrid.Rows.Count; i++)
            {
                StaticData.sm_Data.m_EventNames.Add(Convert.ToString(EventsDataGrid.Rows[i].Cells[EventName.Index].Value));
                StaticData.sm_Data.m_EventDecriptions.Add(Convert.ToString(EventsDataGrid.Rows[i].Cells[EventDescription.Index].Value));
                StaticData.sm_Data.m_EventTimes.Add(Convert.ToDouble(EventsDataGrid.Rows[i].Cells[TimeOfEvent.Index].Value));
            }

            BroadcastEventData(StaticData.sm_Data.m_EventTimes, StaticData.sm_Data.m_EventNames, StaticData.sm_Data.m_EventDecriptions);
        }

        public static void Clear()
        {
            StaticData.sm_Data.m_LegNames.Clear();
            StaticData.sm_Data.m_LegDescriptions.Clear();
            StaticData.sm_Data.m_LegStartTimes.Clear();
            StaticData.sm_Data.m_LegEndTimes.Clear();
            StaticData.sm_Data.m_EventNames.Clear();
            StaticData.sm_Data.m_EventDecriptions.Clear();
            StaticData.sm_Data.m_EventTimes.Clear();

            StaticDeInit();
            StaticInit();
        }

        public static int GetNumLegs()
        {
            return StaticData.sm_Data.m_LegNames.Count;
        }

        public static double GetLegStartTime(int iLeg)
        {
            return StaticData.sm_Data.m_LegStartTimes[iLeg];
        }

        public static double GetLegEndTime(int iLeg)
        {
            return StaticData.sm_Data.m_LegEndTimes[iLeg];
        }

        public static string GetLegNameForTime(double fTime)
        {
            for (int i = 0; i < StaticData.sm_Data.m_LegStartTimes.Count; i++ )
            {
                if ((fTime >= StaticData.sm_Data.m_LegStartTimes[i]) && (fTime <= StaticData.sm_Data.m_LegEndTimes[i]))
                {
                    return StaticData.sm_Data.m_LegNames[i];
                }
            }
            return null;
        }

        public void InitFromStaticSerializedData()
        {
            StaticData data = StaticData.sm_Data;
            LegsDataGrid.Rows.Clear();
            for (int i = 0; i < data.m_LegNames.Count; i++)
            {
                if (data.m_LegNames[i].Length > 0)
                {
                    LegsDataGrid.Rows.Add();
                    LegsDataGrid.Rows[i].Cells[LegName.Index].Value = data.m_LegNames[i];
                    LegsDataGrid.Rows[i].Cells[LegDescription.Index].Value = data.m_LegDescriptions[i];
                    LegsDataGrid.Rows[i].Cells[StartTime.Index].Value = data.m_LegStartTimes[i];
                    LegsDataGrid.Rows[i].Cells[EndTime.Index].Value = data.m_LegEndTimes[i];
                }
            }

            EventsDataGrid.Rows.Clear();
            for (int i = 0; i < data.m_EventNames.Count; i++)
            {
                if (data.m_EventNames[i].Length > 0)
                {
                    EventsDataGrid.Rows.Add();
                    EventsDataGrid.Rows[i].Cells[EventName.Index].Value = data.m_EventNames[i];
                    EventsDataGrid.Rows[i].Cells[EventDescription.Index].Value = data.m_EventDecriptions[i];
                    EventsDataGrid.Rows[i].Cells[TimeOfEvent.Index].Value = data.m_EventTimes[i];
                }
            }
        }

        [JsonObject(MemberSerialization.OptOut)]
        private class SerializedData : DockableDrawable.SerializedDataBase
        {
            public SerializedData(DockableDrawable parent)
                : base(parent) { }
        };

        public override DockableDrawable.SerializedDataBase CreateSerializedData()
        {
            SerializedData data = new SerializedData(this);
            return data;
        }

        public override void InitFromSerializedData(SerializedDataBase data_base)
        {
        }

        static public void PostInitFromStaticData()
        {
            BroadcastEventData(StaticData.sm_Data.m_EventTimes, StaticData.sm_Data.m_EventNames, StaticData.sm_Data.m_EventDecriptions);
        }

        public MetaDataWindow()
        {
            InitializeComponent();

            LegsDataGrid.ContextMenuStrip = LegContextMenuStrip;
            EventsDataGrid.ContextMenuStrip = EventContextMenuStrip;

            LegsDataGrid.CellEndEdit += OnCellEndEdit;
            EventsDataGrid.CellEndEdit += OnCellEndEdit;
            EventsDataGrid.DoubleClick += EventsDataGrid_DoubleClick;
            LegsDataGrid.DoubleClick += LegsDataGrid_DoubleClick;

            InitFromStaticSerializedData();
        }

        void EventsDataGrid_DoubleClick(object sender, EventArgs e)
        {
            if (EventsDataGrid.CurrentRow != null)
            {
                double fTimeSelected = Convert.ToDouble(EventsDataGrid.CurrentRow.Cells[TimeOfEvent.Index].Value);
                BroadcastOnTimeSelected(this, fTimeSelected);
            }
        }

        void LegsDataGrid_DoubleClick(object sender, EventArgs e)
        {
            if (LegsDataGrid.CurrentRow != null)
            {
                double fStartTime = Convert.ToDouble(LegsDataGrid.CurrentRow.Cells[StartTime.Index].Value);
                double fEndTime = Convert.ToDouble(LegsDataGrid.CurrentRow.Cells[EndTime.Index].Value);
                if (fStartTime >= 0.0 && fEndTime > fStartTime)
                {
                    BroadcastOnTimeRangeSelected(fStartTime, fEndTime);
                }
            }
        }


        void OnCellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //Keep the static data up to date
            CreateStaticSerializedData();

            sm_LegsGraphOverlay.NotifyDataChanged();
            sm_EventsGraphOverlay.NotifyDataChanged();
        }

        protected override void OnTimeSelected(object sender, double fTime)
        {
            m_fSelectedTime = fTime;
        }

        protected override void OnTimeRangeSelected(double fTimeA, double fTimeB)
        {
            m_fSelectionStartTime = fTimeA;
            m_fSelectionEndTime = fTimeB;
        }

        private void setStartTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_fSelectionStartTime >= 0.0)
            {
                for (int i = 0; i < LegsDataGrid.SelectedRows.Count; i++)
                {
                    LegsDataGrid.SelectedRows[i].Cells[StartTime.Index].Value = m_fSelectedTime;
                }
                for (int i = 0; i < LegsDataGrid.SelectedCells.Count; i++)
                {
                    LegsDataGrid.Rows[LegsDataGrid.SelectedCells[i].RowIndex].Cells[StartTime.Index].Value = m_fSelectedTime;
                }

                CreateStaticSerializedData();
                sm_LegsGraphOverlay.NotifyDataChanged();
            }
        }

        private void setEndTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_fSelectionEndTime >= 0.0)
            {
                for (int i = 0; i < LegsDataGrid.SelectedRows.Count; i++)
                {
                    LegsDataGrid.SelectedRows[i].Cells[EndTime.Index].Value = m_fSelectedTime;
                }
                for (int i = 0; i < LegsDataGrid.SelectedCells.Count; i++)
                {
                    LegsDataGrid.Rows[LegsDataGrid.SelectedCells[i].RowIndex].Cells[EndTime.Index].Value = m_fSelectedTime;
                }

                CreateStaticSerializedData();
                sm_LegsGraphOverlay.NotifyDataChanged();
            }
        }

        private void setTimeRangeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_fSelectionStartTime >= 0.0 && m_fSelectionEndTime >= 0.0)
            {
                for (int i = 0; i < LegsDataGrid.SelectedRows.Count; i++)
                {
                    LegsDataGrid.SelectedRows[i].Cells[StartTime.Index].Value = m_fSelectionStartTime;
                    LegsDataGrid.SelectedRows[i].Cells[EndTime.Index].Value = m_fSelectionEndTime;
                }
                for (int i = 0; i < LegsDataGrid.SelectedCells.Count; i++)
                {
                    LegsDataGrid.Rows[LegsDataGrid.SelectedCells[i].RowIndex].Cells[StartTime.Index].Value = m_fSelectionStartTime;
                    LegsDataGrid.Rows[LegsDataGrid.SelectedCells[i].RowIndex].Cells[EndTime.Index].Value = m_fSelectionEndTime;
                }

                CreateStaticSerializedData();
                sm_LegsGraphOverlay.NotifyDataChanged();
            }
        }

        private void toCurrentTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_fSelectedTime >= 0.0)
            {
                for (int i = 0; i < EventsDataGrid.SelectedRows.Count; i++)
                {
                    EventsDataGrid.SelectedRows[i].Cells[TimeOfEvent.Index].Value = m_fSelectedTime;
                }
                for (int i = 0; i < EventsDataGrid.SelectedCells.Count; i++)
                {
                    EventsDataGrid.Rows[EventsDataGrid.SelectedCells[i].RowIndex].Cells[TimeOfEvent.Index].Value = m_fSelectedTime;
                }

                CreateStaticSerializedData();
                sm_EventsGraphOverlay.NotifyDataChanged();
            }
        }

        private void highlightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            double fStartTime = -1.0;
            double fEndTime = -1.0;
            //for (int i = 0; i < LegsDataGrid.SelectedRows.Count; i++)
            if (LegsDataGrid.SelectedRows.Count > 0)
            {
                fStartTime = Convert.ToDouble(LegsDataGrid.SelectedRows[0].Cells[StartTime.Index].Value);
                fEndTime = Convert.ToDouble(LegsDataGrid.SelectedRows[0].Cells[EndTime.Index].Value);
              //  break;
            }
            else if (LegsDataGrid.SelectedCells.Count > 0)
            //for (int i = 0; i < LegsDataGrid.SelectedCells.Count; i++)
            {
                fStartTime = Convert.ToDouble(LegsDataGrid.Rows[LegsDataGrid.SelectedCells[0].RowIndex].Cells[StartTime.Index].Value);
                fEndTime = Convert.ToDouble(LegsDataGrid.Rows[LegsDataGrid.SelectedCells[0].RowIndex].Cells[EndTime.Index].Value);
                //break;
            }

            if (fStartTime >= 0.0 && fEndTime > fStartTime)
            {
                BroadcastOnTimeRangeSelected(fStartTime, fEndTime);
            }
        }
    }
}


