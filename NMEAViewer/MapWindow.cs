﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using Newtonsoft.Json;

namespace NMEAViewer
{

    public partial class MapWindow : DockableDrawable
    {
        private GMapRoute m_Route = null;
        private GMapRoute m_RouteHighlight = null;
        private GMapRoute m_EventHighlight = null;
        private List<double> m_RouteTimes = null;
        private GMapOverlay m_RoutesOverlay = null;
	    private GMapOverlay m_TacksOverlay = null;
        private GMapOverlay m_TackEventOverlay = null;
        private GMapOverlay m_EventsOverlay = null;
        private GMapOverlay m_RouteHighlightOverlay = null;
        private GMapOverlay m_EventOverlay = null;
        private GMapOverlay m_BoatOverlay = null;
        GMapMarker m_SelectedMarker = null;
        private Dictionary<GMapMarker, TackAnalysisData> m_MarkerToTackMap;
        private NMEACruncher m_Data;
        private int m_iLastDataProcessed;
        private double m_fStartSelection;
        private double m_fEndSelection;

        [JsonObject(MemberSerialization.OptOut)]
        private class SerializedData : DockableDrawable.SerializedDataBase
        {
            public SerializedData(DockableDrawable parent)
                : base(parent) { }

            public double m_Zoom = 5;
            public double m_Lat = 0.0;
            public double m_Long = 0.0;
            public double m_StartSelectionTime;
            public double m_EndSelectionTime;
            public string m_MapProvider;
            public bool m_TrackCurrent;
            public bool m_TrackSelection;
        };

        public override DockableDrawable.SerializedDataBase CreateSerializedData()
        {
            SerializedData data = new SerializedData(this);
            data.m_Zoom = gMapControl1.Zoom;
            data.m_Lat = gMapControl1.Position.Lat;
            data.m_Long = gMapControl1.Position.Lng;
            data.m_MapProvider = gMapControl1.MapProvider.Name;
            data.m_TrackCurrent = trackCurrentToolStripMenuItem.Checked;
            data.m_TrackSelection = selectionToolStripMenuItem.Checked;
            data.m_StartSelectionTime = m_fStartSelection;
            data.m_EndSelectionTime = m_fEndSelection;
            return data;
        }

        public override void InitFromSerializedData(SerializedDataBase data_base)
        {
            SerializedData data = (SerializedData)data_base;
            gMapControl1.Zoom = data.m_Zoom;
            gMapControl1.Position = new GMap.NET.PointLatLng(data.m_Lat, data.m_Long);
            trackCurrentToolStripMenuItem.Checked = data.m_TrackCurrent;
            selectionToolStripMenuItem.Checked = data.m_TrackSelection;
            m_fStartSelection = data.m_StartSelectionTime;
            m_fEndSelection = data.m_EndSelectionTime;

            gMapControl1.MapProvider = GMap.NET.MapProviders.OpenSeaMapHybridProvider.Instance;
            foreach (var v in GMap.NET.MapProviders.GMapProviders.List)
            {
                if (v.Name == data.m_MapProvider)
                {
                    gMapControl1.MapProvider = v;
                    break;
                }
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            //base.OnMouseWheel(e);

            double factor = (e.Delta + 5000.0) / 5000.0;
            if (factor < 0.1)
            {
                factor = 0.1;
            }
            double zoom = gMapControl1.Zoom;
            zoom *= factor;
            gMapControl1.Zoom = zoom;
        }

        public override void PostInitFromSerializedData(SerializedDataBase data_base)
        {
            if (m_fEndSelection > m_fStartSelection)
            {
                OnTimeRangeSelected(m_fStartSelection, m_fEndSelection);

                if (selectionToolStripMenuItem.Checked)
                {
                    gMapControl1.ZoomAndCenterRoutes(m_RouteHighlightOverlay.Id);
                }
            }
        }

        public MapWindow(NMEACruncher data)
        {
            InitializeComponent();

            m_Data = data;
            m_MarkerToTackMap = new Dictionary<GMapMarker, TackAnalysisData>();
            gMapControl1.MapProvider = GMap.NET.MapProviders.OpenSeaMapHybridProvider.Instance;//   //ArcGIS_Topo_US_2D_MapProvider.Instance;
            gMapControl1.Position = new GMap.NET.PointLatLng(32.0, -117.0);
            gMapControl1.MinZoom = 1;
            gMapControl1.MaxZoom = 24;
            gMapControl1.Zoom = 9;
            gMapControl1.OnMarkerClick += OnMarkerClick;

            //Add the overlay for the route
			m_RoutesOverlay = new GMapOverlay("routes");
			gMapControl1.Overlays.Add(m_RoutesOverlay);

            //Add the overlay for the highlighted route
            m_RouteHighlightOverlay = new GMapOverlay("routehighlight");
            gMapControl1.Overlays.Add(m_RouteHighlightOverlay);

            //Add the overlay for the tacking / gybing route
            m_EventOverlay = new GMapOverlay("eventoverlay");
            gMapControl1.Overlays.Add(m_EventOverlay);

            //And the one for tack points
            m_TacksOverlay = new GMapOverlay("tacks");
			gMapControl1.Overlays.Add(m_TacksOverlay);

            //And one for a specific tack
            m_TackEventOverlay = new GMapOverlay("tackevent");
            gMapControl1.Overlays.Add(m_TackEventOverlay);

            //And the one for tack points
            m_EventsOverlay = new GMapOverlay("events");
            gMapControl1.Overlays.Add(m_EventsOverlay);

            //And the one for the boat?
            m_BoatOverlay = new GMapOverlay("boat");
            gMapControl1.Overlays.Add(m_BoatOverlay);

            //And add a route for the main sailing path
			m_Route = new GMapRoute("Route Taken");
			m_Route.Stroke.Color = System.Drawing.Color.Blue;
			m_Route.Stroke.Width = 1;
			m_RoutesOverlay.Routes.Add(m_Route);
            m_RouteTimes = new List<double>();

            m_RouteHighlight = new GMapRoute("RouteHighlight");
            m_RouteHighlight.Stroke = new Pen(System.Drawing.Color.Red);
            m_RouteHighlight.Stroke.Width = 2;
            m_RouteHighlightOverlay.Routes.Add(m_RouteHighlight);

            m_EventHighlight = new GMapRoute("EventHighlight");
            m_EventHighlight.Stroke = new Pen(System.Drawing.Color.Aqua);
            m_EventHighlight.Stroke.Width = 3;
            m_EventOverlay.Routes.Add(m_EventHighlight);

            SetTimerFrequency(0.5);

            gMapControl1.MouseDown += MapMouseDown;
            gMapControl1.DoubleClick += gMapControl1_DoubleClick;
            gMapControl1.OnMarkerClick += GMapControl1_OnMarkerClick;
            gMapControl1.MouseMove += MapMouseMove;
            gMapControl1.MouseUp += MapMouseUp;
            gMapControl1.MouseLeave += MapMouseLeave;

            BringPathUpToDate();
        }

        private void GMapControl1_OnMarkerClick(GMapMarker marker, MouseEventArgs e)
        {
            //If this is a tack, pop up an extra bunch of data
            if (m_MarkerToTackMap.ContainsKey(marker))
            {
                TackAnalysisData tack = m_MarkerToTackMap[marker];
                if (tack != null)
                {
                    //Found a tack!
                    SetupTackEventOverlay(tack);
                }
            }
        }

        private void SetupTackEventOverlay(TackAnalysisData tack)
        {
            m_TackEventOverlay.Clear();

            //Put markers down at
            //  Start of turn
            //  End of turn
            //  90 percent time
            //  And perhaps a wind direction estimation?
            var TimeStartOfTurn = tack.GetValue(TackAnalysisData.eTackDataTypes.TimeOfStartOfTurn);
            var TimeEndOfTurn = tack.GetValue(TackAnalysisData.eTackDataTypes.TimeOfEndOfTurn);
            var TimeOfSlowest = TimeStartOfTurn + tack.GetValue(TackAnalysisData.eTackDataTypes.TimeToSlowestPoint);
            var TimeOf90Percent = TimeOfSlowest + tack.GetValue(TackAnalysisData.eTackDataTypes.TimeToRegain90PercentSpdFromSlowest);

            //Put in some markers
            QuickAddMarkerAtTime(TimeStartOfTurn, m_TackEventOverlay, GMarkerGoogleType.green_big_go, "Start of turn");
            QuickAddMarkerAtTime(TimeEndOfTurn, m_TackEventOverlay, GMarkerGoogleType.lightblue_dot, "End of turn");
            QuickAddMarkerAtTime(TimeOfSlowest, m_TackEventOverlay, GMarkerGoogleType.orange_dot, "Slowest");
            QuickAddMarkerAtTime(TimeOf90Percent, m_TackEventOverlay, GMarkerGoogleType.green_dot, "Back to 90%");

            //Event line highlight
            m_EventHighlight.Clear();
            for (int i = 0; i < m_Route.Points.Count; i++)
            {
                if (m_RouteTimes[i] > TimeOf90Percent)
                    break;
                if (m_RouteTimes[i] > TimeStartOfTurn)
                {
                    AddPointToRoute(m_Route.Points[i].Lng, m_Route.Points[i].Lat, m_EventHighlight);
                }
            }
        }

        private void QuickAddMarkerAtTime(double timeStartOfTurn, GMapOverlay overlay, GMarkerGoogleType markerType, string infoTip, bool bInfoAlwaysOn=true)
        {
            int iDataIndex = m_Data.GetIndexForTime(timeStartOfTurn);
            if (iDataIndex >= 0)
            {
                if (m_Data.HasDataAtIndex(iDataIndex, NMEACruncher.DataTypes.GPSLat))
                {
                    double fLat = m_Data.GetDataAtIndex(iDataIndex, NMEACruncher.DataTypes.GPSLat);
                    double fLong = m_Data.GetDataAtIndex(iDataIndex, NMEACruncher.DataTypes.GPSLong);
                    GMap.NET.PointLatLng point = new GMap.NET.PointLatLng(fLat, fLong);
                    GMarkerGoogle marker = new GMarkerGoogle(
                        point,
                        markerType
                        );
                    if (!string.IsNullOrEmpty(infoTip))
                    {
                        marker.ToolTipMode =
                            bInfoAlwaysOn ?
                                    MarkerTooltipMode.Always
                                :   MarkerTooltipMode.OnMouseOver;
                        marker.ToolTipText = infoTip;
                    }
                    overlay.Markers.Add(marker);
                }
            }

        }

        void gMapControl1_DoubleClick(object sender, EventArgs e)
        {
            if (m_fMarkedTime >= 0.0)
            {
                BroadcastOnTimeSelected(this, m_fMarkedTime);
            }
            //GMap.NET.PointLatLng worldpnt = gMapControl1.FromLocalToLatLng(e.X, e.Y);

            //int iNearDataIndex = m_Data.FindDataNearLatAndLong(worldpnt.Lat, worldpnt.Lng, 100.0);
            //if (iNearDataIndex >= 0)
            //{
            //    double fTime = m_Data.GetDataAtIndex(iNearDataIndex, NMEACruncher.DataTypes.Time);
            //    BroadcastOnTimeSelected(this, fTime);
            //    SelectTime(fTime);
            //}
        }


        bool m_bMouseRightDown = false;
        bool m_bDirty = false;

        private void MapMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                m_bMouseRightDown = true;
            }
            else if (e.Button == MouseButtons.Left)
            {
                //Find nearest lat and long
                GMap.NET.PointLatLng worldpnt = gMapControl1.FromLocalToLatLng(e.X, e.Y);

                int iNearDataIndex = m_Data.FindDataNearLatAndLong(worldpnt.Lat, worldpnt.Lng, 100.0);
                if (iNearDataIndex >= 0)
                {
                    double fTime = m_Data.GetDataAtIndex(iNearDataIndex, NMEACruncher.DataTypes.Time);
                    //BroadcastOnTimeSelected(this, fTime);
                    SelectTime(fTime);
                }
            }
        }

        private void MapMouseMove(object sender, EventArgs e)
        {
            if (m_bMouseRightDown)
            {
                trackCurrentToolStripMenuItem.Checked = false;

                //Pop some text over the map?
            }
        }

        private void MapMouseUp(object sender, MouseEventArgs e)
        {
            m_bMouseRightDown = false;
        }

        private void MapMouseLeave(object sender, EventArgs e)
        {
            m_bMouseRightDown = false;
        }

        protected override void OnDataReplaced(NMEACruncher newData)
        {
            m_Data = newData;
            m_Route.Clear();
            m_RouteHighlight.Clear();
            m_EventHighlight.Clear();

            m_RouteTimes.Clear();

            m_iLastDataProcessed = 0;

            //Bring the path up to date right away
            BringPathUpToDate();
            m_bDirty = true;
        }

        private void SelectMarker(GMapMarker marker)
	    {
		    if (m_SelectedMarker != null)
		    {
			    m_SelectedMarker.ToolTipMode = MarkerTooltipMode.OnMouseOver;
		    }
		    m_SelectedMarker = marker;
		    m_SelectedMarker.ToolTipMode = MarkerTooltipMode.Always;
	    }

        private void OnMarkerClick(GMapMarker marker, System.Windows.Forms.MouseEventArgs e)
	    {
            if (marker != m_SelectedMarker)
            {
                if (m_MarkerToTackMap.ContainsKey(marker))
                {
                    TackAnalysisData data = m_MarkerToTackMap[marker];
                    if (data != null)
                    {
                        m_fStartSelection = data.GetValue(TackAnalysisData.eTackDataTypes.TimeOfStartOfTurn);
                        m_fEndSelection = data.GetValue(TackAnalysisData.eTackDataTypes.TimeOfEndOfTurn);

                        BroadcastOnTimeRangeSelected(m_fStartSelection, m_fEndSelection);
                        SelectMarker(marker);
                    }
                }
            }
	    }

        protected override void OnEventData(List<double> eventTimes, List<string> eventNames, List<string> eventDescriptions)
        {
            m_EventsOverlay.Markers.Clear();

            for (int i=0 ; i<eventTimes.Count ; i++)
            {
                if (eventNames[i] == null || eventNames[i].Length == 0)
                    continue;

                int iDataIndex = m_Data.GetIndexForTime(eventTimes[i]);
                if (iDataIndex >= 0)
                {
                    if (m_Data.HasDataAtIndex(iDataIndex, NMEACruncher.DataTypes.GPSLat))
                    {
                        double fLat = m_Data.GetDataAtIndex(iDataIndex, NMEACruncher.DataTypes.GPSLat);
                        double fLong = m_Data.GetDataAtIndex(iDataIndex, NMEACruncher.DataTypes.GPSLong);
                        GMap.NET.PointLatLng point = new GMap.NET.PointLatLng(fLat, fLong);
                        GMarkerGoogle marker = new GMarkerGoogle(
                            point,
                            GMarkerGoogleType.orange_small
                            );

                        //Mouse over tool tip
                        string toolTip = eventNames[i];
                        toolTip += "\n\r\n\r";
                        toolTip += eventDescriptions[i];

                        marker.ToolTipMode = MarkerTooltipMode.OnMouseOver;
                        marker.ToolTipText = toolTip;

                        m_EventsOverlay.Markers.Add( marker );
                    }
                }
            }
        }

        protected override void OnNewTackData(List<TackAnalysisData> tackData)
        {
            //Clear the current markers
            m_TacksOverlay.Clear();
            m_TackEventOverlay.Clear();
            m_MarkerToTackMap.Clear();
            m_SelectedMarker = null;

            //Add the new ones, with a bit of meta data and tooltip!
            foreach(TackAnalysisData tack in tackData)
            {
                double fLat = tack.GetValue(TackAnalysisData.eTackDataTypes.Lat);
                double fLong = tack.GetValue(TackAnalysisData.eTackDataTypes.Long);
                GMap.NET.PointLatLng point = new GMap.NET.PointLatLng(fLat, fLong);
				GMarkerGoogle marker = new GMarkerGoogle(
					point,
					tack.IsTack() ? GMarkerGoogleType.yellow_small : GMarkerGoogleType.blue_small
					);

                string legName = MetaDataWindow.GetLegNameForTime(tack.GetValue(TackAnalysisData.eTackDataTypes.TimeOfStartOfTurn));
                string toolTip = "";
                if (legName != null)
                {
                    toolTip += legName;
                    toolTip += ": ";
                }
                toolTip += tack.IsTack() ? "Tack" : "Gybe";
                toolTip += "\n\r";
                toolTip += "\tLoss: ";
                toolTip += string.Format("{0:0.##}", tack.GetValue(TackAnalysisData.eTackDataTypes.TotalLoss));
                toolTip += "\n\r";
                toolTip += "\tAv speed: ";
                toolTip += string.Format("{0:0.##}", tack.GetValue(TackAnalysisData.eTackDataTypes.AverageSpeed));

                marker.ToolTipMode = MarkerTooltipMode.OnMouseOver;
                marker.ToolTipText = toolTip;

                m_TacksOverlay.Markers.Add(marker);
                m_MarkerToTackMap.Add(marker, tack);
            }
        }

        public void AddPointToPath(double fLong, double fLat, double fTime)
		{
			GMap.NET.PointLatLng newPoint = new GMap.NET.PointLatLng();
			newPoint.Lat = fLat;
			newPoint.Lng = fLong;
			m_Route.Points.Add(newPoint);
            m_RouteTimes.Add(fTime);
            m_bDirty = true;
		}

        public void AddPointToRoute(double fLong, double fLat, GMapRoute route)
        {
            GMap.NET.PointLatLng newPoint = new GMap.NET.PointLatLng();
            newPoint.Lat = fLat;
            newPoint.Lng = fLong;
            route.Points.Add(newPoint);
            m_bDirty = true;
        }

        const double kMinMoveForGPS = 1.0;   // m


        private void BringPathUpToDate()
        {
            int iLastItem = m_Data.GetDataCount();
            bool bToProcess = m_iLastDataProcessed < iLastItem;
            while (m_iLastDataProcessed < iLastItem)
            {
                if (m_Data.HasDataAtIndex(m_iLastDataProcessed, NMEACruncher.DataTypes.GPSLat))
                {
                    double fLong = m_Data.GetDataAtIndex(m_iLastDataProcessed, NMEACruncher.DataTypes.GPSLong);
                    double fLat = m_Data.GetDataAtIndex(m_iLastDataProcessed, NMEACruncher.DataTypes.GPSLat);
                    if ((fLat > 0.0) || (fLong != 0.0))
                    {
                        if (m_Route.Points.Count > 0)
                        {
                            GMap.NET.PointLatLng priorPoint = m_Route.Points[m_Route.Points.Count - 1];
                            double fDist = CoordinateUtils.DistanceBetween(priorPoint.Lng, priorPoint.Lat, fLong, fLat);
                            if (fDist > kMinMoveForGPS)
                            {
                                AddPointToPath(fLong, fLat, m_Data.GetDataAtIndex(m_iLastDataProcessed, NMEACruncher.DataTypes.Time));
                                if (fDist > 10000.0)
                                {
                                    Console.WriteLine("Long GPS jump to {0} long {1} lat", fLong, fLat);
                                }
                            }
                        }
                        else
                        {
                            //Just add the point
                            AddPointToPath(fLong, fLat, m_Data.GetDataAtIndex(m_iLastDataProcessed, NMEACruncher.DataTypes.Time));
                        }
                    }
                }
                ++m_iLastDataProcessed;
            }

            if (bToProcess)
            {
                gMapControl1.UpdateRouteLocalPosition(m_Route);
            }

            if (trackCurrentToolStripMenuItem.Checked)
            {
                if (m_Route.Points.Count > 0)
                {
                    gMapControl1.Position = m_Route.Points[m_Route.Points.Count - 1];       
                }
            }
        }

        protected override void OnTimer(object sender, EventArgs e) 
        {
            BringPathUpToDate();

            if (m_bDirty)
            {
                gMapControl1.Refresh();
                m_bDirty = false;
            }
        }

        private void UpdateMarkerStyles()
        {

        }

        double m_fMarkedTime = -1.0;
        private void SelectTime(double fTime)
        {
            if (fTime != m_fMarkedTime)
            {
                m_bDirty = true;

                m_BoatOverlay.Clear();

                int iIndex = m_Data.GetIndexForTime(fTime);
                if (iIndex >= 0)
                {
                    if (m_Data.HasDataAtIndex(iIndex, NMEACruncher.DataTypes.GPSLong))
                    {
                        GMap.NET.PointLatLng hoveredPoint = new GMap.NET.PointLatLng(m_Data.GetDataAtIndex(iIndex, NMEACruncher.DataTypes.GPSLat), m_Data.GetDataAtIndex(iIndex, NMEACruncher.DataTypes.GPSLong));
                        GMarkerGoogle BoatMarker = new GMarkerGoogle(
                                                hoveredPoint,
                                                GMarkerGoogleType.gray_small
                                                );
                        m_BoatOverlay.Markers.Add(BoatMarker);
                        m_fMarkedTime = fTime;
                    }
                }
                else
                {
                    m_fMarkedTime = -1.0;
                }
            }
        }

        protected override void OnTimeHovered(object sender, double fTime)
        {
            SelectTime(fTime);
        }

        protected override void OnTimeSelected(object sender, double fTime)
        {
            SelectTime(fTime);
        }

        protected override void OnEventSelected(TackAnalysisData data)
        {
            SetupTackEventOverlay(data);

            if (selectionToolStripMenuItem.Checked)
            {
                gMapControl1.ZoomAndCenterRoutes(m_EventOverlay.Id);
            }
        }

        protected override void OnTimeRangeSelected(double fTime, double fTime1)
        {
            m_fStartSelection = fTime;
            m_fEndSelection = fTime1;

            //Highlight selected section
            m_RouteHighlight.Clear();
            for (int i = 0; i < m_Route.Points.Count; i++ )
            {
                if (m_RouteTimes[i] > fTime1)
                    break;
                if (m_RouteTimes[i] > fTime)
                {
                    AddPointToRoute(m_Route.Points[i].Lng, m_Route.Points[i].Lat, m_RouteHighlight);
                }
            }

            if (selectionToolStripMenuItem.Checked)
            {
                gMapControl1.ZoomAndCenterRoutes(m_RouteHighlightOverlay.Id);
            }

            gMapControl1.UpdateRouteLocalPosition(m_RouteHighlight);

            //If we contain a single marker, highlight it
            int iSelectedCount = 0;
            GMapMarker soloSelected = null;
            foreach (GMapMarker marker in m_TacksOverlay.Markers)
            {
                if (m_MarkerToTackMap.ContainsKey(marker))
                {
                    TackAnalysisData tack = m_MarkerToTackMap[marker];
                    if (tack != null)
                    {
                        double fTimeOfHeadToWind = tack.GetValue(TackAnalysisData.eTackDataTypes.TimeOfStartOfTurn)
                                                + tack.GetValue(TackAnalysisData.eTackDataTypes.TimeToHeadToWind);
                        if ((fTimeOfHeadToWind >= m_fStartSelection)
                            && (fTimeOfHeadToWind <= m_fEndSelection))
                        {
                            //TODO: Marker is in the selection range, so what?
                            soloSelected = marker;
                            ++iSelectedCount;
                            if (iSelectedCount > 1)
                            {
                                soloSelected = null;
                                break;
                            }
                        }
                    }
                }
            }

            if (soloSelected != null)
            {
                SelectMarker( soloSelected );
            }
        }
 
        public void CentreOnPath()
	    {
            gMapControl1.ZoomAndCenterRoutes(m_RoutesOverlay.Id);
	    }

        private void centerOnMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CentreOnPath();
        }

        private void selectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectionToolStripMenuItem.Checked = !selectionToolStripMenuItem.Checked;
            if (selectionToolStripMenuItem.Checked)
            {
                gMapControl1.ZoomAndCenterRoutes(m_RouteHighlightOverlay.Id);
            }
        }

        private void mapTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mapTypeToolStripMenuItem.DropDownItems.Clear();
            foreach (var v in GMap.NET.MapProviders.GMapProviders.List)
            {
                ToolStripMenuItem newItem = new ToolStripMenuItem(v.Name);
                newItem.Tag = v;
                newItem.Click += newMapType_Click;
                mapTypeToolStripMenuItem.DropDownItems.Add(newItem);
            }

        }

        private void newMapType_Click(object sender, EventArgs e)
        {
            if (sender == null)
            {
                return;
            }

            gMapControl1.MapProvider = (GMap.NET.MapProviders.GMapProvider)(((ToolStripMenuItem)sender).Tag);
        }
    }
}
