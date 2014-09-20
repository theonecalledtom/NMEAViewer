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
    public partial class VideoWindow : DockableDrawable
    {
        const string kUiMode = "full";

        [JsonObject(MemberSerialization.OptOut)]
        private class SerializedData : DockableDrawable.SerializedDataBase
        {
            public SerializedData(DockableDrawable parent)
                : base(parent) { }

            public string m_MovieURL;
            public string m_WindowName;
            public double m_fCurrentPosition = 0.0;
            public double m_fFirstVideoSyncPoint = -1;
            public double m_fFirstExternalSyncPoint = -1;
            public double m_fSecondVideoSyncPoint = -1;
            public double m_fSecondExternalSyncPoint = -1;
        };

        public override DockableDrawable.SerializedDataBase CreateSerializedData()
        {
            SerializedData data = new SerializedData(this);
            data.m_WindowName = Text;
            data.m_MovieURL = WMPPlayer.URL;
            data.m_fCurrentPosition = WMPPlayer.Ctlcontrols.currentPosition;
            data.m_fFirstVideoSyncPoint = m_fFirstVideoSyncPoint;
            data.m_fFirstExternalSyncPoint = m_fFirstExternalSyncPoint;
            data.m_fSecondVideoSyncPoint = m_fSecondVideoSyncPoint;
            data.m_fSecondExternalSyncPoint = m_fSecondExternalSyncPoint;
            return data;
        }

        public override void InitFromSerializedData(SerializedDataBase data_base)
        {
            SerializedData data = (SerializedData)data_base;
            WMPPlayer.URL = data.m_MovieURL;
            Text = data.m_WindowName;
            WMPPlayer_Init();
            WMPPlayer.Ctlcontrols.currentPosition = data.m_fCurrentPosition;
            m_fFirstVideoSyncPoint = data.m_fFirstVideoSyncPoint;
            m_fFirstExternalSyncPoint = data.m_fFirstExternalSyncPoint;
            m_fSecondVideoSyncPoint = data.m_fSecondVideoSyncPoint;
            m_fSecondExternalSyncPoint = data.m_fSecondExternalSyncPoint;
            m_GraphOverlay.SetStartAndEnd(m_fFirstExternalSyncPoint, m_fSecondExternalSyncPoint);
            m_GraphOverlay.SetName(data.m_WindowName);
            //Force something on screen
            ((WMPLib.IWMPControls2)WMPPlayer.Ctlcontrols).step(1);
        }

        public class VideoGraphOverlay : TimeBasedGraph.GraphOverlay
        {
            double m_fStart = -1.0;
            double m_fEnd = -1.0;
            public VideoGraphOverlay(string overLayname)
            {
                OverlayName = overLayname;
            }

            public void SetName(string name)
            {
                OverlayName = name;
            }

            public void SetStartAndEnd(double fStart, double fEnd)
            {
                m_fStart = fStart;
                m_fEnd = fEnd;
            }

            public override void Draw(Graphics g, double fLeftTime, double fRightTime)
            {
                if (m_fStart < 0.0 || m_fEnd < 0.0)
                    return;

                double height = (double)g.Clip.GetBounds(g).Height;
                double width = (double)g.Clip.GetBounds(g).Width;

                double fDY = height * 0.7 ;
                double fY = fDY;
                //if (fDY > 15)
                //{
                //    fDY = 15;
                //}
                Pen outlinePen = new Pen(new SolidBrush(Color.CadetBlue));
                outlinePen.Width = 3;
                System.Drawing.Font legFont = new System.Drawing.Font("Arial", 10);

                if ((m_fEnd > m_fStart) && ((m_fStart < fRightTime) && (m_fEnd > fLeftTime)))
                {
                    double startX = width * (m_fStart - fLeftTime) / (fRightTime - fLeftTime);
                    double endX = width * (m_fEnd - fLeftTime) / (fRightTime - fLeftTime);

                    g.DrawString(OverlayName, legFont, System.Drawing.Brushes.CadetBlue, (float)(startX + 3.0), (float)(fY + 3.0));
                    g.DrawLine(outlinePen, (float)startX, (float)fY, (float)endX, (float)fY);
                    g.DrawLine(outlinePen, (float)startX, (float)fY, (float)startX, (float)(fY + 10.0));
                    g.DrawLine(outlinePen, (float)endX, (float)fY, (float)endX, (float)(fY + 10.0));
                }
            }
        }

        VideoGraphOverlay m_GraphOverlay;
        double m_fFirstVideoSyncPoint = -1;
        double m_fFirstExternalSyncPoint = -1;
        double m_fSecondVideoSyncPoint = -1;
        double m_fSecondExternalSyncPoint = -1;
        double m_fCurrentSelectedTime = -1;

        public VideoWindow()
        {
            InitializeComponent();
            WMPPlayer_Init();
            m_GraphOverlay = new VideoGraphOverlay("NoFile");
        }
        
        void WMPPlayer_Init()
        {
            WMPPlayer.enableContextMenu = false;
            WMPPlayer.uiMode = kUiMode;
            WMPPlayer.MouseDownEvent += WMPPlayer_MouseDownEvent;
        }

        void WMPPlayer_MouseDownEvent(object sender, AxWMPLib._WMPOCXEvents_MouseDownEvent e)
        {
            if (e.nButton == 2)
            {
                contextMenuStrip1.Show(WMPPlayer, e.fX + WMPPlayer.Left, e.fY + WMPPlayer.Top);
            }
        }

        public VideoWindow(System.String nameOfWindow, System.String nameOfMovie)
        {
            InitializeComponent();
            WMPPlayer_Init();
            WMPPlayer.URL = nameOfMovie;
            Text = nameOfWindow;
            m_GraphOverlay = new VideoGraphOverlay(nameOfWindow);
        }

        protected override void OnTimeSelected(object sender, double fTime)
        {
            m_fCurrentSelectedTime = fTime;
            if (WMPPlayer.currentMedia == null)
                return;

            if (m_fSecondExternalSyncPoint > -1.0 && m_fFirstExternalSyncPoint > -1.0 && m_fSecondExternalSyncPoint > m_fFirstExternalSyncPoint)
            {
                if (m_fSecondVideoSyncPoint > m_fFirstVideoSyncPoint)
                {
                    double fExternalProp = (m_fCurrentSelectedTime - m_fFirstExternalSyncPoint) / (m_fSecondExternalSyncPoint - m_fFirstExternalSyncPoint);
                    double fMediaTime = (fExternalProp * (m_fSecondVideoSyncPoint - m_fFirstVideoSyncPoint)) + m_fFirstVideoSyncPoint;
                    if (fMediaTime < 0.0)
                    {
                        fMediaTime = 0.0;
                    }
                    else if (fMediaTime > WMPPlayer.currentMedia.duration)
                    {
                        fMediaTime = WMPPlayer.currentMedia.duration;
                    }

                    //Push through to media player
                    WMPPlayer.Ctlcontrols.currentPosition = fMediaTime;
                    if (WMPPlayer.Ctlcontrols.currentPosition < WMPPlayer.currentMedia.duration)
                    {
                        if (WMPPlayer.playState != WMPLib.WMPPlayState.wmppsPlaying)
                        {
                            //Pause it at least
                            ((WMPLib.IWMPControls2)WMPPlayer.Ctlcontrols).pause();

                            //Try again?
                            WMPPlayer.Ctlcontrols.currentPosition = fMediaTime;

                            //This forces something visible on screen
                            ((WMPLib.IWMPControls2)WMPPlayer.Ctlcontrols).step(1);
                        }
                    }
                    WMPPlayer.Refresh();
                }
            }
            //else if (WMPPlayer.Ctlcontrols != null)
            //{
            //    WMPPlayer.Ctlcontrols.currentPosition = fTime;
            //    if ((WMPPlayer.currentMedia!=null) && (WMPPlayer.Ctlcontrols.currentPosition < WMPPlayer.currentMedia.duration))
            //    {
            //        if (WMPPlayer.playState != WMPLib.WMPPlayState.wmppsPlaying)
            //        {
            //            //This forces something visible on screen
            //            ((WMPLib.IWMPControls2)WMPPlayer.Ctlcontrols).step(1);
            //        }
            //    }
            //    WMPPlayer.Refresh();
            //}
        }

        void SetFirstSyncPoint(double fExternalRecordingTime)
        {
            m_fFirstExternalSyncPoint = fExternalRecordingTime;
            m_fFirstVideoSyncPoint = WMPPlayer.Ctlcontrols.currentPosition;

            m_GraphOverlay.SetStartAndEnd(m_fFirstExternalSyncPoint, m_fSecondVideoSyncPoint);
        }

        void SetSecondSyncPoint(double fExternalRecordingTime)
        {
            m_fSecondExternalSyncPoint = fExternalRecordingTime;
            m_fSecondVideoSyncPoint = WMPPlayer.Ctlcontrols.currentPosition;

            m_GraphOverlay.SetStartAndEnd(m_fFirstExternalSyncPoint, m_fSecondVideoSyncPoint);
        }

        bool SetTimeWithinSyncArea(double fExternalTime)
        {
            if (fExternalTime >= m_fFirstExternalSyncPoint && fExternalTime <= m_fSecondExternalSyncPoint)
            {
                double fProp = (fExternalTime - m_fFirstExternalSyncPoint) / (m_fSecondExternalSyncPoint - m_fFirstExternalSyncPoint);
                double fPosition = m_fFirstVideoSyncPoint + fProp * (m_fSecondVideoSyncPoint - m_fFirstVideoSyncPoint);
                if (fPosition > 0.0f && fPosition < WMPPlayer.currentMedia.duration)
                {
                    WMPPlayer.Ctlcontrols.currentPosition = fPosition;
                    return true;
                }
            }
            return false;
        }

        private void setFirstSyncPointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_fCurrentSelectedTime >= 0.0)
            {
                SetFirstSyncPoint(m_fCurrentSelectedTime);
            }
        }

        private void setSecondSyncPointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_fCurrentSelectedTime >= 0.0)
            {
                SetSecondSyncPoint(m_fCurrentSelectedTime);
            }
        }
    }
}
