using System.Collections.Generic;
using System.IO;
using ECGAnalysisSystem.Interfaces;

namespace ECGAnalysisSystem.DataAccess
{
    /// <summary>
    /// Class that implements IDataProvider
    /// </summary>
    class CSVLoader : IDataProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public List<string> LoadData(FileStream stream)
        {
            List<string> rawData = new List<string>();

            using (StreamReader reader = new StreamReader(stream))
            {
                while (reader.Peek() >= 0)
                {
                    rawData.Add(reader.ReadLine());
                }
            }

            return rawData;
        }
    }
}