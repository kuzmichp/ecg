using System;
using System.Collections.Generic;
using OxyPlot;

namespace ECGAnalysisSystem.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    interface IQRSDetector
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filteredData"></param>
        /// <returns></returns>
        List<Tuple<DataPoint, int>> DetectPeaksNeighbourhood(List<DataPoint> filteredData);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputData"></param>
        /// <param name="approxPeaks"></param>
        /// <returns></returns>
        List<DataPoint> DetectQRSPeaks(List<DataPoint> inputData, List<Tuple<DataPoint, int>> approxPeaks);
    }
}
