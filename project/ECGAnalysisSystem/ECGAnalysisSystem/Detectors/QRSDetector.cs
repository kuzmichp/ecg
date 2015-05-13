using System;
using System.Collections.Generic;
using System.Linq;
using ECGAnalysisSystem.Interfaces;
using OxyPlot;

namespace ECGAnalysisSystem.Detectors
{
    class QRSDetector : IQRSDetector
    {
        public List<DataPoint> Detect(List<DataPoint> filteredData)
        {
            DataPoint[] QRSPoints = new DataPoint[filteredData.Count];

            double treshold = 0;

            const int frame = 250;
            const double alphaMax = 0.1;
            const double alphaMin = 0.01;

            for (int i = 0; i < 200; i++)
            {
                if (filteredData[i].Y > treshold)
                {
                    treshold = filteredData[i].Y;
                }
            }

            for (int i = 0; i < filteredData.Count; i += frame)
            {
                double peak = 0;
                int index = 0;

                // Check out of range
                if (i + frame > filteredData.Count)
                {
                    index = filteredData.Count;
                }
                else
                {
                    index = i + frame;
                }

                // Finding local maxima
                for (int j = i; j < index; j++)
                {
                    if (filteredData[j].Y > peak)
                    {
                        peak = filteredData[j].Y;
                    }
                }

                // 
                bool added = false;
                for (int j = i; j < index; j++)
                {
                    if (filteredData[j].Y > treshold && !added)
                    {
                        added = true;
                        QRSPoints[j] = new DataPoint(filteredData[j].X, 1);
                    }
                    else
                    {
                        QRSPoints[j] = new DataPoint(filteredData[j].X, 0);
                    }
                }


                // It can be canged
                double gamma = (new Random()).NextDouble() > 0.5 ? 0.15 : 0.20;
                double alpha = (new Random()).NextDouble() * (alphaMax - alphaMin) + alphaMin;

                treshold = alpha * gamma * peak + (1 - alpha) * treshold;
            }

            return QRSPoints.ToList();
        }
    }
}