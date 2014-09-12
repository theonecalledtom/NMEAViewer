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
    public partial class MapWindow : DockableDrawable
    {
        private GMap.NET.WindowsForms.GMapRoute m_Route = null;
        private GMap.NET.WindowsForms.GMapRoute m_RouteHighlight = null;
        private List<double> m_RouteTimes = null;
        private GMap.NET.WindowsForms.GMapOverlay m_RoutesOverlay = null;
	    private GMap.NET.WindowsForms.GMapOverlay m_TacksOverlay = null;
        private GMap.NET.WindowsForms.GMapOverlay m_EventsOverlay = null;
        private GMap.NET.WindowsForms.GMapOverlay m_RouteHighlightOverlay = null;
        private GMap.NET.WindowsForms.GMapOverlay m_BoatOverlay = null;
        GMap.NET.WindowsForms.GMapMarker m_SelectedMarker = null;
        private Dictionary<GMap.NET.WindowsForms.GMapMarker, TackAnalysisData> m_MarkerToTackMap;
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
            //TODO: Not sure how to get the provider from the string
            //data.m_MapProvider = gMapControl1.MapProvider.Name;
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
            m_MarkerToTackMap = new Dictionary<GMap.NET.WindowsForms.GMapMarker, TackAnalysisData>();
            gMapControl1.MapProvider = GMap.NET.MapProviders.ArcGIS_Topo_US_2D_MapProvider.Instance;
            gMapControl1.Position = new GMap.NET.PointLatLng(32.0, -117.0);
            gMapControl1.MinZoom = 1;
            gMapControl1.MaxZoom = 24;
            gMapControl1.Zoom = 9;
            gMapControl1.OnMarkerClick += OnMarkerClick;

            //Add the overlay for the route
			m_RoutesOverlay = new GMap.NET.WindowsForms.GMapOverlay("routes");
			gMapControl1.Overlays.Add(m_RoutesOverlay);

            //Add the overlay for the highlighted route
            m_RouteHighlightOverlay = new GMap.NET.WindowsForms.GMapOverlay("routehighlight");
            gMapControl1.Overlays.Add(m_RouteHighlightOverlay);

            //And the one for tack points
			m_TacksOverlay = new GMap.NET.WindowsForms.GMapOverlay("tacks");
			gMapControl1.Overlays.Add(m_TacksOverlay);

            //And the one for tack points
            m_EventsOverlay = new GMap.NET.WindowsForms.GMapOverlay("events");
            gMapControl1.Overlays.Add(m_EventsOverlay);

            //And the one for the boat?
            m_BoatOverlay = new GMap.NET.WindowsForms.GMapOverlay("boat");
            gMapControl1.Overlays.Add(m_BoatOverlay);

            //And add a route for the main sailing path
			m_Route = new GMap.NET.WindowsForms.GMapRoute("Route Taken");
			m_Route.Stroke.Color = System.Drawing.Color.Blue;
			m_Route.Stroke.Width = 1;
			m_RoutesOverlay.Routes.Add(m_Route);
            m_RouteTimes = new List<double>();

            m_RouteHighlight = new GMap.NET.WindowsForms.GMapRoute("RouteHighlight");
            m_RouteHighlight.Stroke = new Pen(System.Drawing.Color.Red);
            m_RouteHighlight.Stroke.Width = 2;
            m_RouteHighlightOverlay.Routes.Add(m_RouteHighlight);

            SetTimerFrequency(0.5);

            gMapControl1.MouseDown += MapMouseDown;
            gMapControl1.MouseMove += MapMouseMove;
            gMapControl1.MouseUp += MapMouseUp;
            gMapControl1.MouseLeave += MapMouseLeave;

            BringPathUpToDate();
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
                    BroadcastOnTimeSelected(this, fTime);
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
            m_RouteTimes.Clear();

            m_iLastDataProcessed = 0;

            //Bring the path up to date right away
            BringPathUpToDate();
            m_bDirty = true;
        }

        private void SelectMarker(GMap.NET.WindowsForms.GMapMarker marker)
	    {
		    if (m_SelectedMarker != null)
		    {
			    m_SelectedMarker.ToolTipMode = GMap.NET.WindowsForms.MarkerTooltipMode.OnMouseOver;
		    }
		    m_SelectedMarker = marker;
		    m_SelectedMarker.ToolTipMode = GMap.NET.WindowsForms.MarkerTooltipMode.Always;
	    }

        private void OnMarkerClick(GMap.NET.WindowsForms.GMapMarker marker, System.Windows.Forms.MouseEventArgs e)
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
                        GMap.NET.WindowsForms.Markers.GMarkerGoogle marker = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(
                            point,
                            GMap.NET.WindowsForms.Markers.GMarkerGoogleType.orange_small
                            );

                        //Mouse over tool tip
                        string toolTip = eventNames[i];
                        toolTip += "\n\r\n\r";
                        toolTip += eventDescriptions[i];

                        marker.ToolTipMode = GMap.NET.WindowsForms.MarkerTooltipMode.OnMouseOver;
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
            m_MarkerToTackMap.Clear();
            m_SelectedMarker = null;

            //Add the new ones, with a bit of meta data and tooltip!
            foreach(TackAnalysisData tack in tackData)
            {
                double fLat = tack.GetValue(TackAnalysisData.eTackDataTypes.Lat);
                double fLong = tack.GetValue(TackAnalysisData.eTackDataTypes.Long);
                GMap.NET.PointLatLng point = new GMap.NET.PointLatLng(fLat, fLong);
				GMap.NET.WindowsForms.Markers.GMarkerGoogle marker = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(
					point,
					tack.IsTack() ? GMap.NET.WindowsForms.Markers.GMarkerGoogleType.yellow_small : GMap.NET.WindowsForms.Markers.GMarkerGoogleType.blue_small
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
                toolTip += "\tLoss (Hdg): ";
                toolTip += string.Format("{0:0.##}", tack.GetValue(TackAnalysisData.eTackDataTypes.LossDueToHeading));
                toolTip += "\n\r";
                toolTip += "\tLoss (Spd): ";
                toolTip += string.Format("{0:0.##}", tack.GetValue(TackAnalysisData.eTackDataTypes.LossDueToSpeed));
                toolTip += "\n\r";
                toolTip += "\tAv speed: ";
                toolTip += string.Format("{0:0.##}", tack.GetValue(TackAnalysisData.eTackDataTypes.AverageSpeed));

                marker.ToolTipMode = GMap.NET.WindowsForms.MarkerTooltipMode.OnMouseOver;
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

        public void AddPointToHighlight(double fLong, double fLat)
        {
            GMap.NET.PointLatLng newPoint = new GMap.NET.PointLatLng();
            newPoint.Lat = fLat;
            newPoint.Lng = fLong;
            m_RouteHighlight.Points.Add(newPoint);
            m_bDirty = true;
        }

        const double PI = 3.141592653589793;
        const double DegToRad = (2.0 * PI / 360.0);
        const double R = 6371.0 * 1000.0;    // m
        const double kMinMoveForGPS = 1.0;   // m

        public static double CalculateDistanceBetweenLongsAndLats(double fFromLong, double fFromLat, double fToLong, double fToLat)
        {
	        double dLat = (fFromLat - fToLat) * DegToRad;
	        double dLon = (fFromLong - fToLong) * DegToRad;
	        double lat1 = fToLat * DegToRad;
	        double lat2 = fFromLat * DegToRad;
	        double fDLatOver2 = dLat * 0.5f;
	        double fDLongOver2 = dLon * 0.5f;
	        double a = Math.Sin(fDLatOver2) * Math.Sin(fDLatOver2) +
		        Math.Sin(fDLongOver2) * Math.Sin(fDLongOver2) * Math.Cos(lat1) * Math.Cos(lat2);
	        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

	        double d = R * c;
	        return d;
        }

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
                    if (m_Route.Points.Count > 0)
                    {
                        GMap.NET.PointLatLng priorPoint = m_Route.Points[m_Route.Points.Count - 1];
                        double fDist = CalculateDistanceBetweenLongsAndLats(priorPoint.Lng, priorPoint.Lat, fLong, fLat);
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
                        GMap.NET.WindowsForms.Markers.GMarkerGoogle BoatMarker = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(
                                                hoveredPoint,
                                                GMap.NET.WindowsForms.Markers.GMarkerGoogleType.gray_small
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
                    AddPointToHighlight(m_Route.Points[i].Lng, m_Route.Points[i].Lat);
                }
            }

            if (selectionToolStripMenuItem.Checked)
            {
                gMapControl1.ZoomAndCenterRoutes(m_RouteHighlightOverlay.Id);
            }

            gMapControl1.UpdateRouteLocalPosition(m_RouteHighlight);

            //If we contain a single marker, highlight it
            int iSelectedCount = 0;
            GMap.NET.WindowsForms.GMapMarker soloSelected = null;
            foreach (GMap.NET.WindowsForms.GMapMarker marker in m_TacksOverlay.Markers)
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
    }
}
