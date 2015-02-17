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
                string[] parts = input.Split(delimiters);
                int iCount = parts.Length;
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
                    m_fBoatSpeeds[i] = Convert.ToDouble(parts[i * 2 + 1]);
                    m_fAngles[i] = Convert.ToDouble(parts[i * 2 + 2]);
                }
                return true;
            }

            public double GetBoatSpeed(double fAngleToWind)
            {
                fAngleToWind = Math.Min(Math.Abs(fAngleToWind), 179.9);
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
        }

        DataRow[] m_Rows;

        public double GetBestPolarSpeed(double fWindSpeed, double fAngleToWind)
        {
            if ((m_Rows == null) || (m_Rows.Length == 0))
                return 0.0;

            if (fWindSpeed <= m_Rows[0].m_fWindSpeed)
            {
                return m_Rows[0].GetBoatSpeed(fAngleToWind) * fWindSpeed / m_Rows[0].m_fWindSpeed;
            }

            if (fWindSpeed >= m_Rows[m_Rows.Length - 1].m_fWindSpeed)
            {
                return m_Rows[m_Rows.Length - 1].GetBoatSpeed(fAngleToWind);
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

            m_Rows = new DataRow[fileData.Length];
            for (int i = 0; i < m_Rows.Length; i++ )
            {
                m_Rows[i] = new DataRow();
                m_Rows[i].FromString(fileData[i]);
            }

            return true;
        }
    }
}
