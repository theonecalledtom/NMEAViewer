using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMEAViewer
{
    class AngleUtil
    {
        public const double PI = 3.141592653589793;
        public const double DegToRad = (2.0 * PI / 360.0);
        public const double RadToDeg = 1.0f / DegToRad;

        public static void Swap(ref double rA, ref double rB)
        {
            double fTmp = rA;
            rA = rB;
            rB = fTmp;
        }

        public static double ShortAngle(double fAngle1, double fAngle2)
        {
            double fDelta = fAngle2 - fAngle1;
            while (fDelta > 180.0f) fDelta -= 360.0f;
            while (fDelta < -180.0f) fDelta += 360.0f;
            return fDelta;
        }

        public static double ContainAngle0To360(double fAngle)
        {
            while (fAngle > 360.0f) fAngle -= 360.0f;
            while (fAngle < 0.0f) fAngle += 360.0f;
            return fAngle;
        }

        public static double ContainAngleMinus180To180(double fAngle)
        {
            while (fAngle > 180.0f) fAngle -= 360.0f;
            while (fAngle < -180.0f) fAngle += 360.0f;
            return fAngle;
        }

        public static double CalculateAngleAverage(NMEACruncher data, NMEACruncher.DataTypes dataType, int iStartIndex, int iEndIndex)
        {
            double fPin = data.GetDataAtIndex(iStartIndex, dataType);
            double fAverage = fPin;
            for (int i = iStartIndex + 1; i < iEndIndex; i++)
            {
                double fInputAngle = data.GetDataAtIndex(i, dataType);
                while (fInputAngle - fPin > 180.0f) fInputAngle -= 360.0f;
                while (fInputAngle - fPin < -180.0f) fInputAngle += 360.0f;
                fAverage += fInputAngle;
            }
            fAverage /= (double)Math.Max(1, iEndIndex - iStartIndex);
            return fAverage;
        }

        public static void CalculateAngleRangeAndAverage(NMEACruncher data, NMEACruncher.DataTypes dataType, int iStartIndex, int iEndIndex, ref double fMin, ref double fMax, ref double fAverage)
        {
            double fPin = data.GetDataAtIndex(iStartIndex, dataType);
            fAverage = fPin;
            fMin = fPin;
            fMax = fPin;
            for (int i = iStartIndex + 1; i < iEndIndex; i++)
            {
                double fInputAngle = data.GetDataAtIndex(i, dataType);
                while (fInputAngle - fPin > 180.0f) fInputAngle -= 360.0f;
                while (fInputAngle - fPin < -180.0f) fInputAngle += 360.0f;
                fMin = Math.Min(fMin, fInputAngle);
                fMax = Math.Max(fMax, fInputAngle);
                fAverage += fInputAngle;
            }
            fAverage /= (double)Math.Max(1, iEndIndex - iStartIndex);
        }

        public static void CalculateRangeAndAverage(NMEACruncher data, NMEACruncher.DataTypes dataType, int iStartIndex, int iEndIndex, ref double fMin, ref double fMax, ref double fAverage)
        {
            fAverage = data.GetDataAtIndex(iStartIndex, dataType);
            fMax = fAverage;
            fMin = fAverage;
            for (int i = (iStartIndex + 1); i < iEndIndex; i++)
            {
                double fValue = data.GetDataAtIndex(i, dataType);
                fMin = Math.Min(fMin, fValue);
                fMax = Math.Max(fMax, fValue);
                fAverage += fValue;
            }
            fAverage *= 1.0f / (double)Math.Max(1, iEndIndex - iStartIndex);
        }

        public static double CalculateAverage(NMEACruncher data, NMEACruncher.DataTypes dataType, int iStartIndex, int iEndIndex)
        {
            double fAverage = data.GetDataAtIndex(iStartIndex, dataType);
            for (int i = iStartIndex + 1; i < iEndIndex; i++)
            {
                fAverage += data.GetDataAtIndex(i, dataType);
            }
            fAverage *= 1.0f / (double)Math.Max(1, iEndIndex - iStartIndex);
            return fAverage;
        }
    }

    public class TackAnalysisData
    {
        public enum eTackDataTypes
        {
            Lat,
            Long,
            InitialHeading,
            TimeOfStartOfTurn,
            TimeToHeadToWind,
            TimeToSlowestPoint,
            TimeToEndOfTurn,
            TimeToInitialTurnEnd,// = -1.0f;
            TimeToCounterSteerEnd,// = -1.0f;
            TimeOfEndOfTurn,
            TimeToRegain90PercentSpdFromSlowest,
            SpeedPreTack,
            SlowestSpeed,
            AverageSpeed,
            AngleTackedThrough,
            AngleSteeredThrough,
            Leeway,
            LossDueToSpeed,
            LossDueToHeading,
            TotalLoss,
            FinalHeading,
            WindDirectionAtStart,
            AverageTWS,
            AverageTWD
        };
        double[] m_Values;
        bool m_bIsTack = true;

        public void SetIsTack(bool itIs)
        {
            m_bIsTack = itIs;
        }

        public bool IsTack()
        {
            return m_bIsTack;
        }

        public double GetValue(eTackDataTypes dt)
        {
            return m_Values[(int)dt];
        }

        public double GetValue(int dt)
        {
            if (dt < GetNumValues())
            {
                return m_Values[(int)dt];
            }
            return 0.0;
        }

        public void SetValue(eTackDataTypes dt, double fValue)
        {
            m_Values[(int)dt] = fValue;
        }

        public void SetValue(int dt, double fValue)
        {
            if (dt < GetNumValues())
            {
                m_Values[(int)dt] = fValue;
            }
        }

        static public int GetNumValues()
        {
            return Enum.GetNames(typeof(eTackDataTypes)).Length;
        }

        static public string GetValueName(int iValue)
        {
            if (iValue < GetNumValues())
            {
                return Enum.GetNames(typeof(eTackDataTypes))[iValue];
            }
            return "Unknown";
        }

        public TackAnalysisData()
        {
            m_Values = new double[GetNumValues()];
            foreach (int i in m_Values)
            {
                m_Values[i] = 0.0;
            }
            m_Values[(int)eTackDataTypes.TimeToInitialTurnEnd] = -1.0;
            m_Values[(int)eTackDataTypes.TimeToCounterSteerEnd] = -1.0;
        }
    }

    public class TackAnalysis
    {
        NMEACruncher m_Data;
        List<TackAnalysisData> m_TackData;

        public TackAnalysis(NMEACruncher data)
        {
            m_Data = data;
            m_TackData = new List<TackAnalysisData>();
        }

        public void OnDataReplaced(NMEACruncher newData)
        {
            m_Data = newData;
        }

        List<int> FindTackPoints(int iStartSearch, int iEndSearch)
        {
            if (m_Data.GetDataCount() <= 0)
                return null;

            if (iStartSearch >= m_Data.GetDataCount())
            {
                return null;
            }

            if (iEndSearch >= m_Data.GetDataCount())
            {
                return null;
            }

            List<int> tackPoints = new List<int>();
            int iToSearch = iEndSearch - 1;
            for (int i = iStartSearch; i < iToSearch - 1; i++)
            {
                //Avoid slip tacking.... Kraken is parked nearly dead downwind!
                if (m_Data.GetDataAtIndex(i, NMEACruncher.DataTypes.BoatSpeed) + m_Data.GetDataAtIndex(i, NMEACruncher.DataTypes.GPSSOG) < 1.0)
                {
                    continue;
                }

                double fAwa_0 = m_Data.GetDataAtIndex(i, NMEACruncher.DataTypes.AWA);
                double fAwa_Plus1 = m_Data.GetDataAtIndex(i+1, NMEACruncher.DataTypes.AWA);

                if ((fAwa_0 > 0.0) ^ (fAwa_Plus1 > 0.0))
                {
                    //Change in direction!
                    int iStart = System.Math.Max(0, i - 20);
                    double fAngleAveragePre = AngleUtil.CalculateAngleAverage(m_Data, NMEACruncher.DataTypes.AWA, iStart, i);
                    double fAvPrePoint = AngleUtil.ContainAngleMinus180To180(fAngleAveragePre);

                    int iTail = System.Math.Min(i + 20, iToSearch);
                    double fAngleAveragePost = AngleUtil.CalculateAngleAverage(m_Data, NMEACruncher.DataTypes.AWA, i, iTail);
                    double fAvPostPoint = AngleUtil.ContainAngleMinus180To180(fAngleAveragePost);

                    //Let's check we don't revert shortly
                    //TODO: Search better....
                    if ((fAvPrePoint > 0.0f) ^ (fAvPostPoint > 0.0f))
                    {
                        tackPoints.Add(i);

                        //Make sure we skip a decent distance into the future to avoid recording multiple points on bad gybes etc
                        const int kSkipAhead = 25;
                        i += kSkipAhead;
                    }
                }
            }

            return tackPoints;
        }

        //We do this a lot so this reduces text...
        double AvAWAInRange(int iStart, int iFinish)
        {
            return AngleUtil.CalculateAngleAverage(m_Data, NMEACruncher.DataTypes.AWA, iStart, iFinish);
        }

        public bool UseSOG = false;
        double BoatSpeed(int i)
        {
            if (!UseSOG && m_Data.HasDataAtIndex(i, NMEACruncher.DataTypes.BoatSpeed))
            {
                return m_Data.GetDataAtIndex(i, NMEACruncher.DataTypes.BoatSpeed);
            }
            //Fallback to GPS speed
            return m_Data.GetDataAtIndex(i, NMEACruncher.DataTypes.GPSSOG);
        }

        double SampleTime(int i)
        {
            return m_Data.GetDataAtIndex(i, NMEACruncher.DataTypes.Time);
        }

        double GPSHeading(int i)
        {
            return m_Data.GetDataAtIndex(i, NMEACruncher.DataTypes.GPSHeading);
        }

        double BoatHeading(int i)
        {
            return m_Data.GetDataAtIndex(i, NMEACruncher.DataTypes.BoatHeading);
        }

        double TWD(int i)
        {
            return m_Data.GetDataAtIndex(i, NMEACruncher.DataTypes.TWD);
        }

        const double DegToRad = (2.0 * Math.PI / 360.0); //TODO: This is in MapWindow.cs also

        //TackAnalysisData RunTackAnalysis2(int iFirstSample, int iLastSample, bool bUseAWARange, double fAWAMin, double fAWAMax)
        //{
        //    TackAnalysisData rOutData = new TackAnalysisData(); //Might be discarded
        //    iFirstSample = Math.Max(0, iFirstSample);
        //    iLastSample = Math.Min(m_Data.GetDataCount() - 1, iLastSample);

        //    rOutData.SetValue(TackAnalysisData.eTackDataTypes.InitialHeading,
        //                        AngleUtil.ContainAngle0To360(m_Data.GetDataAtIndex(iFirstSample, NMEACruncher.DataTypes.BoatHeading))
        //            );
        //    rOutData.SetValue(TackAnalysisData.eTackDataTypes.WindDirectionAtStart,
        //                        TWD(iFirstSample)
        //             );



        //    return null;
        //}

        TackAnalysisData RunTackAnalysis(int iFirstSample, int iLastSample, bool bUseAWARange, double fAWAMin, double fAWAMax)
        {
            TackAnalysisData rOutData = new TackAnalysisData(); //Might be discarded
            iFirstSample = Math.Max(0, iFirstSample);
            iLastSample = Math.Min(m_Data.GetDataCount() - 1, iLastSample);

            rOutData.SetValue(TackAnalysisData.eTackDataTypes.InitialHeading,
                                AngleUtil.ContainAngle0To360(m_Data.GetDataAtIndex(iFirstSample, NMEACruncher.DataTypes.BoatHeading))
                    );
            rOutData.SetValue(TackAnalysisData.eTackDataTypes.WindDirectionAtStart,
                                TWD(iFirstSample)
                     );
            int iThroughWind = iFirstSample;
            int iEarlier = Math.Max(0, iFirstSample - 4);
            double fAverageAWAIn = AvAWAInRange(iEarlier, iThroughWind);

            bool bStartPositive = fAverageAWAIn > 0.0f;
            for (; iThroughWind < iLastSample; iThroughWind++)
            {
                //Look for a run of opposed values, not a single value with a flipped result
                const int kAverageToVerify = 4;
                int iToAverageMax = Math.Min(iThroughWind + kAverageToVerify, m_Data.GetDataCount());
                int iCountOpposed = 0;
                for (int i = iThroughWind; i < iToAverageMax; i++)
                {
                    if (bStartPositive == (m_Data.GetDataAtIndex(i, NMEACruncher.DataTypes.AWA) > 0.0))
                    {
                        break;
                    }
                    iCountOpposed++;
                }

                if (iCountOpposed == kAverageToVerify)
                {
                    break;
                }
            }

            rOutData.SetIsTack(Math.Abs(m_Data.GetDataAtIndex(iThroughWind, NMEACruncher.DataTypes.AWA)) < 60.0);


            //Look at the curve, when do we start changing to the iSlowest / tack point. When do we stop changing direction consistently
            int iStartOfTurn = iThroughWind - 1;
            double fDelta = AngleUtil.ShortAngle(m_Data.GetDataAtIndex(iStartOfTurn, NMEACruncher.DataTypes.AWA), m_Data.GetDataAtIndex(iThroughWind, NMEACruncher.DataTypes.AWA));
            //Look at steering trend over several samples to account for instrument and steering vagaries.
            const int kiRangeToSampleDelta = 8;
            while (iStartOfTurn > iFirstSample)	//? Could use zero and do the sample range automatically eventually
            {
                int iStartOfSampleOne = Math.Max(0, iStartOfTurn - kiRangeToSampleDelta);
                int iEndOfSampleTwo = Math.Min(m_Data.GetDataCount(), iStartOfTurn + kiRangeToSampleDelta);
                double fEarlierAWAAv = AvAWAInRange(iStartOfSampleOne, iStartOfTurn);
                double fLaterAWAAv = AvAWAInRange(iStartOfTurn, iEndOfSampleTwo);
                double fPriorDelta = AngleUtil.ShortAngle(fEarlierAWAAv, fLaterAWAAv);
                if ((fPriorDelta > 0.0f) ^ (fDelta > 0.0f))
                {
                    //Counter steering! Perhaps go further though?
                    break;
                }
                --iStartOfTurn;
            }

            double fStartOfTurn = SampleTime(iStartOfTurn);
            int iEndOfTurn = iThroughWind + 1;
            int kMaxSearchForEnd = Math.Min(iLastSample + 10, m_Data.GetDataCount()); //? Could use zero and do the sample range automatically
            const int kGrooveCount = 4;
            const int kSamplesToCheckForChange = 10;
            const double kfAvAWAForGroove = 2.0;
            int iGrooveCount = 0;
            fDelta = AngleUtil.ShortAngle(m_Data.GetDataAtIndex(iStartOfTurn, NMEACruncher.DataTypes.AWA), m_Data.GetDataAtIndex(iThroughWind, NMEACruncher.DataTypes.AWA));
            while (iEndOfTurn < kMaxSearchForEnd)
            {
                //TODO:

                int iStartOfSampleOne = Math.Max(0, iEndOfTurn - kSamplesToCheckForChange);
                int iEndOfSampleTwo = Math.Min(m_Data.GetDataCount(), iEndOfTurn + kSamplesToCheckForChange);
                double fEarlierAWAAv = AvAWAInRange(iStartOfSampleOne, iEndOfTurn);
                double fLaterAWAAv = AvAWAInRange(iEndOfTurn, iEndOfSampleTwo);
                double fPriorDelta = AngleUtil.ShortAngle(fEarlierAWAAv, fLaterAWAAv);

                if (Math.Abs(fPriorDelta) < kfAvAWAForGroove)
                {
                    ++iGrooveCount;
                    if (iGrooveCount == kGrooveCount)
                    {
                        //Search back to find any period we're above the AWA for consistently
                        int iEndOfCounterSteer = kMaxSearchForEnd;
                        while (iEndOfCounterSteer > iThroughWind)
                        {
                            double fNewAWA = Math.Abs(AvAWAInRange(iEndOfCounterSteer - kSamplesToCheckForChange, iEndOfCounterSteer));
                            if (rOutData.IsTack() ? 
                                        (fNewAWA > Math.Abs(fLaterAWAAv) + kfAvAWAForGroove) 
                                    :   (fNewAWA < Math.Abs(fLaterAWAAv) - kfAvAWAForGroove)
                                )
                            {
                                rOutData.SetValue(TackAnalysisData.eTackDataTypes.TimeToCounterSteerEnd, SampleTime(iEndOfCounterSteer) - fStartOfTurn);
                                break;
                            }
                            --iEndOfCounterSteer;
                        }
                        break;
                    }
                }
                else
                {
                    iGrooveCount = 0;
                }
                ++iEndOfTurn;
            }

            //Check the AWA for entry and exit is within tolerances
            if (bUseAWARange)
            {
                const int kiSamplesToScan = 10;
                int iPreAngleStart = Math.Max(0, iStartOfTurn - kiSamplesToScan);
                double fAWAEntry = Math.Abs(AngleUtil.ContainAngleMinus180To180(AvAWAInRange(iPreAngleStart, iStartOfTurn)));
                if ((fAWAEntry > fAWAMin) && (fAWAEntry < fAWAMax))	//Are we outside the upwind and downwind cones?
                {
                    return null;
                }

                int iEndOfPostTurn = Math.Min(iEndOfTurn + kiSamplesToScan, m_Data.GetDataCount());
                double fAWAExit = Math.Abs(AngleUtil.ContainAngleMinus180To180(AvAWAInRange(iEndOfTurn, iEndOfPostTurn)));
                if ((fAWAExit > fAWAMin) && (fAWAExit < fAWAMax))	//Are we outside the upwind and downwind cones?
                {
                    return null;
                }
            }

            //Store wind information
            rOutData.SetValue(TackAnalysisData.eTackDataTypes.AverageTWD, AngleUtil.ContainAngle0To360(AngleUtil.CalculateAngleAverage(m_Data, NMEACruncher.DataTypes.TWD, iStartOfTurn, iEndOfTurn)));
            rOutData.SetValue(TackAnalysisData.eTackDataTypes.AverageTWS, AngleUtil.CalculateAverage(m_Data, NMEACruncher.DataTypes.TWS, iStartOfTurn, iEndOfTurn));

            int iSlowPoint = 0;
            double fSlowestSpdPostTack = 666.6f;
            for (int i = iThroughWind; i < iLastSample; i++)
            {
                double fBoatSpeed = BoatSpeed(i);
                if (fBoatSpeed < fSlowestSpdPostTack)
                {
                    fSlowestSpdPostTack = fBoatSpeed;
                    iSlowPoint = i;
                }
            }
            int iCountBeforeTack = iThroughWind - iFirstSample;
            int iCountAfterTack = iLastSample - iThroughWind;
            //3) Look at boat speed as a % of polar speed on entry
            //float fAverageSpdPreTack = 0.0f;
            int iNumSamplesInEntry = (iCountBeforeTack * 2) / 3;					//Area at begining to consider pre tack
            int iNumSamplesPostTack = (iCountAfterTack * 2) / 3;					//Area at end to consider final path
            int iSizeBufferZonePostTack = iCountAfterTack - iNumSamplesPostTack;	//Area to look for slowing
            int iEntryEnd = iFirstSample + iNumSamplesInEntry;
            int iExitStart = iLastSample - iNumSamplesPostTack;
            if (iNumSamplesInEntry > 0)
            {
                double fPreTackSpd = BoatSpeed(iStartOfTurn);


                //Find the time to acheive 90% of this post tack, allow speed to drop first?
                int iBackToNinetyPercent = iSlowPoint;
                for (; iBackToNinetyPercent < iLastSample; iBackToNinetyPercent++)
                {
                    if (BoatSpeed(iBackToNinetyPercent) > fPreTackSpd * 0.95f)
                    {
                        break;
                    }
                }

                double fTimeOfSlowest = SampleTime(iSlowPoint);
                double fEndOfTurn = SampleTime(iEndOfTurn);
                double fThroughWind = SampleTime(iThroughWind);

                rOutData.SetValue(TackAnalysisData.eTackDataTypes.Long, m_Data.GetDataAtIndex(iThroughWind, NMEACruncher.DataTypes.GPSLong));
                rOutData.SetValue(TackAnalysisData.eTackDataTypes.Lat, m_Data.GetDataAtIndex(iThroughWind, NMEACruncher.DataTypes.GPSLat));

                rOutData.SetValue(TackAnalysisData.eTackDataTypes.TimeOfStartOfTurn, fStartOfTurn);
                rOutData.SetValue(TackAnalysisData.eTackDataTypes.TimeToHeadToWind, fThroughWind - fStartOfTurn);
                rOutData.SetValue(TackAnalysisData.eTackDataTypes.TimeToSlowestPoint, fTimeOfSlowest - fStartOfTurn);
                rOutData.SetValue(TackAnalysisData.eTackDataTypes.TimeToEndOfTurn, fEndOfTurn - fStartOfTurn);
                rOutData.SetValue(TackAnalysisData.eTackDataTypes.TimeOfEndOfTurn, fEndOfTurn);
                rOutData.SetValue(TackAnalysisData.eTackDataTypes.TimeToRegain90PercentSpdFromSlowest, -1.0f);
                double fTimeOf90Percent = SampleTime(iBackToNinetyPercent);
                if (iBackToNinetyPercent < iLastSample)
                {
                    rOutData.SetValue(TackAnalysisData.eTackDataTypes.TimeToRegain90PercentSpdFromSlowest, fTimeOf90Percent - fTimeOfSlowest);
                }

                double fAverageSpeedInTurn = BoatSpeed(iStartOfTurn);
                for (int i = iStartOfTurn + 1; i < iEndOfTurn; i++)
                {
                    //TODO: Account for time properly!
                    fAverageSpeedInTurn += BoatSpeed(i);
                }
                fAverageSpeedInTurn /= (double)Math.Max(1, iEndOfTurn - iStartOfTurn);

                //if (fAverageSpeedInTurn < 0.1)
                //{
                //    return null;
                //}

                rOutData.SetValue(TackAnalysisData.eTackDataTypes.SpeedPreTack, fPreTackSpd);
                rOutData.SetValue(TackAnalysisData.eTackDataTypes.SlowestSpeed, BoatSpeed(iSlowPoint));
                rOutData.SetValue(TackAnalysisData.eTackDataTypes.AverageSpeed, fAverageSpeedInTurn);

                //TODO: Work out discrepency here - downwind seems dodgy
                //Calculate entry and exit directions, and tacking angle
                double fEntryDirection = AngleUtil.ContainAngle0To360(AngleUtil.CalculateAngleAverage(m_Data, NMEACruncher.DataTypes.GPSHeading, iFirstSample, iEntryEnd));
                double fExitDirection = AngleUtil.ContainAngle0To360(AngleUtil.CalculateAngleAverage(m_Data, NMEACruncher.DataTypes.GPSHeading, iExitStart, iLastSample));
                double fTackingAngle = AngleUtil.ShortAngle(fEntryDirection, fExitDirection);
                rOutData.SetValue(TackAnalysisData.eTackDataTypes.AngleTackedThrough, fTackingAngle);

                double fEntryDirection_Steered = AngleUtil.ContainAngle0To360(AngleUtil.CalculateAngleAverage(m_Data, NMEACruncher.DataTypes.BoatHeading, iFirstSample, iEntryEnd));
                double fExitDirection_Steered = AngleUtil.ContainAngle0To360(AngleUtil.CalculateAngleAverage(m_Data, NMEACruncher.DataTypes.BoatHeading, iExitStart, iLastSample));
                double fTackingAngle_Steered = AngleUtil.ShortAngle(fEntryDirection_Steered, fExitDirection_Steered);

                rOutData.SetValue(TackAnalysisData.eTackDataTypes.AngleSteeredThrough, fTackingAngle_Steered);
                //That was lovely. How, about, leeway.
                double fLeeway = (Math.Abs(fTackingAngle_Steered) - Math.Abs(fTackingAngle)) * 0.5f;
                rOutData.SetValue(TackAnalysisData.eTackDataTypes.Leeway, fLeeway);

                //Estimation of time lost due to slowdown / acceleration
                double fPropSpdUpDownWind = Math.Abs(Math.Cos((TWD(iStartOfTurn) - BoatHeading(iStartOfTurn) + 180.0f) * DegToRad));
                double fPreTackSpdOnWind = BoatSpeed(iStartOfTurn) * fPropSpdUpDownWind;

                //Get an earlier sample
                int iInitialSpdNum = 10;// iThroughWind - iStartOfTurn;
                int iStartOfPreTurnSample = Math.Max(0, iStartOfTurn - iInitialSpdNum * 2);
                int iEndOfPreTurnSample = Math.Max(0, iStartOfTurn - iInitialSpdNum);
                double fAverageSpdPreTack = 0;
                for (int i = iStartOfPreTurnSample; i < iEndOfPreTurnSample; i++)
                {
                    fAverageSpdPreTack += BoatSpeed(i);
                }
                fAverageSpdPreTack *= 1.0f / (double)Math.Max(1, iInitialSpdNum);

                double fDistanceThroughAcceleration = 0.0f;
                double fCouldHaveTravelledThroughAcceleration = 0.0f;
                for (int i = iStartOfTurn; i < iBackToNinetyPercent; i++)
                {
                    double fTimeOfSample = (SampleTime(i) - SampleTime(i - 1)) * (1.0f / 3600.0f); //Converts seconds to hours, speed is in nautical miles per hour
                    fDistanceThroughAcceleration += BoatSpeed(i) * fTimeOfSample;
                    fCouldHaveTravelledThroughAcceleration += fAverageSpdPreTack * fTimeOfSample;
                }

                double fDistanceLost = Math.Max(0.0f, fCouldHaveTravelledThroughAcceleration - fDistanceThroughAcceleration);

                //Calculate "slippage" on new tack.
                //	-	Extra leeway while travelling slower
                //	-	Going deep to accelerate
                //	-	1) Get final heading
                int iEndOfFinalHeading = Math.Min(iBackToNinetyPercent + (iInitialSpdNum * 2), m_Data.GetDataCount());
                double fFinalHeading = AngleUtil.ContainAngle0To360(AngleUtil.CalculateAngleAverage(m_Data, NMEACruncher.DataTypes.GPSHeading, iBackToNinetyPercent, iEndOfFinalHeading));

                //	-	2) Calculate amount of slipping below this
                double fDistanceSailedBelowFinalHeading = 0.0f;
                for (int i = iThroughWind; i < iEndOfFinalHeading; i++)
                {
                    double fHeadingDelta = AngleUtil.ShortAngle(GPSHeading(i), fFinalHeading);
                    if ((fHeadingDelta > 0.0f) ^ (fTackingAngle > 0.0f))
                    {
                        double fDistance = BoatSpeed(i) * (SampleTime(i) - SampleTime(i - 1)) * (1.0f / 3600.0f);
                        fDistanceSailedBelowFinalHeading += fDistance * Math.Abs(Math.Sin(fHeadingDelta * DegToRad));
                    }
                }


                double fBoatLengthsLostToSpeed = fDistanceLost * 6076.12f / (36.0f);
                double fBoatLengthsLostToHeading = fDistanceSailedBelowFinalHeading * 6076.12f / (36.0f);
                rOutData.SetValue(TackAnalysisData.eTackDataTypes.LossDueToSpeed, fBoatLengthsLostToSpeed);
                rOutData.SetValue(TackAnalysisData.eTackDataTypes.LossDueToHeading, fBoatLengthsLostToHeading);
                rOutData.SetValue(TackAnalysisData.eTackDataTypes.TotalLoss, fBoatLengthsLostToHeading + fBoatLengthsLostToSpeed);

                rOutData.SetValue(TackAnalysisData.eTackDataTypes.FinalHeading, fFinalHeading);
                return rOutData;
            }

            return null;

        }

        public void AnalyseSection(int iStartSearch, int iEndSearch, bool bAppend, bool bUseAWARange, double fMaxAWAForTack, double fMinAWAForGybe)
        {
            if (!bAppend)
            {
                m_TackData.Clear();
            }
            List<int> tackPoints = FindTackPoints(iStartSearch, iEndSearch);
            if (tackPoints != null)
            {
                Console.WriteLine("--------------AnalyseSection------------------");
                Console.WriteLine("Found {0:D} tack points", tackPoints.Count);
                foreach (int iTackPoint in tackPoints)
                {
                    double fTackTime = m_Data.GetDataAtIndex(iTackPoint, NMEACruncher.DataTypes.Time);
                    int iStartIndex = m_Data.GetIndexForTime(fTackTime - 60.0);
                    if (iStartIndex >= 0)
                    {
                        int iEndIndex = m_Data.GetIndexForTime(fTackTime + 60.0);
                        if (iEndIndex >= iStartIndex)
                        {
                            TackAnalysisData newData = RunTackAnalysis(iStartIndex, iEndIndex, bUseAWARange, fMaxAWAForTack, fMinAWAForGybe);
                            if (newData != null)
                            {
                                Console.WriteLine("--- Accepted {0:D}", iTackPoint);
                                m_TackData.Add(newData);
                            }
                            else
                            {
                                Console.WriteLine("--- Rejected {0:D}", iTackPoint);
                            }
                        }
                    }
                }
                Console.WriteLine("--------------------------------");

                //Send data to all windows
                DockableDrawable.BroadcastNewTackData(this, m_TackData);
            }
        }

        public int GetNumTacks()
        {
            return m_TackData.Count;
        }

        public TackAnalysisData GetData(int i)
        {
            return m_TackData[i];
        }
    }
}
