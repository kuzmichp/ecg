using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using PropertyChanged;

namespace ECGAnalysisSystem.ViewModel
{
    /// <summary>
    /// ViewModel class for main application view
    /// </summary>
    [ImplementPropertyChanged]
    internal class MainViewModel
    {
        public MainViewModel()
        {

        }

        public ICommand Load
        {
            get { return new RelayCommand<object>(LoadExecute, () => true); }
        }

        private async void LoadExecute(object o)
        {
            MetroWindow metroWindow = o as MetroWindow;
            await metroWindow.ShowMessageAsync("This is the title", "Some message");
        }
    }
}
