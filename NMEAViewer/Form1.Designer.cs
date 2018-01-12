namespace NMEAViewer
{
    partial class PAMainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            WeifenLuo.WinFormsUI.Docking.DockPanelSkin dockPanelSkin1 = new WeifenLuo.WinFormsUI.Docking.DockPanelSkin();
            WeifenLuo.WinFormsUI.Docking.AutoHideStripSkin autoHideStripSkin1 = new WeifenLuo.WinFormsUI.Docking.AutoHideStripSkin();
            WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient1 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient1 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.DockPaneStripSkin dockPaneStripSkin1 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripSkin();
            WeifenLuo.WinFormsUI.Docking.DockPaneStripGradient dockPaneStripGradient1 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient2 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient2 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient3 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.DockPaneStripToolWindowGradient dockPaneStripToolWindowGradient1 = new WeifenLuo.WinFormsUI.Docking.DockPaneStripToolWindowGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient4 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient5 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.DockPanelGradient dockPanelGradient3 = new WeifenLuo.WinFormsUI.Docking.DockPanelGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient6 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            WeifenLuo.WinFormsUI.Docking.TabGradient tabGradient7 = new WeifenLuo.WinFormsUI.Docking.TabGradient();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripComboBox1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.newProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadRecordingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reprocessToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadPolarDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripComboBox2 = new System.Windows.Forms.ToolStripDropDownButton();
            this.newVideoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newGraphToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newHistogramToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newTackingWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newMetaDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.connectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.upwindAnalysisToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpProvider1 = new System.Windows.Forms.HelpProvider();
            this.OpenVideoFile = new System.Windows.Forms.OpenFileDialog();
            this.OpenRecording = new System.Windows.Forms.OpenFileDialog();
            this.OpenProjectFile = new System.Windows.Forms.OpenFileDialog();
            this.SaveProjectFile = new System.Windows.Forms.SaveFileDialog();
            this.MainDockPanel = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.OpenPolarFile = new System.Windows.Forms.OpenFileDialog();
            this.comPortTesterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripComboBox1,
            this.toolStripComboBox2});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(473, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "Window";
            // 
            // toolStripComboBox1
            // 
            this.toolStripComboBox1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripComboBox1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newProjectToolStripMenuItem,
            this.loadProjectToolStripMenuItem,
            this.saveProjectToolStripMenuItem,
            this.loadRecordingToolStripMenuItem,
            this.reprocessToolStripMenuItem,
            this.loadPolarDataToolStripMenuItem});
            this.toolStripComboBox1.Name = "toolStripComboBox1";
            this.toolStripComboBox1.Size = new System.Drawing.Size(38, 22);
            this.toolStripComboBox1.Text = "File";
            // 
            // newProjectToolStripMenuItem
            // 
            this.newProjectToolStripMenuItem.Name = "newProjectToolStripMenuItem";
            this.newProjectToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.newProjectToolStripMenuItem.Text = "New Project";
            this.newProjectToolStripMenuItem.Click += new System.EventHandler(this.newProjectToolStripMenuItem_Click);
            // 
            // loadProjectToolStripMenuItem
            // 
            this.loadProjectToolStripMenuItem.Name = "loadProjectToolStripMenuItem";
            this.loadProjectToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.loadProjectToolStripMenuItem.Text = "Load Project";
            this.loadProjectToolStripMenuItem.Click += new System.EventHandler(this.loadProjectToolStripMenuItem_Click);
            // 
            // saveProjectToolStripMenuItem
            // 
            this.saveProjectToolStripMenuItem.Name = "saveProjectToolStripMenuItem";
            this.saveProjectToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.saveProjectToolStripMenuItem.Text = "Save Project";
            this.saveProjectToolStripMenuItem.Click += new System.EventHandler(this.saveProjectToolStripMenuItem_Click);
            // 
            // loadRecordingToolStripMenuItem
            // 
            this.loadRecordingToolStripMenuItem.Name = "loadRecordingToolStripMenuItem";
            this.loadRecordingToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.loadRecordingToolStripMenuItem.Text = "Load recording";
            this.loadRecordingToolStripMenuItem.Click += new System.EventHandler(this.loadRecordingToolStripMenuItem_Click);
            // 
            // reprocessToolStripMenuItem
            // 
            this.reprocessToolStripMenuItem.Name = "reprocessToolStripMenuItem";
            this.reprocessToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.reprocessToolStripMenuItem.Text = "Reprocess";
            this.reprocessToolStripMenuItem.Click += new System.EventHandler(this.reprocessToolStripMenuItem_Click);
            // 
            // loadPolarDataToolStripMenuItem
            // 
            this.loadPolarDataToolStripMenuItem.Name = "loadPolarDataToolStripMenuItem";
            this.loadPolarDataToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.loadPolarDataToolStripMenuItem.Text = "Load Polar Data";
            this.loadPolarDataToolStripMenuItem.Click += new System.EventHandler(this.loadPolarDataToolStripMenuItem_Click);
            // 
            // toolStripComboBox2
            // 
            this.toolStripComboBox2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripComboBox2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newVideoToolStripMenuItem,
            this.newGraphToolStripMenuItem,
            this.newMapToolStripMenuItem,
            this.newHistogramToolStripMenuItem,
            this.newTackingWindowToolStripMenuItem,
            this.newMetaDataToolStripMenuItem,
            this.connectionToolStripMenuItem,
            this.upwindAnalysisToolStripMenuItem,
            this.comPortTesterToolStripMenuItem});
            this.toolStripComboBox2.Name = "toolStripComboBox2";
            this.toolStripComboBox2.Size = new System.Drawing.Size(64, 22);
            this.toolStripComboBox2.Text = "Window";
            // 
            // newVideoToolStripMenuItem
            // 
            this.newVideoToolStripMenuItem.Name = "newVideoToolStripMenuItem";
            this.newVideoToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.newVideoToolStripMenuItem.Text = "New Video";
            this.newVideoToolStripMenuItem.Click += new System.EventHandler(this.newVideoToolStripMenuItem_Click);
            // 
            // newGraphToolStripMenuItem
            // 
            this.newGraphToolStripMenuItem.Name = "newGraphToolStripMenuItem";
            this.newGraphToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.newGraphToolStripMenuItem.Text = "New Graph";
            this.newGraphToolStripMenuItem.Click += new System.EventHandler(this.newGraphToolStripMenuItem_Click);
            // 
            // newMapToolStripMenuItem
            // 
            this.newMapToolStripMenuItem.Name = "newMapToolStripMenuItem";
            this.newMapToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.newMapToolStripMenuItem.Text = "New Map";
            this.newMapToolStripMenuItem.Click += new System.EventHandler(this.newMapToolStripMenuItem_Click);
            // 
            // newHistogramToolStripMenuItem
            // 
            this.newHistogramToolStripMenuItem.Name = "newHistogramToolStripMenuItem";
            this.newHistogramToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.newHistogramToolStripMenuItem.Text = "New Histogram";
            this.newHistogramToolStripMenuItem.Click += new System.EventHandler(this.newHistogramToolStripMenuItem_Click);
            // 
            // newTackingWindowToolStripMenuItem
            // 
            this.newTackingWindowToolStripMenuItem.Name = "newTackingWindowToolStripMenuItem";
            this.newTackingWindowToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.newTackingWindowToolStripMenuItem.Text = "New Tacking Window";
            this.newTackingWindowToolStripMenuItem.Click += new System.EventHandler(this.newTackingWindowToolStripMenuItem_Click);
            // 
            // newMetaDataToolStripMenuItem
            // 
            this.newMetaDataToolStripMenuItem.Name = "newMetaDataToolStripMenuItem";
            this.newMetaDataToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.newMetaDataToolStripMenuItem.Text = "New Meta Data";
            this.newMetaDataToolStripMenuItem.Click += new System.EventHandler(this.newMetaDataToolStripMenuItem_Click);
            // 
            // connectionToolStripMenuItem
            // 
            this.connectionToolStripMenuItem.Name = "connectionToolStripMenuItem";
            this.connectionToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.connectionToolStripMenuItem.Text = "Connection";
            this.connectionToolStripMenuItem.Click += new System.EventHandler(this.connectionToolStripMenuItem_Click);
            // 
            // upwindAnalysisToolStripMenuItem
            // 
            this.upwindAnalysisToolStripMenuItem.Name = "upwindAnalysisToolStripMenuItem";
            this.upwindAnalysisToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.upwindAnalysisToolStripMenuItem.Text = "Upwind Analysis";
            this.upwindAnalysisToolStripMenuItem.Click += new System.EventHandler(this.upwindAnalysisToolStripMenuItem_Click);
            // 
            // OpenVideoFile
            // 
            this.OpenVideoFile.FileName = "dockPanelGradient7.mov";
            this.OpenVideoFile.Filter = "mov files (*.mov)|*.mov|avi files (*.avi)|*.avi|mp4 files (*.mp4)|*.mp4|All files" +
    " (*.*)|*.*";
            this.OpenVideoFile.FilterIndex = 4;
            this.OpenVideoFile.ReadOnlyChecked = true;
            this.OpenVideoFile.Title = "Add Video to Project";
            // 
            // OpenRecording
            // 
            this.OpenRecording.FileName = "MyRecording.dat";
            this.OpenRecording.Filter = "input files (*.dat, *.prc)|*.dat;*.prc";
            this.OpenRecording.ReadOnlyChecked = true;
            // 
            // OpenProjectFile
            // 
            this.OpenProjectFile.Filter = "xml files (*.xml)|*.xml";
            // 
            // SaveProjectFile
            // 
            this.SaveProjectFile.Filter = "xml files (*.xml)|*.xml";
            // 
            // MainDockPanel
            // 
            this.MainDockPanel.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.MainDockPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.MainDockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainDockPanel.Location = new System.Drawing.Point(0, 25);
            this.MainDockPanel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MainDockPanel.Name = "MainDockPanel";
            this.MainDockPanel.Size = new System.Drawing.Size(473, 300);
            dockPanelGradient1.EndColor = System.Drawing.SystemColors.ControlLight;
            dockPanelGradient1.StartColor = System.Drawing.SystemColors.ControlLight;
            autoHideStripSkin1.DockStripGradient = dockPanelGradient1;
            tabGradient1.EndColor = System.Drawing.SystemColors.Control;
            tabGradient1.StartColor = System.Drawing.SystemColors.Control;
            tabGradient1.TextColor = System.Drawing.SystemColors.ControlDarkDark;
            autoHideStripSkin1.TabGradient = tabGradient1;
            autoHideStripSkin1.TextFont = new System.Drawing.Font("Segoe UI", 9F);
            dockPanelSkin1.AutoHideStripSkin = autoHideStripSkin1;
            tabGradient2.EndColor = System.Drawing.SystemColors.ControlLightLight;
            tabGradient2.StartColor = System.Drawing.SystemColors.ControlLightLight;
            tabGradient2.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripGradient1.ActiveTabGradient = tabGradient2;
            dockPanelGradient2.EndColor = System.Drawing.SystemColors.Control;
            dockPanelGradient2.StartColor = System.Drawing.SystemColors.Control;
            dockPaneStripGradient1.DockStripGradient = dockPanelGradient2;
            tabGradient3.EndColor = System.Drawing.SystemColors.ControlLight;
            tabGradient3.StartColor = System.Drawing.SystemColors.ControlLight;
            tabGradient3.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripGradient1.InactiveTabGradient = tabGradient3;
            dockPaneStripSkin1.DocumentGradient = dockPaneStripGradient1;
            dockPaneStripSkin1.TextFont = new System.Drawing.Font("Segoe UI", 9F);
            tabGradient4.EndColor = System.Drawing.SystemColors.ActiveCaption;
            tabGradient4.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            tabGradient4.StartColor = System.Drawing.SystemColors.GradientActiveCaption;
            tabGradient4.TextColor = System.Drawing.SystemColors.ActiveCaptionText;
            dockPaneStripToolWindowGradient1.ActiveCaptionGradient = tabGradient4;
            tabGradient5.EndColor = System.Drawing.SystemColors.Control;
            tabGradient5.StartColor = System.Drawing.SystemColors.Control;
            tabGradient5.TextColor = System.Drawing.SystemColors.ControlText;
            dockPaneStripToolWindowGradient1.ActiveTabGradient = tabGradient5;
            dockPanelGradient3.EndColor = System.Drawing.SystemColors.ControlLight;
            dockPanelGradient3.StartColor = System.Drawing.SystemColors.ControlLight;
            dockPaneStripToolWindowGradient1.DockStripGradient = dockPanelGradient3;
            tabGradient6.EndColor = System.Drawing.SystemColors.InactiveCaption;
            tabGradient6.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Vertical;
            tabGradient6.StartColor = System.Drawing.SystemColors.GradientInactiveCaption;
            tabGradient6.TextColor = System.Drawing.SystemColors.InactiveCaptionText;
            dockPaneStripToolWindowGradient1.InactiveCaptionGradient = tabGradient6;
            tabGradient7.EndColor = System.Drawing.Color.Transparent;
            tabGradient7.StartColor = System.Drawing.Color.Transparent;
            tabGradient7.TextColor = System.Drawing.SystemColors.ControlDarkDark;
            dockPaneStripToolWindowGradient1.InactiveTabGradient = tabGradient7;
            dockPaneStripSkin1.ToolWindowGradient = dockPaneStripToolWindowGradient1;
            dockPanelSkin1.DockPaneStripSkin = dockPaneStripSkin1;
            this.MainDockPanel.TabIndex = 5;
            // 
            // OpenPolarFile
            // 
            this.OpenPolarFile.FileName = "myPolarFile";
            this.OpenPolarFile.Filter = "pol files (*.pol)|*.pol";
            // 
            // comPortTesterToolStripMenuItem
            // 
            this.comPortTesterToolStripMenuItem.Name = "comPortTesterToolStripMenuItem";
            this.comPortTesterToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.comPortTesterToolStripMenuItem.Text = "ComPortTester";
            this.comPortTesterToolStripMenuItem.Click += new System.EventHandler(this.comPortTesterToolStripMenuItem_Click);
            // 
            // PAMainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(473, 325);
            this.Controls.Add(this.MainDockPanel);
            this.Controls.Add(this.toolStrip1);
            this.IsMdiContainer = true;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "PAMainWindow";
            this.Text = "NMEA Performance Analyzer";
            this.Load += new System.EventHandler(this.PAMainWindow_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripComboBox1;
        private System.Windows.Forms.ToolStripMenuItem loadRecordingToolStripMenuItem;
        private System.Windows.Forms.HelpProvider helpProvider1;
        private System.Windows.Forms.OpenFileDialog OpenVideoFile;
        private System.Windows.Forms.ToolStripMenuItem newProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveProjectToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog OpenRecording;
        private System.Windows.Forms.ToolStripMenuItem loadProjectToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog OpenProjectFile;
        private System.Windows.Forms.SaveFileDialog SaveProjectFile;
        private WeifenLuo.WinFormsUI.Docking.DockPanel MainDockPanel;
        private System.Windows.Forms.ToolStripDropDownButton toolStripComboBox2;
        private System.Windows.Forms.ToolStripMenuItem newVideoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newGraphToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newMapToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newHistogramToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reprocessToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newTackingWindowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newMetaDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadPolarDataToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog OpenPolarFile;
        private System.Windows.Forms.ToolStripMenuItem connectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem upwindAnalysisToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem comPortTesterToolStripMenuItem;
    }
}

