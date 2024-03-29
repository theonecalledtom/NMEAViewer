﻿using System;
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
    public partial class Histogram : DockableDrawable
    {
        NMEACruncher m_Data;
        int m_iCount = 20;
        int[] m_PortTackBuckets;
        int[] m_StbdTackBuckets;
        double m_fMinValue;
        double m_fMaxValue;
        double m_fAverageValuePort;
        double m_fAverageValueStbd;
        double m_fStartTime;
        double m_fEndTime;
        double m_fDataStartTime;
        double m_fDataEndTime;
        double m_fUserSelectedStartTime;
        double m_fUserSelectedEndTime;
        string m_CurrentSelectedType;
        static string m_LastSelectedType = "BoatSpeed";

        //To map the old values in behind new values
        System.Drawing.Rectangle m_OldPortRect;
        System.Drawing.Rectangle m_OldStbdRect;
        double m_fOldMinValue;
        double m_fOldMaxValue;

        double m_fTopOfBuckets;
        double m_fBottomOfBuckets;
        float m_fCentreOffset;
        bool m_bHasHoveredValue;
        double m_fHoveredValue;
        bool m_bHoverOnPort;
        double m_fAnchor = 0.0;
        double m_fSecondsTracked = 60.0;
        bool m_bAngleConstraint = false;

        [JsonObject(MemberSerialization.OptOut)]
        private class SerializedData : DockableDrawable.SerializedDataBase
        {
            public SerializedData(DockableDrawable parent)
                : base(parent) { }
            public string m_CurrentSelectedType;
            public double m_fStartTime;
            public double m_fEndTime;
            public double m_fSecondsTracked = 60.0;
            public int m_iCount = 15;
            public bool m_bFollowSelection;
            public bool m_bFollowCurrent;
            public bool m_bShowHistory;
        };

        public override DockableDrawable.SerializedDataBase CreateSerializedData()
        {
            SerializedData data = new SerializedData(this);
            data.m_CurrentSelectedType = m_CurrentSelectedType;
            data.m_fStartTime = m_fStartTime;
            data.m_fEndTime = m_fEndTime;
            data.m_iCount = m_iCount;
            data.m_fSecondsTracked = m_fSecondsTracked;
            data.m_bFollowSelection = followSelectionToolStripMenuItem.Checked;
            data.m_bFollowCurrent = followCurrentToolStripMenuItem.Checked;
            data.m_bShowHistory = historyToolStripMenuItem.Checked;
            return data;
        }

        public override void InitFromSerializedData(SerializedDataBase data_base)
        {
            base.InitFromSerializedData(data_base);

            SerializedData data = (SerializedData)data_base;
            m_fStartTime = data.m_fStartTime;
            m_fEndTime = data.m_fEndTime;
            m_fSecondsTracked = data.m_fSecondsTracked;
            m_iCount = data.m_iCount;
            m_CurrentSelectedType = data.m_CurrentSelectedType;
            m_bHasHoveredValue = false;
            followCurrentToolStripMenuItem.Checked = data.m_bFollowCurrent;
            followSelectionToolStripMenuItem.Checked = data.m_bFollowSelection;
            historyToolStripMenuItem.Checked = data.m_bShowHistory;

            if (ContextMenu != null)
            {
                for (int i=0 ; i<ContextMenu.MenuItems.Count ; i++)
                {
                    ContextMenu.MenuItems[i].Checked = (ContextMenu.MenuItems[i].Text == m_CurrentSelectedType);
                }
            }

            RebuildHistogram(m_fStartTime, m_fEndTime);
        }

        protected override void OnDataReplaced(NMEACruncher newData)
        {
            m_Data = newData;
            RebuildHistogram(m_fStartTime, m_fEndTime);
        }

        public Histogram(NMEACruncher data)
        {
            InitializeComponent();

            SetTimerFrequency(10.0); //?? Make dynamic ?

            m_Data = data;
        }

        void RebuildToCurrentTimeRange()
        {
            double delta = m_fEndTime - m_fStartTime;
            if (delta <= 0.0)
            {
                delta = m_fSecondsTracked;   //TODO:edit
            }

            if (delta > 0.0)
            {
                m_fEndTime = m_Data.GetEndTime();
                m_fStartTime = Math.Max(0.0, m_fEndTime - delta);
                RebuildHistogram(m_fStartTime, m_fEndTime);
            }
        }

        protected override void OnTimer(object sender, EventArgs e)
        {
            base.OnTimer(sender, e);

            if (followCurrentToolStripMenuItem.Checked)
            {
                RebuildToCurrentTimeRange();
                double delta = m_fEndTime - m_fStartTime;
                if (delta <= 0.0)
                {
                    delta = m_fSecondsTracked;   //TODO:edit
                }

                if (delta > 0.0)
                {
                    m_fEndTime = m_Data.GetEndTime();
                    m_fStartTime = Math.Max(0.0, m_fEndTime - delta);
                    RebuildHistogram(m_fStartTime, m_fEndTime);
                }
            }
        }

        private void Histogram_Load(object sender, EventArgs e)
        {
            CreateContextMenu();

            RebuildHistogram(0.0, 0.0);

            HistogramSurface.Paint += Histogram_Paint;
            HistogramSurface.Resize += HistogramResized;
        }

        private void HistogramResized(object sender, EventArgs e)
        {
            DrawGraph();

            HistogramSurface.Refresh();
        }

        private void CreateContextMenu()
        {
            ContextMenu = new System.Windows.Forms.ContextMenu();
            ContextMenu.Name = "Data selection";
            int iCount = NMEACruncher.GetNumDataTypes();
            for (int i = 0; i < iCount; i++)
            {
                if (m_Data.HasDataForEntry(i) && (NMEACruncher.GetDataRangeForType(i) != NMEACruncher.DataRangeTypes.NoGraph))
                {
                    MenuItem mnuItem = new MenuItem();
                    mnuItem.Text = NMEACruncher.GetNameOfEntry(i);
                    mnuItem.Click += new System.EventHandler(this.menuItem_Click);

                    if (NMEACruncher.GetNameOfEntry(i) == m_LastSelectedType)
                    {
                        mnuItem.Checked = true;
                    }

                    ContextMenu.MenuItems.Add(mnuItem);
                }
                m_CurrentSelectedType = m_LastSelectedType;
            }
            m_bHasHoveredValue = false;
        }

        private void menuItem_Click(Object sender, System.EventArgs e)
        {
            int iCount = NMEACruncher.GetNumDataTypes();
            int iMenuEntry = 0;
            for (int i = 0; i < iCount; i++)
            {
                if (m_Data.HasDataForEntry(i) && (NMEACruncher.GetDataRangeForType(i) != NMEACruncher.DataRangeTypes.NoGraph))
                {
                    ContextMenu.MenuItems[iMenuEntry].Checked = ContextMenu.MenuItems[iMenuEntry] == sender;
                    if (ContextMenu.MenuItems[iMenuEntry].Checked)
                    {
                        m_LastSelectedType = ContextMenu.MenuItems[iMenuEntry].Text;
                        m_CurrentSelectedType = ContextMenu.MenuItems[iMenuEntry].Text;
                    }
                    iMenuEntry++;
                }
            }

            m_bHasHoveredValue = false;

            RebuildHistogram(m_fStartTime, m_fEndTime);

            DrawGraph();

            HistogramSurface.Refresh();
        }

        Bitmap m_SurfaceForLines;

        private void ValidateGraphSurface()
        {
            // create image to which we will draw, only if we need to
            if ((m_SurfaceForLines == null) || !((HistogramSurface.Size.Width == m_SurfaceForLines.Size.Width) && (HistogramSurface.Size.Height == m_SurfaceForLines.Size.Height)))
            {
                if (HistogramSurface.Size.Height > 0)
                {
                    m_SurfaceForLines = new Bitmap(HistogramSurface.Size.Width, HistogramSurface.Size.Height);
                }
            }
        }


        private void DrawGraph()
        {
            Bitmap lastSurface = null;
            System.Drawing.Color clearColor = System.Drawing.Color.Gray;
            if (historyToolStripMenuItem.Checked && (m_SurfaceForLines != null))
            {
                //Cache off old surface...? 
                lastSurface = new Bitmap(m_SurfaceForLines);
            }

            ValidateGraphSurface();

            Graphics g = Graphics.FromImage(m_SurfaceForLines);

            if (m_PortTackBuckets == null)
            {
                g.Clear(System.Drawing.Color.LightGray);
                return;
            }

            g.Clear(clearColor);
            
            int iMaxHorz = 0;
            int iMaxPort = 0;
            int iMaxStbd = 0;
            for (int i = 0; i < m_iCount; i++)
            {
                if (m_PortTackBuckets[i] > iMaxPort)
                {
                    iMaxPort = m_PortTackBuckets[i];
                }
                if (m_StbdTackBuckets[i] > iMaxStbd)
                {
                    iMaxStbd = m_StbdTackBuckets[i];
                }
            }

            iMaxHorz = iMaxPort + iMaxStbd;
            if (iMaxHorz == 0)
            {
                //nowt to draw
                return;
            }

            int type = NMEACruncher.GetIndexOfDataType(m_CurrentSelectedType);
            if (type < 0)
            {
                return;
            }

            System.Drawing.Font font = new System.Drawing.Font("Arial", 10);
            System.String minString = "Min: ";
            minString += String.Format("{0:F2}", m_bAngleConstraint ? AngleUtil.ContainAngle0To360(m_fMinValue) : m_fMinValue);
            System.Drawing.SizeF sizeOnScreenMin = g.MeasureString(minString, font);
            System.String maxString = "Max: ";
            maxString += String.Format("{0:F2}", m_bAngleConstraint ? AngleUtil.ContainAngle0To360(m_fMaxValue) : m_fMaxValue);
            System.Drawing.SizeF sizeOnScreenMax = g.MeasureString(maxString, font);

            float fImageWidth = (float)HistogramSurface.Right - (float)HistogramSurface.Left;
            m_fCentreOffset = (float)iMaxPort * fImageWidth / (float)iMaxHorz;
            float fPortSize = m_fCentreOffset * 0.9f;
            float fStbdSize = (fImageWidth - m_fCentreOffset) * 0.9f;
            float fTopOfBuckets = sizeOnScreenMax.Height;
            float fBottomOfBuckets = ((float)HistogramSurface.Bottom) - HistogramSurface.Top - sizeOnScreenMin.Height;
            float fBucketHeight = (fBottomOfBuckets - fTopOfBuckets) / (float)(m_iCount);
            float fBucketBottom = fBottomOfBuckets;

            m_fTopOfBuckets = fTopOfBuckets;
            m_fBottomOfBuckets = fBottomOfBuckets;

            if (lastSurface != null)
            {
                // Create a new color matrix and set the alpha value to 0.5
                System.Drawing.Imaging.ColorMatrix cm = new System.Drawing.Imaging.ColorMatrix();
                cm.Matrix00 = cm.Matrix11 = cm.Matrix22 = cm.Matrix44 = 1.0f;
                cm.Matrix33 = 0.25f;

                // Create a new image attribute object and set the color matrix to
                // the one just created
                System.Drawing.Imaging.ImageAttributes ia = new System.Drawing.Imaging.ImageAttributes();
                ia.SetColorMatrix(cm);

                // Draw the original image with the image attributes specified, remap the coordinates to match current port / starboard and value range
                double fOldMinProp = (m_fOldMinValue - m_fMinValue) / (m_fMaxValue - m_fMinValue);
                double fRemappedBottom = m_fBottomOfBuckets + fOldMinProp * (m_fTopOfBuckets - m_fBottomOfBuckets);

                double fOldMaxProp = (m_fOldMaxValue - m_fMinValue) / (m_fMaxValue - m_fMinValue);
                double fRemappedTop = m_fBottomOfBuckets + fOldMaxProp * (m_fTopOfBuckets - m_fBottomOfBuckets);

                // First the port side
                g.DrawImage(lastSurface,
                    new Rectangle(0, (int)fRemappedTop, (int)m_fCentreOffset, (int)fRemappedBottom),
                    m_OldPortRect.Left, m_OldPortRect.Top, m_OldPortRect.Width, m_OldPortRect.Height, GraphicsUnit.Pixel,
                    ia);

                // Then the starboard
                g.DrawImage(lastSurface,
                    new Rectangle((int)m_fCentreOffset, (int)fRemappedTop, HistogramSurface.Width, (int)fRemappedBottom),
                    m_OldStbdRect.Left, m_OldStbdRect.Top, m_OldStbdRect.Width, m_OldStbdRect.Height, GraphicsUnit.Pixel,
                    ia);

                //m_OldPortRect.Location = new System.Drawing.Point((int)(m_fCentreOffset - fPortSize), (int)m_fTopOfBuckets);
                //m_OldPortRect.Size = new System.Drawing.Size((int)fPortSize, (int)(m_fBottomOfBuckets - m_fTopOfBuckets));
                //m_OldStbdRect.Location = new System.Drawing.Point((int)(m_fCentreOffset), (int)m_fTopOfBuckets);
                //m_OldStbdRect.Size = new System.Drawing.Size((int)fStbdSize, (int)(m_fBottomOfBuckets - m_fTopOfBuckets));
                //m_fOldMinValue = m_fMinValue;
                //m_fOldMaxValue = m_fMaxValue;

            }

            //Create brushes
            System.Drawing.SolidBrush myBrush0 = new System.Drawing.SolidBrush(System.Drawing.Color.Red);
            System.Drawing.SolidBrush myBrush1 = new System.Drawing.SolidBrush(System.Drawing.Color.DarkRed);
            System.Drawing.SolidBrush myBrush2 = new System.Drawing.SolidBrush(System.Drawing.Color.Green);
            System.Drawing.SolidBrush myBrush3 = new System.Drawing.SolidBrush(System.Drawing.Color.DarkGreen);
            for (int i = 0; i < m_iCount; i++)
            {
                float fPortLeft = m_fCentreOffset - fPortSize * ((float)m_PortTackBuckets[i]) / (float)iMaxPort;
                float fStbdRight = m_fCentreOffset + fStbdSize * ((float)m_StbdTackBuckets[i]) / (float)iMaxStbd;
                float fBucketTop = fBucketBottom - fBucketHeight;
                g.FillRectangle((i & 1) > 0 ? myBrush0 : myBrush1, fPortLeft, fBucketTop, m_fCentreOffset - fPortLeft, fBucketHeight);
                g.FillRectangle((i & 1) > 0 ? myBrush2 : myBrush3, m_fCentreOffset, fBucketTop, fStbdRight - m_fCentreOffset, fBucketHeight);

                fBucketBottom -= fBucketHeight;
            }

            //Store values for mapping the history image next time around
            //m_OldPortRect.Location = new System.Drawing.Point((int)(m_fCentreOffset - fPortSize), (int)m_fTopOfBuckets);
            //m_OldPortRect.Size = new System.Drawing.Size((int)fPortSize, (int)(m_fBottomOfBuckets - m_fTopOfBuckets));
            //m_OldStbdRect.Location = new System.Drawing.Point((int)(m_fCentreOffset), (int)m_fTopOfBuckets);
            //m_OldStbdRect.Size = new System.Drawing.Size((int)fStbdSize, (int)(m_fBottomOfBuckets - m_fTopOfBuckets));
            m_OldPortRect.Location = new System.Drawing.Point(0, (int)m_fTopOfBuckets);
            m_OldPortRect.Size = new System.Drawing.Size((int)m_fCentreOffset, (int)(m_fBottomOfBuckets - m_fTopOfBuckets));
            m_OldStbdRect.Location = new System.Drawing.Point((int)(m_fCentreOffset), (int)m_fTopOfBuckets);
            m_OldStbdRect.Size = new System.Drawing.Size(HistogramSurface.Width - (int)m_fCentreOffset, (int)(m_fBottomOfBuckets - m_fTopOfBuckets));
            m_fOldMinValue = m_fMinValue;
            m_fOldMaxValue = m_fMaxValue;


            //Print the min
            float fLeftMin = m_fCentreOffset - sizeOnScreenMin.Width * 0.5f;
            if (fLeftMin < 10.0f)
            {
                fLeftMin = 10.0f;
            }
            else if ((fLeftMin + sizeOnScreenMin.Width) > (HistogramSurface.Right - HistogramSurface.Left))
            {
                float fDelta = (HistogramSurface.Right - HistogramSurface.Left) - (fLeftMin + sizeOnScreenMin.Width);
                fLeftMin += fDelta;
            }
            float fTopMin = HistogramSurface.Bottom - HistogramSurface.Top - sizeOnScreenMin.Height;
            g.DrawString(minString, font, System.Drawing.Brushes.LightGray, fLeftMin, fTopMin);

            //Print the max
            float fLeftMax = m_fCentreOffset - sizeOnScreenMax.Width * 0.5f;
            if (fLeftMax < 10.0f)
            {
                fLeftMax = 10.0f;
            }
            else if ((fLeftMax + sizeOnScreenMax.Width) > (HistogramSurface.Right - HistogramSurface.Left))
            {
                float fDelta = (HistogramSurface.Right - HistogramSurface.Left) - (fLeftMax + sizeOnScreenMax.Width);
                fLeftMax += fDelta;
            }
            g.DrawString(maxString, font, System.Drawing.Brushes.LightGray, fLeftMax, 0.0f);

            //Draw a line for the average value
            if (m_fMaxValue > m_fMinValue)
            {
                System.Drawing.Pen pen = new System.Drawing.Pen(System.Drawing.Color.LightGray, 3);

                if (fPortSize > 0.0f)
                {
                    float fAveragePortProp = (float)((m_fAverageValuePort - m_fMinValue) / (m_fMaxValue - m_fMinValue));
                    float fAveragePortY = (float)fBottomOfBuckets + (fTopOfBuckets - fBottomOfBuckets) * fAveragePortProp;
                    g.DrawLine(pen, m_fCentreOffset, fAveragePortY, 0.0f, fAveragePortY);

                    System.String avStringPort = "Av: ";
                    avStringPort += String.Format("{0:F2}", m_bAngleConstraint ? AngleUtil.ContainAngle0To360(m_fAverageValuePort) : m_fAverageValuePort);
                    g.DrawString(avStringPort, font, System.Drawing.Brushes.LightGray, 10.0f, fAveragePortY);
                }

                if (fStbdSize > 0.0f)
                {
                    float fAverageStbdProp = (float)((m_fAverageValueStbd - m_fMinValue) / (m_fMaxValue - m_fMinValue));
                    float fAverageStbdY = (float)fBottomOfBuckets + (fTopOfBuckets - fBottomOfBuckets) * fAverageStbdProp;
                    g.DrawLine(pen, m_fCentreOffset, fAverageStbdY, (float)(HistogramSurface.Right - HistogramSurface.Left), fAverageStbdY);

                    System.String avStringStbd = "Av: ";
                    avStringStbd += String.Format("{0:F2}", m_bAngleConstraint ? AngleUtil.ContainAngle0To360(m_fAverageValueStbd) : m_fAverageValueStbd);
                    System.Drawing.SizeF sizeOnScreenAvStbd = g.MeasureString(avStringStbd, font);
                    g.DrawString(avStringStbd, font, System.Drawing.Brushes.LightGray, (float)(HistogramSurface.Right - HistogramSurface.Left) - (10.0f + sizeOnScreenAvStbd.Width), fAverageStbdY);
                }
            }
        }

        protected override void OnTimeHovered(object sender, double fTime)
        {
            int type = NMEACruncher.GetIndexOfDataType(m_CurrentSelectedType);
            if (type < 0)
            {
                return;
            }

            int iDataIndex = m_Data.GetIndexForTime(fTime);
            if (iDataIndex >= 0)
            {
                m_bHasHoveredValue = true;
                m_fHoveredValue = m_Data.GetDataAtIndex(iDataIndex, type);
                if ((type == (int)NMEACruncher.DataTypes.AWA) && (type == (int)NMEACruncher.DataTypes.TWA))
                {
                    m_fHoveredValue = Math.Abs(m_fHoveredValue);
                }
                m_bHoverOnPort = m_Data.GetDataAtIndex(iDataIndex, NMEACruncher.DataTypes.AWA) < 0.0;

                //Force a repaint
                //TODO: could probably delay this to an update tick
                HistogramSurface.Refresh();
            }
        }

        private void Histogram_Paint(object sender, PaintEventArgs e)
        {
            // Create a local version of the graphics object for the PictureBox.
            Graphics g = e.Graphics;

            e.Graphics.DrawImage(m_SurfaceForLines, 0, 0);

            //Draw the overlay information
            if (m_bHasHoveredValue && (m_fMaxValue > (m_fMinValue+1.0)))
            {
                float fLeft, fRight;
                if (m_bHoverOnPort)
                {
                    fLeft = HistogramSurface.Left;
                    fRight = m_fCentreOffset;
                }
                else 
                {
                    fLeft = m_fCentreOffset;
                    fRight = HistogramSurface.Right;
                }

                double useValue = m_fHoveredValue;
                if (m_bAngleConstraint)
                {
                    while (useValue - m_fAnchor > 180.0f) useValue -= 360.0f;
                    while (useValue - m_fAnchor < -180.0f) useValue += 360.0f;
                }

                double fHoveredProp = (useValue - m_fMinValue) / (m_fMaxValue - m_fMinValue);
                float fHoveredY = (float)(m_fBottomOfBuckets + (m_fTopOfBuckets - m_fBottomOfBuckets) * fHoveredProp);
                System.Drawing.Pen pen = new System.Drawing.Pen(System.Drawing.Color.LightSteelBlue, 1);
                g.DrawLine(pen, fLeft, fHoveredY, fRight, fHoveredY);
            }
        }

        protected override void OnTimeRangeSelected(double fTimeA, double fTimeB)
        {
            m_fUserSelectedStartTime = fTimeA;
            m_fUserSelectedEndTime = fTimeB;
            if (followSelectionToolStripMenuItem.Checked)
            {
                m_fStartTime = fTimeA;
                m_fEndTime = fTimeB;

                RebuildHistogram(m_fStartTime, m_fEndTime);
            }
        }

        private bool GetData(double fStartTime, double fEndTime)
        {
            //Go through current selection and count into the buckets
            int type = NMEACruncher.GetIndexOfDataType(m_CurrentSelectedType);
            if (type < 0)
            {
                return false;
            }
            bool bAbsOnly = false;
            bool bMinimumAngle = false;
            m_bAngleConstraint = false;

            if (NMEACruncher.GetDataRangeForType(type) == NMEACruncher.DataRangeTypes.Direction)
            {
                //case (int)NMEACruncher.DataTypes.EstCurrentDir:
                //case (int)NMEACruncher.DataTypes.GPSHeading:
                //case (int)NMEACruncher.DataTypes.BoatHeading:
                //case (int)NMEACruncher.DataTypes.TWD:
                bMinimumAngle = true;
                m_bAngleConstraint = true;
            }
            else
            {
                switch (type)
                {
                    case (int)NMEACruncher.DataTypes.AWA:
                        bAbsOnly = true;
                        break;
                    case (int)NMEACruncher.DataTypes.TWA:
                        bAbsOnly = true;
                        bMinimumAngle = true;   //TODO: Why this and not 
                        break;
                }
            }

            fStartTime = fEndTime > fStartTime ? fStartTime : 0.0f;
            fEndTime = fEndTime > fStartTime ? fEndTime : m_Data.m_fTimeOfLastEntry;

            int iFirstSample = m_Data.GetIndexForTime(fStartTime);
            if (iFirstSample < 0)
                return false;

            int iLastSample = m_Data.GetIndexForTime(fEndTime);
            if (iLastSample < 0)
                return false;

            //Find range to split buckets over (even split for port and starboard tack data)
            m_fAnchor = bAbsOnly ? System.Math.Abs(m_Data.GetDataAtIndex(iFirstSample, type)) : m_Data.GetDataAtIndex(iFirstSample, type);
            double fMin = m_fAnchor;
            double fMax = m_fAnchor;

            //Store
            m_fDataStartTime = m_Data.GetDataAtIndex(iFirstSample, 0);
            m_fDataEndTime = m_Data.GetDataAtIndex(iLastSample, 0);

            //ReAnchor to the average, 
            double fAverage = 0.0;
            if (bMinimumAngle)
            {
                int tmpCount = 0;
                for (int i = iFirstSample + 1; i < iLastSample; i++)
                {
                    double fValue = (bAbsOnly ? System.Math.Abs(m_Data.GetDataAtIndex(i, type)) : m_Data.GetDataAtIndex(i, type));
                    while (fValue - m_fAnchor > 180.0f) fValue -= 360.0f;
                    while (fValue - m_fAnchor < -180.0f) fValue += 360.0f;
                    fAverage += fValue;
                    tmpCount++;
                }
                fAverage *= 1.0f / (double)(iLastSample - (iFirstSample + 1));
                m_fAnchor = fAverage;
            }

            for (int i = iFirstSample + 1; i < iLastSample; i++)
            {
                double fValue = (bAbsOnly ? System.Math.Abs(m_Data.GetDataAtIndex(i, type)) : m_Data.GetDataAtIndex(i, type));
                if (bMinimumAngle)
                {
                    while (fValue - m_fAnchor > 180.0f) fValue -= 360.0f;
                    while (fValue - m_fAnchor < -180.0f) fValue += 360.0f;
                }
                if (fValue < fMin)
                {
                    fMin = fValue;
                }
                if (fValue > fMax)
                {
                    fMax = fValue;
                }
            }

            m_fMinValue = fMin;
            m_fMaxValue = fMax;

            double fRange = fMax - fMin;
            //if (fRange <= 0.0f)
            //{
            //    Console.WriteLine("**Histogram: No range to split into buckets**");
            //    return false;
            //}

            double fAvPort = 0.0f;
            double fAvStbd = 0.0f;
            int iCountPort = 0;
            int iCountStbd = 0;
            double fScalar = fRange > 0.0? ((double)(m_iCount + 1)) / fRange : 0.0;
            for (int i = iFirstSample; i < iLastSample; i++)
            {
                double fValue = (bAbsOnly ? System.Math.Abs(m_Data.GetDataAtIndex(i, type)) : m_Data.GetDataAtIndex(i, type));
                if (bMinimumAngle)
                {
                    while (fValue - m_fAnchor > 180.0f) fValue -= 360.0f;
                    while (fValue - m_fAnchor < -180.0f) fValue += 360.0f;
                }
                bool bPort = m_Data.GetDataAtIndex(i, NMEACruncher.DataTypes.AWA) < 0.0;
                int iBucket = (int)((float)(fValue - fMin) * fScalar);
                if (iBucket < 0) iBucket = 0;
                if (iBucket > m_iCount - 1) iBucket = m_iCount - 1;
                if (bPort)
                {
                    m_PortTackBuckets[iBucket]++;
                    fAvPort += fValue;
                    iCountPort++;
                }
                else
                {
                    m_StbdTackBuckets[iBucket]++;
                    fAvStbd += fValue;
                    iCountStbd++;
                }
            }

            m_fAverageValuePort = iCountPort > 0 ? fAvPort / (float)iCountPort : 0.0f;
            m_fAverageValueStbd = iCountStbd > 0 ? fAvStbd / (float)iCountStbd : 0.0f;
            return true;
        }


        public void RebuildHistogram(double fStartTime, double fEndTime)
        {
            m_PortTackBuckets = new int[m_iCount];
            m_StbdTackBuckets = new int[m_iCount];

            for (int i = 0; i < m_iCount; i++)
            {
                m_PortTackBuckets[i] = m_StbdTackBuckets[i] = 0;
            }

            //Fill data...
            GetData(fStartTime, fEndTime);

            //Set title to something useful
            Text = m_CurrentSelectedType;
            Text += "(";
            if (m_fDataEndTime > m_fDataStartTime)
            {
                Text += (int)m_fDataStartTime;
                Text += "->";
                Text += (int)m_fDataEndTime;
            }
            else
            {
                Text += "*";
            }
            Text += ")";

            
            //Trigger a refresh
            DrawGraph();

            HistogramSurface.Refresh();
        }

        private void toCurrentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            double delta = m_fEndTime - m_fStartTime;
            if (delta > 0.0)
            {
                m_fEndTime = m_Data.GetEndTime();
                m_fStartTime = Math.Max(0.0, m_fEndTime - delta);
                RebuildHistogram(m_fStartTime, m_fEndTime);
            }
        }

        private void followCurrentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            followCurrentToolStripMenuItem.Checked = !followCurrentToolStripMenuItem.Checked;
            if (followCurrentToolStripMenuItem.Checked)
            {
                followSelectionToolStripMenuItem.Checked = false;
            }
        }

        private void toolStripNumBuckets(object sender, EventArgs e)
        {
            m_iCount = Convert.ToInt32(sender.ToString());
            RebuildHistogram(m_fStartTime, m_fEndTime);
        }

        private void toolStripTimeTracked(object sender, EventArgs e)
        {
            m_fSecondsTracked = (double)Convert.ToInt32(sender.ToString());
            if (followCurrentToolStripMenuItem.Checked)
            {
                RebuildToCurrentTimeRange();
            }
        }

        private void historyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            historyToolStripMenuItem.Checked = !historyToolStripMenuItem.Checked;
            RebuildHistogram(m_fStartTime, m_fEndTime);
        }

        private void toSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            followCurrentToolStripMenuItem.Checked = false;
            if (m_fUserSelectedEndTime != m_fUserSelectedStartTime)
            {
                m_fStartTime = m_fUserSelectedStartTime;
                m_fEndTime = m_fUserSelectedEndTime;
                RebuildHistogram(m_fStartTime, m_fEndTime);
            }
        }

        private void followSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            followSelectionToolStripMenuItem.Checked = !followSelectionToolStripMenuItem.Checked;
            if (followSelectionToolStripMenuItem.Checked)
            {
                followCurrentToolStripMenuItem.Checked = false;

                if (m_fUserSelectedEndTime != m_fUserSelectedStartTime)
                {
                    m_fStartTime = m_fUserSelectedStartTime;
                    m_fEndTime = m_fUserSelectedEndTime;
                    RebuildHistogram(m_fStartTime, m_fEndTime);
                }
            }
        }

    }
}
