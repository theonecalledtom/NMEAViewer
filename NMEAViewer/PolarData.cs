using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMEAViewer
{
    public class PolarData
    {
        public class DataRow
        {
            public double m_fWindSpeed;
            double[] m_fAngles;
            double[] m_fBoatSpeeds;
            public double m_fBestAngleDown;
            public double m_fBestVMGDown;
            public double m_fBestSpeedDown;
            public double m_fBestAngleUp;
            public double m_fBestVMGUp;
            public double m_fBestSpeedUp;

            public bool FromString(string input)
            {
                Char[] delimiters = new Char[] { ',', ' ', '\t' };
                string[] parts1 = input.Split(delimiters);
                List<string> parts = new List<String>();
                foreach (string s in parts1)
                {
                    if (s.Length > 0)
                    {
                        parts.Add(s);
                    }
                }

                if (parts[0] == "!")
                {
                    return false;
                }

                int iCount = parts.Count;
                if ((iCount & 1) == 0)
                {
                    Console.WriteLine("DataRow incorrect - was expecting an odd number of results (wind speed + angle boat speed combinations). Row was:");
                    Console.WriteLine(input);
                    return false;
                }

                if (iCount < 3)
                {
                    Console.WriteLine("DataRow incorrect - was expecting at least three values in row! Row was:");
                    Console.WriteLine(input);
                    return false;
                }

                m_fWindSpeed = Convert.ToDouble(parts[0]);
                int numberOfFields = iCount >> 1;
                m_fBoatSpeeds = new double[numberOfFields];
                m_fAngles = new double[numberOfFields];
                for (int i = 0; i < numberOfFields; i++)
                {
                    m_fBoatSpeeds[i] = Convert.ToDouble(parts[i * 2 + 2]);
                    m_fAngles[i] = Convert.ToDouble(parts[i * 2 + 1]);

                    double upDownSpeed = Math.Cos(m_fAngles[i] * AngleUtil.DegToRad) * m_fBoatSpeeds[i];
                    if (upDownSpeed > m_fBestVMGUp)
                    {
                        m_fBestVMGUp = upDownSpeed;
                        m_fBestAngleUp = m_fAngles[i];
                        m_fBestSpeedUp = m_fBoatSpeeds[i];
                    }
                    else if (-upDownSpeed > m_fBestVMGDown)
                    {
                        m_fBestVMGDown = -upDownSpeed;
                        m_fBestAngleDown = m_fAngles[i];
                        m_fBestSpeedDown = m_fBoatSpeeds[i];
                    }
                }

                //Sometimes items get out of order... (legitimately)
                //...trickle sort forwards...
                for (int i = 0; i < numberOfFields - 1; i++)
                {
                    int i0 = i;
                    int i1 = i + 1;
                    double f0 = m_fAngles[i0];
                    double f1 = m_fAngles[i1];
                    if (f0 > f1)
                    {
                        m_fAngles[i0] = f1;
                        m_fAngles[i1] = f0;

                        double fTmp = m_fBoatSpeeds[i0];
                        m_fBoatSpeeds[i0] = m_fBoatSpeeds[i1];
                        m_fBoatSpeeds[i1] = fTmp;
                    }
                }

                //...and backwards...
                for (int i = numberOfFields - 1; i > 0; i--)
                {
                    int i0 = i - 1;
                    int i1 = i;
                    double f0 = m_fAngles[i0];
                    double f1 = m_fAngles[i1];
                    if (f0 > f1)
                    {
                        m_fAngles[i0] = f1;
                        m_fAngles[i1] = f0;

                        double fTmp = m_fBoatSpeeds[i0];
                        m_fBoatSpeeds[i0] = m_fBoatSpeeds[i1];
                        m_fBoatSpeeds[i1] = fTmp;
                    }
                }

                return true;
            }

            public int GetDataCounts()
            {
                return m_fAngles.Length;
            }

            public double GetNthAngle(int i)
            {
                return m_fAngles[i];
            }

            public double GetNthBoatSpeed(int i)
            {
                return m_fBoatSpeeds[i];
            }

            public double GetBoatSpeed(double fAngleToWind)
            {
                fAngleToWind = Math.Min(Math.Abs(fAngleToWind), 179.9);
                if (m_fAngles[0] > fAngleToWind)
                {
                    return m_fBoatSpeeds[0] * fAngleToWind / m_fAngles[0]; //Not very accurate!
                }

                for (int i = 1; i < m_fAngles.Length; i++)
                {
                    if (m_fAngles[i] > fAngleToWind)
                    {
                        double fLerp = (fAngleToWind - m_fAngles[i - 1]) / (m_fAngles[i] - m_fAngles[i - 1]);
                        return m_fBoatSpeeds[i - 1] + (m_fBoatSpeeds[i] - m_fBoatSpeeds[i - 1]) * fLerp;
                    }
                }
                return 0.0;
            }

            public double GetBestSpeedUp()
            {
                return m_fBestSpeedUp;
            }

            public double GetBestSpeedDown()
            {
                return m_fBestSpeedDown;
            }

            public DataRow Lerp(DataRow other, double fRowLerpProp)
            {
                DataRow r = new DataRow();

                r.m_fWindSpeed = m_fWindSpeed + (other.m_fWindSpeed - m_fWindSpeed) * fRowLerpProp;
                r.m_fBestAngleDown = m_fBestAngleDown + (other.m_fBestAngleDown - m_fBestAngleDown) * fRowLerpProp;
                r.m_fBestVMGDown = m_fBestVMGDown + (other.m_fBestVMGDown - m_fBestVMGDown) * fRowLerpProp;
                r.m_fBestSpeedDown = m_fBestSpeedDown + (other.m_fBestSpeedDown - m_fBestSpeedDown) * fRowLerpProp;
                r.m_fBestAngleUp = m_fBestAngleUp + (other.m_fBestAngleUp - m_fBestAngleUp) * fRowLerpProp;
                r.m_fBestVMGUp = m_fBestVMGUp + (other.m_fBestVMGUp - m_fBestVMGUp) * fRowLerpProp;
                r.m_fBestSpeedUp = m_fBestSpeedUp + (other.m_fBestSpeedUp - m_fBestSpeedUp) * fRowLerpProp;
               
                r.m_fAngles = new double[m_fAngles.Length];
                r.m_fBoatSpeeds = new double[m_fBoatSpeeds.Length];
                for (int i = 0; i<r.m_fAngles.Length ; i++)
                {
                    r.m_fAngles[i] = m_fAngles[i] + (other.m_fAngles[i] - m_fAngles[i]) * fRowLerpProp;
                    r.m_fBoatSpeeds[i] = m_fBoatSpeeds[i] + (other.m_fBoatSpeeds[i] - m_fBoatSpeeds[i]) * fRowLerpProp;
                }
                return r;
            }

            public DataRow Scale(double fScale)
            {
                DataRow r = new DataRow();

                r.m_fWindSpeed = (m_fWindSpeed) * fScale;
                r.m_fBestAngleDown = (m_fBestAngleDown) * fScale;
                r.m_fBestVMGDown = (m_fBestVMGDown) * fScale;
                r.m_fBestSpeedDown = (m_fBestSpeedDown) * fScale;
                r.m_fBestAngleUp = (m_fBestAngleUp) * fScale;
                r.m_fBestVMGUp = (m_fBestVMGUp) * fScale;
                r.m_fBestSpeedUp = (m_fBestSpeedUp) * fScale;

                r.m_fAngles = new double[m_fAngles.Length];
                r.m_fBoatSpeeds = new double[m_fBoatSpeeds.Length];
                for (int i = 0; i < r.m_fAngles.Length; i++)
                {
                    r.m_fAngles[i] = (m_fAngles[i]) * fScale;
                    r.m_fBoatSpeeds[i] = (m_fBoatSpeeds[i]) * fScale;
                }
                return r;
            }
        }


        List<DataRow> m_Rows;

        public DataRow GetData(double fWindSpeed)
        {      
            if (fWindSpeed <= m_Rows[0].m_fWindSpeed)
            {
                return m_Rows[0].Scale(fWindSpeed / m_Rows[0].m_fWindSpeed);
            }
            else if (fWindSpeed >= m_Rows.Last().m_fWindSpeed)
            {
                return m_Rows.Last().Scale(1.0f);
            }
            int iPreceedingRow = 1;
            while (fWindSpeed > m_Rows[iPreceedingRow + 1].m_fWindSpeed)
            {
                iPreceedingRow++;
            }

            double fRowLerpProp = (fWindSpeed - m_Rows[iPreceedingRow].m_fWindSpeed) / (m_Rows[iPreceedingRow + 1].m_fWindSpeed - m_Rows[iPreceedingRow].m_fWindSpeed);
            return m_Rows[iPreceedingRow].Lerp(m_Rows[iPreceedingRow + 1], fRowLerpProp);
        }

        public double GetBestPolarSpeed(double fWindSpeed, double fAngleToWind)
        {
            if ((m_Rows == null) || (m_Rows.Count == 0))
                return 0.0;

            if (fWindSpeed <= m_Rows[0].m_fWindSpeed)
            {
                return m_Rows[0].GetBoatSpeed(fAngleToWind) * fWindSpeed / m_Rows[0].m_fWindSpeed;
            }

            if (fWindSpeed >= m_Rows.Last().m_fWindSpeed)
            {
                return m_Rows.Last().GetBoatSpeed(fAngleToWind);
            }

            int iPreceedingRow = 1;
            while (fWindSpeed > m_Rows[iPreceedingRow+1].m_fWindSpeed)
            {
                iPreceedingRow++;
            }
            
            //Get the two results and lerp them
            double fBSP0 = m_Rows[iPreceedingRow].GetBoatSpeed(fAngleToWind);
            double fBSP1 = m_Rows[iPreceedingRow+1].GetBoatSpeed(fAngleToWind);
            double fRowLerpProp = (fWindSpeed - m_Rows[iPreceedingRow].m_fWindSpeed) / (m_Rows[iPreceedingRow + 1].m_fWindSpeed - m_Rows[iPreceedingRow].m_fWindSpeed);
            return fBSP0 + ((fBSP1 - fBSP0) * fRowLerpProp);
        }

        public double GetBestUpwindVMG(double fWindSpeed)
        {
            if ((m_Rows == null) || (m_Rows.Count == 0))
                return 0.0;

            if (fWindSpeed <= m_Rows[0].m_fWindSpeed)
            {
                return m_Rows[0].GetBestSpeedUp() * fWindSpeed / m_Rows[0].m_fWindSpeed;
            }

            if (fWindSpeed >= m_Rows.Last().m_fWindSpeed)
            {
                return m_Rows.Last().GetBestSpeedUp();
            }

            int iPreceedingRow = 1;
            while (fWindSpeed > m_Rows[iPreceedingRow + 1].m_fWindSpeed)
            {
                iPreceedingRow++;
            }

            double fBSP0 = m_Rows[iPreceedingRow].GetBestSpeedUp();
            double fBSP1 = m_Rows[iPreceedingRow + 1].GetBestSpeedUp();
            double fRowLerpProp = (fWindSpeed - m_Rows[iPreceedingRow].m_fWindSpeed) / (m_Rows[iPreceedingRow + 1].m_fWindSpeed - m_Rows[iPreceedingRow].m_fWindSpeed);
            return fBSP0 + ((fBSP1 - fBSP0) * fRowLerpProp);
        }

        public double GetBestUpwindAngle(double fWindSpeed)
        {
            if ((m_Rows == null) || (m_Rows.Count == 0))
                return 0.0;

            if (fWindSpeed <= m_Rows[0].m_fWindSpeed)
            {
                return m_Rows[0].m_fBestAngleUp;
            }

            if (fWindSpeed >= m_Rows.Last().m_fWindSpeed)
            {
                return m_Rows.Last().m_fBestAngleUp;
            }

            int iPreceedingRow = 1;
            while (fWindSpeed > m_Rows[iPreceedingRow + 1].m_fWindSpeed)
            {
                iPreceedingRow++;
            }

            double fTWA0 = m_Rows[iPreceedingRow].m_fBestAngleUp;
            double fTWA1 = m_Rows[iPreceedingRow + 1].m_fBestAngleUp;
            double fRowLerpProp = (fWindSpeed - m_Rows[iPreceedingRow].m_fWindSpeed) / (m_Rows[iPreceedingRow + 1].m_fWindSpeed - m_Rows[iPreceedingRow].m_fWindSpeed);
            return fTWA0 + ((fTWA1 - fTWA0) * fRowLerpProp);
        }

        public double GetBestDownwindAngle(double fWindSpeed)
        {
            if ((m_Rows == null) || (m_Rows.Count == 0))
                return 0.0;

            if (fWindSpeed <= m_Rows[0].m_fWindSpeed)
            {
                return m_Rows[0].m_fBestAngleDown;
            }

            if (fWindSpeed >= m_Rows.Last().m_fWindSpeed)
            {
                return m_Rows.Last().m_fBestAngleDown;
            }

            int iPreceedingRow = 1;
            while (fWindSpeed > m_Rows[iPreceedingRow + 1].m_fWindSpeed)
            {
                iPreceedingRow++;
            }

            double fTWA0 = m_Rows[iPreceedingRow].m_fBestAngleDown;
            double fTWA1 = m_Rows[iPreceedingRow + 1].m_fBestAngleDown;
            double fRowLerpProp = (fWindSpeed - m_Rows[iPreceedingRow].m_fWindSpeed) / (m_Rows[iPreceedingRow + 1].m_fWindSpeed - m_Rows[iPreceedingRow].m_fWindSpeed);
            return fTWA0 + ((fTWA1 - fTWA0) * fRowLerpProp);
        }

        public double GetBestDownwindVMG(double fWindSpeed)
        {
            if ((m_Rows == null) || (m_Rows.Count == 0))
                return 0.0;

            if (fWindSpeed <= m_Rows[0].m_fWindSpeed)
            {
                return m_Rows[0].GetBestSpeedDown() * fWindSpeed / m_Rows[0].m_fWindSpeed;
            }

            if (fWindSpeed >= m_Rows.Last().m_fWindSpeed)
            {
                return m_Rows.Last().GetBestSpeedDown();
            }

            int iPreceedingRow = 1;
            while (fWindSpeed > m_Rows[iPreceedingRow + 1].m_fWindSpeed)
            {
                iPreceedingRow++;
            }

            double fBSP0 = m_Rows[iPreceedingRow].GetBestSpeedDown();
            double fBSP1 = m_Rows[iPreceedingRow + 1].GetBestSpeedDown();
            double fRowLerpProp = (fWindSpeed - m_Rows[iPreceedingRow].m_fWindSpeed) / (m_Rows[iPreceedingRow + 1].m_fWindSpeed - m_Rows[iPreceedingRow].m_fWindSpeed);
            return fBSP0 + ((fBSP1 - fBSP0) * fRowLerpProp);
        }

        public bool Load(string fileName)
        {
            if (!System.IO.File.Exists(fileName))
            {
                Console.WriteLine("Polar file '{0}' does not exist", fileName);
                return false;
            }
            string[] fileData = System.IO.File.ReadAllLines(fileName, Encoding.ASCII);

            if (fileData.Length < 1)
            {
                Console.WriteLine("Polar file '{0}' does not contain valid content", fileName);
                return false;
            }

            m_Rows = new List<DataRow>();
            foreach(string s in fileData)
            {
                DataRow row = new DataRow();
                if (row.FromString(s))
                {
                    m_Rows.Add(row);
                }
            }

            return true;
        }
    }
}
