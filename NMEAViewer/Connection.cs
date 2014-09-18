using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NMEAViewer
{
    public partial class Connection : Form
    {
        int m_iBytesRead = 0;
        //NMEAStreamReader m_Reader;
        DataWriter m_DataWriter;
        MetaDataSerializer m_MetaData;
        DateTime m_StartTime;
        System.IO.Stream m_SimulationStream;
        DataReader m_SimulationDataReader;
        Timer m_SimulationTimer;

        public delegate void VoidConsumer();  // defines a delegate type
        public delegate void StringConsumer(string value);  // defines a delegate type
        public delegate void OnNewConnectionType();
        public delegate void OnNewData(string value, double fElapsedTime);
        public event OnNewConnectionType OnNewConnection;
        public event OnNewData OnDataRecieved;

        public static Connection sm_Connection = null;

        public Connection(MetaDataSerializer metaData)
        {
            InitializeComponent();

            SearchForPorts();

            serialPort1.DataReceived += SerialPort_DataReceived;
            serialPort1.ErrorReceived += SerialPort_ErrorReceived;

            //m_Reader = reader;
            m_MetaData = metaData;

            OutputFileName.Text = metaData.OutputDataFileName;
            SimulationFileName.Text = metaData.SimDataFileName;

            sm_Connection = this;
            Disposed += Connection_Disposed;
        }

        void Connection_Disposed(object sender, EventArgs e)
        {
            sm_Connection = null;
        }

        private void SetOpenCloseButtonState()
        {
            if (serialPort1.IsOpen)
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
                OpenClose.Enabled = OpenPortComboList.Items.Count > 0;
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
            foreach (string s in System.IO.Ports.SerialPort.GetPortNames())
            {
                OpenPortComboList.Items.Add(s);
            }

            if (OpenPortComboList.Items.Count > 0)
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
            }
        }

        private void OpenPortComboList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Console.Write("Selected " + OpenPortComboList.SelectedItem + "\n");
        }

        private void OpenClose_Click(object sender, EventArgs e)
        {
            if (OpenClose.Text == "Connect")
            {
                if (OutputFileEnabled.Checked)
                {
                    OpenOutputFile();
                }

                serialPort1.PortName = OpenPortComboList.SelectedItem.ToString();
                serialPort1.Open();
                m_iBytesRead = 0;
                m_StartTime = DateTime.UtcNow;

                //Let others know
                OnNewConnection();
            }
            else 
            {
                if (serialPort1.IsOpen)
                {
                    serialPort1.Close();
                    m_DataWriter.End();
                    m_DataWriter = null;
                }
            }

            SetOpenCloseButtonState();
        }

        private void OnDataReceivedMainThread(string sdata)
        {
            m_iBytesRead += sdata.Length;
            BytesReadNumber.Value = m_iBytesRead;

            //Write to the output file if it exists
            if (m_DataWriter != null)
            {
                m_DataWriter.WriteNMEADataSegment(sdata);
            }

            //Send data to any listeners
            OnDataRecieved(sdata, (DateTime.UtcNow - m_StartTime).TotalSeconds);
        }

        private void SerialPort_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            System.IO.Ports.SerialPort sp = (System.IO.Ports.SerialPort)sender;
            string s = sp.ReadExisting();
            Console.WriteLine(s);
            Invoke(new StringConsumer(OnDataReceivedMainThread), s);
        }

        private void SerialPort_ErrorReceived(object sender, System.IO.Ports.SerialErrorReceivedEventArgs e)
        {
            System.IO.Ports.SerialPort sp = (System.IO.Ports.SerialPort)sender;
            sp.Close();
            Invoke(new VoidConsumer(SetOpenCloseButtonState));
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
                if (m_MetaData != null)
                {
                    m_MetaData.OutputDataFileName = OutputFileDialog.FileName;

                    //We'll want to load thiat back up!
                    m_MetaData.InputDataFileName = OutputFileDialog.FileName;
                    m_MetaData.MarkForAutoSave();
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
            }

            System.IO.Stream s = OutputFileDialog.OpenFile();
            if (s != null)
            {
                m_DataWriter = new DataWriter();
                m_DataWriter.Start(s);
                return true;
            }
            return false;
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
                        }
                        else 
                        {
                            SimulationFileName.Text = "Failed to read!";
                            m_SimulationStream.Close();
                            m_SimulationStream = null;
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
    }
}
