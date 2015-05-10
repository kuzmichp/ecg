using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;
using ECGAnalysisSystem.DataAccess;
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
        private IFilter hpfFilter;
        private IFilter lpfFilter;

        #endregion

        #region UI Properties

        public bool PlotGridVisibility { get; private set; }

        #endregion

        #region Data Properties

        public List<DataPoint> Data { get; private set; }
        public List<DataPoint> HPFFilteredData { get; private set; }
        public List<DataPoint> LPFFilteredData { get; private set; }

        #endregion

        #region Constructors

        public MainViewModel()
        {
            dataProvider = new CSVLoader();
            dataParser = new CSVParser();
            hpfFilter = new HighPassFilter();
            lpfFilter = new LowPassFilter();
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
            HPFFilteredData = ((HighPassFilter) hpfFilter).Denoise(Data);
        }

        private void ApplyLPFExecute(object obj)
        {
            LPFFilteredData = ((LowPassFilter) lpfFilter).Denoise(HPFFilteredData);
        }

        #endregion
    }
}