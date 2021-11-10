using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMEAViewer
{
    public class PolarData
    {
        class DataRow
        {
            public double m_fWindSpeed;
            double[] m_fAngles;
            double[] m_fBoatSpeeds;

            public bool FromString(string input)
            {
                Char[] delimiters = new Char[] { ',', ' ', '\t'};
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

                m_fWindSpeed = Convert.ToDouble( parts[0] );
                int numberOfFields = iCount >> 1;
                m_fBoatSpeeds = new double[numberOfFields];
                m_fAngles = new double[numberOfFields];
                for (int i = 0; i < numberOfFields; i++)
                {
                    m_fBoatSpeeds[i] = Convert.ToDouble(parts[i * 2 + 2]);
                    m_fAngles[i] = Convert.ToDouble(parts[i * 2 + 1]);
                }

                //Sometimes items get out of order... (legitimately)
                //...trickle sort forwards...
                for (int i = 0; i < numberOfFields-1; i++)
                {
                    int i0 = i;
                    int i1 = i+1;
                    double f0 = m_fAngles[ i0 ];
                    double f1 = m_fAngles[ i1 ];
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
                    int i0 = i-1;
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
                        return m_fBoatSpeeds[i] + (m_fBoatSpeeds[i] - m_fBoatSpeeds[i-1]) * fLerp;
                    }
                }
                return 0.0;
            }

            public double GetBestSpeedUp()
            {
                double bestSpd = 0.0;
                for (int i = 0; i < m_fAngles.Length; i++)
                {
                    if (m_fAngles[i] < 90.0)
                    {
                        bestSpd = Math.Max(bestSpd, m_fBoatSpeeds[i] * Math.Cos(m_fAngles[i] * AngleUtil.DegToRad));
                    }
                }
                return bestSpd;
            }

            public double GetBestSpeedDown()
            {
                double bestSpd = 0.0;
                for (int i = 0; i < m_fAngles.Length; i++)
                {
                    if (m_fAngles[i] > 90.0)
                    {
                        bestSpd = Math.Max(bestSpd, m_fBoatSpeeds[i] * Math.Abs(Math.Cos(m_fAngles[i] * AngleUtil.DegToRad)));
                    }
                }
                return bestSpd;
            }
        }

        List<DataRow> m_Rows;

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
