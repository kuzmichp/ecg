using System.Collections.Generic;
using ECGAnalysisSystem.Interfaces;
using OxyPlot;

namespace ECGAnalysisSystem.Filters
{
    /// <summary>
    /// Class represents Low-Pass Filter
    /// </summary>
    public class LowPassFilter : IFilter
    {
        /// <summary>
        /// Method for ECG signal denoising
        /// </summary>
        /// <param name="noisedSignal">Input noised signal</param>
        /// <returns>Denoised signal</returns>
        public List<DataPoint> Denoise(List<DataPoint> noisedSignal)
        {
            int signalLength = noisedSignal.Count;
            List<DataPoint> denoisedSignal = new List<DataPoint>(signalLength);
            
            for (int i = 0; i < signalLength; i++)
            {
                double sum = 0;
                if (i + 30 < signalLength)
                {
                    for (int j = i; j < i + 30; j++)
                    {
                        double current = noisedSignal[j].Y * noisedSignal[j].Y;
                        sum += current;
                    }
                }
                else if (i + 30 >= signalLength)
                {
                    int over = i + 30 - signalLength;
                    for (int j = i; j < signalLength; j++)
                    {
                        double current = noisedSignal[j].Y * noisedSignal[j].Y;
                        sum += current;
                    }
                    for (int j = 0; j < over; j++)
                    {
                        double current = noisedSignal[j].Y * noisedSignal[j].Y;
                        sum += current;
                    }
                }

                denoisedSignal.Add(new DataPoint(noisedSignal[i].X, sum));
            }

            return denoisedSignal;
        }
    }
}