using System;
using System.Collections.Generic;

namespace NMEAViewer
{
    public class NMEACruncher
    {
        //3,4   == fixed reading of gps longitude
        //5     == added VTG (GPS SOG and COG)
        //6     == fixed problem where initial gps was zero
        //7     == remember to write m_HasData, write sparse data
        //8     == plus Polar data      --  Shouldn't need to save...
        //9     == plus Polar % data    --  Shouldn't need to save
        //                              --  Also shouldn't need to save TWA as it's calculated
        int iProcessedVersion = 9;
        List<SOutputData> m_CrunchedData;
        double m_fTimePerEntry = 0.0;
        public double m_fTimeOfLastEntry = 0.0;
        PolarData m_BoatPolars;
        public enum DataTypes
        {
            Time,
            AWS,
            AWA,
            TWS,
            TWD,
            TWA,
            GPSHeading,
            GPSSOG,
            GPSLong,            
            GPSLat,
            BoatSpeed,
            BoatHeading,
            PolarSpeed,
            PropPolarSpeed
        };

        public enum DataRangeTypes
        {
            NoGraph,            //Don't try to plot this!
            BoatSpeed,          //Plot data 0->Max
            WindSpeed,          //Plot data 0->Max (use different Max to BoatSpeed)
            RelativeAngle,      //Plot data -180 -> 180
            Direction,          //Plot data 0 -> 360
            Percentage          //Plot data 0 -> 100
        };

        public static int GetNumDataRanges()
        {
            return Enum.GetNames(typeof(DataRangeTypes)).Length;
        }

        public double GetRangePadding(int iRangeType)
        {
            double[] fPadding = {
                0.0,
                1.0,
                1.0,
                10.0,
                10.0,
                5.0
            };

            return fPadding[ iRangeType ];
        }
    
        public static System.Drawing.Color GetColorForType(int type)
        {
            System.Drawing.Color[] colors = {
                System.Drawing.Color.Black,         //Time
                System.Drawing.Color.White,         //AWS
                System.Drawing.Color.Yellow,        //AWA
                System.Drawing.Color.DarkRed,       //TWS
                System.Drawing.Color.LavenderBlush, //TWD
                System.Drawing.Color.Purple,        //TWA
                System.Drawing.Color.Pink,          //GPSHeading
                System.Drawing.Color.SlateGray,     //GPSSOG
                System.Drawing.Color.Black,         //GPSLong
                System.Drawing.Color.Black,         //GPSLat
                System.Drawing.Color.Blue,          //BoatSpeed
                System.Drawing.Color.DarkSeaGreen,  //BoatHeading
                System.Drawing.Color.LightGray,     //PolarSpeed
                System.Drawing.Color.DarkGray       //PropPolarSpeed
            };
            return colors[type];
        }
        public static DataRangeTypes GetDataRangeForType(int type)
        {
            DataRangeTypes[] dataRanges ={
                    DataRangeTypes.NoGraph,       //Time,            
                    DataRangeTypes.WindSpeed,     //"AWS",
                    DataRangeTypes.RelativeAngle, //"AWA",
                    DataRangeTypes.WindSpeed,     //"TWS",
                    DataRangeTypes.Direction,     //"TWD",
                    DataRangeTypes.RelativeAngle, //"TWA",
                    DataRangeTypes.Direction,     //"GPS_Heading",
                    DataRangeTypes.BoatSpeed,     //"GPS_SOG",
                    DataRangeTypes.NoGraph,       //GPS_Long,            
                    DataRangeTypes.NoGraph,       //GPS_Lat,            
                    DataRangeTypes.BoatSpeed,     //"BoatSpeed",
                    DataRangeTypes.Direction,     //"BoatHeading"
                    DataRangeTypes.BoatSpeed,     //"PolarSpeed"
                    DataRangeTypes.Percentage     //"PropPolarSpeed"
                };

            if (type >= 0 && type < GetNumDataTypes())
            {
                return dataRanges[(int)type];
            }
            return DataRangeTypes.BoatSpeed;
        }

        public static int GetNumDataTypes()
        {
            return Enum.GetNames(typeof(DataTypes)).Length;
        }

        public static System.String GetNameOfEntry(int type)
        {
            return Enum.GetNames(typeof(DataTypes))[type];
        }

        public static int GetIndexOfDataType(System.String dataName)
        {
            for (int i = 0; i < GetNumDataTypes(); i++)
            {
                if (GetNameOfEntry(i) == dataName)
                {
                    return i;
                }   
            }
            return -1;
        }

        public bool HasDataForEntry(int type)
        {
            if (type < GetNumDataTypes())
            {
                return true;
            }
            return false;
        }

        public class SOutputData
        {
            static Random random = new Random();
           
            public void SetValue(DataTypes dataType, double fValue)
            {
                SetValue((int)dataType, fValue);
            }

            public void SetValue(int dataType, double fValue)
            {
                if (dataType < GetNumDataTypes())
                {
                    m_fDataValues[dataType] = fValue;
                    m_HasData |= (1 << dataType);
                }
            }

            public bool HasData(NMEACruncher.DataTypes dataType)
            {
                return HasData((int)dataType);  
            }

            public bool HasData(int dataType)
            {
                return (m_HasData & (1 << dataType)) != 0;  
            }

            public double GetValue(int dataType)
            {
                if (dataType < GetNumDataTypes())
                {
                    return m_fDataValues[dataType];
                }
                return 0.0;
            }

            public double GetValue(DataTypes dataType)
            {
                return m_fDataValues[(int)dataType];
            }

            private double GetRandomNumber(double minimum, double maximum)
            {
                return random.NextDouble() * (maximum - minimum) + minimum;
            }
            
            public void Write(System.IO.BinaryWriter bwriter)
            {
                bwriter.Write(m_HasData);
                for (int iData = 0; iData < GetNumDataTypes(); iData++)
                {
                    if ((m_HasData & (1 << iData)) != 0)
                    {
                        bwriter.Write(m_fDataValues[iData]);
                    }
                }
            }

            public void Read(System.IO.BinaryReader breader)
            {
                m_HasData = breader.ReadInt32();
                for (int iData = 0; iData < GetNumDataTypes(); iData++)
                {
                    if ((m_HasData & (1 << iData)) != 0)
                    {
                        m_fDataValues[iData] = breader.ReadDouble();
                    }
                }
            }

            //public void RandForTest()
            //{
            //    int iDataCount = GetNumDataTypes();
            //    for (int i = 0; i < iDataCount; i++)
            //    {
            //        m_fDataValues[i] = GetRandomNumber(-i, i);
            //    }
            //}

            public void Lerp(double fLerp, SOutputData a, SOutputData b)
            {
                int iDataCount = GetNumDataTypes();
                for (int i = 0; i < iDataCount; i++)
                {
                    m_fDataValues[i] = a.m_fDataValues[i] + (b.m_fDataValues[i] - a.m_fDataValues[i]) * fLerp;
                }
            }

            public double Lerp(int iDataTYpe, double fLerp, SOutputData a, SOutputData b)
            {
                int iDataCount = GetNumDataTypes();
                if (iDataTYpe < iDataCount)
                {
                    return a.m_fDataValues[iDataTYpe] + (b.m_fDataValues[iDataTYpe] - a.m_fDataValues[iDataTYpe]) * fLerp;
                }
                return 0.0;
            }

            public SOutputData()
            {
                int iDataCount = GetNumDataTypes();
                m_fDataValues = new double[iDataCount];
                for (int i=0 ; i<iDataCount ; i++)
                {
                    m_fDataValues[i] = 0.0;
                }
            }
            double[] m_fDataValues;
            Int32 m_HasData;
        }
        
        public double GetEndTime()
        {
            return m_fTimeOfLastEntry;
        }

        public int GetDataCount()
        {
            if (m_CrunchedData == null)
                return 0;
            return m_CrunchedData.Count;
        }

        public int GetIndexForTime(double fTime)
        {
            if (m_CrunchedData == null)
            {
                return -1;
            }

            if (fTime > m_fTimeOfLastEntry)
            {
                return m_CrunchedData.Count - 1;
            }

            return System.Math.Min((int)(fTime * ((double)m_CrunchedData.Count) / m_fTimeOfLastEntry), m_CrunchedData.Count-1);
        }

        public bool HasDataAtIndex(int iIndex, DataTypes dataType)
        {
            return m_CrunchedData[iIndex].HasData( dataType );
        }

        public bool HasDataAtIndex(int iIndex, int dataType)
        {
            return m_CrunchedData[iIndex].HasData(dataType);
        }

        public double GetDataAtIndex(int iIndex, int iType)
        {
            return m_CrunchedData[iIndex].GetValue(iType);
        }

        public double GetDataAtIndex(int iIndex, DataTypes type)
        {
            return m_CrunchedData[iIndex].GetValue(type);
        }

        public SOutputData GetLerpedDataAtTime(double fTimeToSample)
        {
            if (m_CrunchedData == null || m_CrunchedData.Count == 0)
            {
                return null;
            }
            if (fTimeToSample > m_fTimeOfLastEntry)
            {
                //Return the end
                return m_CrunchedData[m_CrunchedData.Count-1];
            }
            if (fTimeToSample < 0.0)
            {
                //The begining
                return m_CrunchedData[0];
            }

            SOutputData ret = new SOutputData();

            double fCount = (double)m_CrunchedData.Count;
            double fIndex = fCount * fTimeToSample / m_fTimeOfLastEntry;
            double fLerp = fIndex - System.Math.Floor(fIndex);
            int iIndexA = System.Math.Min((int)fIndex, m_CrunchedData.Count-1);
            int iIndexB = System.Math.Min(iIndexA + 1, m_CrunchedData.Count-1);

            ret.Lerp(fLerp, m_CrunchedData[iIndexA], m_CrunchedData[iIndexB]);
            return ret;
        }

        public double GetLerpedDataAtTime(int dataType, double fTimeToSample)
        {
            if (m_CrunchedData == null || m_CrunchedData.Count == 0)
            {
                return 0.0;
            }
            if (fTimeToSample > m_fTimeOfLastEntry)
            {
                //Return the end
                return m_CrunchedData[m_CrunchedData.Count - 1].GetValue(dataType);
            }
            if (fTimeToSample < 0.0)
            {
                //The begining
                return m_CrunchedData[0].GetValue(dataType);
            }

            SOutputData ret = new SOutputData();

            double fCount = (double)m_CrunchedData.Count;
            double fIndex = fCount * fTimeToSample / m_fTimeOfLastEntry;
            double fLerp = fIndex - System.Math.Floor(fIndex);
            int iIndexA = System.Math.Min((int)fIndex, m_CrunchedData.Count - 1);
            int iIndexB = System.Math.Min(iIndexA + 1, m_CrunchedData.Count - 1);

            return ret.Lerp(dataType, fLerp, m_CrunchedData[iIndexA], m_CrunchedData[iIndexB]);
        }

        public bool HasMatchingProcessedFile(System.String fileName)
        {
            if (!System.IO.File.Exists(fileName + ".prc"))
            {
                return false;
            }
            System.IO.BinaryReader breader = new System.IO.BinaryReader(new System.IO.FileStream(fileName + ".prc", System.IO.FileMode.Open));
            bool bRet = breader.ReadInt32() == iProcessedVersion;
            breader.Close();
            return bRet;
        }

        public int FindDataNearLatAndLong(double fLat, double fLong, double fTolerance)
        {
            if (m_CrunchedData != null)
            {
                //TODO: Sparse spatial map?
                SOutputData best = null;
                double bestDist = fTolerance;
                foreach(SOutputData data in m_CrunchedData)
                {
                    double fDist = MapWindow.CalculateDistanceBetweenLongsAndLats(fLong, fLat, data.GetValue(DataTypes.GPSLong), data.GetValue(DataTypes.GPSLat));
                    if (fDist < bestDist)
                    {
                        bestDist = fDist;
                        best = data;
                    }
                }

                if (best != null)
                {
                    return m_CrunchedData.IndexOf(best);
                }
            }
            return -1;
        }

        public void ProcessNewData(string sdata, NMEAStreamReader streamReader, double fElapsedTime)
        {
            streamReader.ProcessData(sdata.ToCharArray(), fElapsedTime);

            SOutputData newData = new SOutputData();
            if (m_CrunchedData == null)
            {
                m_CrunchedData = new List<SOutputData>();
            }
            m_CrunchedData.Add(newData);
            streamReader.ProcessCurrentData(newData, m_BoatPolars, fElapsedTime);

            m_fTimeOfLastEntry = Math.Max(m_fTimeOfLastEntry, fElapsedTime);
        }

        public void SetPolarData(PolarData data)
        {
            m_BoatPolars = data;

            //Recalculate polar speeds for all current data
            if (m_CrunchedData != null)
            {
                foreach (SOutputData outputData in m_CrunchedData)
                {
                    double fPolarSpd = 0.0;
                    if (m_BoatPolars != null)
                    {
                        fPolarSpd = data.GetBestPolarSpeed(outputData.GetValue(DataTypes.TWS), outputData.GetValue(DataTypes.TWA));
                    }

                    //Make sure we've cleared our data
                    outputData.SetValue(DataTypes.PolarSpeed, fPolarSpd);

                    if (fPolarSpd > 0.0)
                    {
                        double fPercentage = 100.0 * outputData.GetValue(DataTypes.BoatSpeed) / fPolarSpd;

                        outputData.SetValue(DataTypes.PropPolarSpeed, Math.Min(100.0, fPercentage));
                    }
                }
            }
        }

        public bool ProcessFile(System.String fileName, NMEAStreamReader streamReader)
        { 
            //Load file into memory... 
            System.IO.StreamReader s = new System.IO.StreamReader(fileName);
            if (s == null)
            {
                return false;
            }

            m_CrunchedData = new List<SOutputData>();

            DataReader data = new DataReader();
            if (data.StartRead(s.BaseStream))
            {
                //For now just crunch all the data into it's processed form
                while (data.ReadSection())
                { 
                    SOutputData newData = new SOutputData();
                    m_CrunchedData.Add(newData);
                    streamReader.ProcessData(data.GetData().ToCharArray(), data.m_fElapsedTime);
                    streamReader.ProcessCurrentData(newData, m_BoatPolars, data.m_fElapsedTime);
                }

                if (m_CrunchedData.Count > 0)
                {
                    m_fTimeOfLastEntry = m_CrunchedData[m_CrunchedData.Count - 1].GetValue(0);
                    m_fTimePerEntry = m_fTimeOfLastEntry / (double)m_CrunchedData.Count;
                }
            }

            WriteProcessedData(fileName + ".prc");

            s.Close();
            return true;
        }

        public void WriteProcessedData(string fileName)
        {
            System.IO.BinaryWriter bwriter = new System.IO.BinaryWriter(new System.IO.FileStream(fileName, System.IO.FileMode.Create));

            bwriter.Write(iProcessedVersion);
            bwriter.Write(m_fTimePerEntry);
            bwriter.Write(m_fTimeOfLastEntry);
            bwriter.Write(GetNumDataTypes());
            for (int i = 0; i < GetNumDataTypes();  i++)
            {
                bwriter.Write(GetNameOfEntry(i));
            }
            bwriter.Write(m_CrunchedData.Count);
            foreach (SOutputData output in m_CrunchedData)
            {
                output.Write(bwriter);
            }

            bwriter.Close();
        }

        public void ReadProcessedData(string fileName)
        {
            System.IO.BinaryReader breader = new System.IO.BinaryReader(new System.IO.FileStream(fileName, System.IO.FileMode.Open));

            if (breader.ReadInt32() == iProcessedVersion)
            {
                m_fTimePerEntry = breader.ReadDouble();
                m_fTimeOfLastEntry = breader.ReadDouble();

                //Look at what data is in the file and match it against our own data
                int iNumDataTypesInFile = breader.ReadInt32();
                string[] dataTypeNamesInFile = new string[iNumDataTypesInFile];
                int[] iDataMappingArray = new int[iNumDataTypesInFile];
                for (int i = 0; i < iNumDataTypesInFile; i++)
                {
                    dataTypeNamesInFile[i] = breader.ReadString();
                    iDataMappingArray[i] = GetIndexOfDataType(dataTypeNamesInFile[i]);
                }

                m_CrunchedData = new List<SOutputData>();
                int iDataCount = breader.ReadInt32();
                for (int i = 0; i < iDataCount; i++ )
                {
                    SOutputData output = new SOutputData();
                    int hasData = breader.ReadInt32();
                    for (int iDataInFile = 0; iDataInFile < iNumDataTypesInFile; iDataInFile++)
                    {
                        if ((hasData & (1 << iDataInFile)) != 0)
                        {
                            double fValue = breader.ReadDouble();
                            if (iDataMappingArray[iDataInFile] > -1)
                            {
                                output.SetValue(iDataMappingArray[iDataInFile], fValue);
                            }
                        }
                    }
                    m_CrunchedData.Add(output);
                }
            }
            breader.Close();
        }
    }
}
