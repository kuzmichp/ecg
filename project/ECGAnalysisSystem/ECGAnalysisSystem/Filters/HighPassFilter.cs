using System.Collections.Generic;
using ECGAnalysisSystem.Interfaces;
using OxyPlot;

namespace ECGAnalysisSystem.Filters
{
    /// <summary>
    /// Class represents High-Pass Filter for ECG signal denoising
    /// </summary>
    public class HighPassFilter : IFilter
    {
        /// <summary>
        /// Method for denoising input ECG signal
        /// </summary>
        /// <param name="noisedSignal">Input noised signal</param>
        /// <returns>Denoised signal</returns>
        public List<DataPoint> Denoise(List<DataPoint> noisedSignal)
        {
			int signalLength = noisedSignal.Count;
            const double filterLength = 9.0;
			
            List<DataPoint> denoisedSignal = new List<DataPoint>(signalLength);

            for (int i = 0; i < signalLength; i++)
            {
                double maSignal = 0, delayedSignal = 0;
				double maSignalSum = 0;
				
				#region Delayed System
                int delayedSignalIndex = (int) (i - (filterLength + 1) / 2);
				if (delayedSignalIndex < 0) 
				{
					delayedSignalIndex = signalLength + delayedSignalIndex;
				}
                delayedSignal = noisedSignal[delayedSignalIndex].Y;
				#endregion
				
				#region Moving Average Filter
                for (int j = i; j > i - filterLength; j--)
                {
                    int inputSignalIndex = j;
                    if (inputSignalIndex < 0) 
					{
						inputSignalIndex = signalLength + inputSignalIndex;
					}
                    maSignalSum += noisedSignal[inputSignalIndex].Y;
                }
                maSignal = (1 / filterLength) * maSignalSum;
				#endregion
                
				denoisedSignal.Add(new DataPoint(noisedSignal[i].X, delayedSignal - maSignal));
            }

            return denoisedSignal;
        }
    }
}