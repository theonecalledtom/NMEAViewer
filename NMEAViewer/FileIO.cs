using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NMEAViewer
{
    public class DataSegment
    {
        public DateTime m_TimeOfData;
        public string m_Data;

        public DataSegment(string data)
        {
            m_TimeOfData = DateTime.UtcNow;
            m_Data = data;
        }

        public DataSegment(System.IO.BinaryReader reader)
        {
            try
            {
                m_TimeOfData = DateTime.FromBinary(reader.ReadInt64());
                m_Data = reader.ReadString();
            }
            catch// (System.IO.EndOfStreamException)
            {
            }
        }

        public void Write(System.IO.BinaryWriter writer)
        {
            writer.Write(m_TimeOfData.ToBinary());
            writer.Write(m_Data);
        }
    }

    class CPP_SYSTEMTIME
    {
        int wYear;
        int wMonth;
        int wDayOfWeek;
        int wDay;
        int wHour;
        int wMinute;
        int wSecond;
        int wMilliseconds;
        public void Read(System.IO.BinaryReader reader)
        {
            wYear = reader.ReadInt16();
            wMonth = reader.ReadInt16();
            wDayOfWeek = reader.ReadInt16();
            wDay = reader.ReadInt16();
            wHour = reader.ReadInt16();
            wMinute = reader.ReadInt16();
            wSecond = reader.ReadInt16();
            wMilliseconds = reader.ReadInt16();
        }

        public DateTime ToDateTime()
        {
            return new DateTime(wYear, wMonth, wDay, wHour, wMinute, wSecond, wMilliseconds);
        }
    }

    public class DataWriter
    {
        System.IO.BinaryWriter m_DataWriter;

        public void Start(System.IO.Stream s)
        {
            m_DataWriter = new System.IO.BinaryWriter(s);

            if (m_DataWriter != null)
            {
                m_DataWriter.Write(DataReader.kConnectionDataVersion);
                m_DataWriter.Write(DateTime.UtcNow.ToBinary());
            }
        }

        public void WriteNMEADataSegment(string sdata)
        {
            if (m_DataWriter == null)
                return;

            DataSegment segment = new DataSegment(sdata);
            segment.Write(m_DataWriter);
        }

        public void End()
        {
            if (m_DataWriter == null)
                return;
            m_DataWriter.Close();
            m_DataWriter = null;
        }
    }

    public class DataReader
    {
        public const int kMinConnectionDataVersion = 100;
        public const int kConnectionDataVersion = kMinConnectionDataVersion + 1;
        public const int kOldCPPDataVersion = 1;

        System.IO.BinaryReader m_DataReader;
        //NMEAStreamReader m_NMEAReader;
        DateTime m_StartTime;    //To estimate original packet arrival time
        DateTime m_LastTime;     //To estimate original packet arrival time
        DateTime m_LastUTC;
        string m_String;
        public double m_fElapsedTime;
        int m_iVersion = 0;
        public int m_iDataRead = 0;
        public string GetData()
        { 
            return m_String;
        }
        public bool StartRead(System.IO.Stream s)
        {
            if (s.Length < 32)  //Arbitrary safety check.... make sure we have data! TODO: better format verification
                return false;

            m_DataReader = new System.IO.BinaryReader(s);
            if (m_DataReader == null)
                return false;

            //m_NMEAReader = nmeaReader;

            m_iVersion = m_DataReader.ReadInt32();
            if (m_iVersion >= kConnectionDataVersion)
            {
                m_StartTime = DateTime.FromBinary( m_DataReader.ReadInt64() );
                m_LastTime = m_StartTime;
                m_LastUTC = m_StartTime.ToUniversalTime();
                return true;
            }
            else if (m_iVersion == kOldCPPDataVersion)
            {
                CPP_SYSTEMTIME cppTime = new CPP_SYSTEMTIME();
                cppTime.Read(m_DataReader);
                m_StartTime = cppTime.ToDateTime();
                m_LastTime = m_StartTime;
                m_LastUTC = m_LastTime.ToUniversalTime();
                return true;
            }

            //Unknown format, make sure we don't try and read it
            m_DataReader = null;
            return false;
        }

        public bool ReadSection()
        {
            if (m_DataReader == null)
            {
                return false;
            }
                
            if (m_DataReader.BaseStream.Position == m_DataReader.BaseStream.Length)
            {
                return false;
            }

            if (m_StartTime == null)
            {
                return false;
            }

            if (m_iVersion >= kMinConnectionDataVersion)
            {
                //Current format
                DataSegment newSegment = new DataSegment(m_DataReader);
                if (newSegment.m_Data == null)
                {
                    return false;
                }
                m_iDataRead += newSegment.m_Data.Length;
                m_fElapsedTime = (newSegment.m_TimeOfData - m_StartTime).TotalSeconds;
                m_LastTime = newSegment.m_TimeOfData;
                m_LastUTC = m_LastTime.ToUniversalTime();
                //if (m_NMEAReader != null)
                //{
                //    //m_NMEAReader.ProcessData(newSegment.m_Data.ToCharArray(), m_fElapsedTime);
                //}
                m_String = newSegment.m_Data;
                return true;
            }
            else 
            {
                //Read length (int32)
                int length = m_DataReader.ReadInt32();
                //Old cpp format (nmeaconnection.cpp)
                //Read time (float)
                m_iDataRead += length;
                m_fElapsedTime = m_DataReader.ReadSingle();

                //Calculate time
                m_LastTime = m_StartTime.AddSeconds(m_fElapsedTime);
                m_LastUTC = m_LastTime.ToUniversalTime();

                //Read NMEA data to char array
                //char[] charArray = ;
                m_String = new string(m_DataReader.ReadChars(length));

                //if (m_NMEAReader != null)
                //{
                //    m_NMEAReader.ProcessData(charArray, m_fElapsedTime);
                //}
                return true;
            }
        }   
    }
}