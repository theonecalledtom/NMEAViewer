namespace NMEAViewer
{
    partial class Connection
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
            this.components = new System.ComponentModel.Container();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.FindPorts = new System.Windows.Forms.Button();
            this.OpenPortComboList = new System.Windows.Forms.ComboBox();
            this.OpenClose = new System.Windows.Forms.Button();
            this.BytesReadLabel = new System.Windows.Forms.Label();
            this.BytesReadNumber = new System.Windows.Forms.NumericUpDown();
            this.OutputFileName = new System.Windows.Forms.TextBox();
            this.BrowseForOutput = new System.Windows.Forms.Button();
            this.OutputFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.OutputFileGroupBox = new System.Windows.Forms.GroupBox();
            this.OutputFileEnabled = new System.Windows.Forms.CheckBox();
            this.SimulationGroup = new System.Windows.Forms.GroupBox();
            this.label2PlaybackSpeed = new System.Windows.Forms.Label();
            this.PlaybackSpeed = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_DataRead = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.SimulationFileName = new System.Windows.Forms.TextBox();
            this.OpenCloseSimulation = new System.Windows.Forms.Button();
            this.PortGroupBox = new System.Windows.Forms.GroupBox();
            this.OpenRecordingDialog = new System.Windows.Forms.OpenFileDialog();
            this.IPAndPort = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.BytesReadNumber)).BeginInit();
            this.OutputFileGroupBox.SuspendLayout();
            this.SimulationGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PlaybackSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_DataRead)).BeginInit();
            this.PortGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // serialPort1
            // 
            this.serialPort1.BaudRate = 4800;
            this.serialPort1.ReadTimeout = 1000;
            this.serialPort1.WriteBufferSize = 1024;
            // 
            // FindPorts
            // 
            this.FindPorts.Location = new System.Drawing.Point(4, 44);
            this.FindPorts.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.FindPorts.Name = "FindPorts";
            this.FindPorts.Size = new System.Drawing.Size(61, 19);
            this.FindPorts.TabIndex = 0;
            this.FindPorts.Text = "Find";
            this.FindPorts.UseVisualStyleBackColor = true;
            this.FindPorts.Click += new System.EventHandler(this.FindPorts_Click);
            // 
            // OpenPortComboList
            // 
            this.OpenPortComboList.FormattingEnabled = true;
            this.OpenPortComboList.Location = new System.Drawing.Point(73, 44);
            this.OpenPortComboList.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.OpenPortComboList.Name = "OpenPortComboList";
            this.OpenPortComboList.Size = new System.Drawing.Size(124, 21);
            this.OpenPortComboList.TabIndex = 1;
            this.OpenPortComboList.SelectedIndexChanged += new System.EventHandler(this.OpenPortComboList_SelectedIndexChanged);
            // 
            // OpenClose
            // 
            this.OpenClose.Enabled = false;
            this.OpenClose.Location = new System.Drawing.Point(4, 68);
            this.OpenClose.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.OpenClose.Name = "OpenClose";
            this.OpenClose.Size = new System.Drawing.Size(61, 19);
            this.OpenClose.TabIndex = 2;
            this.OpenClose.Text = "Open";
            this.OpenClose.UseVisualStyleBackColor = true;
            this.OpenClose.Click += new System.EventHandler(this.OpenClose_Click);
            // 
            // BytesReadLabel
            // 
            this.BytesReadLabel.AutoSize = true;
            this.BytesReadLabel.Enabled = false;
            this.BytesReadLabel.Location = new System.Drawing.Point(70, 70);
            this.BytesReadLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.BytesReadLabel.Name = "BytesReadLabel";
            this.BytesReadLabel.Size = new System.Drawing.Size(57, 13);
            this.BytesReadLabel.TabIndex = 3;
            this.BytesReadLabel.Text = "Bytes read";
            // 
            // BytesReadNumber
            // 
            this.BytesReadNumber.Enabled = false;
            this.BytesReadNumber.InterceptArrowKeys = false;
            this.BytesReadNumber.Location = new System.Drawing.Point(132, 68);
            this.BytesReadNumber.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.BytesReadNumber.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.BytesReadNumber.Name = "BytesReadNumber";
            this.BytesReadNumber.Size = new System.Drawing.Size(64, 20);
            this.BytesReadNumber.TabIndex = 4;
            this.BytesReadNumber.ThousandsSeparator = true;
            // 
            // OutputFileName
            // 
            this.OutputFileName.Location = new System.Drawing.Point(4, 49);
            this.OutputFileName.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.OutputFileName.Name = "OutputFileName";
            this.OutputFileName.Size = new System.Drawing.Size(142, 20);
            this.OutputFileName.TabIndex = 6;
            // 
            // BrowseForOutput
            // 
            this.BrowseForOutput.Location = new System.Drawing.Point(154, 48);
            this.BrowseForOutput.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.BrowseForOutput.Name = "BrowseForOutput";
            this.BrowseForOutput.Size = new System.Drawing.Size(46, 19);
            this.BrowseForOutput.TabIndex = 8;
            this.BrowseForOutput.Text = "Browse";
            this.BrowseForOutput.UseVisualStyleBackColor = true;
            this.BrowseForOutput.Click += new System.EventHandler(this.BrowseForOutput_Click);
            // 
            // OutputFileDialog
            // 
            this.OutputFileDialog.FileName = "outputfile.dat";
            this.OutputFileDialog.Filter = "dat files *.dat|*.dat";
            // 
            // OutputFileGroupBox
            // 
            this.OutputFileGroupBox.Controls.Add(this.BrowseForOutput);
            this.OutputFileGroupBox.Controls.Add(this.OutputFileEnabled);
            this.OutputFileGroupBox.Controls.Add(this.OutputFileName);
            this.OutputFileGroupBox.Location = new System.Drawing.Point(7, 141);
            this.OutputFileGroupBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.OutputFileGroupBox.Name = "OutputFileGroupBox";
            this.OutputFileGroupBox.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.OutputFileGroupBox.Size = new System.Drawing.Size(200, 77);
            this.OutputFileGroupBox.TabIndex = 9;
            this.OutputFileGroupBox.TabStop = false;
            this.OutputFileGroupBox.Text = "Output file";
            // 
            // OutputFileEnabled
            // 
            this.OutputFileEnabled.AutoSize = true;
            this.OutputFileEnabled.Checked = true;
            this.OutputFileEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.OutputFileEnabled.Location = new System.Drawing.Point(4, 27);
            this.OutputFileEnabled.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.OutputFileEnabled.Name = "OutputFileEnabled";
            this.OutputFileEnabled.Size = new System.Drawing.Size(65, 17);
            this.OutputFileEnabled.TabIndex = 7;
            this.OutputFileEnabled.Text = "Enabled";
            this.OutputFileEnabled.UseVisualStyleBackColor = true;
            this.OutputFileEnabled.CheckedChanged += new System.EventHandler(this.OutputFileEnabled_CheckedChanged);
            // 
            // SimulationGroup
            // 
            this.SimulationGroup.Controls.Add(this.label2PlaybackSpeed);
            this.SimulationGroup.Controls.Add(this.PlaybackSpeed);
            this.SimulationGroup.Controls.Add(this.numericUpDown_DataRead);
            this.SimulationGroup.Controls.Add(this.label1);
            this.SimulationGroup.Controls.Add(this.SimulationFileName);
            this.SimulationGroup.Controls.Add(this.OpenCloseSimulation);
            this.SimulationGroup.Location = new System.Drawing.Point(7, 223);
            this.SimulationGroup.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.SimulationGroup.Name = "SimulationGroup";
            this.SimulationGroup.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.SimulationGroup.Size = new System.Drawing.Size(199, 84);
            this.SimulationGroup.TabIndex = 10;
            this.SimulationGroup.TabStop = false;
            this.SimulationGroup.Text = "Simulation";
            // 
            // label2PlaybackSpeed
            // 
            this.label2PlaybackSpeed.AutoSize = true;
            this.label2PlaybackSpeed.Location = new System.Drawing.Point(47, 64);
            this.label2PlaybackSpeed.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2PlaybackSpeed.Name = "label2PlaybackSpeed";
            this.label2PlaybackSpeed.Size = new System.Drawing.Size(83, 13);
            this.label2PlaybackSpeed.TabIndex = 7;
            this.label2PlaybackSpeed.Text = "Playback speed";
            // 
            // PlaybackSpeed
            // 
            this.PlaybackSpeed.Location = new System.Drawing.Point(133, 63);
            this.PlaybackSpeed.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.PlaybackSpeed.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.PlaybackSpeed.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.PlaybackSpeed.Name = "PlaybackSpeed";
            this.PlaybackSpeed.Size = new System.Drawing.Size(62, 20);
            this.PlaybackSpeed.TabIndex = 6;
            this.PlaybackSpeed.Value = new decimal(new int[] {
            32,
            0,
            0,
            0});
            // 
            // numericUpDown_DataRead
            // 
            this.numericUpDown_DataRead.Enabled = false;
            this.numericUpDown_DataRead.InterceptArrowKeys = false;
            this.numericUpDown_DataRead.Location = new System.Drawing.Point(133, 40);
            this.numericUpDown_DataRead.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.numericUpDown_DataRead.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.numericUpDown_DataRead.Name = "numericUpDown_DataRead";
            this.numericUpDown_DataRead.Size = new System.Drawing.Size(62, 20);
            this.numericUpDown_DataRead.TabIndex = 5;
            this.numericUpDown_DataRead.ThousandsSeparator = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Enabled = false;
            this.label1.Location = new System.Drawing.Point(70, 41);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Bytes read";
            // 
            // SimulationFileName
            // 
            this.SimulationFileName.Enabled = false;
            this.SimulationFileName.Location = new System.Drawing.Point(73, 18);
            this.SimulationFileName.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.SimulationFileName.Name = "SimulationFileName";
            this.SimulationFileName.Size = new System.Drawing.Size(122, 20);
            this.SimulationFileName.TabIndex = 1;
            // 
            // OpenCloseSimulation
            // 
            this.OpenCloseSimulation.Location = new System.Drawing.Point(5, 18);
            this.OpenCloseSimulation.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.OpenCloseSimulation.Name = "OpenCloseSimulation";
            this.OpenCloseSimulation.Size = new System.Drawing.Size(56, 19);
            this.OpenCloseSimulation.TabIndex = 0;
            this.OpenCloseSimulation.Text = "Open";
            this.OpenCloseSimulation.UseVisualStyleBackColor = true;
            this.OpenCloseSimulation.Click += new System.EventHandler(this.OpenCloseSimulation_Click);
            // 
            // PortGroupBox
            // 
            this.PortGroupBox.Controls.Add(this.label2);
            this.PortGroupBox.Controls.Add(this.IPAndPort);
            this.PortGroupBox.Controls.Add(this.FindPorts);
            this.PortGroupBox.Controls.Add(this.OpenClose);
            this.PortGroupBox.Controls.Add(this.BytesReadNumber);
            this.PortGroupBox.Controls.Add(this.OpenPortComboList);
            this.PortGroupBox.Controls.Add(this.BytesReadLabel);
            this.PortGroupBox.Location = new System.Drawing.Point(7, 10);
            this.PortGroupBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.PortGroupBox.Name = "PortGroupBox";
            this.PortGroupBox.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.PortGroupBox.Size = new System.Drawing.Size(200, 127);
            this.PortGroupBox.TabIndex = 11;
            this.PortGroupBox.TabStop = false;
            this.PortGroupBox.Text = "Port";
            // 
            // OpenRecordingDialog
            // 
            this.OpenRecordingDialog.FileName = "SomeRecording.dat";
            this.OpenRecordingDialog.Filter = "dat files *.dat|*.dat";
            // 
            // IPAndPort
            // 
            this.IPAndPort.Location = new System.Drawing.Point(73, 18);
            this.IPAndPort.Name = "IPAndPort";
            this.IPAndPort.Size = new System.Drawing.Size(122, 20);
            this.IPAndPort.TabIndex = 5;
            this.IPAndPort.Text = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "a.b.c.d:Port";
            // 
            // Connection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(285, 323);
            this.Controls.Add(this.PortGroupBox);
            this.Controls.Add(this.SimulationGroup);
            this.Controls.Add(this.OutputFileGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Connection";
            this.Text = "Connection";
            ((System.ComponentModel.ISupportInitialize)(this.BytesReadNumber)).EndInit();
            this.OutputFileGroupBox.ResumeLayout(false);
            this.OutputFileGroupBox.PerformLayout();
            this.SimulationGroup.ResumeLayout(false);
            this.SimulationGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PlaybackSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_DataRead)).EndInit();
            this.PortGroupBox.ResumeLayout(false);
            this.PortGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button FindPorts;
        private System.Windows.Forms.ComboBox OpenPortComboList;
        private System.Windows.Forms.Button OpenClose;
        private System.Windows.Forms.Label BytesReadLabel;
        private System.Windows.Forms.NumericUpDown BytesReadNumber;
        private System.Windows.Forms.TextBox OutputFileName;
        private System.Windows.Forms.Button BrowseForOutput;
        private System.Windows.Forms.SaveFileDialog OutputFileDialog;
        private System.Windows.Forms.GroupBox OutputFileGroupBox;
        private System.Windows.Forms.CheckBox OutputFileEnabled;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.GroupBox SimulationGroup;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox SimulationFileName;
        private System.Windows.Forms.Button OpenCloseSimulation;
        private System.Windows.Forms.GroupBox PortGroupBox;
        private System.Windows.Forms.NumericUpDown numericUpDown_DataRead;
        private System.Windows.Forms.OpenFileDialog OpenRecordingDialog;
        private System.Windows.Forms.NumericUpDown PlaybackSpeed;
        private System.Windows.Forms.Label label2PlaybackSpeed;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RichTextBox IPAndPort;
    }
}