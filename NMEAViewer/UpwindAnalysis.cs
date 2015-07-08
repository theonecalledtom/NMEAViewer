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
    public partial class UpwindAnalysis : DockableDrawable
    {
        NMEACruncher m_Data;
        PolarData m_PolarData;

        double StartTime = 0.0;
        double EndTime = 0.0;

        class DataPoint
        {
            public double BoatHeading = 0.0;
            public double BoatSpeed = 0.0;
            public double PolarVMG = 0.0;
            public double TimeSpent = 0.0;
        }

        class CrunchedData
        {
            public double AverageAngle = 0.0;
            public double AverageSpeed = 0.0;
            public double AverageVMG = 0.0;
            public double AveragePolarVMG = 0.0;
            public double TimeSpent = 0.0;
        }

        //Outputs
        CrunchedData PortData = new CrunchedData();
        CrunchedData StarboardData = new CrunchedData();
        CrunchedData CombinedData = new CrunchedData();

        double m_fStartSelection = 0.0f;
        double m_fEndSelection = 0.0f;

        class SingleTackData
        {

        }

        public UpwindAnalysis(NMEACruncher data, PolarData polarData)
        {
            InitializeComponent();

            m_Data = data;
            m_PolarData = polarData;
        }

        public void SetPolarData(PolarData data)
        {
            m_PolarData = data;

            CalculateData();
        }

        [JsonObject(MemberSerialization.OptOut)]
        private class SerializedData : DockableDrawable.SerializedDataBase
        {
            public SerializedData(DockableDrawable parent)
                : base(parent) { }
            public double StartTime;
            public double EndTime;
            public bool FollowSelection;
            public bool UseGPS;
        };

        public override DockableDrawable.SerializedDataBase CreateSerializedData()
        {
            SerializedData data = new SerializedData(this);
            data.StartTime = StartTime;
            data.EndTime = EndTime;
            data.FollowSelection = checkBox_FollowSelection.Checked;
            data.UseGPS = checkBox_UseGPS.Checked;
            return data;
        }

        public override void InitFromSerializedData(SerializedDataBase data_base)
        {
            base.InitFromSerializedData(data_base);

            SerializedData data = (SerializedData)data_base;
            StartTime = data.StartTime;
            EndTime = data.EndTime;
            checkBox_FollowSelection.Checked = data.FollowSelection;
            checkBox_UseGPS.Checked = data.UseGPS;
            if (EndTime > StartTime)
            {
                m_fEndSelection = EndTime;
                m_fStartSelection = StartTime;
                CalculateData();
            }
        }

        public override void PostInitFromSerializedData(SerializedDataBase data_base)
        {
        }

        protected override void OnTimeRangeSelected(double fTimeA, double fTimeB)
        {
            m_fStartSelection = fTimeA > fTimeB ? fTimeB : fTimeA;
            m_fEndSelection = fTimeB > fTimeA ? fTimeB : fTimeA;

            if (checkBox_FollowSelection.Checked)
            {
                CalculateData();
            }
        }

        private void CalculateData()
        {
            if (m_fEndSelection > m_fStartSelection)
            {
                StartTime = m_fStartSelection;
                EndTime = m_fEndSelection;

                //Need to build tables left and right sail angles and speeds.
                //Lets discard the top and bottom 15% of samples based on sailing angle
                //so this will mean building and sorting lists of data on each tack

                int iStartIndex = m_Data.GetIndexForTime(m_fStartSelection);
                int iEndIndex = m_Data.GetIndexForTime(m_fEndSelection);
                if (iEndIndex > iStartIndex)
                {
                    int iCountPort = 0;
                    int iCountStarboard = 0;
                    for (int i=iStartIndex ; i<iEndIndex ; i++)
                    {
                        if (m_Data.GetDataAtIndex(i, NMEACruncher.DataTypes.AWA) < 0.0)
                        {
                            ++iCountPort;
                        }
                        else
                        {
                            ++iCountStarboard;
                        }
                    }

                    if (iCountPort > 0 && iCountStarboard > 0)
                    {
                        DataPoint[] portArray = new DataPoint[iCountPort];
                        DataPoint[] starboardArray = new DataPoint[iCountStarboard];

                        NMEACruncher.DataTypes speedType = checkBox_UseGPS.Checked ? NMEACruncher.DataTypes.GPSSOG : NMEACruncher.DataTypes.BoatSpeed;
                        NMEACruncher.DataTypes headingType = checkBox_UseGPS.Checked ? NMEACruncher.DataTypes.GPSHeading : NMEACruncher.DataTypes.BoatHeading;

                        //Fill the arrays with our sampled data
                        iCountPort = 0;
                        iCountStarboard = 0;
                        for (int i = iStartIndex; i < iEndIndex; i++)
                        {
                            double awa = m_Data.GetDataAtIndex(i, NMEACruncher.DataTypes.AWA);
                            if (awa < 0.0)
                            {
                                portArray[iCountPort] = new DataPoint();
                                portArray[iCountPort].BoatHeading = m_Data.GetDataAtIndex(i, headingType);
                                portArray[iCountPort].BoatSpeed = m_Data.GetDataAtIndex(i, speedType);
                                portArray[iCountPort].TimeSpent = i > 0 ? 
                                        m_Data.GetDataAtIndex(i, NMEACruncher.DataTypes.Time) - m_Data.GetDataAtIndex(i-1, NMEACruncher.DataTypes.Time) 
                                    :   0.0;    //Shouldn't hit this very often!

                                if (iCountPort > 0)
                                {
                                    portArray[iCountPort].BoatHeading = AngleUtil.Anchor(portArray[iCountPort].BoatHeading, portArray[0].BoatHeading);
                                }

                                if (m_PolarData != null)
                                {
                                    double tws = m_Data.GetDataAtIndex(i, NMEACruncher.DataTypes.TWS);
                                    if (Math.Abs(awa) < 60.0)
                                    {
                                        portArray[iCountPort].PolarVMG = m_PolarData.GetBestUpwindVMG(tws);
                                    }
                                    else
                                    {
                                        portArray[iCountPort].PolarVMG = m_PolarData.GetBestDownwindVMG(tws);
                                    }
                                }

                                ++iCountPort;
                            }
                            else
                            {
                                starboardArray[iCountStarboard] = new DataPoint();
                                starboardArray[iCountStarboard].BoatHeading = m_Data.GetDataAtIndex(i, headingType);
                                starboardArray[iCountStarboard].BoatSpeed = m_Data.GetDataAtIndex(i, speedType);
                                starboardArray[iCountStarboard].TimeSpent = i > 0 ? 
                                        m_Data.GetDataAtIndex(i, NMEACruncher.DataTypes.Time) - m_Data.GetDataAtIndex(i-1, NMEACruncher.DataTypes.Time) 
                                    :   0.0;    //Shouldn't hit this very often!

                                if (iCountStarboard > 0)
                                {
                                    starboardArray[iCountStarboard].BoatHeading = AngleUtil.Anchor(starboardArray[iCountStarboard].BoatHeading, starboardArray[0].BoatHeading);
                                }

                                if (m_PolarData != null)
                                {
                                    double tws = m_Data.GetDataAtIndex(i, NMEACruncher.DataTypes.TWS);
                                    if (Math.Abs(awa) < 60.0)
                                    {
                                        starboardArray[iCountStarboard].PolarVMG = m_PolarData.GetBestUpwindVMG(tws);
                                    }
                                    else
                                    {
                                        starboardArray[iCountStarboard].PolarVMG = m_PolarData.GetBestDownwindVMG(tws);
                                    }
                                }
                                
                                ++iCountStarboard;
                            }
                        }

                        //Sort the arrays based on boat heading, pinned of course
                        DataPoint[] portArraySortedByHeading = portArray.OrderBy(result => result.BoatHeading).ToArray();
                        DataPoint[] starboardArraySortedByHeading = starboardArray.OrderBy(result => result.BoatHeading).ToArray();

                        CrunchTackData(portArraySortedByHeading, PortData, portArray.Length / 8);
                        CrunchTackData(starboardArraySortedByHeading, StarboardData, starboardArray.Length / 8);

                        CombinedData.TimeSpent = PortData.TimeSpent + StarboardData.TimeSpent;

                        //Ignore time spent on each tack for averaging purposes
                        CombinedData.AverageSpeed = (PortData.AverageSpeed + StarboardData.AverageSpeed) * 0.5f;
                        CombinedData.AverageAngle = AngleUtil.CalculateAverage(PortData.AverageAngle, StarboardData.AverageAngle);

                        //Now calculate the VMG on each tack and combined
                        double angleToPort = AngleUtil.ShortAngle(CombinedData.AverageAngle, PortData.AverageAngle);
                        double angleToStarboard = AngleUtil.ShortAngle(CombinedData.AverageAngle, StarboardData.AverageAngle);
                        PortData.AverageVMG = PortData.AverageSpeed * Math.Cos(angleToPort * AngleUtil.DegToRad);
                        StarboardData.AverageVMG = StarboardData.AverageSpeed * Math.Cos(angleToStarboard * AngleUtil.DegToRad);

                        //And the average VMG!
                        CombinedData.AverageVMG = (PortData.AverageVMG + StarboardData.AverageVMG) * 0.5f;
                        CombinedData.AveragePolarVMG = (PortData.AveragePolarVMG + StarboardData.AveragePolarVMG) * 0.5f;

                        StartTimeText.Text = string.Format("{0:0.00}", StartTime);
                        EndTimeText.Text = string.Format("{0:0.00}", EndTime);
                        
                        TimeOnPort.Text = string.Format("{0:0.00}", PortData.TimeSpent);
                        AvAnglePort.Text = string.Format("{0:0.00}", PortData.AverageAngle);
                        AvSpdPort.Text = string.Format("{0:0.00}", PortData.AverageSpeed);
                        AvVMGPort.Text = string.Format("{0:0.00}", PortData.AverageVMG);
                        TimeOnStarboard.Text = string.Format("{0:0.00}", StarboardData.TimeSpent);
                        AvAngleStarboard.Text = string.Format("{0:0.00}", StarboardData.AverageAngle);
                        AvSpdStarboard.Text = string.Format("{0:0.00}", StarboardData.AverageSpeed);
                        AvVMGStarboard.Text = string.Format("{0:0.00}", StarboardData.AverageVMG);
                        AverageAngle.Text = string.Format("{0:0.00}", CombinedData.AverageAngle);
                        AverageSpeed.Text = string.Format("{0:0.00}", CombinedData.AverageSpeed);
                        AverageVMG.Text = string.Format("{0:0.00}", CombinedData.AverageVMG);

                        textPolarVMGPort.Text = string.Format("{0:0.00}", ((PortData.AverageVMG * 100.0f) / PortData.AveragePolarVMG));
                        textPolarVMGStarboard.Text = string.Format("{0:0.00}", ((StarboardData.AverageVMG * 100.0f) / StarboardData.AveragePolarVMG));
                        textPolarVMGAverage.Text = string.Format("{0:0.00}", ((CombinedData.AverageVMG * 100.0f) / CombinedData.AveragePolarVMG));

                        textTackingAngle.Text = string.Format("{0:0.00}", Math.Abs(AngleUtil.ShortAngle(PortData.AverageAngle, StarboardData.AverageAngle)));
                    }
                }
            }
        }

        private void CrunchTackData(DataPoint[] dataSortedByAngle, CrunchedData dataOut, int iDiscard)
        {
            int iMax = dataSortedByAngle.Length - iDiscard;
            double fHeadingRoot = dataSortedByAngle[iDiscard].BoatHeading;
            int iCount = iMax - iDiscard;
            dataOut.AverageAngle = 0.0;
            dataOut.AverageSpeed = 0.0;
            dataOut.AveragePolarVMG = 0.0;
            dataOut.TimeSpent = 0.0;
            for (int i=iDiscard ; i<iMax ; i++)
            {
                double time = dataSortedByAngle[i].TimeSpent;
                dataOut.AverageAngle += AngleUtil.ShortAngle(dataSortedByAngle[i].BoatHeading, fHeadingRoot) * time;
                dataOut.AverageSpeed += dataSortedByAngle[i].BoatSpeed * time;
                dataOut.AveragePolarVMG += dataSortedByAngle[i].PolarVMG * time;
                dataOut.TimeSpent += dataSortedByAngle[i].TimeSpent * time;
            }
            dataOut.AverageAngle *= 1.0 / dataOut.TimeSpent;
            dataOut.AverageAngle += fHeadingRoot;
            dataOut.AverageSpeed *= 1.0 / dataOut.TimeSpent;
            dataOut.AveragePolarVMG *= 1.0 / dataOut.TimeSpent;
        }

        private void Update_Click(object sender, EventArgs e)
        {
            //Take the selection and find the port and starboard tack data
            CalculateData();
        }

        private void checkBox_FollowSelection_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_FollowSelection.Checked)
            {
                CalculateData();
            }
        }

        private void checkBox_UseGPS_CheckedChanged(object sender, EventArgs e)
        {
            CalculateData();
        }
    }
}
