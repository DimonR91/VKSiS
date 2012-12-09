using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO.Ports;
using System.Threading;

namespace vksis_lab_1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<string> _speedComboBoxItems = new List<string> { "115200", "57600", "38400", "19200", "9600", "4800", "2400" };
        private string _currentCom = string.Empty;
        private string _currentSpeed = string.Empty;
        SerialPort _serialPort;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MyInitializator()
        {
            endButton.IsEnabled = false;
            comComboBox.ItemsSource = SerialPort.GetPortNames().OrderBy(currentName => currentName);
            speedComboBox.ItemsSource = _speedComboBoxItems;
            SendTextBox.IsEnabled = false;
            ReceiveTextBox.IsReadOnly = true;
            comComboBox.SelectedIndex = 0;
            speedComboBox.SelectedIndex = 0;
        }

        private void speedComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _currentSpeed =  e.AddedItems[0].ToString();
        }

        private void comComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _currentCom = e.AddedItems[0].ToString();
        }

        private void beginButton_Click(object sender, RoutedEventArgs e)
        {
            beginButton.IsEnabled = false;
            endButton.IsEnabled = true;
            SendTextBox.IsEnabled = true;
            InitComPort();
            speedComboBox.IsEnabled = false;
            comComboBox.IsEnabled = false;
        }

        private void endButton_Click(object sender, RoutedEventArgs e)
        {
            endButton.IsEnabled = false;
            beginButton.IsEnabled = true;
            SendTextBox.IsEnabled = false;
            _serialPort.Close();
            _serialPort.Dispose();
            speedComboBox.IsEnabled = true;
            comComboBox.IsEnabled = true;
            SendTextBox.Clear();
            ReceiveTextBox.Clear();
        }

        private void InitComPort()
        {
            _serialPort = new SerialPort(_currentCom, int.Parse(_currentSpeed), Parity.None, 8, StopBits.One);
            _serialPort.Open();

            _serialPort.ErrorReceived += new SerialErrorReceivedEventHandler(serialPort_ErrorReceived);
            _serialPort.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived);
        }

        void serialPort_DataReceived(object sender, EventArgs e)
        {
            int bytesToRead = _serialPort.BytesToRead;
            byte[] data = new byte[bytesToRead];
            _serialPort.Read(data, 0, bytesToRead);
            Dispatcher.Invoke(new Action(delegate
                {
                    for (int i = 0; i < bytesToRead; i++)
                    {
                        ReceiveTextBox.Text += (char)data[i];
                    }
                }));
        }

        void serialPort_ErrorReceived(object sender, EventArgs e)
        {
            MessageBox.Show("Error!");
        }

        private void SendTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.Changes.First().RemovedLength > 0)
                return;

            _serialPort.RtsEnable = true;
            _serialPort.Write(new char[]{SendTextBox.Text[SendTextBox.Text.Length - 1]}, 0, 1);
            Thread.Sleep(100);
            _serialPort.RtsEnable = false;
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            MyInitializator();
        }


    }
}
