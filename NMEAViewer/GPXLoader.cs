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

        struct WaypointData
        {
            public double latitude;
            public double longitude;
            public DateTime timeUtc;
            public double timeSinceLast;
            public double angleSinceLast;
            public double distanceSinceLast;
            public double speedSinceLast;
        };

        WaypointData[] Waypoints;

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
                    int wayPointCount = 0;
                    var data = GpsData.Parse(stream);
                    for (int itrack = 0; itrack < data.Tracks.Count; itrack++)
                    {
                        var track = data.Tracks[itrack];

                        for (int isegment = 0; isegment < track.Segments.Count; isegment++)
                        {
                            var segment = track.Segments[isegment];
                            wayPointCount += segment.Waypoints.Count;
                        }
                    }

                    Waypoints = new WaypointData[wayPointCount];

                    int iCounter=0;
                    for (int itrack = 0; itrack < data.Tracks.Count; itrack++)
                    {
                        var track = data.Tracks[itrack];

                        for (int isegment = 0; isegment < track.Segments.Count; isegment++)
                        {
                            var segment = track.Segments[isegment];
                            for (int iwaypoint = 0; iwaypoint < segment.Waypoints.Count; iwaypoint++)
                            {
                                var waypointLoaded = segment.Waypoints[iwaypoint];
                                ref var newWaypoint = ref Waypoints[iCounter];
         
                                //Cache our waypoint information
                                newWaypoint.latitude = waypointLoaded.Coordinate.Latitude;
                                newWaypoint.longitude = waypointLoaded.Coordinate.Longitude;
                                newWaypoint.timeUtc = (DateTime)waypointLoaded.TimeUtc;
                                if (iCounter > 0)
                                {
                                    ref var lastWaypoint = ref Waypoints[iCounter-1];
                                    var span = newWaypoint.timeUtc - lastWaypoint.timeUtc;
                                    newWaypoint.timeSinceLast = span.TotalSeconds;
                                    newWaypoint.angleSinceLast = CoordinateUtils.HeadingTo(
                                             lastWaypoint.longitude,
                                             lastWaypoint.latitude,
                                             newWaypoint.longitude,
                                             newWaypoint.latitude
                                         );
                                    newWaypoint.distanceSinceLast = CoordinateUtils.DistanceBetween(
                                             lastWaypoint.longitude,
                                             lastWaypoint.latitude,
                                             newWaypoint.longitude,
                                             newWaypoint.latitude
                                         );
                                    
                                    //Calculate a rought and ready speed
                                    var spdMetresPerSecond = newWaypoint.distanceSinceLast / newWaypoint.timeSinceLast;
                                    newWaypoint.speedSinceLast = spdMetresPerSecond * CoordinateUtils.HoursToSeconds / CoordinateUtils.NMtoM;
                                }
                                iCounter++;
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
