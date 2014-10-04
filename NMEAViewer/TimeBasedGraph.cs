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
    public partial class TimeBasedGraph : DockableDrawable
    {
        public class GraphOverlay
        {
            public static List<GraphOverlay> sm_OverlayList = new List<GraphOverlay>();
            protected string OverlayName;
            public GraphOverlay()
            {
                sm_OverlayList.Add(this);
            }
            ~GraphOverlay()
            {
                sm_OverlayList.Remove(this);
            }
            public void Remove()
            {
                if (OnRemoved != null)
                {
                    OnRemoved();
                }
            }
            public string GetName() { return OverlayName; }
            public virtual void Draw(Graphics g, double fLeftTime, double fRightTime) { }
            public void NotifyDataChanged() 
            {
                if (OnDataChanged != null)
                {
                    OnDataChanged();
                }
            }
            public delegate void OnRemoveDelegate();  // defines a delegate type            
            public delegate void OnChangeDelegate();  // defines a delegate type            
            public event OnRemoveDelegate OnRemoved;
            public event OnChangeDelegate OnDataChanged;
        }

        [JsonObject(MemberSerialization.OptOut)]
        private class SerializedData : DockableDrawable.SerializedDataBase
        {
            public SerializedData(DockableDrawable parent)
                : base(parent) { }

            public List<string> CheckedButtons;
            public List<string> ActiveOverlays;
            //public double m_fSelectionStartTime = -1.0f;
            //public double m_fSelectionEndTime = -1.0f;
            //public double m_fSelectedTime = -1.0f;
            public double m_fGraphStartTime = -1.0;
            public double m_fGraphEndTime = -1.0;
            public bool m_bTrackSelection = false;
            public bool m_bExpandToLatest = false;
            public bool m_bMoveToLatest = false;
            public bool m_bDirectionsAsArrows = false;
        };

        public override DockableDrawable.SerializedDataBase CreateSerializedData()
        {
            SerializedData data = new SerializedData(this);
            data.m_fGraphStartTime = m_fGraphStartTime;
            data.m_fGraphEndTime = m_fGraphEndTime;
            //data.m_fSelectedTime = m_fSelectedTime;
            //data.m_fSelectionStartTime = m_fSelectionStartTime;
            //data.m_fSelectionEndTime = m_fSelectionEndTime;
            data.m_bTrackSelection = trackSelectionToolStripMenuItem.Checked;
            data.m_bExpandToLatest = expandToLatestToolStripMenuItem.Checked;
            data.m_bMoveToLatest = moveToLatestToolStripMenuItem.Checked;
            data.CheckedButtons = new List<string>();
            for (int i = 0; i < m_ContextMenu.MenuItems.Count; i++)
            {
                if (m_ContextMenu.MenuItems[i].Checked)
                {
                    data.CheckedButtons.Add(m_ContextMenu.MenuItems[i].Text);
                }
            }
            data.ActiveOverlays = new List<string>();
            foreach (GraphOverlay go in m_ActiveOverlays)
            {
                data.ActiveOverlays.Add( go.GetName() );
            }
            return data;
        }

        public override void InitFromSerializedData(SerializedDataBase data_base)
        {
            base.InitFromSerializedData(data_base);

            SerializedData data = (SerializedData)data_base;
            m_fGraphStartTime = data.m_fGraphStartTime;
            m_fGraphEndTime = data.m_fGraphEndTime;

            //m_fSelectedTime = data.m_fSelectedTime;
            //m_fSelectionStartTime = data.m_fSelectionStartTime;
            //m_fSelectionEndTime = data.m_fSelectionEndTime;

            moveToLatestToolStripMenuItem.Checked = data.m_bMoveToLatest;
            expandToLatestToolStripMenuItem.Checked = data.m_bExpandToLatest;
            trackSelectionToolStripMenuItem.Checked = data.m_bTrackSelection;
            
            //Set the checked state
            for (int iMenu = 0; iMenu < m_ContextMenu.MenuItems.Count; iMenu++)
            {
                m_ContextMenu.MenuItems[iMenu].Checked = false;
            }

            if (data.CheckedButtons != null)
            {
                for (int i = 0; i < data.CheckedButtons.Count; i++)
                {
                    for (int iMenu = 0; iMenu < m_ContextMenu.MenuItems.Count; iMenu++)
                    {
                        if (m_ContextMenu.MenuItems[iMenu].Text == data.CheckedButtons[i])
                        {
                            m_ContextMenu.MenuItems[iMenu].Checked = true;
                            break;
                        }
                    }
                }
            }

            //if (m_fSelectedTime >= 0.0)
            //{
            //    BroadcastOnTimeSelected(this, m_fSelectedTime);
            //}

            //if (m_fSelectionEndTime > m_fSelectionStartTime)
            //{
            //    BroadcastOnTimeRangeSelected(m_fSelectionStartTime, m_fSelectionEndTime);
            //}
        }

        public override void PostInitFromSerializedData(SerializedDataBase data_base)
        {
           SerializedData data = (SerializedData)data_base;
           if (data.ActiveOverlays != null)
           {
               foreach (string overlayName in data.ActiveOverlays)
               {
                   foreach (GraphOverlay go in GraphOverlay.sm_OverlayList)
                   {
                       if (go.GetName() == overlayName)
                       {
                           if (!m_ActiveOverlays.Contains(go))
                           {
                               m_ActiveOverlays.Add(go);
                           }
                       }
                   }
               }
           }
        }


        double m_fGraphStartTime = -1.0;
        double m_fGraphEndTime = -1.0;

        System.Windows.Forms.ContextMenu m_ContextMenu;
        NMEACruncher m_Data;
        MetaDataSerializer m_MetaData;
        List<GraphOverlay> m_ActiveOverlays = new List<GraphOverlay>();
        double m_fMouseHoveredTime = -1.0f;
        double m_fSelectionStartTime = -1.0f;
        double m_fSelectionEndTime = -1.0f;
        double m_fSelectedTime = -1.0;
        
        bool m_bSelecting = false;
        bool m_bSliding = false;
        int m_SlidingLastMouseX;

        public TimeBasedGraph(NMEACruncher data, MetaDataSerializer metaData)
        {
            InitializeComponent();

            GraphSurface.Paint += GraphSurfacePaint;
            GraphSurface.SizeChanged += GraphResized;
            GraphSurface.MouseMove += GraphMouseHover;
            GraphSurface.MouseLeave += GraphMouseLeave;
            GraphSurface.MouseDown += GraphMouseDown;
            GraphSurface.MouseUp += GraphMouseUp;
            GraphSurface.MouseWheel += GraphSurface_MouseWheel;
            GraphSurface.MouseMove += GraphSurface_MouseMove;
            MouseWheel += GraphSurface_MouseWheel;
            //GraphSurface.KeyPress += KeyPressHandler;
            KeyDown += KeyDownHandler;
            KeyPreview = true;

            //Set to the data object
            m_Data = data;
            m_MetaData = metaData;

            if (m_MetaData.m_GraphStyleInfo == null)
            {
                m_MetaData.m_GraphStyleInfo = TimeBasedGraphDataTypes.CreateNewStyleInfo();
            }

            m_ContextMenu = CreateContextMenu();
            ContextMenu = m_ContextMenu;

            overlayListToolStripMenuItem.DropDownOpening += overlayListToolStripMenuItem_DropDownOpening;
            //overlayListToolStripMenuItem.DropDownItemClicked += overlayListToolStripMenuItem_Click;

            ValidateGraphLineSurface();

            RefreshGraph();
        }

        void overlayListToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            overlayListToolStripMenuItem.DropDownItems.Clear();
            foreach (GraphOverlay overlay in GraphOverlay.sm_OverlayList)
            {
                ToolStripMenuItem newItem = new ToolStripMenuItem(overlay.GetName());
                newItem.Tag = overlay;
                newItem.Checked = m_ActiveOverlays.Contains(overlay);
                newItem.Click += overlayListToolStripMenuItem_Click;
                overlayListToolStripMenuItem.DropDownItems.Add(newItem);
            }
        }

        void overlayListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem itemClicked = (ToolStripMenuItem)sender;
            if (itemClicked != null)
            {
                GraphOverlay overlayCLicked = (GraphOverlay)itemClicked.Tag;
                if (overlayCLicked != null)
                {
                    bool bWasEnabled = m_ActiveOverlays.Contains( overlayCLicked );//.m_bEnabled;
                    itemClicked.Checked = !itemClicked.Checked;
                    if (itemClicked.Checked)
                    {
                        overlayCLicked.OnRemoved += overlayCLicked_OverlayChanged;
                        overlayCLicked.OnDataChanged += overlayCLicked_OverlayChanged;
                        m_ActiveOverlays.Add(overlayCLicked);
                    }
                    else if (bWasEnabled)
                    {
                        overlayCLicked.OnRemoved -= overlayCLicked_OverlayChanged;
                        overlayCLicked.OnDataChanged -= overlayCLicked_OverlayChanged;
                        m_ActiveOverlays.Remove(overlayCLicked);
                    }
                    RefreshOverlay();
                }
            }
        }

        void overlayCLicked_OverlayChanged()
        {
            RefreshOverlay();
        }

        const double kfZoomPerClick = 1.25;
        const double kfMaxZoom = 211.75823681357508476708062516991;   //1.25 ^ 24
        double m_fCentreOffset;
        void GraphSurface_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta < 0)
            {
                if (m_fGraphEndTime > 0.0)
                {
                    double fCurrentTimeRange = m_fGraphEndTime - m_fGraphStartTime;
                    double fNewTimeRange = fCurrentTimeRange * kfZoomPerClick;
                    double fEdgeIncrease = (fNewTimeRange - fCurrentTimeRange) * 0.5;
                    if (fNewTimeRange > m_Data.GetEndTime())
                    {
                        m_fGraphStartTime = 0.0;
                        m_fGraphEndTime = m_Data.GetEndTime();
                    }
                    else
                    {
                        m_fGraphStartTime = Math.Max(m_fGraphStartTime - fEdgeIncrease, 0.0);
                        m_fGraphEndTime = Math.Min(m_fGraphEndTime + fEdgeIncrease, m_Data.GetEndTime());
                        if ((m_fGraphEndTime >= m_Data.GetEndTime()) && (m_fGraphStartTime <= 0.0))
                        {
                            //Allow graph to expand with new data
                            m_fGraphStartTime = 0.0;
                            m_fGraphEndTime = m_Data.GetEndTime();
                        }
                    }
                }
            }
            else
            {
                double fCurrentTimeRange = m_fGraphEndTime - m_fGraphStartTime;
                double fNewTimeRange = fCurrentTimeRange / kfZoomPerClick;
                double fEdgeDecrease = (fCurrentTimeRange - fNewTimeRange) * 0.5;

                m_fGraphStartTime += fEdgeDecrease;
                m_fGraphEndTime -= fEdgeDecrease;
            }
            RefreshGraph();
        }

        public void KeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.KeyData == (Keys.A | Keys.Control))
            {
                //Ctrl-A = select all
                m_fSelectionStartTime = 0.0;
                m_fSelectionEndTime = m_Data.GetEndTime();
                BroadcastOnTimeRangeSelected(m_fSelectionStartTime, m_fSelectionEndTime);
                RefreshOverlay();
            }
            else if (e.KeyData == Keys.Z)
            {
                if (m_fSelectionEndTime > m_fSelectionStartTime)
                {
                    if ((m_fGraphStartTime == m_fSelectionStartTime) && (m_fGraphEndTime == m_fSelectionEndTime))
                    {
                        m_fGraphStartTime = 0.0;
                        m_fGraphEndTime = m_Data.GetEndTime();
                    }
                    else 
                    {
                        m_fGraphStartTime = m_fSelectionStartTime;
                        m_fGraphEndTime = m_fSelectionEndTime;
                    }
                    RefreshGraph();
                }
            }
        }

        private System.Windows.Forms.ContextMenu CreateContextMenu()
        {
            System.Windows.Forms.ContextMenu ctxtMenu = new System.Windows.Forms.ContextMenu();
            ctxtMenu.Name = "Data selection";
            int iCount = NMEACruncher.GetNumDataTypes();
            for (int i = 0; i < iCount; i++)
            {
                if (m_Data.HasDataForEntry(i) && (NMEACruncher.GetDataRangeForType(i) != NMEACruncher.DataRangeTypes.NoGraph))
                {
                    MenuItem mnuItem = new MenuItem();
                    mnuItem.Text = NMEACruncher.GetNameOfEntry(i);
                    mnuItem.Click += new System.EventHandler(this.menuItem_Click);

                    if (NMEACruncher.GetNameOfEntry(i) == "BoatSpeed")
                    {
                        mnuItem.Checked = true;
                    }

                    ctxtMenu.MenuItems.Add(mnuItem);
                }
            }
            return ctxtMenu;
        }

        private void menuItem_Click(Object sender, System.EventArgs e)
        {
            int iCount = NMEACruncher.GetNumDataTypes();
            int iMenuEntry = 0;
            for (int i = 0; i < iCount; i++)
            {
                if (m_Data.HasDataForEntry(i) && (NMEACruncher.GetDataRangeForType(i) != NMEACruncher.DataRangeTypes.NoGraph))
                {
                    if (ContextMenu.MenuItems[iMenuEntry] == sender)
                    {
                        ContextMenu.MenuItems[iMenuEntry].Checked = !ContextMenu.MenuItems[iMenuEntry].Checked;
                    }
                    iMenuEntry++;
                }
            }

            RefreshGraph();
        }

        private void RefreshGraph()
        {
            RedrawGraphLines();

            RefreshOverlay();
        }

        private void RefreshOverlay()
        {
            //Force the redraw
            GraphSurface.Refresh();
        }

        private double ConvertXToTime(double screenX)
        {
            double fScreenProp = screenX / (double)(GraphSurface.ClientSize.Width);
            return m_fGraphStartTime + (m_fGraphEndTime - m_fGraphStartTime) * fScreenProp;
        }

        private double ConvertScreenDXToTime(double screenDX)
        {
            double fScreenProp = screenDX / (double)(GraphSurface.ClientSize.Width);
            return (m_fGraphEndTime - m_fGraphStartTime) * fScreenProp;
        }

        void GraphSurface_MouseMove(object sender, MouseEventArgs e)
        {
            m_fMouseHoveredTime = ConvertXToTime((double)(e.X - GraphSurface.ClientRectangle.Left));

            if (m_bSelecting)
            {
                m_fSelectionEndTime = ConvertXToTime((double)(e.X - GraphSurface.ClientRectangle.Left));
            }

            if (m_bSliding)
            {
                ContextMenu = null;

                m_fCentreOffset += e.X - m_SlidingLastMouseX;

                double fDelta = ConvertScreenDXToTime(e.X - m_SlidingLastMouseX);
                m_fGraphEndTime = Math.Max(0.0, Math.Min(m_Data.GetEndTime(), m_fGraphEndTime - fDelta));
                m_fGraphStartTime = Math.Max(0.0, Math.Min(m_Data.GetEndTime()-1.0, m_fGraphStartTime - fDelta));
                    
                RefreshGraph();

                m_SlidingLastMouseX = e.X;

            }
        }

        private void GraphMouseLeave(object sender, EventArgs e)
        {
            m_fMouseHoveredTime = -1.0f;
            m_bSelecting = false;
            m_bSliding = false;
            ContextMenu = m_ContextMenu;
        }

        private void GraphMouseUp(object sender, EventArgs e)
        {
            if (m_bSliding)
            {
                m_bSliding = false;

                ContextMenu = m_ContextMenu;
            }

            if (m_bSelecting)
            {
                if (m_fSelectionEndTime != m_fSelectionStartTime)
                {
                    if (m_fSelectionStartTime > m_fSelectionEndTime)
                    {
                        double fTmp = m_fSelectionEndTime;
                        m_fSelectionEndTime = m_fSelectionStartTime;
                        m_fSelectionStartTime = fTmp;
                    }

                    if (m_fSelectionStartTime >= 0.0 && m_fSelectionEndTime > 0.0)
                    {
                        BroadcastOnTimeRangeSelected(m_fSelectionStartTime, m_fSelectionEndTime);
                    }
                }
                m_bSelecting = false;
            }
        }

        private void GraphMouseDown(object sender, MouseEventArgs e)
        {
            if ((e.Button & System.Windows.Forms.MouseButtons.Left) != 0)
            {
                m_fSelectionStartTime = m_fMouseHoveredTime;
                m_fSelectionEndTime = m_fMouseHoveredTime;
                m_bSelecting = true;

                //Send the time over as a sync point for all instruments
                m_fSelectedTime = m_fMouseHoveredTime;
                BroadcastOnTimeSelected(this, m_fMouseHoveredTime);
            }

            if ((e.Button & System.Windows.Forms.MouseButtons.Right) != 0)
            {
                m_bSliding = true;
                m_SlidingLastMouseX = e.X;
            }
        }

        private void GraphMouseHover(object sender, MouseEventArgs e)
        {
            m_fMouseHoveredTime = ConvertXToTime((double)(e.X - GraphSurface.ClientRectangle.Left));

            //Send the time over as a sync point for all instruments
            BroadcastOnTimeHovered(this, m_fMouseHoveredTime);
            
            if (m_bSelecting)
            {
                m_fSelectionEndTime = m_fMouseHoveredTime;
            }
            RefreshOverlay();
        }

        protected override void OnNewGraphStyle()
        {
            RefreshGraph();
        }

        protected override void OnTimeSelected(object sender, double fTime)
        {
            m_fSelectionStartTime = m_fSelectionEndTime = m_fSelectedTime = m_fMouseHoveredTime = fTime;
            
            RefreshOverlay();
        }

        protected override void OnTimeRangeSelected(double fTimeA, double fTimeB)
        {
            m_fSelectionStartTime = fTimeA;
            m_fSelectionEndTime = fTimeB;

            if (trackSelectionToolStripMenuItem.Checked)
            {
                m_fGraphStartTime = m_fSelectionStartTime;
                m_fGraphEndTime = m_fSelectionEndTime;
                RefreshGraph();
            }
            else
            {
                RefreshOverlay();
            }
        }

        private void GraphResized(object sender, EventArgs e)
        {
            ValidateGraphLineSurface();

            RefreshGraph();
        }

        protected override void OnDataReplaced(NMEACruncher newData)
        {
            m_Data = newData;
            RefreshGraph();
        }

        Bitmap m_SurfaceForLines;

        private void ValidateGraphLineSurface()
        {
            // create image to which we will draw, only if we need to
            if ((m_SurfaceForLines == null) || !((GraphSurface.Size.Width == m_SurfaceForLines.Size.Width) && (GraphSurface.Size.Height == m_SurfaceForLines.Size.Height)))
            {
                if (GraphSurface.Size.Height > 0)
                {
                    m_SurfaceForLines = new Bitmap(GraphSurface.Size.Width, GraphSurface.Size.Height);
                }
            }
        }

        private static double FixValueForGraph(double fValue, double fLastValue, int iDataType)
        {
            if (NMEACruncher.GetDataRangeForType(iDataType) == NMEACruncher.DataRangeTypes.Direction)
            {
                while ((fValue - fLastValue) > 180) fValue -= 360.0;
                while ((fValue - fLastValue) < -180) fValue += 360.0;
            }

            if (NMEACruncher.GetDataRangeForType(iDataType) == NMEACruncher.DataRangeTypes.RelativeAngle)
            {
                return Math.Abs(fValue);
            }
            return fValue;
        }

        private static double FixValueForGraph(double fValue, int iDataType)
        {
            //if (NMEACruncher.GetDataRangeForType(iDataType) == NMEACruncher.DataRangeTypes.Direction)
            //{
            //    //while ((fValue - fLastValue) > 180) fValue -= 360.0;
            //    //while ((fValue - fLastValue) < -180) fValue += 360.0;
            //}

            if (NMEACruncher.GetDataRangeForType(iDataType) == NMEACruncher.DataRangeTypes.RelativeAngle)
            {
                return Math.Abs(fValue);
            }
            return fValue;
        }

        bool m_bDataAppended;
        protected override void OnTimer(object sender, EventArgs e) 
        {
            if (m_bDataAppended)
            {
                if (expandToLatestToolStripMenuItem.Checked)
                {
                    m_fGraphEndTime = m_Data.GetEndTime();
                }
                else if (moveToLatestToolStripMenuItem.Checked)
                {
                    double delta = m_Data.GetEndTime() - m_fGraphEndTime;
                    m_fGraphEndTime += delta;
                    m_fGraphStartTime += delta;
                }
                m_fGraphEndTime = Math.Min(m_fGraphEndTime, m_Data.GetEndTime());
                m_fGraphStartTime = Math.Max(m_fGraphStartTime, 0.0);
                RefreshGraph();
                m_bDataAppended = false;
            }
            else
            {
                m_fGraphEndTime = Math.Min(m_fGraphEndTime, m_Data.GetEndTime());
                m_fGraphStartTime = Math.Max(m_fGraphStartTime, 0.0);
            }
        }

        protected override void OnDataAppended()
        {
            //TODO: Should be able to allocate an extra bit of graph surface and draw into it for a while at same zoom factor
            m_bDataAppended = true;
            SetTimerFrequency(1.0);
        }

        System.Drawing.Color GetColorForType(int iType)
        {
            return m_MetaData.m_GraphStyleInfo.m_DataStyleList[iType].m_Color;
        }

        int GetWidthForType(int iType)
        {
            return m_MetaData.m_GraphStyleInfo.m_DataStyleList[iType].m_iThickness;
        }

        const double PI = 3.141592653589793;
        const double DegToRad = (2.0 * PI / 360.0);
        private void RedrawGraphLines()
        {
            Graphics g = Graphics.FromImage(m_SurfaceForLines);
            // get a Graphics object via which we will draw to the image
            //var g = Graphics.FromImage(m_SurfaceForLines);
            if ((m_Data.GetEndTime() <= 0.0) || (m_Data.GetDataCount() <= 0))
            {
                g.Clear(Color.LightGray);
                //g.DrawString("No Data", )
                return;
            }

            if (m_fGraphEndTime < 0.0)
            {
                m_fGraphStartTime = 0.0;
                m_fGraphEndTime = m_Data.GetEndTime();
                expandToLatestToolStripMenuItem.Checked = true;
            }

            g.Clear( m_MetaData.m_GraphStyleInfo.m_BackgroundColor );

            //Simple plot for N indices
            //TODO: Investigate making end points more accurate
            int iStartIndex = Math.Max(0, m_Data.GetIndexForTime(m_fGraphStartTime));
            int iEndIndex = Math.Min(m_Data.GetDataCount()-1, m_Data.GetIndexForTime(m_fGraphEndTime));

            //Find the number of ranges we need to plot over, then get mins and maxes 
            int iNumDataRanges = NMEACruncher.GetNumDataRanges();
            List<int>[] typesByRange = new List<int>[iNumDataRanges];
            int iTypeCount = NMEACruncher.GetNumDataTypes();
            int iMenuEntry = 0;
            List<double>[] fLastValue = new List<double>[iNumDataRanges];
            List<int> directionalDataTypes = new List<int>();
            List<Brush> directionalDataTypePens = new List<Brush>();
            for (int iType = 0; iType < iTypeCount; iType++)
            {
                if (m_Data.HasDataForEntry(iType) && (NMEACruncher.GetDataRangeForType(iType) != NMEACruncher.DataRangeTypes.NoGraph))
                {
                    if (m_ContextMenu.MenuItems[iMenuEntry].Checked)
                    {
                        if (m_MetaData.m_GraphStyleInfo.m_DataStyleList[iType].m_AsLine)
                        {
                            int iRangeType = (int)NMEACruncher.GetDataRangeForType(iType);
                            if (typesByRange[iRangeType] == null)
                            {
                                typesByRange[iRangeType] = new List<int>();
                                fLastValue[iRangeType] = new List<double>();
                            }
                            typesByRange[iRangeType].Add(iType);
                            fLastValue[iRangeType].Add(0.0);    //For later manipulation
                        }
                        if (m_MetaData.m_GraphStyleInfo.m_DataStyleList[iType].m_AsDirection)
                        {
                            directionalDataTypes.Add(iType);
                            Brush newBrush = new SolidBrush(GetColorForType(iType));
                            //newPen.Width = GetWidthForType(iType);
                            directionalDataTypePens.Add(newBrush);
                        }
                    }
                    iMenuEntry++;
                }
            }

            double[] fMaxs = new double[iNumDataRanges];
            double[] fMins = new double[iNumDataRanges];
            bool[] bHasData = new bool[iNumDataRanges];
            for (int iRangeType = 0; iRangeType < iNumDataRanges; iRangeType++)
            {
                bHasData[iRangeType] = false;
                fMaxs[iRangeType] = fMins[iRangeType] = -1.0;
            }

            //Create pens for all data lines
            List<Pen>[] pens = new List<Pen>[iNumDataRanges];
            for (int iRangeType = 1; iRangeType < iNumDataRanges; iRangeType++)
            {
                if (typesByRange[iRangeType] != null)
                {
                    pens[iRangeType] = new List<Pen>();
                    for (int iDataTypeIndex = 0; iDataTypeIndex < typesByRange[iRangeType].Count; iDataTypeIndex++)
                    {
                        Pen newPen = new Pen(new SolidBrush(GetColorForType(typesByRange[iRangeType][iDataTypeIndex])));
                        newPen.Width = GetWidthForType(typesByRange[iRangeType][iDataTypeIndex]);
                        pens[iRangeType].Add(newPen);
                    }
                }
            }

            double fAvailableHeight = GraphSurface.ClientSize.Height;
            if (directionalDataTypes.Count > 0)
            {
                //Stack directions at the bottom of the graph, with arrows indicating direction of value
                //Height per direction
                float fHeightPerDirection = Math.Min(40.0f, Math.Max(20.0f, ((float)GraphSurface.ClientSize.Height) * 0.25f / (float)directionalDataTypes.Count));
                float fCentreY = GraphSurface.ClientSize.Height - fHeightPerDirection * 0.5f;
                float fWidthPerDirection = fHeightPerDirection * 0.5f;
                float fCount = Math.Max(1.0f, ((float)GraphSurface.ClientSize.Width) / fWidthPerDirection);
                double fTimeToIncrementPerDirection = (m_fGraphEndTime - m_fGraphStartTime) / fCount;
                fAvailableHeight -= (double)fHeightPerDirection * (double)directionalDataTypes.Count;
                Point[] points = new Point[3];
                Point[] pointsInv = new Point[3];
                int iWidth = Math.Max(1, (int)(fWidthPerDirection * 0.2f));
                int iArrowLength = (int)fWidthPerDirection;
                pointsInv[0].X = 0;
                pointsInv[0].Y = 0;
                pointsInv[2].X = iWidth;
                pointsInv[2].Y = iArrowLength;
                pointsInv[1].X = -iWidth;
                pointsInv[1].Y = -iArrowLength;
                points[0].X = 0;
                points[0].Y = -iArrowLength;
                points[1].X = iWidth;
                points[1].Y = 0;
                points[2].X = -iWidth;
                points[2].Y = 0;
                for (int i = 0; i < directionalDataTypes.Count; i++)
                {
                    float fCentreX = fHeightPerDirection * 0.5f;
                    double fTime = m_fGraphStartTime;
                    int iDataType = directionalDataTypes[i];
                    //TODO: Make the graph more accurate
                    int iLastEntry = -1;
                    bool bInverted = m_MetaData.m_GraphStyleInfo.m_DataStyleList[iDataType].m_InvertedArrow;
                    //Draw strip of values at bottom of graph
                    while (fTime < m_fGraphEndTime)
                    {
                        int iEntry = m_Data.GetIndexForTime(fTime);
                        if (iLastEntry >= 0 && iLastEntry <= iEntry)
                        {
                            double value = m_Data.GetDataAverageInclusive(iLastEntry, iEntry, iDataType);

                            g.ResetTransform();
                            g.TranslateTransform(fCentreX, fCentreY);
                            g.RotateTransform((float)value);

                            var pen = directionalDataTypePens[i];
                            if (bInverted)
                            {
                                g.FillPolygon(pen, pointsInv, System.Drawing.Drawing2D.FillMode.Alternate);
                            }
                            else
                            {
                                g.FillPolygon(pen, points, System.Drawing.Drawing2D.FillMode.Winding);
                            }
                        }

                        iLastEntry = iEntry;
                        fCentreX += fWidthPerDirection;
                        fTime += fTimeToIncrementPerDirection;                        
                    }
                    fCentreY -= fHeightPerDirection;
                }
                g.ResetTransform();
            }

            for (int iEntry = iStartIndex; iEntry < iEndIndex; iEntry++)
            {
                for (int iRangeType = 1; iRangeType < iNumDataRanges; iRangeType++)
                {
                    if ((typesByRange[iRangeType]) != null)// && (iRangeType != (int)NMEACruncher.DataRangeTypes.Direction))
                    {
                        for (int iDataTypeIndex = 0; iDataTypeIndex < typesByRange[iRangeType].Count; iDataTypeIndex++)
                        {
                            int iDataType = typesByRange[iRangeType][iDataTypeIndex];
                            if (m_Data.HasDataAtIndex(iEntry, iDataType))
                            {
                                double fValue = m_Data.GetDataAtIndex(iEntry, iDataType);
                                if (bHasData[iRangeType])
                                {
                                    double fPinValue = fLastValue[iRangeType][iDataTypeIndex];
                                    fValue = FixValueForGraph(fValue, fPinValue, iDataType);
                                    fMins[iRangeType] = System.Math.Min(fMins[iRangeType], fValue);
                                    fMaxs[iRangeType] = System.Math.Max(fMaxs[iRangeType], fValue);
                                }
                                else
                                {
                                    fMins[iRangeType] = fMaxs[iRangeType] = FixValueForGraph(fValue, iDataType);
                                    bHasData[iRangeType] = true;
                                }
                                fLastValue[iRangeType][iDataTypeIndex] = fValue;
                            }
                        }
                    }
                }
            }

            for (int iRangeType = 1; iRangeType < iNumDataRanges; iRangeType++)
            {
                if (typesByRange[iRangeType] != null)
                {
                    fMins[iRangeType] -= m_Data.GetRangePadding(iRangeType);
                    fMaxs[iRangeType] += m_Data.GetRangePadding(iRangeType);
                }
            }

            float fHorzPerEntry = (float)(GraphSurface.ClientSize.Width) / (float)(iEndIndex - iStartIndex);

            //Each line needs a coordinate to draw a segment from, cache off the Y values for the first segments
            List<double>[] fLastY = new List<double>[iNumDataRanges];
            List<double>[] fSummedY = new List<double>[iNumDataRanges];
            List<double>[] fCountY = new List<double>[iNumDataRanges];
            List<float>[] fLastDrawnX = new List<float>[iNumDataRanges];
            for (int iRangeType = 1; iRangeType < iNumDataRanges; iRangeType++)
            {
                if ((typesByRange[iRangeType] != null))// && (iRangeType != (int)NMEACruncher.DataRangeTypes.Direction))
                {
                    fLastY[iRangeType] = new List<double>();
                    fCountY[iRangeType] = new List<double>();
                    fSummedY[iRangeType] = new List<double>();
                    fLastDrawnX[iRangeType] = new List<float>();
                    for (int iDataTypeIndex = 0; iDataTypeIndex < typesByRange[iRangeType].Count; iDataTypeIndex++)
                    {
                        int iDataType = typesByRange[iRangeType][iDataTypeIndex];
                        double fPinValue = fLastValue[iRangeType][iDataTypeIndex];
                        double fValue = FixValueForGraph(m_Data.GetDataAtIndex(iStartIndex, iDataType), fPinValue, iDataType);
                        double fValueY = fAvailableHeight * (1.0 - (fValue - fMins[iRangeType]) / (fMaxs[iRangeType] - fMins[iRangeType]));
                        fLastY[iRangeType].Add(fValueY);
                        fLastValue[iRangeType][iDataTypeIndex] = fValue;
                        fCountY[iRangeType].Add(0.0);       //Prepping for later manipulation
                        fSummedY[iRangeType].Add(0.0);      //Prepping for later manipulation
                        fLastDrawnX[iRangeType].Add(0.0f);   //Prepping for later manipulation
                    }
                }
            }

            const int nPixelsPerLine = 4;
            int nItems = Math.Max(GraphSurface.Width, 2) / nPixelsPerLine;
            double fDTime = (m_fGraphEndTime - m_fGraphStartTime) / (double)nItems;
            float fLastX = 0.0f;
            float fCurrentX = fLastX + (float)nPixelsPerLine;
            int iLastDataIndex = Math.Max(0,m_Data.GetIndexForTime(m_fGraphStartTime));
            double fLineTime = m_fGraphStartTime + fDTime;
            bool bHasPreceeding = false;
            while (fLineTime < m_fGraphEndTime)
            {
                int iDataIndexEnd = m_Data.GetIndexForTime(fLineTime);
                if (iDataIndexEnd > iLastDataIndex)
                {
                    //We have values to average, 
                    for (int iDataIndex = iLastDataIndex; iDataIndex < iDataIndexEnd; iDataIndex++)
                    {
                        for (int iRangeType = 1; iRangeType < iNumDataRanges; iRangeType++)
                        {
                            if ((typesByRange[iRangeType] != null))
                            {
                                for (int iDataTypeIndex = 0; iDataTypeIndex < typesByRange[iRangeType].Count; iDataTypeIndex++)
                                {
                                    int iDataType = typesByRange[iRangeType][iDataTypeIndex];
                                    double fPinValue = fLastValue[iRangeType][iDataTypeIndex];
                                    double fNewValue = m_Data.GetDataAtIndex(iDataIndex, iDataType);
                                    fSummedY[iRangeType][iDataTypeIndex] += FixValueForGraph(fNewValue, fPinValue, iDataType);
                                }
                            }
                        }
                    }

                    //Now draw the lines we have and update fLastValue
                    double fInvForAverage = 1.0 / (double)(iDataIndexEnd - iLastDataIndex); //May want too max / min, draw polygons or something
                    for (int iRangeType = 1; iRangeType < iNumDataRanges; iRangeType++)
                    {
                        if ((typesByRange[iRangeType] != null))
                        {
                            for (int iDataTypeIndex = 0; iDataTypeIndex < typesByRange[iRangeType].Count; iDataTypeIndex++)
                            {
                                double fNewValue = fSummedY[iRangeType][iDataTypeIndex] * fInvForAverage;
                                double fYValue = fAvailableHeight * (1.0 - (fNewValue - fMins[iRangeType]) / (fMaxs[iRangeType] - fMins[iRangeType]));

                                if (bHasPreceeding)
                                {
                                    g.DrawLine(
                                            pens[iRangeType][iDataTypeIndex],
                                            fLastX, (float)fLastY[iRangeType][iDataTypeIndex],
                                            fCurrentX, (float)fYValue
                                        );
                                }

                                fLastY[iRangeType][iDataTypeIndex] = fYValue;
                                fLastValue[iRangeType][iDataTypeIndex] = fNewValue;
                                fSummedY[iRangeType][iDataTypeIndex] = 0.0;
                            }
                        }
                    }

                    bHasPreceeding = true;
                    iLastDataIndex = iDataIndexEnd;
                    fLastX = fCurrentX;
                }
                fCurrentX += nPixelsPerLine;
                fLineTime += fDTime;
            }

            //Now draw lines for the graph
            //float fX = fHorzPerEntry;
            //for (int iEntry = iStartIndex + 1; iEntry < iEndIndex; iEntry++)
            //{
            //    for (int iRangeType = 1; iRangeType < iNumDataRanges; iRangeType++)
            //    {
            //        if ((typesByRange[iRangeType] != null))// && (iRangeType != (int)NMEACruncher.DataRangeTypes.Direction))
            //        {
            //            for (int iDataTypeIndex = 0; iDataTypeIndex < typesByRange[iRangeType].Count; iDataTypeIndex++)
            //            {   //fix this so that we account for uneven time steps, full width on bc_2 data and you can see the 
            //                //hovered values do not agree with the graph lines
                            
            //                int iDataType = typesByRange[iRangeType][iDataTypeIndex];
            //                double fPinValue = fLastValue[iRangeType][iDataTypeIndex];
            //                double fValue = FixValueForGraph(m_Data.GetDataAtIndex(iEntry, iDataType), fPinValue, iDataType);
            //                fLastValue[iRangeType][iDataTypeIndex] = fValue;

            //                fValue = fAvailableHeight * (1.0 - (fValue - fMins[iRangeType]) / (fMaxs[iRangeType] - fMins[iRangeType]));

            //                fSummedY[iRangeType][iDataTypeIndex] = System.Math.Max(fSummedY[iRangeType][iDataTypeIndex], fValue);
            //                //fSummedY[iRangeType][iDataTypeIndex] += fValue;
            //                fCountY[iRangeType][iDataTypeIndex] += 1.0;

            //                const double kPixelsPadding = 1.0;
            //                if (System.Math.Floor(fX) > System.Math.Floor(fLastDrawnX[iRangeType][iDataTypeIndex] + kPixelsPadding))
            //                {
            //                    double fDrawnValue = fSummedY[iRangeType][iDataTypeIndex];
            //                    g.DrawLine(
            //                            pens[iRangeType][iDataTypeIndex],
            //                            (float)fLastDrawnX[iRangeType][iDataTypeIndex], (float)fLastY[iRangeType][iDataTypeIndex],
            //                            (float)fX, (float)fDrawnValue
            //                        );

            //                    fLastDrawnX[iRangeType][iDataTypeIndex] = fX;

            //                    //Store for next time
            //                    fLastY[iRangeType][iDataTypeIndex] = fDrawnValue;
            //                    fCountY[iRangeType][iDataTypeIndex] = 0.0;
            //                    fSummedY[iRangeType][iDataTypeIndex] = 0.0;
            //                }
            //            }
            //        }
            //    }
            //    fX += fHorzPerEntry;
            //}
        }

        private void GraphSurfacePaint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(m_SurfaceForLines, 0, 0);

            if (m_fSelectionStartTime != m_fSelectionEndTime)
            {
                double fWidthScale = GraphSurface.ClientSize.Width / (m_fGraphEndTime - m_fGraphStartTime);
                float fSelectedStart = (float)((m_fSelectionStartTime - m_fGraphStartTime) * fWidthScale);
                float fSelectedEnd = (float)((m_fSelectionEndTime - m_fGraphStartTime) * fWidthScale);
                if (fSelectedEnd < fSelectedStart)
                {
                    float fTmp = fSelectedStart;
                    fSelectedStart = fSelectedEnd;
                    fSelectedEnd = fTmp;
                }

                Pen selectedPen = new Pen(new SolidBrush(Color.Gray));
                selectedPen.Width = 4.0f;
                e.Graphics.DrawRectangle(
                        selectedPen,
                        fSelectedStart, 5.0f,
                        fSelectedEnd - fSelectedStart, (float)(GraphSurface.ClientSize.Height - 10.0)
                    );
            }

            if (m_fMouseHoveredTime >= 0.0f)
            {
                float fMousePoint = (float)((m_fMouseHoveredTime - m_fGraphStartTime) * GraphSurface.ClientSize.Width / (m_fGraphEndTime - m_fGraphStartTime));
                Pen overlayPen = new Pen(new SolidBrush(Color.DarkOliveGreen));
                overlayPen.Width = 2.0f;
                e.Graphics.DrawLine(overlayPen, (float)fMousePoint, 0.0f, (float)fMousePoint, GraphSurface.ClientSize.Height);
            }

            //Draw the overlays
            foreach (GraphOverlay overlay in m_ActiveOverlays)
            {
                overlay.Draw(e.Graphics, m_fGraphStartTime, m_fGraphEndTime);
            }

            //Draw a string of the current data, top left
            if (m_fMouseHoveredTime >= 0.0)
            {
                int iData = m_Data.GetIndexForTime(m_fMouseHoveredTime);
                int iCount = 0;
                if (iData >= 0)
                {
                    System.Drawing.Font font = new System.Drawing.Font("Arial", 10);
                    float XOffset = 5.0f;
                    for (int i = 0; i < m_ContextMenu.MenuItems.Count; i++)
                    {
                        if (m_ContextMenu.MenuItems[i].Checked)
                        {
                            int dataType = NMEACruncher.GetIndexOfDataType(m_ContextMenu.MenuItems[i].Text);
                            if (dataType >= 0)
                            {
                                double fValue = m_Data.GetDataAtIndex(iData, dataType);
                                string dataValue =  iCount > 0 ? ", " : "";
 
                                dataValue += m_ContextMenu.MenuItems[i].Text;
                                dataValue += ": ";
                                dataValue += string.Format("{0:0.##}", fValue);

                                System.Drawing.SizeF sizeOnScreenMin = e.Graphics.MeasureString(dataValue, font);
                                System.Drawing.SolidBrush myBrush0 = new System.Drawing.SolidBrush(GetColorForType(dataType));
                                e.Graphics.DrawString(dataValue, font, myBrush0, XOffset, 5.0f);
                                XOffset += sizeOnScreenMin.Width;
                                ++iCount;
                            }
                        }
                    }
                }
            }
        }

        private void trackSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            trackSelectionToolStripMenuItem.Checked = !trackSelectionToolStripMenuItem.Checked;
            if (trackSelectionToolStripMenuItem.Checked)
            {
                m_fGraphStartTime = m_fSelectionStartTime;
                m_fGraphEndTime = m_fSelectionEndTime;
                RefreshGraph();
            }
        }

        TimeBasedGraphDataTypes GraphStyleWindow;
        private void graphStyleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TimeBasedGraphDataTypes.CanCreate())
            {
                GraphStyleWindow = new TimeBasedGraphDataTypes(m_MetaData);
                GraphStyleWindow.Show();
            }
            else
            {
                //Assume we own it. Perhaps make a static?
                GraphStyleWindow.Show();
            }
        }

        private void moveToLatestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            moveToLatestToolStripMenuItem.Checked = !moveToLatestToolStripMenuItem.Checked;
            if (moveToLatestToolStripMenuItem.Checked)
            {
                expandToLatestToolStripMenuItem.Checked = false;
            }
        }

        private void expandToLatestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            expandToLatestToolStripMenuItem.Checked = !expandToLatestToolStripMenuItem.Checked;
            if (expandToLatestToolStripMenuItem.Checked)
            {
                moveToLatestToolStripMenuItem.Checked = false;
            }
        }
    }
}
