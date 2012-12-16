using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace vksis_lab_4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        enum AddressType
        {
            Unicast,
            Multicast,
            Broadcast
        }

        private int[] _bufDestGroupId = new[] { 1, 2, 2, 1 };
        private int[] _bufDestId = new[] { 2, 1, 2, 1 };

        private string[] _curDestGroupId = new string[4];
        private string[] _curDestId = new string[4];

        private string[] _curSourceGroupId = new string[4];
        private string[] _curSourceId = new string[4];

        public MainWindow()
        {
            InitializeComponent();
            UpdateAddresses();
        }

        private void UpdateAddresses()
        {
            _curDestGroupId[0] = Get4CharsString(DestGroupId00.Text);
            _curDestId[0] = Get4CharsString(DestId00.Text);

            _curDestGroupId[1] = Get4CharsString(DestGroupId01.Text);
            _curDestId[1] = Get4CharsString(DestId01.Text);

            _curDestGroupId[2] = Get4CharsString(DestGroupId10.Text);
            _curDestId[2] = Get4CharsString(DestId10.Text);

            _curDestGroupId[3] = Get4CharsString(DestGroupId11.Text);
            _curDestId[3] = Get4CharsString(DestId11.Text);


            _curSourceGroupId[0] = Get4CharsString(SourceGroupId00.Text);
            _curSourceId[0] = Get4CharsString(SourceId00.Text);

            _curSourceGroupId[1] = Get4CharsString(SourceGroupId01.Text);
            _curSourceId[1] = Get4CharsString(SourceId01.Text);

            _curSourceGroupId[2] = Get4CharsString(SourceGroupId10.Text);
            _curSourceId[2] = Get4CharsString(SourceId10.Text);

            _curSourceGroupId[3] = Get4CharsString(SourceGroupId11.Text);
            _curSourceId[3] = Get4CharsString(SourceId11.Text);
        }

        private string Get4CharsString(string text)
        {
            char[] charArray = text.ToCharArray();
            Array.Reverse(charArray);

            char[] result = new[] { '0', '0', '0', '0' };
            for (int i = 0; i < charArray.Length && i < 4; i++)
            {
                result[i] = charArray[i];
            }
            Array.Reverse(result);
            return new string(result);
        }

        private string Get8CharsString(string text)
        {
            char[] charArray = text.ToCharArray();
            Array.Reverse(charArray);

            char[] result = new[] { '0', '0', '0', '0', '0', '0', '0', '0' };
            for (int i = 0; i < charArray.Length && i < 8; i++)
            {
                result[i] = charArray[i];
            }
            Array.Reverse(result);
            return new string(result);
        }

        private void ComboBox00_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch((sender as ComboBox).SelectedIndex)
            {
                case (int)AddressType.Unicast:
                    DestGroupId00.Text = Convert.ToString(_bufDestGroupId[0], 2);
                    DestId00.Text = Convert.ToString(_bufDestId[0], 2);
                    DestGroupId00.IsEnabled = true;
                    DestId00.IsEnabled = true;
                    break;
                case (int)AddressType.Multicast:
                    DestGroupId00.IsEnabled = true;
                    DestGroupId00.Text = Convert.ToString(_bufDestGroupId[0], 2);
                    DestId00.IsEnabled = false;
                    if (DestId00.Text != "1111")
                    {
                        _bufDestId[0] = Convert.ToInt32(DestId00.Text, 2);
                    }
                    DestId00.Text = "1111";
                    break;
                case (int)AddressType.Broadcast:

                    DestGroupId00.IsEnabled = false;
                    if (DestGroupId00.Text != "1111")
                    {
                        _bufDestGroupId[0] = Convert.ToInt32(DestGroupId00.Text, 2);
                    }
                    DestGroupId00.Text = "1111";

                    DestId00.IsEnabled = false;
                    if (DestId00.Text != "1111")
                    {
                        _bufDestId[0] = Convert.ToInt32(DestId00.Text, 2);
                    }
                    DestId00.Text = "1111";
                    break;
            }
        }

        private void ComboBox01_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch ((sender as ComboBox).SelectedIndex)
            {
                case (int)AddressType.Unicast:
                    DestGroupId01.Text = Convert.ToString(_bufDestGroupId[1], 2);
                    DestId01.Text = Convert.ToString(_bufDestId[1], 2);
                    DestGroupId01.IsEnabled = true;
                    DestId01.IsEnabled = true;
                    break;
                case (int)AddressType.Multicast:
                    DestGroupId01.IsEnabled = true;
                    DestGroupId01.Text = Convert.ToString(_bufDestGroupId[1], 2);
                    DestId01.IsEnabled = false;
                    if (DestId01.Text != "1111")
                    {
                        _bufDestId[1] = Convert.ToInt32(DestId01.Text, 2);
                    }
                    DestId01.Text = "1111";
                    break;
                case (int)AddressType.Broadcast:

                    DestGroupId01.IsEnabled = false;
                    if (DestGroupId01.Text != "1111")
                    {
                        _bufDestGroupId[1] = Convert.ToInt32(DestGroupId01.Text, 2);
                    }
                    DestGroupId01.Text = "1111";

                    DestId01.IsEnabled = false;
                    if (DestId01.Text != "1111")
                    {
                        _bufDestId[1] = Convert.ToInt32(DestId01.Text, 2);
                    }
                    DestId01.Text = "1111";
                    break;
            }
        }

        private void ComboBox10_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch ((sender as ComboBox).SelectedIndex)
            {
                case (int)AddressType.Unicast:
                    DestGroupId10.Text = Convert.ToString(_bufDestGroupId[2], 2);
                    DestId10.Text = Convert.ToString(_bufDestId[2], 2);
                    DestGroupId10.IsEnabled = true;
                    DestId10.IsEnabled = true;
                    break;
                case (int)AddressType.Multicast:
                    DestGroupId10.IsEnabled = true;
                    DestGroupId10.Text = Convert.ToString(_bufDestGroupId[2], 2);
                    DestId10.IsEnabled = false;
                    if (DestId10.Text != "1111")
                    {
                        _bufDestId[2] = Convert.ToInt32(DestId10.Text, 2);
                    }
                    DestId10.Text = "1111";
                    break;
                case (int)AddressType.Broadcast:

                    DestGroupId10.IsEnabled = false;
                    if (DestGroupId10.Text != "1111")
                    {
                        _bufDestGroupId[2] = Convert.ToInt32(DestGroupId10.Text, 2);
                    }
                    DestGroupId10.Text = "1111";

                    DestId10.IsEnabled = false;
                    if (DestId10.Text != "1111")
                    {
                        _bufDestId[2] = Convert.ToInt32(DestId10.Text, 2);
                    }
                    DestId10.Text = "1111";
                    break;
            }
        }

        private void ComboBox11_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch ((sender as ComboBox).SelectedIndex)
            {
                case (int)AddressType.Unicast:
                    DestGroupId11.Text = Convert.ToString(_bufDestGroupId[3], 2);
                    DestId11.Text = Convert.ToString(_bufDestId[3], 2);
                    DestGroupId11.IsEnabled = true;
                    DestId11.IsEnabled = true;
                    break;
                case (int)AddressType.Multicast:
                    DestGroupId11.IsEnabled = true;
                    DestGroupId11.Text = Convert.ToString(_bufDestGroupId[3], 2);
                    DestId11.IsEnabled = false;
                    if (DestId11.Text != "1111")
                    {
                        _bufDestId[3] = Convert.ToInt32(DestId11.Text, 2);
                    }
                    DestId11.Text = "1111";
                    break;
                case (int)AddressType.Broadcast:

                    DestGroupId11.IsEnabled = false;
                    if (DestGroupId11.Text != "1111")
                    {
                        _bufDestGroupId[3] = Convert.ToInt32(DestGroupId11.Text, 2);
                    }
                    DestGroupId11.Text = "1111";

                    DestId11.IsEnabled = false;
                    if (DestId11.Text != "1111")
                    {
                        _bufDestId[3] = Convert.ToInt32(DestId11.Text, 2);
                    }
                    DestId11.Text = "1111";
                    break;
            }
        }

        private bool MatchAddress(string destAddr, string sourceAddr)
        {
            int destAddrInt = Convert.ToInt32(destAddr, 16);
            string destAddrBin = Get8CharsString(Convert.ToString(destAddrInt, 2));

            char[] dest = destAddrBin.ToCharArray();
            char[] source = sourceAddr.ToCharArray();

            for (int i = 0; i < source.Length; i++)
            {
                if (source[i] == '1' && dest[i] == '0')
                {
                    return false;
                }
            }

            return true;
        }

        private bool MatchAddressStrict(string destAddr, string sourceAddr)
        {
            int destAddrInt = Convert.ToInt32(destAddr, 16);
            string destAddrBin = Get8CharsString(Convert.ToString(destAddrInt, 2));

            char[] dest = destAddrBin.ToCharArray();
            char[] source = sourceAddr.ToCharArray();

            for (int i = 0; i < source.Length; i++)
            {
                if (source[i] != dest[i])
                {
                    return false;
                }
            }

            return true;
        }

        private bool MatchAddressStrictFirstPart(string destAddr, string sourceAddr)
        {
            int destAddrInt = Convert.ToInt32(destAddr, 16);
            string destAddrBin = Get8CharsString(Convert.ToString(destAddrInt, 2));

            char[] dest = destAddrBin.ToCharArray();
            char[] source = sourceAddr.ToCharArray();

            for (int i = 0; i < source.Length; i++)
            {
                if (i < 4)
                {
                    if (source[i] != dest[i])
                    {
                        return false;
                    }
                }
                else
                {
                    if (source[i] == '1' && dest[i] == '0')
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private void ClearOutputs()
        {
            Output00.Clear();
            Output01.Clear();
            Output10.Clear();
            Output11.Clear();
        }

        private void Input00_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    UpdateAddresses();
                    ClearOutputs();
                    if (Input00.Text.Length > 0)
                    {
                        var packetHelper = new PacketHelper((byte) Convert.ToInt16("01111110", 2),
                                                            Input00.Text.ToCharArray(),
                                                            19,
                                                            (byte) Convert.ToInt16(string.Format("{0}{1}", _curDestGroupId[0], _curDestId[0]), 2),
                                                            (byte) Convert.ToInt16(string.Format("{0}{1}", _curSourceGroupId[0], _curSourceId[0]), 2),
                                                            1);

                        ShowMessages(packetHelper.GetPackets(), ComboBox00.SelectedIndex);
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Input01_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    UpdateAddresses();
                    ClearOutputs();
                    if (Input01.Text.Length > 0)
                    {
                        var packetHelper = new PacketHelper((byte)Convert.ToInt16("01111110", 2),
                                                            Input01.Text.ToCharArray(),
                                                            19,
                                                            (byte)Convert.ToInt16(string.Format("{0}{1}", _curDestGroupId[1], _curDestId[1]), 2),
                                                            (byte)Convert.ToInt16(string.Format("{0}{1}", _curSourceGroupId[1], _curSourceId[1]), 2),
                                                            1);

                        ShowMessages(packetHelper.GetPackets(), ComboBox01.SelectedIndex);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
            
        private void Input10_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    UpdateAddresses();
                    ClearOutputs();
                    if (Input10.Text.Length > 0)
                    {
                        var packetHelper = new PacketHelper((byte)Convert.ToInt16("01111110", 2),
                                                            Input10.Text.ToCharArray(),
                                                            19,
                                                            (byte)Convert.ToInt16(string.Format("{0}{1}", _curDestGroupId[2], _curDestId[2]), 2),
                                                            (byte)Convert.ToInt16(string.Format("{0}{1}", _curSourceGroupId[2], _curSourceId[2]), 2),
                                                            1);

                        ShowMessages(packetHelper.GetPackets(), ComboBox10.SelectedIndex);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Input11_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    UpdateAddresses();
                    ClearOutputs();
                    if (Input11.Text.Length > 0)
                    {
                        var packetHelper = new PacketHelper((byte)Convert.ToInt16("01111110", 2),
                                                            Input11.Text.ToCharArray(),
                                                            19,
                                                            (byte)Convert.ToInt16(string.Format("{0}{1}", _curDestGroupId[3], _curDestId[3]), 2),
                                                            (byte)Convert.ToInt16(string.Format("{0}{1}", _curSourceGroupId[3], _curSourceId[3]), 2),
                                                            1);

                        ShowMessages(packetHelper.GetPackets(), ComboBox11.SelectedIndex);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ShowMessages(IEnumerable<Packet> packets, int AddrTypeIdx)
        {
            foreach (var packet in packets)
            {
                if (AddrTypeIdx == (int)AddressType.Broadcast)
                {
                    // 00
                    if (MatchAddress(packet.DestinationAddress,
                                     string.Format("{0}{1}", _curSourceGroupId[0], _curSourceId[0])))
                    {
                        Output00.Text += packet.Message;
                    }
                    // 01
                    if (MatchAddress(packet.DestinationAddress,
                                     string.Format("{0}{1}", _curSourceGroupId[1], _curSourceId[1])))
                    {
                        Output01.Text += packet.Message;
                    }
                    // 10
                    if (MatchAddress(packet.DestinationAddress,
                                     string.Format("{0}{1}", _curSourceGroupId[2], _curSourceId[2])))
                    {
                        Output10.Text += packet.Message;
                    }
                    // 11
                    if (MatchAddress(packet.DestinationAddress,
                                     string.Format("{0}{1}", _curSourceGroupId[3], _curSourceId[3])))
                    {
                        Output11.Text += packet.Message;
                    }
                }
                else if (AddrTypeIdx == (int)AddressType.Multicast)
                {
                    // 00
                    if (MatchAddressStrictFirstPart(packet.DestinationAddress,
                                     string.Format("{0}{1}", _curSourceGroupId[0], _curSourceId[0])))
                    {
                        Output00.Text += packet.Message;
                    }
                    // 01
                    if (MatchAddressStrictFirstPart(packet.DestinationAddress,
                                     string.Format("{0}{1}", _curSourceGroupId[1], _curSourceId[1])))
                    {
                        Output01.Text += packet.Message;
                    }
                    // 10
                    if (MatchAddressStrictFirstPart(packet.DestinationAddress,
                                     string.Format("{0}{1}", _curSourceGroupId[2], _curSourceId[2])))
                    {
                        Output10.Text += packet.Message;
                    }
                    // 11
                    if (MatchAddressStrictFirstPart(packet.DestinationAddress,
                                     string.Format("{0}{1}", _curSourceGroupId[3], _curSourceId[3])))
                    {
                        Output11.Text += packet.Message;
                    }
                }
                else
                {
                    // 00
                    if (MatchAddressStrict(packet.DestinationAddress,
                                     string.Format("{0}{1}", _curSourceGroupId[0], _curSourceId[0])))
                    {
                        Output00.Text += packet.Message;
                    }
                    // 01
                    if (MatchAddressStrict(packet.DestinationAddress,
                                     string.Format("{0}{1}", _curSourceGroupId[1], _curSourceId[1])))
                    {
                        Output01.Text += packet.Message;
                    }
                    // 10
                    if (MatchAddressStrict(packet.DestinationAddress,
                                     string.Format("{0}{1}", _curSourceGroupId[2], _curSourceId[2])))
                    {
                        Output10.Text += packet.Message;
                    }
                    // 11
                    if (MatchAddressStrict(packet.DestinationAddress,
                                     string.Format("{0}{1}", _curSourceGroupId[3], _curSourceId[3])))
                    {
                        Output11.Text += packet.Message;
                    }
                }
            }
        }
    }
}
