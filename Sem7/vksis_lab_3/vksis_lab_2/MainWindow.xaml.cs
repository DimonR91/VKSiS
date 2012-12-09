using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.IO.Ports;
using System.Threading;

namespace vksis_lab_2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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
        }

        private void InputTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    if (InputTextBox.Text.Length > 0)
                    {
                        var packetHelper = new PacketHelper(InputTextBox.Text.ToCharArray(), 3);
                        List<Packet> packets = packetHelper.GetPackets((bool)MakeErrorCheckBox.IsChecked);

                        PacketInfoTextBox.Clear();
                        var infoStringBuilder = new StringBuilder();
                        var outputStringBuilder = new StringBuilder();
                        for (int i = 0; i < packets.Count(); i++)
                        {
                            infoStringBuilder.AppendLine(string.Format("Packet #{0}", i));
                            string errorText = packets[i].GetErrorInfo();
                            if(!string.IsNullOrEmpty(errorText))
                            {
                                infoStringBuilder.AppendLine(errorText);
                            }
                            infoStringBuilder.AppendLine(packets[i].GetEncodedData());
                            infoStringBuilder.AppendLine();

                            outputStringBuilder.Append(packets[i].GetDecodedData());
                        }
                        PacketInfoTextBox.Text = infoStringBuilder.ToString();
                        OutputTextBox.Text = outputStringBuilder.ToString();
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
