using System.Windows;
using System.Windows.Input;

namespace vksis_lab_6
{
    /// <summary>
    /// Interaction logic for StationWindow.xaml
    /// </summary>
    public partial class StationWindow : Window
    {
        private string _readMessage;
        private string _sendMessage;

        public int SourceAddress { get; private set; }
        public int DestinationAddress { get; private set; }

        public StationWindow(int sourceAddr, int destAddr)
        {
            InitializeComponent();
            SourceAddress = sourceAddr;
            SourceId.Text = SourceAddress.ToString();
            DestinationAddress = destAddr;
            DestId.Text = DestinationAddress.ToString();
        }

        public StationWindow(int sourceAddr)
        {
            InitializeComponent();
            SourceAddress = sourceAddr;
            SourceId.Text = sourceAddr.ToString();
            DestinationAddress = sourceAddr != 0 ? sourceAddr - 1 : 0;
            DestId.Text = DestinationAddress.ToString();
        }

        private void Input_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                int sourceAddr;
                int destAddr;
                if (int.TryParse(DestId.Text, out destAddr) && int.TryParse(SourceId.Text, out sourceAddr))
                {
                    SourceAddress = sourceAddr;
                    DestinationAddress = destAddr;
                    _sendMessage = InputTextBox.Text;
                }
                else
                {
                    MessageBox.Show("Bad destination addr!");
                }
            }
        }

        public void ReadMessage(Message message)
        {
            // read message?
            if (message.SFS.AC.T == 1 && message.FcsCoverage.DA == SourceAddress)
            {
                message.EFS.FS.A = 1;
                OutputTextBox.Text = message.FcsCoverage.INFO;
            }

            // our message, need delete and frame => marker
            else if (message.SFS.AC.T == 1 && message.FcsCoverage.SA == SourceAddress && message.EFS.FS.A == 1)
            {
                message.SFS.AC.T = 0;
            }

            // send?
            else if (!string.IsNullOrEmpty(_sendMessage))
            {
                // if marker => frame
                if(message.SFS.AC.T == 0)
                {
                    message.SFS.AC.T = 1;
                    message.FcsCoverage.INFO = _sendMessage;
                    _sendMessage = string.Empty;
                    message.FcsCoverage.DA = DestinationAddress;
                    message.FcsCoverage.SA = SourceAddress;
                    message.EFS.FS.A = 0;
                }
            }
        }
    }
}
