using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using Newtonsoft.Json;

namespace NMEAViewer
{
    public partial class PAMainWindow : Form
    {
        private NMEACruncher m_Data;
        private MetaDataSerializer m_MetaData;
        private DeserializeDockContent m_deserializeDockContent;
        private NMEAStreamReader m_Reader;
        private bool m_bSavedOnExit;
        private ApplicationSettings m_AppSettings;
        private PolarData m_PolarData;
        Timer m_UpdateTick;

        double m_fTimeToAutoSaveSettings = 0.0;

        public PAMainWindow()
        {
            MetaDataWindow.StaticInit();

            InitializeComponent();

            InitProjectData();

            m_deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);
            m_Reader = new NMEAStreamReader();

            ClientSizeChanged += PAMainWindow_ClientSizeChanged;
            LocationChanged += PAMainWindow_LocationChanged;

            m_PolarData = new PolarData();
            m_AppSettings = ApplicationSettings.Load();
            if (m_AppSettings == null)
            {
                m_AppSettings = new ApplicationSettings();
                m_AppSettings.ProjectName = "lastlayout.xml";
            }
            else
            {
                if ((m_AppSettings.PolarDataName != null) && (m_AppSettings.PolarDataName.Length > 0))
                {
                    m_PolarData.Load(m_AppSettings.PolarDataName);
                }

                if ((m_AppSettings.MainWindowState != null) && (m_AppSettings.MainWindowState.Length > 0))
                {
                    WindowState = (FormWindowState)Enum.Parse(
                            typeof(FormWindowState),
                            m_AppSettings.MainWindowState
                            );
                }

                if (m_AppSettings.MainWindowLocation != null && m_AppSettings.MainWindowSize != null)
                {
                    this.DesktopBounds = new Rectangle(m_AppSettings.MainWindowLocation, m_AppSettings.MainWindowSize);
                }
            }

            //Start the update tick late - if the user gets dialogs during startup we don't want to drive the update until after they are cleared
            m_UpdateTick = new Timer();
            m_UpdateTick.Enabled = true;
            m_UpdateTick.Interval = 1000;   //Think about stuff every second
            m_UpdateTick.Tick += m_UpdateTick_Tick;
        }

        void PAMainWindow_LocationChanged(object sender, EventArgs e)
        {
            if (m_AppSettings.MainWindowLocation != Location)
            {
                m_AppSettings.MainWindowState = Enum.GetName(typeof(FormWindowState), this.WindowState);
                m_AppSettings.MainWindowLocation = Location;
                m_fTimeToAutoSaveSettings = MetaDataSerializer.kTimeToAutoSave;
            }
        }

        void PAMainWindow_ClientSizeChanged(object sender, EventArgs e)
        {
            if (m_AppSettings.MainWindowSize != ClientSize)
            {
                m_AppSettings.MainWindowState = Enum.GetName(typeof(FormWindowState), this.WindowState);
                m_AppSettings.MainWindowSize = ClientSize;
                m_fTimeToAutoSaveSettings = MetaDataSerializer.kTimeToAutoSave;
            }
        }

        void m_UpdateTick_Tick(object sender, EventArgs e)
        {
            if (m_MetaData.m_fTimeToAutoSave > 0.0)
            {
                m_MetaData.m_fTimeToAutoSave -= ((double)m_UpdateTick.Interval) * 0.001;
                if (m_MetaData.m_fTimeToAutoSave <= 0.0)
                {
                    if (m_AppSettings.ProjectName != null)
                    {
                        SaveProject(m_AppSettings.ProjectName);
                    }
                    else 
                    {
                        SaveProject("lastlayout.xml");
                    }
                }
            }

            if (m_fTimeToAutoSaveSettings > 0.0)
            {
                m_fTimeToAutoSaveSettings -= ((double)m_UpdateTick.Interval) * 0.001;
                if (m_fTimeToAutoSaveSettings <= 0.0)
                {
                    m_AppSettings.Save();
                }
            }
        }

        ~PAMainWindow()
        {
            MetaDataWindow.StaticDeInit();
        }

        private void InitProjectData()
        {
            m_Data = new NMEACruncher(1.0);
            //m_MetaData = new MetaDataSerializer();
            m_Data.SetPolarData(m_PolarData);
            m_Data.PostProcess();

            
        }

        //TODO: Make a factory!
        private DockableDrawable CreateWindowOfType(string typeName)
        {
            //if (typeName == typeof(NMEAViewer.VLCVideoWindow).ToString())
            //{
            //    return new VLCVideoWindow();
            //}
            if (typeName == typeof(NMEAViewer.VideoWindow).ToString())
            {
                return new VideoWindow();
            }
            else if (typeName == typeof(NMEAViewer.TimeBasedGraph).ToString())
            {
                return new TimeBasedGraph(m_Data, m_MetaData);
            }
            else if (typeName == typeof(NMEAViewer.Histogram).ToString())
            {
                return new Histogram(m_Data);
            }
            else if (typeName == typeof(NMEAViewer.MapWindow).ToString())
            {
                return new MapWindow(m_Data);
            }
            else if (typeName == typeof(NMEAViewer.MetaDataWindow).ToString())
            {
                return new MetaDataWindow();
            }
            else if (typeName == typeof(NMEAViewer.TackingWindow).ToString())
            {
                return new TackingWindow(m_Data);
            }
            else if (typeName == typeof(NMEAViewer.UpwindAnalysis).ToString())
            {
                return new UpwindAnalysis(m_Data, m_PolarData);
            }
            return null;
        }

        private IDockContent GetContentFromPersistString(string persistString)
        {
            string[] parsedStrings = persistString.Split(new char[] { ',' });
            if (parsedStrings.Length == 2)
            {
                DockableDrawable newDockable = CreateWindowOfType(parsedStrings[0]);
                if (newDockable!=null)
                {
                    //Rather than putting a load of information in the persist string
                    //We ID each window and have a parallel file of per window information
                    newDockable.SetID( Convert.ToInt32(parsedStrings[1]) );
                    newDockable.SetMDICloseCallback(new DockableDrawable.VoidConsumer(OnMDIWindowClose));
                }
                return newDockable;
            }

            return CreateWindowOfType(persistString);
        }



        protected void SaveProject(string projectXmlName)
        {
            MainDockPanel.SaveAsXml(projectXmlName);

            //Save the parallel json file
            m_MetaData.ParseProjectMetaData();

            //TODO: Save main window position and size!

            //string output = JsonConvert.SerializeObject(m_MetaData.m_ArrayOfWindowData, Formatting.Indented, new JsonSerializerSettings
            string output = JsonConvert.SerializeObject(m_MetaData, Formatting.Indented, new JsonSerializerSettings
                                {
                                    TypeNameHandling = TypeNameHandling.All
                                }
                            );
            string jsonFile = projectXmlName + ".json";
            System.IO.StreamWriter sOut = System.IO.File.CreateText(jsonFile);
            if (sOut != null)
            {
                sOut.Write(output);
            }
            sOut.Close();
        }

        private void CloseAllDocuments()
        {
            if (MainDockPanel.DocumentStyle == DocumentStyle.SystemMdi)
            {
                foreach (Form form in MdiChildren)
                    form.Close();
            }
            else
            {
                //The sample for DockPanelSuite closes Documents, which doesn't seem to include the panes
                //loaded by XML.
                while (MainDockPanel.Contents.Count > 0)
                {
                    MainDockPanel.Contents[0].DockHandler.Close();
                }
            }
        }

        public void CloseProject()
        {
            //Fairly obvious
            CloseAllDocuments();

            //Clear events etc
            MetaDataWindow.Clear();

            //Reinit the project data
            InitProjectData();
        }

        void SetProjectName(string name)
        {
            m_AppSettings.ProjectName = name;
            string nameNoExt = System.IO.Path.GetFileNameWithoutExtension(name);
            this.Text = nameNoExt;
            
            m_AppSettings.Save();
        }

        protected bool LoadProject(string projectXmlName)
        {
            if (System.IO.File.Exists(projectXmlName))
            {
                MainDockPanel.SuspendLayout();

                //Destroy existing content
                CloseProject();

                //Update name and settings
                SetProjectName(projectXmlName);

                //And load in the settings for this project
                string jsonFile = projectXmlName + ".json";
                if (System.IO.File.Exists(jsonFile))
                {
                    string jsonData = System.IO.File.ReadAllText(jsonFile);
                    if (jsonData != null)
                    {
                        m_MetaData = JsonConvert.DeserializeObject<MetaDataSerializer>(jsonData, new JsonSerializerSettings
                            {
                                TypeNameHandling = TypeNameHandling.All
                            }
                        );

                        ReadInputData(false);

                        if (m_MetaData != null)
                        {
                            //Make sure our graph style info is complete and up to date
                            m_MetaData.m_GraphStyleInfo = TimeBasedGraphDataTypes.ValidateStyleInfo(m_MetaData.m_GraphStyleInfo);

                            //Possible TODO: Factory this?
                            //Load the static settings
                            if ((m_MetaData.m_DictonaryOfStaticData != null) && m_MetaData.m_DictonaryOfStaticData.ContainsKey(typeof(MetaDataWindow.StaticData).ToString()))
                            {
                                MetaDataWindow.StaticData.sm_Data = (MetaDataWindow.StaticData)m_MetaData.m_DictonaryOfStaticData[typeof(MetaDataWindow.StaticData).ToString()];
                            }
                        }
                    }
                }

                //Setup new windows
                MainDockPanel.LoadFromXml(projectXmlName, new DeserializeDockContent(GetContentFromPersistString));

                if (m_MetaData != null)
                {
                    if (m_MetaData.m_ArrayOfWindowData != null)
                    {
                        //And load in the settings for each pane from the json file
                        for (int i = 0; i < m_MetaData.m_ArrayOfWindowData.Count; i++)
                        {
                            if (m_MetaData.m_ArrayOfWindowData[i] != null)
                            {
                                DockableDrawable windowReferedTo = DockableDrawable.GetFromID(m_MetaData.m_ArrayOfWindowData[i].m_ID);
                                if (windowReferedTo != null)
                                {
                                    windowReferedTo.InitFromSerializedData(m_MetaData.m_ArrayOfWindowData[i]);
                                }
                            }
                        }

                        //And allow a second phase of initialization after all the data has been processed (cross window links)
                        for (int i = 0; i < m_MetaData.m_ArrayOfWindowData.Count; i++)
                        {
                            if (m_MetaData.m_ArrayOfWindowData[i] != null)
                            {
                                DockableDrawable windowReferedTo = DockableDrawable.GetFromID(m_MetaData.m_ArrayOfWindowData[i].m_ID);
                                if (windowReferedTo != null)
                                {
                                    windowReferedTo.PostInitFromSerializedData(m_MetaData.m_ArrayOfWindowData[i]);
                                }
                            }
                        }

                        //Same post init but for static data
                        MetaDataWindow.PostInitFromStaticData();
                    }

                }

                MainDockPanel.ResumeLayout(true, true);
                return m_MetaData != null;
            }
            return false;
        }

        private void OnMDIWindowClose()
        {
            //Save out setup for next run
            if (!m_bSavedOnExit)
            {
                m_bSavedOnExit = true;
                SaveProject(m_AppSettings.ProjectName);

                if (m_fTimeToAutoSaveSettings >= 0.0)
                {
                    //Had a pending save!
                    m_AppSettings.Save();
                }
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HookupUserAddedPanel(new DockableDrawable());
        }

        private void HookupUserAddedPanel(DockableDrawable newPanel)
        {
            if (MainDockPanel.DocumentStyle == DocumentStyle.SystemMdi)
            {
                newPanel.MdiParent = this;
                newPanel.Show();
            }
            else
            {
                newPanel.Show(MainDockPanel);
            }

            //Make sure we can ID this window
            newPanel.AssignNewID();
            newPanel.SetMDICloseCallback(new DockableDrawable.VoidConsumer(OnMDIWindowClose));

            //Trigger save
            m_MetaData.MarkForAutoSave();
        }

        private void newVideoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenVideoFile.InitialDirectory = m_MetaData.VideoFilePath;
            if (OpenVideoFile.ShowDialog() == DialogResult.OK)
            {
                System.IO.Stream s = OpenVideoFile.OpenFile();
                if (s != null)
                {
                    ///Restore to same place next time
                    OpenVideoFile.InitialDirectory = System.IO.Path.GetDirectoryName(OpenVideoFile.FileName);
                    m_MetaData.VideoFilePath = System.IO.Path.GetDirectoryName(OpenVideoFile.FileName);
                    //HookupPanel(new VideoWindow(OpenVideoFile.SafeFileName, OpenVideoFile.FileName));
                    HookupUserAddedPanel(new VideoWindow(OpenVideoFile.SafeFileName, OpenVideoFile.FileName));
                }
            }
        }

        private void newGraphToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HookupUserAddedPanel(new TimeBasedGraph(m_Data, m_MetaData));
        }

        private void ReadInputData(bool bForceReprocessing)
        {
            //If we already processed this file read it.
            if ((m_MetaData != null) && (m_MetaData.InputDataFileName != null))
            {
                if (!bForceReprocessing && m_Data.HasMatchingProcessedFile(m_MetaData.InputDataFileName))
                {
                    m_Data.ReadProcessedData(m_MetaData.InputDataFileName + ".prc");
                }
                else
                {
                    m_Data.ProcessFile(m_MetaData.InputDataFileName, m_Reader);
                }
                
                //Get windows to refresh their data
                DockableDrawable.BroadcastDataReplaced(m_Data);

                //Trigger save
                m_MetaData.MarkForAutoSave();
            }
        }

        private void OnDataRecieved(string newData, double fElapsedTime)
        {
            m_Data.ProcessNewData(newData, m_Reader, fElapsedTime);

            DockableDrawable.BroadcastDataAppended();
        }

        private void loadRecordingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenRecording.FileName = m_MetaData.InputDataFileName;
            OpenRecording.InitialDirectory = System.IO.Path.GetDirectoryName(m_MetaData.InputDataFileName);
            if (OpenRecording.ShowDialog() == DialogResult.OK)
            {
                m_MetaData.InputDataFileName = OpenRecording.FileName;

                if (System.IO.Path.GetExtension(m_MetaData.InputDataFileName) == "prc")
                {
                    m_Data.ReadProcessedData(m_MetaData.InputDataFileName + ".prc");

                    //Get windows to refresh their data
                    DockableDrawable.BroadcastDataReplaced(m_Data);

                    //Trigger save
                    m_MetaData.MarkForAutoSave();
                }
                else
                {
                    ReadInputData(false);
                }
            }
        }

        private void reprocessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //OpenRecording.FileName = m_MetaData.InputDataFileName;
            //OpenRecording.InitialDirectory = System.IO.Path.GetDirectoryName(m_MetaData.InputDataFileName);
            //if (OpenRecording.ShowDialog() == DialogResult.OK)
            {
//                m_MetaData.InputDataFileName = OpenRecording.FileName;

                ReadInputData(true);
            }
        }

        private void newMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HookupUserAddedPanel(new MapWindow(m_Data));
        }

        private void saveProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SaveProjectFile.ShowDialog() == DialogResult.OK)
            {
                SaveProject(SaveProjectFile.FileName);

                SetProjectName(SaveProjectFile.FileName);
            }
        }

        private void loadProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((m_AppSettings.ProjectName != null) && (m_AppSettings.ProjectName.Length > 0))
            {
                OpenProjectFile.InitialDirectory = System.IO.Path.GetDirectoryName(m_AppSettings.ProjectName);
            }

            if (OpenProjectFile.ShowDialog() == DialogResult.OK)
            {
                //Save the current project
                if (m_AppSettings.ProjectName != null)
                {
                    SaveProject(m_AppSettings.ProjectName);
                }

                LoadProject(OpenProjectFile.FileName);
            }
        }

        private void PAMainWindow_Load(object sender, EventArgs e)
        {
            if (m_AppSettings.ProjectName != null)
            {
                LoadProject(m_AppSettings.ProjectName);
            }
            else
            {
                LoadProject("lastlayout.xml");
            }

            if (m_MetaData == null)
            {
                m_MetaData = new MetaDataSerializer();
            }
        }

        void PortConnection_OnNewConnection()
        {
            //Blitz everything we have.
            //TODO: Consider prompting for save, we might be overwriting good data
            InitProjectData();

            //Get windows to refresh their data
            DockableDrawable.BroadcastDataReplaced(m_Data);
        }

        private void newHistogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HookupUserAddedPanel(new Histogram(m_Data));
        }

        private void newProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Dump current data, close current windows
            CloseProject();

            //Get windows to refresh their data
            DockableDrawable.BroadcastDataReplaced(m_Data);

            //Load our default layout
            LoadProject("DefaultNewProjectLayout.xml");
            
            //Prompt user to save out the project file
            if (SaveProjectFile.ShowDialog() == DialogResult.OK)
            {
                SetProjectName(SaveProjectFile.FileName);

                SaveProject(SaveProjectFile.FileName);

                //And auto set the connection output name
                m_MetaData.OutputDataFileName = m_AppSettings.ProjectName + ".dat";
                
                //And make sure we auto read it next time (TODO: Do we need input and output names?)
                m_MetaData.InputDataFileName = m_AppSettings.ProjectName + ".dat";

                //Make sure we get saved with our new data
                m_MetaData.MarkForAutoSave();

                //Set the name
                this.Name = System.IO.Path.GetFileNameWithoutExtension(m_AppSettings.ProjectName);
            }
        }

        private void newTackingWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HookupUserAddedPanel( new TackingWindow(m_Data) );
        }

        private void newMetaDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HookupUserAddedPanel(new MetaDataWindow());
        }

        private void loadPolarDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((m_AppSettings.PolarDataName != null) && (m_AppSettings.PolarDataName.Length > 0))
            {
                OpenPolarFile.InitialDirectory = System.IO.Path.GetDirectoryName(m_AppSettings.PolarDataName);
            }
            if (OpenPolarFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                m_AppSettings.PolarDataName = OpenPolarFile.FileName;
                m_AppSettings.Save();
                m_PolarData.Load(m_AppSettings.PolarDataName);
                m_Data.SetPolarData(m_PolarData);
            }
        }

        private void connectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Connection.sm_Connection == null)
            {
                Connection.sm_Connection = new Connection(m_MetaData, m_AppSettings);
                Connection.sm_Connection.OnNewConnection += PortConnection_OnNewConnection;
                Connection.sm_Connection.OnDataRecieved += OnDataRecieved;
                Connection.sm_Connection.Show();
            }
            else
            {
                Connection.sm_Connection.BringToFront();
            }
        }

        private void upwindAnalysisToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HookupUserAddedPanel(new UpwindAnalysis(m_Data, m_PolarData));
        }

        private void comPortTesterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ComOutTester.sm_Tester == null)
            {
                ComOutTester.sm_Tester = new ComOutTester();
                ComOutTester.sm_Tester.Show();
            }
            else
            {
                ComOutTester.sm_Tester.BringToFront();
            }
        }
    }
}
