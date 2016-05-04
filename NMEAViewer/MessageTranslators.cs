using System;
using System.Text;
using System.Collections.Generic;
//using System.Math;

namespace NMEAViewer
{
    public abstract class NMEAMessage_Base
    {
        public double SafeToDouble(string str)
        {
            return str.Length > 0 ? Convert.ToDouble(str) : 0.0; 
        }
        public static double ContainAngle180(double angle)
        {
            while (angle > 180.0)
                angle -= 360.0;
            while (angle <-180.0)
                angle += 360.0;
            return angle;
        }
        public static double SumAnglesForAverage(double A, double B, double C)
        {
            while (B - A > 180.0)
            {
                B -= 360.0;
            }
            while (B - A < -180.0)
            {
                B += 360.0;
            }
            return C + B;
        }

        public const double kMSToKnots = 1.94384;
        public double m_fElapsedTime;
        public abstract string GetKey();
        public abstract void ProcessMessage(string sentance);
        public abstract void ProcessData(NMEACruncher.SOutputData outputData);
        public string m_LastSourceId;
        public Dictionary<string, int> SourceIdCounts = new Dictionary<string, int>();
        public void SetSource(string sourceId)
        {
            m_LastSourceId = sourceId;
            if (!SourceIdCounts.ContainsKey(sourceId))
            {
                SourceIdCounts.Add(sourceId, 1);
            }
            else
            {
                SourceIdCounts[sourceId]++;
            }
        }
    };

    public class NMEAMessage_BoatSpeed : NMEAMessage_Base
    {
        double m_fHeading = 0.0;
        double m_fSpeed = 0.0;
        int m_iCurrentAvCount = 0;

        public override string GetKey() { return "VHW"; }
        public override void ProcessMessage(string sentance)
        {
            //$CCVHW,63.36,T,63.36,M,0.00,N,0.00,K*55
            var parts = sentance.Split(',');
            m_fSpeed *= (double)m_iCurrentAvCount;
            if (m_iCurrentAvCount > 0)
            {
                m_fHeading = SumAnglesForAverage(m_fHeading, SafeToDouble(parts[0]), m_fHeading * (double)m_iCurrentAvCount);
            }
            else
            {
                m_fHeading = SafeToDouble(parts[0]);
            }
            m_fSpeed += SafeToDouble(parts[4]);
            ++m_iCurrentAvCount;
            m_fHeading *= 1.0 / (double)m_iCurrentAvCount;
            m_fSpeed *= 1.0 / (double)m_iCurrentAvCount;
        }
        public override void ProcessData(NMEACruncher.SOutputData outputData)
        {
            outputData.SetValue(NMEACruncher.DataTypes.BoatSpeed, m_fSpeed);
            outputData.SetValue(NMEACruncher.DataTypes.BoatHeading, m_fHeading);
            m_iCurrentAvCount = 0;
        }
    };

    public class NMEAMessage_TrueWind : NMEAMessage_Base
    {
        double m_fAngle = 0.0;
        double m_fSpeed = 0.0;
        int m_iCurrentAvCount = 0;
        public override string GetKey()
        {
            return "MWD";
        }

        public override void ProcessMessage(string sentance)
        {
            //263.22,T,263.22,M,2.99,N,1.54,M*46	
            //Speed in metre's per second
            var parts = sentance.Split(',');
            //Start with a sanity check - I've seen messages with strange numbers...
            double newSpeed = parts[4].Length > 0? SafeToDouble(parts[4]) : 0.0;
            if (parts[5] == "M")
            {
                newSpeed *= kMSToKnots;
            }

            if (newSpeed > m_fSpeed + 50.0) 
            {
                Console.WriteLine("Discarded wind info '%s'", sentance);
                return;
            }
            double fTrueAngle = SafeToDouble(parts[0]);
            m_fSpeed *= (double)m_iCurrentAvCount;
            if (m_iCurrentAvCount > 0)
            {
                m_fAngle = SumAnglesForAverage(m_fAngle, fTrueAngle, m_fAngle * (double)m_iCurrentAvCount);
            }
            else
            {
                m_fAngle = fTrueAngle;
            }

            m_fSpeed += newSpeed;
            m_iCurrentAvCount++;
            m_fAngle /= (double)m_iCurrentAvCount;
            m_fSpeed /= (double)m_iCurrentAvCount;
        }

        public override void ProcessData(NMEACruncher.SOutputData outputData)
        {
            outputData.SetValue(NMEACruncher.DataTypes.TWS, m_fSpeed);
            outputData.SetValue(NMEACruncher.DataTypes.TWD, m_fAngle);
            m_iCurrentAvCount = 0;
        }
    }

    public class NMEAMessage_ApparentWind : NMEAMessage_Base
    {
        double m_fAngle = 0.0f;
        double m_fSpeed = 0.0f;
        int m_iCurrentAvCount = 0;
        public override string GetKey() { return "MWV"; }
        public override void ProcessMessage(string sentance)
        {
            //183.29,R,0.56,M,A*3C
            var parts = sentance.Split(',');
            double newSpeed = SafeToDouble(parts[2]);
            if (parts[3] == "M")
            {
                newSpeed *= kMSToKnots;
            }
            if (newSpeed > m_fSpeed + 50.0)
            {
                Console.WriteLine("Discarded wind info '%s'", sentance);
                return;
            }

            m_fSpeed *= (double)m_iCurrentAvCount;
            double newAngle = SafeToDouble(parts[0]);
            if (m_iCurrentAvCount > 0)
            {
                m_fAngle = SumAnglesForAverage(m_fAngle, newAngle, m_fAngle * (double)m_iCurrentAvCount);
            }
            else
            {
                m_fAngle = newAngle;
            }

            m_fSpeed += newSpeed;
            m_iCurrentAvCount++;

            //Keep the numbers real
            m_fAngle /= (double)m_iCurrentAvCount;
            m_fSpeed /= (double)m_iCurrentAvCount;

            if (m_fAngle < -160.0 && m_fAngle > -162.0)
            {
                Console.WriteLine("Dubious");
            }
        }
        public override void ProcessData(NMEACruncher.SOutputData outputData)
        {
            outputData.SetValue(NMEACruncher.DataTypes.AWS, m_fSpeed);
            if (m_fAngle < -160.0 && m_fAngle > -162.0)
            {
                Console.WriteLine("Dubious");
            }
            double fContained = ContainAngle180(m_fAngle);
            if (fContained < -160.0 && fContained > -162.0)
            {
                Console.WriteLine("Dubious");
            }
            outputData.SetValue(NMEACruncher.DataTypes.AWA, fContained);
            m_iCurrentAvCount = 0;
        }
    };

    public class NMEAMessage_GPSPosition : NMEAMessage_Base
    {
        double m_fPosLongitude;	//X coord
        double m_fPosLatitude;	//Y coord
        Int16 m_Hours;
        Int16 m_Minutes;
        Int16 m_Seconds;
        bool m_bHasData;

        //I've found we need to avoid out of sequence GPS 
        int m_iZeroCount;
        int m_iStrangeValueCount;
        int m_iTimeVerificationCount;
        bool m_bHasSequentialGPS;

        public override string GetKey()
        {
            return "GLL";
        }

        public override void ProcessMessage(string sentance)
        {
            //DDmm.mmm,N,DDmmm.mmm,W
            //3312.370,N,11723.521,W,211641,A*29
            var parts = sentance.Split(',');
            if (parts[0].Length == 0)
                return;

            if (parts[4].Length == 0)
                return;

            if (m_bHasData)
            {
                //I was getting some dodgy out of sequence GPS data so have started filtering out odd looking jumps - more than 30 minutes in either direction
                Int16 newHours = Convert.ToInt16(parts[4].Substring(0, 2));
                Int16 newMinutes = Convert.ToInt16(parts[4].Substring(2, 2));
                Int16 newSeconds = Convert.ToInt16(parts[4].Substring(4, 2));
                int iNewDay = 0;
                if ((newHours == 0) && (m_Hours == 23))
                {
                    iNewDay = 1;
                }
                DateTime oldTime = new DateTime(2014, 1, 1,             m_Hours,    m_Minutes,  m_Seconds);
                DateTime newTime = new DateTime(2014, 1, 1+ iNewDay,    newHours,   newMinutes, newSeconds);
                double fDeltaSeconds = (newTime - oldTime).TotalSeconds;
                if (fDeltaSeconds < 0.0)
                {
                    //Crap.
                    m_iStrangeValueCount++;
                    m_iTimeVerificationCount = 0;
                }
                else if (fDeltaSeconds > 60.0 * 5.0)    //Five minutes of non reporting GPS?
                {
                    m_iStrangeValueCount++;
                    m_iTimeVerificationCount = 0;
                }
                else
                {
                    m_iStrangeValueCount = 0;

                    if (fDeltaSeconds == 0.0)
                    {
                        ++m_iZeroCount;
                    }
                    else
                    {
                        //Looks like a good sequence
                        m_iZeroCount = 0;
                        m_iTimeVerificationCount++;
                    }
                }
                
                if (!m_bHasSequentialGPS)
                {
                    //Accept time anyway, we're still lost
                    m_Hours = newHours;
                    m_Minutes = newMinutes;
                    m_Seconds = newSeconds;
                    if (m_iTimeVerificationCount > 10)
                    {
                        m_bHasSequentialGPS = true;
                    }
                }
                else if (m_iTimeVerificationCount > 0)
                {
                    m_Hours = newHours;
                    m_Minutes = newMinutes;
                    m_Seconds = newSeconds;
                }
                else
                {
                    //Am worried about the need to look for a new base
                    return;
                }
            }

            m_bHasData = true;

            //Lat
            {
                string latDeg = parts[0].StartsWith("0") ? parts[0].Substring(1, 1) : parts[0].Substring(0, 2);
                m_fPosLatitude = SafeToDouble(latDeg);
                string latMin = parts[0].Substring(2, 6);
                m_fPosLatitude += SafeToDouble(latMin) * (1.0 / 60.0);
                if (parts[1] == "S")
                {
                    m_fPosLatitude = -m_fPosLatitude;
                }
            }

            //long
            {
                string longDeg = parts[2].StartsWith("0") ? parts[2].Substring(1, 2) : parts[2].Substring(0, 3);
                m_fPosLongitude = SafeToDouble(longDeg);
                string longMin = parts[2].Substring(3, 6);
                m_fPosLongitude += SafeToDouble(longMin) * (1.0 / 60.0);
                if (parts[3] == "W")
                {
                    m_fPosLongitude = -m_fPosLongitude;
                }
            }
        }

        public override void ProcessData(NMEACruncher.SOutputData outputData)
        {
            if (m_bHasData)
            {
                outputData.SetValue(NMEACruncher.DataTypes.GPSLong, m_fPosLongitude);
                outputData.SetValue(NMEACruncher.DataTypes.GPSLat, m_fPosLatitude);
            //    Console.WriteLine("{0:F5},{1:F5},{2},{3},{4}", m_fPosLongitude, m_fPosLatitude, m_Hours, m_Minutes, m_Seconds);
            }
            else 
            {
            //    Console.WriteLine("Long,Lat", m_fPosLongitude, m_fPosLatitude);
            //    Console.WriteLine("Skipping");
            }
        }
    };

    public class NMEAMessage_GPSsogANDcog: NMEAMessage_Base
    {
        double m_fSOG;
        double m_fCOG;
        int m_iCurrentAvCount;

        public override string GetKey()
        {
            return "VTG";
        }

        public override void ProcessMessage(string sentance)
        {
            // $CCVTG
            //273.60,T,273.60,M,0.10,N,0.19,K*50
            var parts = sentance.Split(',');
            m_fSOG *= (double)m_iCurrentAvCount;
            if (m_iCurrentAvCount > 0)
            {
                m_fCOG = SumAnglesForAverage(m_fCOG, SafeToDouble(parts[0]), m_fCOG * (double)m_iCurrentAvCount);
            }
            else
            {
                m_fCOG = SafeToDouble(parts[0]);
            }
            if (parts[5] == "M")    //?w
            {
                m_fSOG += SafeToDouble(parts[4]) * kMSToKnots;
            }
            else    //Already in knots
            {
                m_fSOG += SafeToDouble(parts[4]);
            }
            ++m_iCurrentAvCount;
            m_fSOG *= 1.0 / (double)m_iCurrentAvCount;
            m_fCOG *= 1.0 / (double)m_iCurrentAvCount;
        }

        public override void ProcessData(NMEACruncher.SOutputData outputData)
        {
            outputData.SetValue(NMEACruncher.DataTypes.GPSSOG, m_fSOG);
            outputData.SetValue(NMEACruncher.DataTypes.GPSHeading, m_fCOG);
            m_iCurrentAvCount = 0;
        }
    };

}