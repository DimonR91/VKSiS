using System;
using System.ComponentModel;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using PacketHelper;

namespace vksis_lab_5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Fields

        private BackgroundWorker _backgroundWorkerReader;
        private BackgroundWorker _backgroundWorkerCollision;
        private const int MagicNumber_SizeOfPacket = 1058;
        private MemoryMappedFile _messageMemoryMappedFile;
        private MemoryMappedFile _collisionFlagMemoryMappedFile;
        private const int SlotTime = 1;
        private const int AttemptLimit = 16;
        private const int BackoffLimit = 10;
        private int _attemptNumber = 0;
        private string _messageToSend;
        private string _readMessage;

        #endregion

        public MainWindow()
        {
            InitializeComponent();
        }

        private void InitSharedMemory()
        {
            _messageMemoryMappedFile = MemoryMappedFile.CreateOrOpen(@"messageMemory", 1024*1024, MemoryMappedFileAccess.ReadWrite);

            _collisionFlagMemoryMappedFile = MemoryMappedFile.CreateOrOpen(@"collisionFlag", 8, MemoryMappedFileAccess.ReadWrite);
            SetCollision(false);
        }

        private void WindowLoaded1(object sender, RoutedEventArgs e)
        {
            try
            {
                InitSharedMemory();

                _backgroundWorkerReader = new BackgroundWorker { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
                _backgroundWorkerReader.DoWork += ReadMessage;
                _backgroundWorkerReader.ProgressChanged += ShowMessage;
                _backgroundWorkerReader.RunWorkerAsync();

                _backgroundWorkerCollision = new BackgroundWorker { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
                _backgroundWorkerCollision.DoWork += DetectCollisionStatus;
                _backgroundWorkerCollision.ProgressChanged += ShowCollisionStatus;
                _backgroundWorkerCollision.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ReadMessage(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                Thread.Sleep(900);
                if (IsCollisionDetected())
                {
                    continue;
                }

                var stringBuilder = new StringBuilder();
                try
                {
                    using (MemoryMappedViewAccessor fileMap = _messageMemoryMappedFile.CreateViewAccessor())
                    {
                        long pos = 0;
                        var boolArray = new byte[MagicNumber_SizeOfPacket];
                        while (true)
                        {
                            fileMap.ReadArray(pos, boolArray, 0, MagicNumber_SizeOfPacket);
                            using (var stream = new MemoryStream(boolArray))
                            {
                                var binaryFormatter = new BinaryFormatter();
                                stringBuilder.Append(((Packet)binaryFormatter.Deserialize(stream)).Message);
                            }
                            pos += MagicNumber_SizeOfPacket;
                        }
                    }
                }
                catch (Exception)
                {
                    if (_readMessage != stringBuilder.ToString())
                    {
                        _readMessage = stringBuilder.ToString();
                        ((BackgroundWorker)sender).ReportProgress(0, _readMessage);
                    }
                }
            }
        }

        private void ShowMessage(object sender, ProgressChangedEventArgs e)
        {
            OutputTextBox.Text += (string)e.UserState;
        }

        private void DetectCollisionStatus(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                Thread.Sleep(500);
                if( IsCollisionDetected())
                {
                    ((BackgroundWorker) sender).ReportProgress(0, "Collision!");   
                }
            }
        }

        private void ShowCollisionStatus(object sender, ProgressChangedEventArgs e)
        {
            PacketInfoTextBox.Text += (string)e.UserState;
        }

        private void InputTextBoxKeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    _attemptNumber = 0;
                    _messageToSend = InputTextBox.Text;
                    TrySendMessage();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void TrySendMessage()
        {
            if (_attemptNumber >= AttemptLimit)
            {
                MessageBox.Show("Excessive Collision Error!");
                return;
            }

            if (!IsCollisionDetected())
            {
                SendMessage();
                Thread.Sleep(1000);
                if(!CheckMessage())
                {
                    SetCollision(true);
                }
                else
                {
                    _attemptNumber = 0;
                    return;   
                }
            }

            int backOff = GetBackOff();
            _attemptNumber++;
            Thread.Sleep(backOff * SlotTime * 1000);

            SetCollision(false);
            TrySendMessage();
        }

        private void SendMessage()
        {
            var packetHelper = new PacketHelper.PacketHelper((byte) Convert.ToInt16("01111110", 2),
                                                             _messageToSend.ToCharArray(),
                                                             19,
                                                             0,
                                                             0,
                                                             1);

            using (MemoryMappedViewAccessor fileMap = _messageMemoryMappedFile.CreateViewAccessor())
            {
                long pos = 0;
                foreach (var packet in packetHelper.GetPackets())
                {
                    var binaryFormatter = new BinaryFormatter();
                    using (var stream = new MemoryStream())
                    {
                        binaryFormatter.Serialize(stream, packet);
                        foreach (var b in stream.ToArray())
                        {
                            fileMap.Write(pos++, b);
                        }
                    }
                }
            }
        }

        private bool IsCollisionDetected()
        {
            bool collisionFlag;
            using (MemoryMappedViewAccessor fileMap = _collisionFlagMemoryMappedFile.CreateViewAccessor())
            {
                collisionFlag = fileMap.ReadBoolean(0);
            }
            return collisionFlag;
        }

        private void SetCollision(bool flag)
        {
            using (MemoryMappedViewAccessor fileMap = _collisionFlagMemoryMappedFile.CreateViewAccessor())
            {
                fileMap.Write(0, flag);
            }
        }

        private int GetBackOff()
        {
            double k = Math.Min(_attemptNumber, BackoffLimit);
            return new Random().Next(0, (int) Math.Pow(2.0, k));
        }

        private void ClearFieldsButtonClick(object sender, RoutedEventArgs e)
        {
            InputTextBox.Clear();
            OutputTextBox.Clear();
            PacketInfoTextBox.Clear();
        }

        private bool CheckMessage()
        {
            var stringBuilder = new StringBuilder();
            try
            {
                using (MemoryMappedViewAccessor fileMap = _messageMemoryMappedFile.CreateViewAccessor())
                {
                    long pos = 0;
                    var boolArray = new byte[MagicNumber_SizeOfPacket];
                    while (true)
                    {
                        fileMap.ReadArray(pos, boolArray, 0, MagicNumber_SizeOfPacket);
                        using (var stream = new MemoryStream(boolArray))
                        {
                            var binaryFormatter = new BinaryFormatter();
                            stringBuilder.Append(((Packet) binaryFormatter.Deserialize(stream)).Message);
                        }
                        pos += MagicNumber_SizeOfPacket;
                    }
                }
            }
            catch (Exception)
            {
            }

            return _messageToSend.CompareTo(stringBuilder.ToString()) == 0;
        }
    }
}
