using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Geo.Gps;
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
            //Pick a file
            openGPXDialog.Title = "Open GPX file";
            if (!string.IsNullOrEmpty(m_GPXFileName))
            {
                openGPXDialog.InitialDirectory = System.IO.Path.GetDirectoryName(m_GPXFileName);
                openGPXDialog.FileName = m_GPXFileName;
            }
            if (openGPXDialog.ShowDialog() == DialogResult.OK)
            {
                m_GPXFileName = openGPXDialog.FileName;
                using (var stream = new FileStream(m_GPXFileName, FileMode.Open))
                {
                    var data = GpsData.Parse(stream);
                    for (int itrack = 0; itrack < data.Tracks.Count; itrack++)
                    {
                        var track = data.Tracks[itrack];

                        for (int isegment = 0; isegment < track.Segments.Count; isegment++)
                        {
                            var segment = track.Segments[isegment];
                            for (int iwaypoint = 0; iwaypoint < segment.Waypoints.Count; iwaypoint++)
                            {
                                var waypoint = segment.Waypoints[iwaypoint];

                                //now what :)
                            }
                        }
                    }
                    //Assert.That(data, Is.EqualTo(null));
                }
            }
        }

        private void PathOffset_Scroll(object sender, EventArgs e)
        {
            //We're scrubbing the timeline back and forth
        }
    }
}
