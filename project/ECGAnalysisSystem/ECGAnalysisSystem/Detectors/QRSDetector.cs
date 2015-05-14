using System;
using System.Collections.Generic;
using System.Linq;
using ECGAnalysisSystem.Interfaces;
using OxyPlot;

namespace ECGAnalysisSystem.Detectors
{
    /// <summary>
    /// Implementations of IQRSDetector interface
    /// </summary>
    class QRSDetector : IQRSDetector
    {
        /// <summary>
        /// Method provides QRS points detection
        /// </summary>
        /// <param name="filteredData">Input data filtered by LowPass filter</param>
        /// <returns>QRS points</returns>
        public List<Tuple<DataPoint, int>> DetectPeaksNeighbourhood(List<DataPoint> filteredData)
        {
            int samplesNum = filteredData.Count;
            double treshold = 0.2; // y treshold

            #region Getting Peaks

            List<Tuple<DataPoint, int>> peaks = new List<Tuple<DataPoint, int>>(samplesNum); 

            // Naive method for getting peaks
            for (int i = 0; i < samplesNum; i++)
            {
                if (i == 0 || i == samplesNum - 1) continue;

                if (filteredData[i].Y > filteredData[i - 1].Y && filteredData[i].Y > filteredData[i + 1].Y)
                {
                    peaks.Add(new Tuple<DataPoint, int>(filteredData[i], i));
                }
            }

            // Apply y-axis treshold do select peaks
            peaks.RemoveAll(point => point.Item1.Y < treshold);

            #endregion

            #region Applying OX Threshold

            List<Tuple<DataPoint, int>> approxPeaks = new List<Tuple<DataPoint, int>>();

            // Selecting exactly one peak per frame by applying x-axis treshold
            for (int i = peaks.Count - 1; i > 0; i--)
            {
                if (Math.Abs(peaks[i].Item1.X - peaks[i - 1].Item1.X) < 0.4) continue;
                approxPeaks.Add(peaks[i]);
            }

            approxPeaks.Add(peaks[0]);
            approxPeaks.Reverse();

            #endregion

            return approxPeaks;
        }

        public List<DataPoint> DetectQRSPeaks(List<DataPoint> input, List<Tuple<DataPoint, int>> approxPeaks)
        {
            List<DataPoint> QRSPeaks = new List<DataPoint>();
            int treshold = 30;

            foreach (var t in approxPeaks)
            {
                if (t.Item2 - treshold < 0) continue;
                if (t.Item2 + treshold > input.Count) break;

                double max = 0;
                int index = -1;

                for (int j = t.Item2 - treshold; j < t.Item2 + treshold; j++)
                {
                    if (input[j].Y > max)
                    {
                        max = input[j].Y;
                        index = j;
                    }
                }

                QRSPeaks.Add(input[index]);
            }

            return QRSPeaks;
        }

        //public List<DataPoint> Detect(List<DataPoint> filteredData)
        //{
        //    DataPoint[] QRSPoints = new DataPoint[filteredData.Count];

        //    double treshold = 0;

        //    const int frame = 250;
        //    const double alphaMax = 0.1;
        //    const double alphaMin = 0.01;

        //    for (int i = 0; i < 200; i++)
        //    {
        //        if (filteredData[i].Y > treshold)
        //        {
        //            treshold = filteredData[i].Y;
        //        }
        //    }

        //    for (int i = 0; i < filteredData.Count; i += frame)
        //    {
        //        double peak = 0;
        //        int index = 0;

        //        // Check out of range
        //        if (i + frame > filteredData.Count)
        //        {
        //            index = filteredData.Count;
        //        }
        //        else
        //        {
        //            index = i + frame;
        //        }

        //        // Finding local maxima
        //        for (int j = i; j < index; j++)
        //        {
        //            if (filteredData[j].Y > peak)
        //            {
        //                peak = filteredData[j].Y;
        //            }
        //        }

        //        // 
        //        bool added = false;
        //        for (int j = i; j < index; j++)
        //        {
        //            if (filteredData[j].Y > treshold && !added)
        //            {
        //                added = true;
        //                QRSPoints[j] = new DataPoint(filteredData[j].X, 1);
        //            }
        //            else
        //            {
        //                QRSPoints[j] = new DataPoint(filteredData[j].X, 0);
        //            }
        //        }


        //        // It can be canged
        //        double gamma = (new Random()).NextDouble() > 0.5 ? 0.15 : 0.20;
        //        double alpha = (new Random()).NextDouble() * (alphaMax - alphaMin) + alphaMin;

        //        treshold = alpha * gamma * peak + (1 - alpha) * treshold;
        //    }

        //    return QRSPoints.ToList();
        //}
    }
}