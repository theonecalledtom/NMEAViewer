using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GMap.NET.WindowsForms;

namespace NMEAViewer
{
    public partial class GPXLoader : DockableDrawable
    {
        private string m_GPXFileName;
        public static GPXLoader Instance;
        private class SerializedData : DockableDrawable.SerializedDataBase
        {
            public string m_GPXFileName;
            
            public SerializedData(DockableDrawable parent)
                : base(parent) { }
        };

        public override DockableDrawable.SerializedDataBase CreateSerializedData()
        {
            SerializedData data = new SerializedData(this);
            data.m_GPXFileName = m_GPXFileName;
            return data;
        }

        public override void InitFromSerializedData(SerializedDataBase data_base)
        {
            base.InitFromSerializedData(data_base);
            SerializedData data = (SerializedData)data_base;
            m_GPXFileName = data.m_GPXFileName;
        }

        public override void PostInitFromSerializedData(SerializedDataBase data_base)
        {
            SerializedData data = (SerializedData)data_base;
        }
        public GPXLoader(NMEACruncher data)
        {
            InitializeComponent();
        }

        private void GPXLoader_Load(object sender, EventArgs e)
        {

        }
    }
}
