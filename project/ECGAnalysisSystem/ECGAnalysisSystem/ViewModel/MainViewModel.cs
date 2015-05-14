using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ECGAnalysisSystem.DataAccess;
using ECGAnalysisSystem.Detectors;
using ECGAnalysisSystem.Filters;
using ECGAnalysisSystem.Interfaces;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using OxyPlot;
using PropertyChanged;

namespace ECGAnalysisSystem.ViewModel
{
    /// <summary>
    /// ViewModel class for main application view
    /// </summary>
    [ImplementPropertyChanged]
    internal class MainViewModel
    {
        #region Interfaces

        private readonly IDataProvider dataProvider;
        private readonly IDataParser dataParser;
        private readonly IFilter HPFFilter;
        private readonly IFilter LPFFilter;
        private readonly IQRSDetector QRSDetector;

        #endregion

        #region UI Properties

        public bool PlotGridVisibility { get; private set; }
        public bool StatisticsGridVisibility { get; private set; }
        public List<StatisticsItem> StatisticsItems { get; set; }
        public int HeartRate { get; set; }
        public string Diagnosis { get; set; }

        #endregion

        #region Data Properties

        public List<DataPoint> Data { get; private set; }
        public List<DataPoint> HPFFilteredData { get; private set; }
        public List<DataPoint> LPFFilteredData { get; private set; }
        public List<DataPoint> QRSPoints { get; private set; }

        #endregion

        #region Constructors

        public MainViewModel()
        {
            dataProvider = new CSVLoader();
            dataParser = new CSVParser();
            HPFFilter = new HighPassFilter();
            LPFFilter = new LowPassFilter();
            QRSDetector = new QRSDetector();

            Data = new List<DataPoint>();
            HPFFilteredData = new List<DataPoint>();
            LPFFilteredData = new List<DataPoint>();
            QRSPoints = new List<DataPoint>();
            StatisticsItems = new List<StatisticsItem>();
        }

        #endregion

        #region Commands

        public ICommand Load
        {
            get { return new RelayCommand<object>(LoadExecute, () => true); }
        }

        public ICommand ApplyHPF
        {
            get { return new RelayCommand<object>(ApplyHPFExecute, () => true); }
        }

        public ICommand ApplyLPF
        {
            get { return new RelayCommand<object>(ApplyLPFExecute, () => true); }
        }

        public ICommand FindQRS
        {
            get { return new RelayCommand<object>(FindQRSExecute, () => true); }
        }

        public ICommand Exit
        {
            get { return new RelayCommand<object>(delegate { Application.Current.Shutdown(); }, () => true); }
        }

        public ICommand Statistics
        {
            get { return new RelayCommand<object>(StatisticsExecute, () => true); }
        }

        #endregion

        #region Methods for Commands

        private async void LoadExecute(object o)
        {
            MetroWindow metroWindow = o as MetroWindow;

            /* Open .csv file */
            OpenFileDialog fileDialog = new OpenFileDialog()
            {
                Filter = "CSV Files (.csv)|*.csv|All Files (*.*)|*.*",
                FilterIndex = 1,
                Multiselect = false
            };

            if (fileDialog.ShowDialog() == false) return;

            //IEnumerable<string> rawData = File.ReadAllLines(fileDialog.FileName);

            /* Get all records */
            List<string> rawData = dataProvider.LoadData(new FileStream(fileDialog.FileName, FileMode.Open));

            if (!rawData.Any())
            {
                await metroWindow.ShowMessageAsync("Error", "Records from the couldn't be loaded.");
                return;
            }

            /* Get appropriate data */
            Data = dataParser.Parse(rawData);
            //Data = tmpData;

            if (!Data.Any())
                await metroWindow.ShowMessageAsync("Error", "ECG data couldn't be parsed.");
            else
            {
                await metroWindow.ShowMessageAsync("Success", "ECG data has been successfully loaded.");
                PlotGridVisibility = true;
            }
        }

        private void ApplyHPFExecute(object obj)
        {
            HPFFilteredData = ((HighPassFilter) HPFFilter).Denoise(Data);
        }

        private void ApplyLPFExecute(object obj)
        {
            LPFFilteredData = ((LowPassFilter) LPFFilter).Denoise(HPFFilteredData);
        }

        private void FindQRSExecute(object obj)
        {
            List<Tuple<DataPoint, int>> approxPeaks = QRSDetector.DetectPeaksNeighbourhood(LPFFilteredData);
            QRSPoints = QRSDetector.DetectQRSPeaks(Data, approxPeaks);
        }

        private void StatisticsExecute(object obj)
        {
            foreach (var point in QRSPoints)
            {
                StatisticsItems.Add(new StatisticsItem(){ElapsedTime = String.Format("{0:0.00}", point.X), Amplitude = String.Format("{0:0.00}", point.Y)});
            }

            HeartRate = CalculateHeartRate(QRSPoints);

            if (HeartRate > 100)
            {
                Diagnosis = "(Tachycardia)";
            }
            else if (HeartRate < 60)
            {
                Diagnosis = "(Bradycardia)";
            }
            else
            {
                Diagnosis = "(Normal Rate)";
            }

            this.StatisticsGridVisibility = true;
            this.PlotGridVisibility = false;
        }

        private int CalculateHeartRate(List<DataPoint> QRSPoints)
        {
            double intervalsLength = 0;
            double avrgIntervalLenght = 0;

            for (int i = 0; i < QRSPoints.Count - 1; i++)
            {
                intervalsLength += QRSPoints[i + 1].X - QRSPoints[i].X;
            }

            avrgIntervalLenght = intervalsLength/(QRSPoints.Count - 1);

            return (int) (60/avrgIntervalLenght);
        }

        #endregion
    }

    class StatisticsItem
    {
        public string TimeLabel
        {
            get { return "Elapsed time:"; }
        }

        public string ElapsedTime { get; set; }

        public string AmplitudeLabel
        {
            get { return "Amplitude:"; }
        }

        public string Amplitude { get; set; } 
    }
}