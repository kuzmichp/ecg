using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        List<DataPoint> Detect(List<DataPoint> filteredData);
    }
}
