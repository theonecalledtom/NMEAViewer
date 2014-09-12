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
    public partial class TimeBasedGraphDataTypes : Form
    {
        [JsonObject(MemberSerialization.OptOut)]
        public class GraphDataStyleInfo
        {
            public string m_DataName;
            public System.Drawing.Color m_Color;
            public int m_iThickness;
            public bool m_AsDirection;
            public bool m_InvertedArrow;
        }

        [JsonObject(MemberSerialization.OptOut)]
        public class GraphStyleInfo
        {
            public System.Drawing.Color         m_BackgroundColor;
            public List<GraphDataStyleInfo> m_DataStyleList;
        }

        GraphStyleInfo m_GraphStyleInfo;
        static bool smb_Created;
        int m_iSelectedDataType;
        public TimeBasedGraphDataTypes(MetaDataSerializer metaData)
        {
            InitializeComponent();

            //Make sure we're setup to save the data
            m_GraphStyleInfo = metaData.m_GraphStyleInfo;

            //Further edits are done on the serialized data directly

            FieldColorPanel.Click += FieldColorPanel_Click;
            BackgroundColorPanel.Click += BackgroundColorPanel_Click;

            Disposed += TimeBasedGraphDataTypes_Disposed;

            if (smb_Created)
            {
                throw new Exception("TimeBasedGraphDataTypes already created");
            }

            int iDefaultDataType = -1;
            for (int i = 0; i < m_GraphStyleInfo.m_DataStyleList.Count; i++ )
            {
                if (NMEACruncher.GetDataRangeForType(i) != NMEACruncher.DataRangeTypes.NoGraph)
                {
                    DataTypeComboBox.Items.Add(m_GraphStyleInfo.m_DataStyleList[i].m_DataName);
                    if (iDefaultDataType < 0)
                    {
                        iDefaultDataType = i;
                    }
                }
            }

            if (iDefaultDataType >= 0)
            {
                SelectDataType(iDefaultDataType);
            }
            smb_Created = true;
        }

        void TimeBasedGraphDataTypes_Disposed(object sender, EventArgs e)
        {
            smb_Created = false;
        }

        public static GraphStyleInfo CreateNewStyleInfo()
        {
            GraphStyleInfo newInfo = new GraphStyleInfo();
            newInfo.m_DataStyleList = new List<GraphDataStyleInfo>();
            newInfo.m_BackgroundColor = System.Drawing.Color.LightGray;

            for (int i = 0; i < NMEACruncher.GetNumDataTypes(); i++)
            {
                GraphDataStyleInfo item = new GraphDataStyleInfo();
                item.m_DataName = NMEACruncher.GetNameOfEntry(i);
                item.m_iThickness = 1;
                item.m_Color = NMEACruncher.GetDefaultColorForType(i);
                item.m_AsDirection = false;

                newInfo.m_DataStyleList.Add(item);
            }
            return newInfo;
        }

        public static GraphStyleInfo ValidateStyleInfo(GraphStyleInfo styleInfo)
        {
            //Make sure our incoming data matches what we expect
            if (styleInfo == null)
            {
                styleInfo = CreateNewStyleInfo();
                return styleInfo;
            }

            //Copy any existing style data into a new list that matches whatever the current data structure is
            List<GraphDataStyleInfo> tmpList = styleInfo.m_DataStyleList;
            styleInfo.m_DataStyleList = new List<GraphDataStyleInfo>();

            for (int i = 0; i < NMEACruncher.GetNumDataTypes();  i++)
            {
                GraphDataStyleInfo item = new GraphDataStyleInfo();
                item.m_DataName = NMEACruncher.GetNameOfEntry(i);
                bool bfilledIn = false;
                foreach (GraphDataStyleInfo oldItem in tmpList)
                {
                    if (oldItem.m_DataName == item.m_DataName)
                    {
                        item.m_AsDirection = oldItem.m_AsDirection;
                        item.m_Color = oldItem.m_Color;
                        item.m_iThickness = oldItem.m_iThickness;
                        tmpList.Remove(oldItem);
                        bfilledIn = true;
                        break;
                    }
                }

                if (!bfilledIn)
                {
                    item.m_iThickness = 1;
                    item.m_Color = NMEACruncher.GetDefaultColorForType(i);
                    item.m_AsDirection = false;
                }

                styleInfo.m_DataStyleList.Add(item);
            }
            return styleInfo;
        }

        public static bool CanCreate()
        {
            return !smb_Created;
        }

        ~TimeBasedGraphDataTypes()
        {
        }

        void SelectDataType(int iDataType)
        {
            m_iSelectedDataType = iDataType;
            NMEACruncher.DataRangeTypes rangeType = NMEACruncher.GetDataRangeForType(m_iSelectedDataType);
            DirectionArrowCheckBox.Enabled = rangeType == NMEACruncher.DataRangeTypes.Direction || rangeType == NMEACruncher.DataRangeTypes.RelativeAngle;
            GraphDataStyleInfo gdsi = m_GraphStyleInfo.m_DataStyleList[m_iSelectedDataType];
            DirectionArrowCheckBox.Checked = gdsi.m_AsDirection;
            FieldColorPanel.BackColor = gdsi.m_Color;
            InvertArrowCheckbox.Checked = gdsi.m_InvertedArrow;

            //LineThicknessComboBox.SelectedIndex = 0;
            for (int i=0 ; i<LineThicknessComboBox.Items.Count ; i++)
            {
                string comboBoxString = LineThicknessComboBox.Items[i].ToString();
                int iComboBoxNumber = Convert.ToInt32(comboBoxString);
                if (gdsi.m_iThickness == iComboBoxNumber)
                {
                    LineThicknessComboBox.SelectedIndex = i;
                    break;
                }
            }

            //We don't assume that 
            for (int i = 0; i < DataTypeComboBox.Items.Count; i++)
            {
                if (gdsi.m_DataName == DataTypeComboBox.Items[i].ToString())
                {
                    DataTypeComboBox.SelectedIndex = i;
                    break;
                }
            }
        }

        void BackgroundColorPanel_Click(object sender, EventArgs e)
        {
            colorDialog1.SolidColorOnly = false;
            colorDialog1.FullOpen = true;
            colorDialog1.Color = m_GraphStyleInfo.m_BackgroundColor;
            if (colorDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                m_GraphStyleInfo.m_BackgroundColor = colorDialog1.Color;
                BackgroundColorPanel.BackColor = m_GraphStyleInfo.m_BackgroundColor;

                DockableDrawable.BroadcastGraphStyleChanged();
            }
        }

        void FieldColorPanel_Click(object sender, EventArgs e)
        {
            colorDialog1.SolidColorOnly = true;
            colorDialog1.FullOpen = true;
            colorDialog1.Color = m_GraphStyleInfo.m_DataStyleList[m_iSelectedDataType].m_Color;
            if (colorDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                m_GraphStyleInfo.m_DataStyleList[m_iSelectedDataType].m_Color = colorDialog1.Color;
                FieldColorPanel.BackColor = colorDialog1.Color;

                DockableDrawable.BroadcastGraphStyleChanged();
            }
        }

        private void DataTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DataTypeComboBox.SelectedIndex >= 0)
            {
                string nameOfData = DataTypeComboBox.Items[DataTypeComboBox.SelectedIndex].ToString();
                int iDataIndex = NMEACruncher.GetIndexOfDataType(nameOfData);
                if (iDataIndex >= 0)
                {
                    SelectDataType(iDataIndex);

                    //DockableDrawable.BroadcastGraphStyleChanged();
                }
            }
        }

        private void DirectionArrowCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (m_GraphStyleInfo.m_DataStyleList[m_iSelectedDataType].m_AsDirection != DirectionArrowCheckBox.Checked)
            {
                m_GraphStyleInfo.m_DataStyleList[m_iSelectedDataType].m_AsDirection = DirectionArrowCheckBox.Checked;

                DockableDrawable.BroadcastGraphStyleChanged();
            }
        }

        private void LineThicknessComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LineThicknessComboBox.SelectedIndex >= 0)
            {
                int iThickness = Convert.ToInt32(LineThicknessComboBox.Items[LineThicknessComboBox.SelectedIndex]);
                if (iThickness != m_GraphStyleInfo.m_DataStyleList[m_iSelectedDataType].m_iThickness)
                {
                    m_GraphStyleInfo.m_DataStyleList[m_iSelectedDataType].m_iThickness = iThickness;
                    DockableDrawable.BroadcastGraphStyleChanged();
                }
            }
        }

        private void InvertArrowCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (m_GraphStyleInfo.m_DataStyleList[m_iSelectedDataType].m_InvertedArrow != InvertArrowCheckbox.Checked)
            {
                m_GraphStyleInfo.m_DataStyleList[m_iSelectedDataType].m_InvertedArrow = InvertArrowCheckbox.Checked;
                DockableDrawable.BroadcastGraphStyleChanged();
            }
        }
    }
}
