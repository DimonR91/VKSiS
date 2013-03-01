using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows;

namespace vksis_lab_6
{
    /// <summary>
    /// Interaction logic for SystemManagerWindow.xaml
    /// </summary>
    public partial class SystemManagerWindow : Window
    {
        private BackgroundWorker _backgroundWorker;
        private readonly List<StationWindow> _stations = new List<StationWindow>();
        private readonly Message _message = new Message();
        private int _curId;

        public SystemManagerWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            RemoveStationButton.IsEnabled = false;

            var station1 = new StationWindow(1, 2);
            station1.Show();
            _stations.Add(station1);
            var station2 = new StationWindow(2, 1);
            station2.Show();
            _stations.Add(station2);

            _backgroundWorker = new BackgroundWorker { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
            _backgroundWorker.DoWork += RunMarker;
            _backgroundWorker.ProgressChanged += ReadMessage;
            _backgroundWorker.RunWorkerAsync();
        }

        private void RunMarker(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                for (int i = 0; i < _stations.Count; i++)
                {
                    _curId = i;
                    Thread.Sleep(2000);
                    ((BackgroundWorker)sender).ReportProgress(0);
                }
            }
        }

        private void ReadMessage(object sender, ProgressChangedEventArgs e)
        {
            _stations[_curId].ReadMessage(_message);
        }

        private void AddStationButton_Click(object sender, RoutedEventArgs e)
        {
            var station = new StationWindow(_stations.Count + 1);
            station.Show();
            _stations.Add(station);
            RemoveStationButton.IsEnabled = true;
        }

        private void RemoveStationButton_Click(object sender, RoutedEventArgs e)
        {
            StationWindow stationToRemove = _stations[_stations.Count - 1];
            stationToRemove.Close();
            _stations.Remove(stationToRemove);
            if(_stations.Count == 2)
            {
                RemoveStationButton.IsEnabled = false;
            }
        }
    }
}
