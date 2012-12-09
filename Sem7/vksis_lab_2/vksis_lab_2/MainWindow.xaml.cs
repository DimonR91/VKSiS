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

namespace vksis_lab_2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IEnumerable<string> _availableCOMPorts;
        private SerialPort _serialPortCOM1;
        private SerialPort _serialPortCOM2;
        private const string COM1 = "COM1";
        private const string COM2 = "COM2";
        private const int Speed115200 = 115200;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            InputTextBox.Clear();
            PacketInfoTextBox.Clear();
            OutputTextBox.Clear();
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            try
            {
                _availableCOMPorts = SerialPort.GetPortNames().OrderBy(currentName => currentName);
                if (!_availableCOMPorts.Contains(COM1) || !_availableCOMPorts.Contains(COM2))
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.AppendLine("You need to have COM1 and COM2 ports!");
                    stringBuilder.AppendLine("Available COM ports:");
                    foreach (string COMPort in _availableCOMPorts)
                    {
                        stringBuilder.AppendLine(COMPort);
                    }
                    throw new ApplicationException(stringBuilder.ToString());
                }
                _serialPortCOM1 = InitComPort(COM1, Speed115200);
                _serialPortCOM2 = InitComPort(COM2, Speed115200);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private SerialPort InitComPort(string COMPort, int speed)
        {
            var serialPort = new SerialPort(COMPort, speed, Parity.None, 8, StopBits.One);
            serialPort.Open();

            serialPort.ErrorReceived += new SerialErrorReceivedEventHandler(serialPort_ErrorReceived);
            serialPort.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived);

            return serialPort;
        }

        void serialPort_DataReceived(object sender, EventArgs e)
        {
            int bytesToRead = _serialPortCOM2.BytesToRead;
            byte[] data = new byte[bytesToRead];
            _serialPortCOM2.Read(data, 0, bytesToRead);

            List<bool> bits = new List<bool>();
            foreach (byte b in data)
            {
                bits.AddRange(PacketHelper.GetBitsOfByte(b));
            }

            List<Packet> packets = Packet.GetPackets(bits, (byte)Convert.ToInt16("01111110", 2));

            Dispatcher.Invoke(new Action(delegate
            {
                StringBuilder outputStringBuilder = new StringBuilder();
                StringBuilder infoStringBuilder = new StringBuilder();

                int n = 1;
                foreach (Packet packet in packets)
                {
                    infoStringBuilder.AppendLine(string.Format("Packet#{0}", n++));
                    infoStringBuilder.AppendLine(string.Format("Stuff Byte: {0}", packet.StuffByte));
                    infoStringBuilder.AppendLine(string.Format("Dest Adr: {0}", packet.DestinationAddress));
                    infoStringBuilder.AppendLine(string.Format("Source Adr: {0}", packet.SourceAddress));
                    infoStringBuilder.AppendLine(string.Format("Control Code: {0}", packet.ControlCode));
                    infoStringBuilder.AppendLine(string.Format(string.Format("Encoded msg:\n{0}", PacketHelper.BinDataToString(packet.EncodedData))));
                    infoStringBuilder.AppendLine();

                    outputStringBuilder.Append(packet.Message);
                }
                PacketInfoTextBox.Text = infoStringBuilder.ToString();
                OutputTextBox.Text = outputStringBuilder.ToString();
            }));
        }

        void serialPort_ErrorReceived(object sender, EventArgs e)
        {
            MessageBox.Show("Error!");
        }

        private void InputTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    if (InputTextBox.Text.Length > 0)
                    {
                        PacketHelper packetHelper = new PacketHelper((byte)Convert.ToInt16("01111110", 2), InputTextBox.Text.ToCharArray(), 19, 1, 1, 1);
                        List<Packet> packets = packetHelper.GetPackets();
                        List<bool> data = new List<bool>();
                        StringBuilder stringBuilder = new StringBuilder();
                        int n = 1;
                        foreach (Packet packet in packets)
                        {
                            data.AddRange(packet.EncodedData);
                            // stringBuilder.AppendLine(string.Format("Packet #{0}", n++));
                            // stringBuilder.AppendLine(PacketHelper.BinDataToString(packet.EncodedData));
                        }

                        // PacketInfoTextBox.Text = stringBuilder.ToString();

                        int numOfBits = data.Count;
                        int numOfCycles = numOfBits % 8 == 0 ? numOfBits : numOfBits + 1;

                        List<byte> dataToSend = new List<byte>();
                        for (int i = 0; i < numOfCycles / 8; i++)
                        {
                            if (i != numOfCycles / 8 - 1)
                            {
                                dataToSend.Add((byte)Convert.ToInt16(PacketHelper.BinDataToString(data.GetRange(i * 8, 8)), 2));
                            }
                            else
                            {
                                dataToSend.Add((byte)Convert.ToInt16(PacketHelper.BinDataToString(data.GetRange(i * 8, numOfBits - i * 8)), 2));
                            }
                        }

                        _serialPortCOM1.RtsEnable = true;
                        _serialPortCOM1.Write(dataToSend.ToArray(), 0, dataToSend.Count);
                        Thread.Sleep(100);
                        _serialPortCOM1.RtsEnable = false;

                        //InputTextBox.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
