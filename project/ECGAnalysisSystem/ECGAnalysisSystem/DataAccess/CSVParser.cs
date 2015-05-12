using System;
using System.Collections.Generic;
using ECGAnalysisSystem.Interfaces;
using OxyPlot;

namespace ECGAnalysisSystem.DataAccess
{
    /// <summary>
    /// Class provides methods for csv files parsing
    /// </summary>
    class CSVParser : IDataParser
    {
        /// <summary>
        /// Parse method implementation for CSV file
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        public List<DataPoint> Parse(List<string> records)
        {
            List<DataPoint> parsedData = new List<DataPoint>();

            foreach (var record in records)
            {
                string[] splittedRecord = record.Split(',');
                parsedData.Add(new DataPoint(Double.Parse(splittedRecord[0]), Double.Parse(splittedRecord[1])));
            }

            return parsedData;

            #region Version 0.9

            //foreach (var record in records)
            //{
            //    string[] splittedRecord = record.Split(',');
            //    yield return new DataPoint(double.Parse(splittedRecord[0]), double.Parse(splittedRecord[1]));
            //}

            #endregion
        }
    }
}