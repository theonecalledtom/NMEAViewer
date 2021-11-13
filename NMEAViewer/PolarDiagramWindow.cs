using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NMEAViewer
{
    public partial class PolarDiagramWindow : DockableDrawable
    {
        private SetWindInputDialog windDialog;
        private NMEACruncher m_Data;
        private PolarData m_PolarData;
        private float TWS = 7.0f;       //Hacked value for testing...
        private float TWA = 60.0f;      //Hacked value for testing...
        private float MaxSpd = 10.0f;
        private string m_PolarFileName;

        private class SerializedData : DockableDrawable.SerializedDataBase
        {
            public string m_PolarFileName;
            public bool m_LiveUpdate;
            public SerializedData(DockableDrawable parent)
                : base(parent) { }
        };

        public override DockableDrawable.SerializedDataBase CreateSerializedData()
        {
            SerializedData data = new SerializedData(this);
            data.m_PolarFileName = m_PolarFileName;
            data.m_LiveUpdate = liveUpdateToolStripMenuItem.Checked;
            return data;
        }

        public override void InitFromSerializedData(SerializedDataBase data_base)
        {
            base.InitFromSerializedData(data_base);
            SerializedData data = (SerializedData)data_base;
            m_PolarFileName = data.m_PolarFileName;
            liveUpdateToolStripMenuItem.Checked = data.m_LiveUpdate;
            if (!string.IsNullOrEmpty(data.m_PolarFileName))
            {
                setPolarFile(data.m_PolarFileName);
            }
        }

        public override void PostInitFromSerializedData(SerializedDataBase data_base)
        {
            SerializedData data = (SerializedData)data_base;
        }

        public PolarDiagramWindow(NMEACruncher data, PolarData polarData)
        {
            InitializeComponent();
            m_Data = data;
            m_PolarData = polarData;

            PolarDrawArea.Paint += PolarDrawArea_Paint;
            PolarDrawArea.Resize += PolarDrawArea_Resize;
        }

        protected override void OnDataReplaced(NMEACruncher newData)
        {
            m_Data = newData;
            PolarDrawArea.Refresh();
        }

        override protected void OnDataAppended()
        {
            var mostRecentTime = m_Data.GetEndTime();
            var index = m_Data.GetIndexForTime(mostRecentTime);
            if ((index >=0) && liveUpdateToolStripMenuItem.Checked)
            {
                bool bNeedsRefresh = false;
                if (m_Data.HasDataAtIndex(index, NMEACruncher.DataTypes.TWS))
                {
                    TWS = (float)m_Data.GetDataAtIndex(index, NMEACruncher.DataTypes.TWS);
                    bNeedsRefresh = true;
                }
                if (m_Data.HasDataAtIndex(index, NMEACruncher.DataTypes.TWA))
                {
                    TWA = (float)m_Data.GetDataAtIndex(index, NMEACruncher.DataTypes.TWA);
                    bNeedsRefresh = true;
                }
                if (bNeedsRefresh)
                {
                    PolarDrawArea.Refresh();
                }
            }
        }

        private void PolarDrawArea_Resize(object sender, EventArgs e)
        {
            PolarDrawArea.Refresh();
        }

        private void SetWindSpeed(float tws)
        {
            TWS = tws;
            PolarDrawArea.Refresh();
        }

        private void SetWindAngle(float twa)
        {
            TWA = twa;
            PolarDrawArea.Refresh();
        }

        void DrawText(PaintEventArgs e)
        {
            System.Drawing.Font font = new System.Drawing.Font("Arial", 10);
            System.Drawing.SolidBrush myBrush0 = new System.Drawing.SolidBrush(Color.White);
            System.Drawing.SolidBrush myBrush1 = new System.Drawing.SolidBrush(Color.Yellow);
            float line = 5.0f;
            float spacing = 13.0f;
            if (!string.IsNullOrEmpty(m_PolarFileName))
            {
                e.Graphics.DrawString(m_PolarFileName, font, myBrush1, 5.0f, line); line += spacing;
            }
            e.Graphics.DrawString("TWS: "+ TWS.ToString(), font, myBrush0, 5.0f, line); line += spacing;
            e.Graphics.DrawString("TWA: " + TWA.ToString(), font, myBrush0, 5.0f, line); line += spacing;
            if (liveUpdateToolStripMenuItem.Checked)
            {
                e.Graphics.DrawString("Live update is on", font, myBrush0, 5.0f, line); line += spacing;
            }
        }

        private void DrawWindSpeedRing(float WindSpd, Pen overlayPen, PaintEventArgs e)
        {
            //Draw polar compass rose
            float width = (float)PolarDrawArea.ClientSize.Width;
            float height = (float)PolarDrawArea.ClientSize.Height;
            float mid_w = width / 2.0f;
            float mid_y = height / 2.0f;
            float length = (float)Math.Sqrt(mid_w * mid_w + mid_y * mid_y);
            float last_y = 0.0f;
            float last_x = 0.0f;
            float MaxLen = Math.Min(mid_w, mid_y);

            var data = m_PolarData.GetData(WindSpd);
            int count = data.GetDataCounts();
            for (int i=0; i<count ; i++)
            {
                float angle = (float)data.GetNthAngle(i);
                float spd = (float)data.GetNthBoatSpeed(i);

                var sin = (float)Math.Sin(AngleUtil.DegToRad * angle);
                var cos = (float)Math.Cos(AngleUtil.DegToRad * angle);

                float new_x = sin * spd * MaxLen / MaxSpd;
                float new_y = -cos * spd * MaxLen / MaxSpd;

                e.Graphics.DrawLine(overlayPen, mid_w + last_x, mid_y + last_y, mid_w + new_x, mid_y + new_y);
                e.Graphics.DrawLine(overlayPen, mid_w - last_x, mid_y + last_y, mid_w - new_x, mid_y + new_y);

                last_x = new_x;
                last_y = new_y;
            }


            /*  float fBestUpwindAndle = (float)m_PolarData.GetBestUpwindAngle(WindSpd);
                float fBestDownwindAndle = (float)m_PolarData.GetBestDownwindAngle(WindSpd);
                for (double angle = 10.0; angle <= 180.0; angle += 10.0)
                {
                    var sin = (float)Math.Sin(AngleUtil.DegToRad * angle);
                    var cos = (float)Math.Cos(AngleUtil.DegToRad * angle);

                    //float xoffset = sin * length;
                    //float yoffset = -cos * length;

                    //e.Graphics.DrawLine(overlayPen, mid_w, mid_y, mid_w + xoffset, mid_y + yoffset);
                    //e.Graphics.DrawLine(overlayPen, mid_w, mid_y, mid_w - xoffset, mid_y + yoffset);

                    float spd = (float)m_PolarData.GetBestPolarSpeed(WindSpd, angle);
                    float new_x = sin * spd * MaxLen / MaxSpd;
                    float new_y = -cos * spd * MaxLen / MaxSpd;

                    e.Graphics.DrawLine(overlayPen, mid_w + last_x, mid_y + last_y, mid_w + new_x, mid_y + new_y);
                    e.Graphics.DrawLine(overlayPen, mid_w - last_x, mid_y + last_y, mid_w - new_x, mid_y + new_y);

                    last_x = new_x;
                    last_y = new_y;
                }
            */
        }

        void DrawAngle(PaintEventArgs e, Pen p, float angle)
        {
            float width = (float)PolarDrawArea.ClientSize.Width;
            float height = (float)PolarDrawArea.ClientSize.Height;
            float mid_w = width / 2.0f;
            float mid_y = height / 2.0f;

            var sinTWA = (float)Math.Sin(AngleUtil.DegToRad * angle);
            var cosTWA = (float)Math.Cos(AngleUtil.DegToRad * angle);

            float spdTWA = (float)m_PolarData.GetBestPolarSpeed(TWS, angle);

            float MaxLen = Math.Min(mid_w, mid_y);
            float twa_x = sinTWA * spdTWA * MaxLen / MaxSpd;
            float twa_y = -cosTWA * spdTWA * MaxLen / MaxSpd;

            e.Graphics.DrawLine(p, mid_w, mid_y, mid_w + twa_x, mid_y + twa_y);
        }

        private void PolarDrawArea_Paint(object sender, PaintEventArgs e)
        {
            Pen overlayPen = new Pen(new SolidBrush(Color.Gray));
            overlayPen.Width = 1.0f;

            DrawWindSpeedRing(4, overlayPen, e);
            //DrawWindSpeedRing(6, overlayPen, e);
            DrawWindSpeedRing(8, overlayPen, e);
            //DrawWindSpeedRing(10, overlayPen, e);
            DrawWindSpeedRing(12, overlayPen, e);
            //DrawWindSpeedRing(14, overlayPen, e);
            //DrawWindSpeedRing(16, overlayPen, e);
            //DrawWindSpeedRing(18, overlayPen, e);
            DrawWindSpeedRing(20, overlayPen, e);

            Pen twsPen = new Pen(new SolidBrush(Color.Blue));
            twsPen.Width = 2.0f;
            DrawWindSpeedRing(TWS, twsPen, e);

            //Draw polar compass rose
            float width = (float)PolarDrawArea.ClientSize.Width;
            float height = (float)PolarDrawArea.ClientSize.Height;
            float mid_w = width / 2.0f;
            float mid_y = height / 2.0f;
            float length = (float)Math.Sqrt(mid_w * mid_w + mid_y * mid_y);
            float MaxLen = Math.Min(mid_w, mid_y);
            

            //Draw current TWA info
            Pen currentPen = new Pen(new SolidBrush(Color.Yellow));
            currentPen.Width = 2.0f;
            DrawAngle(e, currentPen, TWA);

            Pen bestPen = new Pen(new SolidBrush(Color.Blue));
            bestPen.Width = 1.0f;
            
            var bestUp = m_PolarData.GetBestUpwindAngle(TWS);
            DrawAngle(e, bestPen, (float)bestUp);
            DrawAngle(e, bestPen, (float)-bestUp);

            var bestDown = m_PolarData.GetBestDownwindAngle(TWS);
            DrawAngle(e, bestPen, (float)bestDown);
            DrawAngle(e, bestPen, (float)-bestDown);


            //Draw text information
            DrawText(e);
        }

        private void setInputsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (windDialog == null)
            {
                windDialog = new SetWindInputDialog();
                windDialog.WindSpeedInput.ValueChanged += WindSpeedInput_ValueChanged;
                windDialog.WindAngleInput.ValueChanged += WindAngleInput_ValueChanged;
                windDialog.FormClosed += WindDialog_FormClosed;
                windDialog.WindAngleInput.Value = (decimal)TWA;
                windDialog.WindSpeedInput.Value = (decimal)TWS;
            }

            windDialog.Show();
        }

        private void WindDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            windDialog = null;
        }

        private void WindAngleInput_ValueChanged(object sender, EventArgs e)
        {
            if (windDialog != null)
            {
                TWA = (float)windDialog.WindAngleInput.Value;
                PolarDrawArea.Refresh();
            }
        }

        private void WindSpeedInput_ValueChanged(object sender, EventArgs e)
        {
            if (windDialog != null)
            {
                TWS = (float)windDialog.WindSpeedInput.Value;
                PolarDrawArea.Refresh(); 
            }
        }
        private void setPolarFile(string filename)
        {
            PolarData polarData = new PolarData();
            if (polarData.Load(filename))
            {
                //Make a new data file to avoid changing the main file
                //Not sure about this it might get confusing?
                //Perhaps ask if the user wants to apply the file globally or just to this window?
                m_PolarData = polarData;
                m_Data.SetPolarData(m_PolarData);
                m_PolarFileName = filename;
                PolarDrawArea.Refresh();
            }
        }

        private void loadPolarsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if ((m_AppSettings.PolarDataName != null) && (m_AppSettings.PolarDataName.Length > 0))
            //{
            //    openPolarFile.InitialDirectory = System.IO.Path.GetDirectoryName(m_AppSettings.PolarDataName);
            //}
            if (openPolarFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //m_AppSettings.PolarDataName = openPolarFile.FileName;
                //m_AppSettings.Save();
                setPolarFile(openPolarFile.FileName);
                PolarDrawArea.Refresh();
            }
        }

        private void liveUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            liveUpdateToolStripMenuItem.Checked = !liveUpdateToolStripMenuItem.Checked;

            //Just for the text
            PolarDrawArea.Refresh();
        }
    }
}
