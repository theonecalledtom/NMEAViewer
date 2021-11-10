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
        private NMEACruncher data;
        private PolarData polarData;
        private float TWS = 6.0f;       //Hacked value for testing...

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
            base.InitFromSerializedData(data_base);
            SerializedData data = (SerializedData)data_base;
        }

        public override void PostInitFromSerializedData(SerializedDataBase data_base)
        {
            SerializedData data = (SerializedData)data_base;
        }

        public PolarDiagramWindow(NMEACruncher data, PolarData polarData)
        {
            InitializeComponent();

            this.data = data;
            this.polarData = polarData;

            PolarDrawArea.Paint += PolarDrawArea_Paint;
            PolarDrawArea.Resize += PolarDrawArea_Resize;
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

        private void PolarDrawArea_Paint(object sender, PaintEventArgs e)
        {
            Pen overlayPen = new Pen(new SolidBrush(Color.Gray));
            overlayPen.Width = 1.0f;

            float width = (float)PolarDrawArea.ClientSize.Width;
            float height = (float)PolarDrawArea.ClientSize.Height;
            float mid_w = width / 2.0f;
            float mid_y = height / 2.0f;
            float length = (float)Math.Sqrt(mid_w * mid_w + mid_y * mid_y);
            float last_y = 0.0f;
            float last_x = 0.0f;
            float MaxLen = Math.Min(mid_w, mid_y);
            float MaxSpd = 15.0f;
            for (double angle = 35.0; angle <= 180.0 ; angle += 10.0)
            {
                var sin = (float)Math.Sin(AngleUtil.DegToRad * angle);
                var cos = (float)Math.Cos(AngleUtil.DegToRad * angle);

                float xoffset = sin * length;
                float yoffset = -cos * length;

                e.Graphics.DrawLine(overlayPen, mid_w, mid_y, mid_w + xoffset, mid_y + yoffset);
                e.Graphics.DrawLine(overlayPen, mid_w, mid_y, mid_w - xoffset, mid_y + yoffset);

                float spd = (float)polarData.GetBestPolarSpeed(TWS, angle);
                float new_x = sin * spd * MaxLen / MaxSpd;
                float new_y = -cos * spd * MaxLen / MaxSpd;

                e.Graphics.DrawLine(overlayPen, mid_w + last_x, mid_y + last_y, mid_w + new_x, mid_y + new_y);
                e.Graphics.DrawLine(overlayPen, mid_w - last_x, mid_y + last_y, mid_w - new_x, mid_y + new_y);

                last_x = new_x;
                last_y = new_y;
            }
        }
    }
}
