﻿using System;
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
using GMap.NET.WindowsForms.Markers;
using Newtonsoft.Json;

namespace NMEAViewer
{
    public partial class GPXLoader : DockableDrawable
    {
        private string m_GPXFileName;
        public static GPXLoader Instance;
        NMEACruncher m_Data;
        private GMapOverlay m_RoutesOverlay;

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

        [JsonObject(MemberSerialization.OptOut)]
        struct WindData
        {
            public double Time;
            public double TWD;
            public double TWS;
        };

        WaypointData[] Waypoints;
        private GMapRoute m_Route;

        private int m_SelectedWaypoint;

        private class SerializedData : DockableDrawable.SerializedDataBase
        {
            public string m_GPXFileName;
            public WindData[] m_Wind;
            public SerializedData(DockableDrawable parent)
                : base(parent) { }
        };

        public override DockableDrawable.SerializedDataBase CreateSerializedData()
        {
            SerializedData data = new SerializedData(this);
            data.m_GPXFileName = m_GPXFileName;
            LoadGPXData(data.m_GPXFileName);
            data.m_Wind = CreateWindDataFromTable();
            return data;
        }

        public override void InitFromSerializedData(SerializedDataBase data_base)
        {
            base.InitFromSerializedData(data_base);
            SerializedData data = (SerializedData)data_base;
            m_GPXFileName = data.m_GPXFileName;
            CreateTableFromWindData(data.m_Wind);
        }

        public override void PostInitFromSerializedData(SerializedDataBase data_base)
        {
            SerializedData data = (SerializedData)data_base;
        }
        public GPXLoader(NMEACruncher data)
        {
            InitializeComponent();

            m_SelectedWaypoint = -1;
            m_Data = data;

            m_RoutesOverlay = new GMapOverlay("routes");
            Map.Overlays.Add(m_RoutesOverlay);
            Map.MapProvider = GMap.NET.MapProviders.OpenSeaMapHybridProvider.Instance;
            Map.Position = new GMap.NET.PointLatLng(32.0, -117.0);
            Map.MinZoom = 1;
            Map.MaxZoom = 24;
            Map.Zoom = 9;

            m_Route = new GMapRoute("Route Taken");
            m_Route.Stroke.Color = System.Drawing.Color.Blue;
            m_Route.Stroke.Width = 1;
            m_RoutesOverlay.Routes.Add(m_Route);
            //    SetTimerFrequency(0.5);

            //Track changes
            TWDTable.CellValueChanged += TWDTable_CellValueChanged;
        }

        private void TWDTable_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            TWDTable.Sort(TWDTable.Columns[0], ListSortDirection.Ascending);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            //base.OnMouseWheel(e);

            double factor = (e.Delta + 5000.0) / 5000.0;
            if (factor < 0.1)
            {
                factor = 0.1;
            }
            double zoom = Map.Zoom;
            zoom *= factor;
            Map.Zoom = zoom;
        }
        //protected virtual void OnTimer(object sender, EventArgs e) { }

        protected override void OnDataReplaced(NMEACruncher newData)
        {
            m_Data = newData;
        }

        NMEAViewer.NMEACruncher.SOutputData GetCruncherDataFromWaypointData(WaypointData indata, WindData windData)
        {
            var outdata = new NMEAViewer.NMEACruncher.SOutputData();
            outdata.SetValue(NMEAViewer.NMEACruncher.DataTypes.Time, indata.timeSinceStart);
            outdata.SetValue(NMEAViewer.NMEACruncher.DataTypes.GPSLat, indata.latitude);
            outdata.SetValue(NMEAViewer.NMEACruncher.DataTypes.GPSLong, indata.longitude);
            outdata.SetValue(NMEAViewer.NMEACruncher.DataTypes.TWD, windData.TWD);
            outdata.SetValue(NMEAViewer.NMEACruncher.DataTypes.TWS, windData.TWS);

            if (indata.timeSinceStart > 0.0)
            {
                outdata.SetValue(NMEAViewer.NMEACruncher.DataTypes.GPSHeading, indata.angleSinceLast);
                outdata.SetValue(NMEAViewer.NMEACruncher.DataTypes.GPSSOG, indata.speedSinceLast);

                var TWA = AngleUtil.ContainAngleMinus180To180(windData.TWD - indata.angleSinceLast);
                outdata.SetValue(NMEAViewer.NMEACruncher.DataTypes.TWA, TWA);

                //For AWA we'll need to do some math
                //Hmmm, GPS speed is going to be quite random....
                //For solution 1 though we'll use it and see what the data looks like
                var vForward = indata.speedSinceLast + Math.Cos(TWA * AngleUtil.DegToRad) * windData.TWS;

                var vSideToWind = Math.Sin(TWA * AngleUtil.DegToRad) * windData.TWS;

                var AWA = Math.Atan2(vSideToWind, vForward) * AngleUtil.RadToDeg;
                var AWS = Math.Sqrt(vForward * vForward + vSideToWind * vSideToWind);

                outdata.SetValue(NMEAViewer.NMEACruncher.DataTypes.AWA, AWA);
                outdata.SetValue(NMEAViewer.NMEACruncher.DataTypes.AWS, AWS);
            }
            return outdata;
        }

       
        public void AddPointToRoute(double fLong, double fLat, GMapRoute route)
        {
            GMap.NET.PointLatLng newPoint = new GMap.NET.PointLatLng();
            newPoint.Lat = fLat;
            newPoint.Lng = fLong;
            route.Points.Add(newPoint);
            //m_bDirty = true;
        }

        public void LoadGPXData(string fileName)
        {
            m_GPXFileName = fileName;
            using (var stream = new FileStream(m_GPXFileName, FileMode.Open))
            {
                int wayPointCount = 0;
                var data = GpsData.Parse(stream);
                if (data == null)
                {
                    //Output log???
                    return;
                }
                for (int itrack = 0; itrack < data.Tracks.Count; itrack++)
                {
                    var track = data.Tracks[itrack];

                    for (int isegment = 0; isegment < track.Segments.Count; isegment++)
                    {
                        var segment = track.Segments[isegment];
                        wayPointCount += segment.Waypoints.Count;
                    }
                }

                if (wayPointCount == 0)
                {
                    Waypoints = null;
                    m_SelectedWaypoint = -1;
                    m_Route.Clear();
                    m_RoutesOverlay.Markers.Clear();
                    return;
                }
                Waypoints = new WaypointData[wayPointCount];
                m_SelectedWaypoint = 0;
                m_Route.Clear();

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

                            //Add to the drawable route
                            AddPointToRoute(waypointLoaded.Coordinate.Longitude, waypointLoaded.Coordinate.Latitude, m_Route);

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

                //Centre on the route and finish drawing
                Map.ZoomAndCenterRoutes(m_RoutesOverlay.Id);
                Map.Refresh();
            }
        }

        void ConvertToData()
        {
            m_Data.StartNewData();
            WindData[] windData = CreateWindDataFromTable();
            
            int iWindIndex = 0;
            foreach (var wpd in Waypoints)
            {
                while ( (iWindIndex < TWDTable.RowCount - 1)
                        &&  (windData[iWindIndex + 1].Time < wpd.timeSinceStart))
                {
                    ++iWindIndex;
                }
                m_Data.AddNewData(GetCruncherDataFromWaypointData(wpd, windData[iWindIndex]));
            }
            m_Data.EndNewData();
            m_Data.SmoothData();
        }

        private WindData[] CreateWindDataFromTable()
        {
            WindData[] windData = new WindData[TWDTable.RowCount > 0 ? TWDTable.RowCount : 1];
            for (int i = 0; i < TWDTable.RowCount; i++)
            {
                windData[i].Time = Convert.ToDouble(TWDTable.Rows[i].Cells[0].Value);
                windData[i].TWD = Convert.ToDouble(TWDTable.Rows[i].Cells[1].Value);
                windData[i].TWS = Convert.ToDouble(TWDTable.Rows[i].Cells[2].Value);
            }

            if (TWDTable.RowCount <= 0)
            {
                windData[0].Time = 0.0;
                windData[0].TWD = 0.0;
                windData[0].TWS = 0.0;
            }
            return windData;
        }

        private void CreateTableFromWindData(WindData[] m_Wind)
        {
            TWDTable.Rows.Clear();
            if (m_Wind != null)
            {
                foreach (var wnd in m_Wind)
                {
                    int newRow = TWDTable.Rows.Add();
                    var row = TWDTable.Rows[newRow];
                    row.Cells[0].Value = wnd.Time;
                    row.Cells[1].Value = wnd.TWD;
                    row.Cells[2].Value = wnd.TWS;
                }
            }
        }

        private void PathOffset_Scroll(object sender, EventArgs e)
        {
            if (Waypoints == null || Waypoints.Length <= 0)
                return;

            //We're scrubbing the timeline back and forth
            var offset = (double)PathOffset.Value / (double)PathOffset.Maximum;
            var timeOffset = offset * Waypoints.Last().timeSinceStart;
            
            m_RoutesOverlay.Markers.Clear();
            int iWaypoint = 0;
            foreach (var wpd in Waypoints)
            {
                if (wpd.timeSinceStart >= timeOffset)
                {
                    //Put a marker here
                    GMap.NET.PointLatLng point = new GMap.NET.PointLatLng();
                    point.Lat = wpd.latitude;
                    point.Lng = wpd.longitude;
                    GMarkerGoogle marker = new GMarkerGoogle(
                        point,
                        GMarkerGoogleType.orange_small
                        );
                    
                    m_RoutesOverlay.Markers.Add(marker);
                    m_SelectedWaypoint = iWaypoint;
                    break;
                }
                iWaypoint++;
            }
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

        private void AddTWD_Click(object sender, EventArgs e)
        {
            int newRow = TWDTable.Rows.Add();
            if (m_SelectedWaypoint >= 0)
            {
                var row = TWDTable.Rows[newRow];
                row.Cells[0].Value = Waypoints[m_SelectedWaypoint].timeSinceStart;
                TWDTable.Sort(TWDTable.Columns[0], ListSortDirection.Ascending);
                TWDTable.CurrentCell = row.Cells[1];
            }
        }
        
        private void RemoveTWD_Click(object sender, EventArgs e)
        {
            HashSet<DataGridViewRow> rowsToRemove = new HashSet<DataGridViewRow>();
            foreach (DataGridViewRow row in TWDTable.SelectedRows)
            {
                if (!rowsToRemove.Contains(row))
                {
                    rowsToRemove.Add(row);
                }
            }

            foreach (DataGridViewCell cell in TWDTable.SelectedCells)
            {
                if (!rowsToRemove.Contains(cell.OwningRow))
                {
                    rowsToRemove.Add(cell.OwningRow);
                }
            }

            foreach (DataGridViewRow row in rowsToRemove)
            {
                TWDTable.Rows.Remove(row);
            }
        }
    }
}
