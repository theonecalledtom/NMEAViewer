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
        NMEACruncher m_Data;

        struct WaypointData
        {
            public double latitude;
            public double longitude;
            public DateTime timeUtc;
            public double timeSinceLast;
            public double timeSinceStart;
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

            m_Data = data;
        }

        protected override void OnDataReplaced(NMEACruncher newData)
        {
            m_Data = newData;
        }

        NMEAViewer.NMEACruncher.SOutputData GetCruncherDataFromWaypointData(WaypointData indata)
        {
            var outdata = new NMEAViewer.NMEACruncher.SOutputData();
            outdata.SetValue(NMEAViewer.NMEACruncher.DataTypes.GPSLat, indata.latitude);
            outdata.SetValue(NMEAViewer.NMEACruncher.DataTypes.GPSLong, indata.longitude);
            outdata.SetValue(NMEAViewer.NMEACruncher.DataTypes.Time, indata.timeSinceStart);
            if (indata.timeSinceStart > 0.0)
            {
                outdata.SetValue(NMEAViewer.NMEACruncher.DataTypes.GPSHeading, indata.angleSinceLast);
                outdata.SetValue(NMEAViewer.NMEACruncher.DataTypes.GPSSOG, indata.speedSinceLast);
            }
            return outdata;
        }

        private void GPXLoader_Load(object sender, EventArgs e)
        {

        }

        public void LoadGPXData(string fileName)
        {
            m_GPXFileName = fileName;
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

                int iCounter = 0;
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
                                ref var lastWaypoint = ref Waypoints[iCounter - 1];

                                var span = newWaypoint.timeUtc - lastWaypoint.timeUtc;
                                newWaypoint.timeSinceLast = span.TotalSeconds;

                                var spanSinceStart = newWaypoint.timeUtc - Waypoints[0].timeUtc;
                                newWaypoint.timeSinceStart = spanSinceStart.TotalSeconds;

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
            }
        }

        void ConvertToData()
        {
            m_Data.StartNewData();
            foreach (var wpd in Waypoints)
            {
                m_Data.AddNewData(GetCruncherDataFromWaypointData(wpd));
            }
            m_Data.EndNewData();
        }

        private void PathOffset_Scroll(object sender, EventArgs e)
        {
            //We're scrubbing the timeline back and forth
        }

        private void inject_Click(object sender, EventArgs e)
        {
            ConvertToData();

            //Get windows to refresh their data
            DockableDrawable.BroadcastDataReplaced(m_Data);
        }

        private void LoadGPX_Click(object sender, EventArgs e)
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
                LoadGPXData(openGPXDialog.FileName);
            }
        }
    }
}
