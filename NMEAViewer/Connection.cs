using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NMEAViewer
{
    public partial class Connection : DockableDrawable // Form
    {
        int m_iBytesRead = 0;
        //NMEAStreamReader m_Reader;
        DataWriter m_DataWriter;
        MetaDataSerializer m_MetaData;
        ApplicationSettings m_AppSettings;
        DateTime m_StartTime;
        System.IO.Stream m_SimulationStream;
        DataReader m_SimulationDataReader;
        Timer m_SimulationTimer;
        TcpClient TCPConnection;
        bool m_bValidIPAndPort=false;

        //public delegate void VoidConsumer();  // defines a delegate type
        public delegate void DataConsumer(byte []data);  // defines a delegate type
        public delegate void StringConsumer(string value);  // defines a delegate type
        public delegate void OnNewConnectionType();
        public delegate void OnNewData(string value, double fElapsedTime);
        public event OnNewConnectionType OnNewConnection;
        public event OnNewData OnDataRecieved;

        public static Connection sm_Connection = null;


        private class SerializedData : DockableDrawable.SerializedDataBase
        {
            public SerializedData(DockableDrawable parent)
                : base(parent) { }

            public string IP;
            public bool bIPChecked;
            public bool bComChecked;

            public string OutputFileName;
            public string SimulationFileName;
        };

        public override DockableDrawable.SerializedDataBase CreateSerializedData()
        {
            SerializedData data = new SerializedData(this);
            data.IP = IPTextBox.Text;
            data.bIPChecked = IPRadioButton.Checked;
            data.bComChecked = ComportRadioButton.Checked;
            data.OutputFileName = OutputFileName.Text;
            data.SimulationFileName = SimulationFileName.Text;
            
            return data;
        }

        public override void InitFromSerializedData(SerializedDataBase data_base)
        {
            base.InitFromSerializedData(data_base);

            SerializedData data = (SerializedData)data_base;
            IPTextBox.Text = data.IP;
            IPRadioButton.Checked = data.bIPChecked;
            ComportRadioButton.Checked = data.bComChecked;
            OutputFileName.Text = data.OutputFileName;
            SimulationFileName.Text = data.SimulationFileName;

            LogBox.Text += "Restored settings from saved project\r\n";
        }

        public override void PostInitFromSerializedData(SerializedDataBase data_base)
        {
            SerializedData data = (SerializedData)data_base;
            
            //? Try open connection ?
        }

        public Connection(MetaDataSerializer metaData, ApplicationSettings appSettings)
        {
            InitializeComponent();

            m_MetaData = metaData;
            m_AppSettings = appSettings;

            SearchForPorts();

            serialPort1.DataReceived += SerialPort_DataReceived;
            serialPort1.ErrorReceived += SerialPort_ErrorReceived;

            OutputFileName.Text = metaData.OutputDataFileName;
            SimulationFileName.Text = metaData.SimDataFileName;

            IPTextBox.Text = appSettings.IPText;
            PortTextBox.Text = appSettings.IPPortText;

            IPTextBox.TextChanged += IPTextBox_TextChanged;
            PortTextBox.TextChanged += PortTextBox_TextChanged;
            IPRadioButton.Checked = !appSettings.ComportUsed;

            LogBox.Text += "Connection system ready\r\n";
            LogBox.TextChanged += LogBox_TextChanged;

            sm_Connection = this;
            Disposed += Connection_Disposed;

            ValidatePortAndIP();
        }

        private void LogBox_TextChanged(object sender, EventArgs e)
        {
            LogBox.SelectionStart = LogBox.Text.Length;
            LogBox.ScrollToCaret();
        }

        void ValidatePortAndIP()
        {
            bool bOldValue = m_bValidIPAndPort;
            m_bValidIPAndPort = false;
            string[] ips = IPTextBox.Text.Split('.');
            if (ips.Length == 4)
            {
                if (!string.IsNullOrEmpty(PortTextBox.Text))
                {
                    byte byteTest;
                    short shortTest;
                    m_bValidIPAndPort =   
                            byte.TryParse(ips[0], out byteTest)
                        &&  byte.TryParse(ips[0], out byteTest)
                        &&  byte.TryParse(ips[0], out byteTest)
                        &&  byte.TryParse(ips[0], out byteTest)
                        &&  short.TryParse(PortTextBox.Text, out shortTest);
                }
            }

            if (bOldValue != m_bValidIPAndPort)
            {
                SetOpenCloseButtonState();
            }
        }

        private void PortTextBox_TextChanged(object sender, EventArgs e)
        {
            ValidatePortAndIP();
        }

        private void IPTextBox_TextChanged(object sender, EventArgs e)
        {
            ValidatePortAndIP();
        }

        void Connection_Disposed(object sender, EventArgs e)
        {
            sm_Connection = null;
        }

        private void SetOpenCloseButtonState()
        {
            bool bConnected = serialPort1.IsOpen || ((TCPConnection != null) && (TCPConnection.Connected));
            if (bConnected)
            {
                OpenClose.Text = "Disconnect";
                OpenClose.Enabled = true;
                BytesReadNumber.Enabled = true;
                BytesReadLabel.Enabled = true;
                this.ControlBox = false;    //Disables the close button
                OutputFileGroupBox.Enabled = false;
                SimulationGroup.Enabled = false;
            }
            else
            {
                OpenClose.Text = "Connect";
                OpenClose.Enabled = (OpenPortComboList.Items.Count > 0) || (m_bValidIPAndPort);
                BytesReadNumber.Enabled = false;
                BytesReadLabel.Enabled = false;
                this.ControlBox = true;    //Disables the close button
                OutputFileGroupBox.Enabled = true;
                SimulationGroup.Enabled = true;
            }
        }

        private void SearchForPorts()
        {
            //show list of valid com ports
            OpenPortComboList.Items.Clear();

            bool bHasSelected = false;
            var portCount = System.IO.Ports.SerialPort.GetPortNames().Count();
            for (int i = 0; i < portCount ; i++ )
            {
                OpenPortComboList.Items.Add(System.IO.Ports.SerialPort.GetPortNames()[i]);

                if ((m_AppSettings != null) && (m_AppSettings.LastPortConnected != null) && (System.IO.Ports.SerialPort.GetPortNames()[i] == m_AppSettings.LastPortConnected))
                {
                    bHasSelected = true;
                    OpenPortComboList.SelectedItem = OpenPortComboList.Items[i];
                }
            }

            LogBox.Text += "Found ";
            LogBox.Text += portCount.ToString();
            LogBox.Text += " ports\r\n";

            //foreach (string s in System.IO.Ports.SerialPort.GetPortNames())
            //    {
            //        OpenPortComboList.Items.Add(s);
            //    }

            if (!bHasSelected && (OpenPortComboList.Items.Count > 0))
            {
                OpenPortComboList.SelectedItem = OpenPortComboList.Items[0];
            }

            SetOpenCloseButtonState();
        }

        private void FindPorts_Click(object sender, EventArgs e)
        {
            SearchForPorts();

            if (OpenPortComboList.Items.Count == 0)
            {
                MessageBox.Show("No open ports found");
                LogBox.Text += "No open ports found\r\n";
            }
        }

        private void OpenPortComboList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Console.Write("Selected " + OpenPortComboList.SelectedItem + "\n");
        }

        public class StateObject
        {
            public Socket workSocket = null;
            public const int BUFFER_SIZE = 65536;
            public byte[] buffer = new byte[BUFFER_SIZE];
        }

        private void OpenClose_Click(object sender, EventArgs e)
        {
            if (OpenClose.Text == "Connect")
            {
                if (OutputFileEnabled.Checked)
                {
                    OpenOutputFile();
                }

                //If IPAndPort is set we're going to try a TCPConnection
                //otherwise lets look for local serial ports)
                m_AppSettings.ComportUsed = ComportConnectPanel.Enabled;
                if (ComportConnectPanel.Enabled)
                {
                    if (OpenPortComboList.SelectedItem != null)
                    {
                        serialPort1.PortName = OpenPortComboList.SelectedItem.ToString();

                        if (m_AppSettings.LastPortConnected != serialPort1.PortName)
                        {
                            m_AppSettings.LastPortConnected = serialPort1.PortName;
                            m_AppSettings.Save();
                        }

                        //                serialPort1.ReadTimeout = 5000;

                        serialPort1.Open();
                        m_iBytesRead = 0;
                        m_StartTime = DateTime.UtcNow;

                        //Let others know
                        OnNewConnection();
                        LogBox.Text += "Com port opened\r\n";
                    }
                }
                else
                {
                    m_AppSettings.IPText = IPTextBox.Text;
                    m_AppSettings.IPPortText = PortTextBox.Text;
                    m_AppSettings.Save();
                    string[] ips = IPTextBox.Text.Split('.');
                    if (ips.Length == 4)
                    {
                        byte[] ipsArray =
                        {
                            Convert.ToByte(ips[0]),
                            Convert.ToByte(ips[1]),
                            Convert.ToByte(ips[2]),
                            Convert.ToByte(ips[3])
                        };
                        int port = Convert.ToInt32(PortTextBox.Text);
                        var addr = new System.Net.IPAddress(ipsArray);
                        if (TCPConnection == null)
                            TCPConnection = new TcpClient();
                        try {
                            TCPConnection.Connect(addr, port);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.ToString(), "Error trying to connect");
                        }
                        if (TCPConnection.Connected)
                        {
                            var so = new StateObject();
                            so.workSocket = TCPConnection.Client;
                            TCPConnection.Client.BeginReceive(so.buffer, 0, so.buffer.Length, SocketFlags.None, new AsyncCallback(OnTcpRecieve), so);
                            
                            SetOpenCloseButtonState();
                            LogBox.Text += "TCP port opened\r\n";
                        }
                    }
                    else
                    {
                        MessageBox.Show("Address not of the format xxx.xxx.xxx.xxx:port", "Error trying to connect");
                    }
                }
            }
            else 
            {
                if (serialPort1.IsOpen)
                {
                    serialPort1.Close();
                    m_DataWriter.End();
                    m_DataWriter = null;
                    LogBox.Text += "Closed comm port\r\n";

                }

                if (TCPConnection != null && TCPConnection.Connected)
                {
                    if (TCPConnection.GetStream() != null)
                    {
                        TCPConnection.GetStream().Close();
                    }
                    TCPConnection.Close();
                    TCPConnection = null;
                    LogBox.Text += "Closed TCP connection\r\n";
                }
            }

            SetOpenCloseButtonState();
        }

        private void OnTcpRecieve(IAsyncResult ar)
        {
            var so = (StateObject)ar.AsyncState;
            Socket s = so.workSocket;

            try {
                int read = s.EndReceive(ar);

                if (read > 0)
                {
                    Invoke(new StringConsumer(OnDataReceivedMainThread), s);
                }
                s.BeginReceive(so.buffer, 0, StateObject.BUFFER_SIZE, 0,
                                         new AsyncCallback(OnTcpRecieve), so);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        System.IO.FileStream m_RawDataStream;
        private void OnRawDataReceivedMainThread(byte []data)
        {
            m_iBytesRead += data.Length;
            BytesReadNumber.Value = m_iBytesRead;

            if (m_RawDataStream != null)
            {
                m_RawDataStream.Write(data, 0, data.Length);
            }
        }

        private void OnDataReceivedMainThread(string sdata)
        {
            //Write to the output file if it exists
            if (m_DataWriter != null)
            {
                m_DataWriter.WriteNMEADataSegment(sdata);
            }

            //Send data to any listeners
            if (checkBox_Forward.Checked)
            {
                OnDataRecieved(sdata, (DateTime.UtcNow - m_StartTime).TotalSeconds);
            }
        }

        private void SerialPort_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            System.IO.Ports.SerialPort sp = (System.IO.Ports.SerialPort)sender;

            int bytesToRead = sp.BytesToRead;
            if (bytesToRead > 0)
            {
                byte[] data = new byte[bytesToRead];
                sp.Read(data, 0, bytesToRead);
                Invoke(new DataConsumer(OnRawDataReceivedMainThread), data);

                //convert to string
                //string s = sp.ReadExisting();
                var s = System.Text.Encoding.Default.GetString(data);
                Console.WriteLine(s);
                Invoke(new StringConsumer(OnDataReceivedMainThread), s);
            }
        }

        private void SerialPort_ErrorReceived(object sender, System.IO.Ports.SerialErrorReceivedEventArgs e)
        {
            System.IO.Ports.SerialPort sp = (System.IO.Ports.SerialPort)sender;
            sp.Close();
            Invoke(new VoidConsumer(SetOpenCloseButtonState));

            LogBox.Text += "ERRPR:\r\n";
            LogBox.Text += e.ToString();
            LogBox.Text += "\r\n";
        }

        private void SelectOutputFile()
        {
            OutputFileDialog.Title = "Open output file";
            if (OutputFileName.Text.Length > 0)
            {
                OutputFileDialog.InitialDirectory = System.IO.Path.GetDirectoryName(OutputFileName.Text);
                OutputFileDialog.FileName = OutputFileName.Text;
            }
            OutputFileDialog.OverwritePrompt = true;
            if (OutputFileDialog.ShowDialog() == DialogResult.OK)
            {
                OutputFileName.Text = OutputFileDialog.FileName;
                LogBox.Text += "Opening: ";
                LogBox.Text += OutputFileDialog.FileName;
                LogBox.Text += "\r\n";
                if (m_MetaData != null)
                {
                    m_MetaData.OutputDataFileName = OutputFileDialog.FileName;

                    //We'll want to load thiat back up!
                    m_MetaData.InputDataFileName = OutputFileDialog.FileName;
                    m_MetaData.MarkForAutoSave();
                }
                else
                {
                    LogBox.Text += "Null metadata\r\n";
                }
            }
        }

        private bool OpenOutputFile()
        {
            if (OutputFileName.Text.Length > 0)
            {
                if (System.IO.File.Exists(OutputFileName.Text))
                {
                    //confirm override
                    DialogResult result = MessageBox.Show("Output file exists. Do you want to overwrite?", "Warning", MessageBoxButtons.YesNo);
                    if (result == DialogResult.No)
                    {
                        SelectOutputFile();
                    }
                }
            }
            else
            {
                SelectOutputFile();
            }

            if (OutputFileName.Text.Length > 0)
            {
                OutputFileDialog.InitialDirectory = System.IO.Path.GetDirectoryName(OutputFileName.Text);
                OutputFileDialog.FileName = OutputFileName.Text;

                //Default simulation name to this output file
                SimulationFileName = OutputFileName;
            }

            if (m_DataWriter != null)
            {
                m_DataWriter.Stop();
                m_DataWriter = null;
            }

            if (m_RawDataStream != null)
            {
                m_RawDataStream = null;
            }

            System.IO.Stream s = OutputFileDialog.OpenFile();
            if (s != null)
            {
                m_DataWriter = new DataWriter();
                m_DataWriter.Start(s);
            }


            if (checkBox_WriteRaw.Checked)
            {
                var rawStream = OutputFileDialog.FileName + ".raw";
                m_RawDataStream = System.IO.File.OpenWrite(rawStream);
                m_RawDataStream.WriteByte(123);
            }
            return true;
        }

        private void BrowseForOutput_Click(object sender, EventArgs e)
        {
            SelectOutputFile();
        }

        private void OutputFileEnabled_CheckedChanged(object sender, EventArgs e)
        {
            if (OutputFileEnabled.Checked)
            {
                BrowseForOutput.Enabled = true;
                OutputFileName.Enabled = true;
            }
            else
            {
                BrowseForOutput.Enabled = false;
                OutputFileName.Enabled = false;
            }
        }

        private void OpenCloseSimulation_Click(object sender, EventArgs e)
        {
            if (OpenCloseSimulation.Text == "Open")
            {
                OpenRecordingDialog.Title = "Open simulation file";
                if (SimulationFileName.Text.Length > 0)
                {
                    OpenRecordingDialog.InitialDirectory = System.IO.Path.GetDirectoryName(SimulationFileName.Text);
                    OpenRecordingDialog.FileName = System.IO.Path.GetFileName(SimulationFileName.Text);
                }
                if (OpenRecordingDialog.ShowDialog() == DialogResult.OK)
                {
                    if (m_SimulationDataReader != null)
                    {
                        m_SimulationDataReader = null;
                        m_SimulationStream.Close();
                        m_SimulationStream = null;
                        m_SimulationTimer = null;
                    }

                    SimulationFileName.Text = OpenRecordingDialog.FileName;
                    if (m_MetaData != null)
                    {
                        m_MetaData.SimDataFileName = OpenRecordingDialog.FileName;
                    }
                    m_SimulationStream = OpenRecordingDialog.OpenFile();
                    if (m_SimulationStream != null)
                    {
                        m_SimulationDataReader = new DataReader();
              
                        if (m_SimulationDataReader.StartRead(m_SimulationStream))
                        {
                            SimulationFileName.Text = System.IO.Path.GetFileName( OpenRecordingDialog.FileName );
                            OpenCloseSimulation.Text = "Close";

                            m_SimulationTimer = new Timer();
                            m_SimulationTimer.Enabled = true;
                            m_SimulationTimer.Interval = 1; //Kick off a tick right away
                            m_SimulationTimer.Tick += m_SimulationTimer_Tick;

                            PortGroupBox.Enabled = false;
                            OutputFileGroupBox.Enabled = false;

                            //Let others know
                            OnNewConnection();

                            LogBox.Text += "Simulation started\r\n";
                        }
                        else 
                        {
                            SimulationFileName.Text = "Failed to read!";
                            m_SimulationStream.Close();
                            m_SimulationStream = null;
                            LogBox.Text += "Simulation failed to read\r\n";
                        }
                    }
                }
            }
            else
            {
                if (m_SimulationDataReader != null)
                {
                    m_SimulationDataReader = null;
                    m_SimulationStream.Close();
                    m_SimulationStream = null;
                    m_SimulationTimer = null;
                    LogBox.Text += "Simulation closed\r\n";
                }

                OpenCloseSimulation.Text = "Open";
                PortGroupBox.Enabled = true;
                OutputFileGroupBox.Enabled = true;
            }
        }

        void m_SimulationTimer_Tick(object sender, EventArgs e)
        {
             if (m_SimulationDataReader != null)
             {
                 double fElasped = m_SimulationDataReader.m_fElapsedTime;
                 if (m_SimulationDataReader.ReadSection())
                 {
                     double fWait = m_SimulationDataReader.m_fElapsedTime - fElasped;
                     double fSpd = Convert.ToDouble(PlaybackSpeed.Value);
                     m_SimulationTimer.Interval = (int)(double)(fWait * 1000.0 / fSpd);

                     numericUpDown_DataRead.Value = m_SimulationDataReader.m_iDataRead;

                     OnDataRecieved(m_SimulationDataReader.GetData(), m_SimulationDataReader.m_fElapsedTime);
                 }
             }
        }

        private void IPRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (IPRadioButton.Checked)
            {
                IPConnectPanel.Enabled = true;
                ComportConnectPanel.Enabled = false;
            }
            else
            {
                ComportConnectPanel.Enabled = true;
                IPConnectPanel.Enabled = false;
            }
        }

        private void ComportRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (ComportRadioButton.Checked)
            {
                ComportConnectPanel.Enabled = true;
                IPConnectPanel.Enabled = false;
            }
            else
            {
                IPConnectPanel.Enabled = true;
                ComportConnectPanel.Enabled = false;
            }
        }
    }
}
