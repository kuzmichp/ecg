using System.Collections.Generic;
using OxyPlot;

namespace ECGAnalysisSystem.Interfaces
{
    /// <summary>
    /// Interface for digital filters
    /// </summary>
    public interface IFilter
    {
        /// <summary>
        /// Method for signal denoising
        /// </summary>
        /// <param name="noisedSignal">Input noised signal</param>
        /// <returns>Denoised signal</returns>
        List<DataPoint> Denoise(List<DataPoint> noisedSignal);
    }
}
