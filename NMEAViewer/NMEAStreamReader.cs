using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMEAViewer
{
    public class NMEAStreamReader
    {
        private Dictionary<string, NMEAMessage_Base> m_MessageProcessors;

        public void AddProcessor(NMEAMessage_Base newProcessor)
        {
            m_MessageProcessors.Add(newProcessor.GetKey(), newProcessor);
        }

        public NMEAStreamReader()
        {
            m_MessageProcessors = new Dictionary<string, NMEAMessage_Base>();
            AddProcessor( new NMEAMessage_TrueWind() );
            AddProcessor( new NMEAMessage_ApparentWind() );
            AddProcessor( new NMEAMessage_GPSPosition());
            AddProcessor( new NMEAMessage_BoatSpeed());
            AddProcessor(new NMEAMessage_GPSsogANDcog());
        }
        
        int m_iSentancePoint = 0;
        const int kMaxSentance = 512;
        char[] m_Sentance = new char[kMaxSentance];
        public void ProcessData(char[] pData, double fElaspedTime)
        {
            int iReadingPoint = 0;

            //Read sentances out of the buffer.
            //We may already be reading a sentance
            while (iReadingPoint < pData.Length)
            {
                //Skip potential garbage of first read, or any cruft in between messages
                if (m_iSentancePoint == 0)
                {
                    while ((iReadingPoint < pData.Length) && (pData[iReadingPoint] != '$'))
                    {
                        iReadingPoint++;
                    }
                }

                while ((iReadingPoint < pData.Length) && (m_iSentancePoint < kMaxSentance) && (pData[iReadingPoint] != '\r'))
                {
                    if (pData[iReadingPoint] == '$')
                    {
                        if (m_iSentancePoint != 0)
                        {
                            m_iSentancePoint = 0;	//Always the start, something might have gone wrong for this to happen
                        }
                    }
                    m_Sentance[m_iSentancePoint] = pData[iReadingPoint];
                    m_iSentancePoint++;

                    iReadingPoint++;
                }

                if (iReadingPoint == pData.Length)
                    return;

                if (pData[iReadingPoint] == '\r')
                {
                    //We reached the end of a sentance so process it
                    if (m_iSentancePoint < kMaxSentance)
                    {
                        m_Sentance[m_iSentancePoint] = '\0';
                    }

                    if (VerifyChecksum(m_Sentance, m_iSentancePoint))
                    {
                        //Process sentance
                        ProcessSentance(m_Sentance, fElaspedTime);
                    }

                    m_iSentancePoint = 0;
                }
            }
        }

        private bool VerifyChecksum(char[] nmeaSent, int iLength)
        {
            string asStr = new String(nmeaSent, 0, iLength);// (nmeaSent);// = Convert.ToString(nmeaSent);
            int iStartSplit = asStr.LastIndexOf('*');
            if (iStartSplit <= 0)
                return false;
            string chkSum = asStr.Substring(iStartSplit + 1);
            int iValueInString = Convert.ToInt32(chkSum, 16); //Convert.ToInt32(chkSum);

            int iReadPoint = 0;
            if (nmeaSent[iReadPoint] != '$')
            {
                return false;
            }

            ++iReadPoint;
            int iValueCalculated = 0;
            while (nmeaSent[iReadPoint] != 0 && (nmeaSent[iReadPoint] != '*'))
            {
                iValueCalculated ^= nmeaSent[iReadPoint];
                ++iReadPoint;
            }

            return iValueInString == iValueCalculated;
        }

        private void ProcessSentance(char []fullMsg, double fElaspedTime)
        {
            int iTerminator = 0;
            while (iTerminator < fullMsg.Length && fullMsg[iTerminator] != '\0')
            {
                ++iTerminator;
            }
            string sentance = new string(fullMsg, 0, iTerminator);
            string messageType = sentance.Substring(3,3);

            if (m_MessageProcessors.ContainsKey(messageType))
            {
                var processor = m_MessageProcessors[messageType];
                if (processor != null)
                {
                    processor.SetSource(sentance.Substring(1, 2));
                    processor.ProcessMessage(sentance.Substring(7));
                    processor.m_fElapsedTime = fElaspedTime;
                }
            }
        }

        public void ProcessCurrentData(NMEACruncher.SOutputData outputData, PolarData polarData, double fElapsedTime)
        {
            outputData.SetValue(NMEAViewer.NMEACruncher.DataTypes.Time, fElapsedTime);

            //grab data
            foreach (var entry in m_MessageProcessors)
            {
                entry.Value.ProcessData(outputData);
            }

            //fill in TWA, any other values that need filling in (currently assumes we have all this data)
            double fRelativeWind = outputData.GetValue(NMEACruncher.DataTypes.TWD) - outputData.GetValue(NMEACruncher.DataTypes.BoatHeading);
            while (fRelativeWind > 180.0f)
            {
                fRelativeWind -= 360.0f;
            }
            while (fRelativeWind < -180.0f)
            {
                fRelativeWind += 360.0f;
            }
            outputData.SetValue(NMEACruncher.DataTypes.TWA, fRelativeWind);

            //If we have 
            if (polarData != null)
            {
                 double fPolarSpd = polarData.GetBestPolarSpeed(outputData.GetValue(NMEACruncher.DataTypes.TWS), outputData.GetValue(NMEACruncher.DataTypes.TWA));
                 outputData.SetValue(NMEACruncher.DataTypes.PolarSpeed, fPolarSpd);

                if (fPolarSpd > 0.0)
                {
                    double fPercentage = 100.0 * outputData.GetValue(NMEACruncher.DataTypes.BoatSpeed) / fPolarSpd;
                    outputData.SetValue(NMEACruncher.DataTypes.PropPolarSpeed, Math.Min(200.0, fPercentage));
                }
            }

            NMEACruncher.PostProcess(outputData);
        }
    }
}
