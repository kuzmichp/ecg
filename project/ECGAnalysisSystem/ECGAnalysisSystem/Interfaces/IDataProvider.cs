using System.Collections.Generic;
using System.IO;

namespace ECGAnalysisSystem.Interfaces
{
    /// <summary>
    /// Interface for IO operations handling
    /// </summary>
    interface IDataProvider
    {
        /// <summary>
        /// This method is an ECG data parsing base stage
        /// </summary>
        /// <param name="stream">Stream with an ECG data</param>
        /// <returns>Collection of single records (elapsed time - amplitude) in the text form</returns>
        List<string> LoadData(FileStream stream);
    }
}