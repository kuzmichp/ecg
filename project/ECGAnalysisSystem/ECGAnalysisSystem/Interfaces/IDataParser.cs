using System.Collections.Generic;
using OxyPlot;

namespace ECGAnalysisSystem.Interfaces
{
    /// <summary>
    /// Interface for ECG data pasrers
    /// </summary>
    interface IDataParser
    {
        /// <summary>
        /// Method parses ECG data
        /// </summary>
        /// <param name="records">Collection of records</param>
        /// <returns>Parsed data</returns>
        List<DataPoint> Parse(List<string> records);
    }
}