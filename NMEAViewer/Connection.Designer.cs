﻿namespace NMEAViewer
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
            this.checkBox_WriteRaw = new System.Windows.Forms.CheckBox();
            this.OutputFileEnabled = new System.Windows.Forms.CheckBox();
            this.SimulationGroup = new System.Windows.Forms.GroupBox();
            this.checkBox_Loop = new System.Windows.Forms.CheckBox();
            this.label2PlaybackSpeed = new System.Windows.Forms.Label();
            this.PlaybackSpeed = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_DataRead = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.SimulationFileName = new System.Windows.Forms.TextBox();
            this.OpenCloseSimulation = new System.Windows.Forms.Button();
            this.PortGroupBox = new System.Windows.Forms.GroupBox();
            this.checkBox_Forward = new System.Windows.Forms.CheckBox();
            this.ComportConnectPanel = new System.Windows.Forms.Panel();
            this.ComportRadioButton = new System.Windows.Forms.RadioButton();
            this.IPConnectPanel = new System.Windows.Forms.Panel();
            this.IPTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.PortTextBox = new System.Windows.Forms.TextBox();
            this.Port = new System.Windows.Forms.Label();
            this.IPRadioButton = new System.Windows.Forms.RadioButton();
            this.OpenRecordingDialog = new System.Windows.Forms.OpenFileDialog();
            this.LogBox = new System.Windows.Forms.TextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.BytesReadNumber)).BeginInit();
            this.OutputFileGroupBox.SuspendLayout();
            this.SimulationGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PlaybackSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_DataRead)).BeginInit();
            this.PortGroupBox.SuspendLayout();
            this.ComportConnectPanel.SuspendLayout();
            this.IPConnectPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
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
            this.FindPorts.Location = new System.Drawing.Point(16, 21);
            this.FindPorts.Margin = new System.Windows.Forms.Padding(4);
            this.FindPorts.Name = "FindPorts";
            this.FindPorts.Size = new System.Drawing.Size(110, 37);
            this.FindPorts.TabIndex = 0;
            this.FindPorts.Text = "Find";
            this.FindPorts.UseVisualStyleBackColor = true;
            this.FindPorts.Click += new System.EventHandler(this.FindPorts_Click);
            // 
            // OpenPortComboList
            // 
            this.OpenPortComboList.FormattingEnabled = true;
            this.OpenPortComboList.Location = new System.Drawing.Point(136, 21);
            this.OpenPortComboList.Margin = new System.Windows.Forms.Padding(4);
            this.OpenPortComboList.Name = "OpenPortComboList";
            this.OpenPortComboList.Size = new System.Drawing.Size(180, 33);
            this.OpenPortComboList.TabIndex = 1;
            this.OpenPortComboList.SelectedIndexChanged += new System.EventHandler(this.OpenPortComboList_SelectedIndexChanged);
            // 
            // OpenClose
            // 
            this.OpenClose.Enabled = false;
            this.OpenClose.Location = new System.Drawing.Point(8, 337);
            this.OpenClose.Margin = new System.Windows.Forms.Padding(4);
            this.OpenClose.Name = "OpenClose";
            this.OpenClose.Size = new System.Drawing.Size(376, 42);
            this.OpenClose.TabIndex = 2;
            this.OpenClose.Text = "Open";
            this.OpenClose.UseVisualStyleBackColor = true;
            this.OpenClose.Click += new System.EventHandler(this.OpenClose_Click);
            // 
            // BytesReadLabel
            // 
            this.BytesReadLabel.AutoSize = true;
            this.BytesReadLabel.Enabled = false;
            this.BytesReadLabel.Location = new System.Drawing.Point(94, 246);
            this.BytesReadLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.BytesReadLabel.Name = "BytesReadLabel";
            this.BytesReadLabel.Size = new System.Drawing.Size(115, 25);
            this.BytesReadLabel.TabIndex = 3;
            this.BytesReadLabel.Text = "Bytes read";
            // 
            // BytesReadNumber
            // 
            this.BytesReadNumber.Enabled = false;
            this.BytesReadNumber.InterceptArrowKeys = false;
            this.BytesReadNumber.Location = new System.Drawing.Point(210, 246);
            this.BytesReadNumber.Margin = new System.Windows.Forms.Padding(4);
            this.BytesReadNumber.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.BytesReadNumber.Name = "BytesReadNumber";
            this.BytesReadNumber.Size = new System.Drawing.Size(128, 31);
            this.BytesReadNumber.TabIndex = 4;
            this.BytesReadNumber.ThousandsSeparator = true;
            // 
            // OutputFileName
            // 
            this.OutputFileName.Location = new System.Drawing.Point(8, 94);
            this.OutputFileName.Margin = new System.Windows.Forms.Padding(4);
            this.OutputFileName.Name = "OutputFileName";
            this.OutputFileName.Size = new System.Drawing.Size(264, 31);
            this.OutputFileName.TabIndex = 6;
            // 
            // BrowseForOutput
            // 
            this.BrowseForOutput.Location = new System.Drawing.Point(284, 92);
            this.BrowseForOutput.Margin = new System.Windows.Forms.Padding(4);
            this.BrowseForOutput.Name = "BrowseForOutput";
            this.BrowseForOutput.Size = new System.Drawing.Size(104, 37);
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
            this.OutputFileGroupBox.Controls.Add(this.checkBox_WriteRaw);
            this.OutputFileGroupBox.Controls.Add(this.BrowseForOutput);
            this.OutputFileGroupBox.Controls.Add(this.OutputFileEnabled);
            this.OutputFileGroupBox.Controls.Add(this.OutputFileName);
            this.OutputFileGroupBox.Location = new System.Drawing.Point(17, 445);
            this.OutputFileGroupBox.Margin = new System.Windows.Forms.Padding(4);
            this.OutputFileGroupBox.Name = "OutputFileGroupBox";
            this.OutputFileGroupBox.Padding = new System.Windows.Forms.Padding(4);
            this.OutputFileGroupBox.Size = new System.Drawing.Size(400, 219);
            this.OutputFileGroupBox.TabIndex = 9;
            this.OutputFileGroupBox.TabStop = false;
            this.OutputFileGroupBox.Text = "Output file";
            // 
            // checkBox_WriteRaw
            // 
            this.checkBox_WriteRaw.AutoSize = true;
            this.checkBox_WriteRaw.Location = new System.Drawing.Point(12, 144);
            this.checkBox_WriteRaw.Margin = new System.Windows.Forms.Padding(6);
            this.checkBox_WriteRaw.Name = "checkBox_WriteRaw";
            this.checkBox_WriteRaw.Size = new System.Drawing.Size(235, 29);
            this.checkBox_WriteRaw.TabIndex = 9;
            this.checkBox_WriteRaw.Text = "Also write raw bytes";
            this.checkBox_WriteRaw.UseVisualStyleBackColor = true;
            // 
            // OutputFileEnabled
            // 
            this.OutputFileEnabled.AutoSize = true;
            this.OutputFileEnabled.Checked = true;
            this.OutputFileEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.OutputFileEnabled.Location = new System.Drawing.Point(8, 52);
            this.OutputFileEnabled.Margin = new System.Windows.Forms.Padding(4);
            this.OutputFileEnabled.Name = "OutputFileEnabled";
            this.OutputFileEnabled.Size = new System.Drawing.Size(123, 29);
            this.OutputFileEnabled.TabIndex = 7;
            this.OutputFileEnabled.Text = "Enabled";
            this.OutputFileEnabled.UseVisualStyleBackColor = true;
            this.OutputFileEnabled.CheckedChanged += new System.EventHandler(this.OutputFileEnabled_CheckedChanged);
            // 
            // SimulationGroup
            // 
            this.SimulationGroup.Controls.Add(this.checkBox_Loop);
            this.SimulationGroup.Controls.Add(this.label2PlaybackSpeed);
            this.SimulationGroup.Controls.Add(this.PlaybackSpeed);
            this.SimulationGroup.Controls.Add(this.numericUpDown_DataRead);
            this.SimulationGroup.Controls.Add(this.label1);
            this.SimulationGroup.Controls.Add(this.SimulationFileName);
            this.SimulationGroup.Controls.Add(this.OpenCloseSimulation);
            this.SimulationGroup.Location = new System.Drawing.Point(17, 672);
            this.SimulationGroup.Margin = new System.Windows.Forms.Padding(4);
            this.SimulationGroup.Name = "SimulationGroup";
            this.SimulationGroup.Padding = new System.Windows.Forms.Padding(4);
            this.SimulationGroup.Size = new System.Drawing.Size(398, 195);
            this.SimulationGroup.TabIndex = 10;
            this.SimulationGroup.TabStop = false;
            this.SimulationGroup.Text = "Simulation";
            // 
            // checkBox_Loop
            // 
            this.checkBox_Loop.AutoSize = true;
            this.checkBox_Loop.Checked = true;
            this.checkBox_Loop.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_Loop.Location = new System.Drawing.Point(294, 159);
            this.checkBox_Loop.Name = "checkBox_Loop";
            this.checkBox_Loop.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBox_Loop.Size = new System.Drawing.Size(92, 29);
            this.checkBox_Loop.TabIndex = 8;
            this.checkBox_Loop.Text = "Loop";
            this.checkBox_Loop.UseVisualStyleBackColor = true;
            // 
            // label2PlaybackSpeed
            // 
            this.label2PlaybackSpeed.AutoSize = true;
            this.label2PlaybackSpeed.Location = new System.Drawing.Point(94, 123);
            this.label2PlaybackSpeed.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2PlaybackSpeed.Name = "label2PlaybackSpeed";
            this.label2PlaybackSpeed.Size = new System.Drawing.Size(165, 25);
            this.label2PlaybackSpeed.TabIndex = 7;
            this.label2PlaybackSpeed.Text = "Playback speed";
            // 
            // PlaybackSpeed
            // 
            this.PlaybackSpeed.Location = new System.Drawing.Point(266, 121);
            this.PlaybackSpeed.Margin = new System.Windows.Forms.Padding(4);
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
            this.PlaybackSpeed.Size = new System.Drawing.Size(124, 31);
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
            this.numericUpDown_DataRead.Location = new System.Drawing.Point(266, 77);
            this.numericUpDown_DataRead.Margin = new System.Windows.Forms.Padding(4);
            this.numericUpDown_DataRead.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.numericUpDown_DataRead.Name = "numericUpDown_DataRead";
            this.numericUpDown_DataRead.Size = new System.Drawing.Size(124, 31);
            this.numericUpDown_DataRead.TabIndex = 5;
            this.numericUpDown_DataRead.ThousandsSeparator = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Enabled = false;
            this.label1.Location = new System.Drawing.Point(140, 79);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 25);
            this.label1.TabIndex = 4;
            this.label1.Text = "Bytes read";
            // 
            // SimulationFileName
            // 
            this.SimulationFileName.Enabled = false;
            this.SimulationFileName.Location = new System.Drawing.Point(146, 35);
            this.SimulationFileName.Margin = new System.Windows.Forms.Padding(4);
            this.SimulationFileName.Name = "SimulationFileName";
            this.SimulationFileName.Size = new System.Drawing.Size(240, 31);
            this.SimulationFileName.TabIndex = 1;
            // 
            // OpenCloseSimulation
            // 
            this.OpenCloseSimulation.Location = new System.Drawing.Point(10, 35);
            this.OpenCloseSimulation.Margin = new System.Windows.Forms.Padding(4);
            this.OpenCloseSimulation.Name = "OpenCloseSimulation";
            this.OpenCloseSimulation.Size = new System.Drawing.Size(112, 38);
            this.OpenCloseSimulation.TabIndex = 0;
            this.OpenCloseSimulation.Text = "Open";
            this.OpenCloseSimulation.UseVisualStyleBackColor = true;
            this.OpenCloseSimulation.Click += new System.EventHandler(this.OpenCloseSimulation_Click);
            // 
            // PortGroupBox
            // 
            this.PortGroupBox.Controls.Add(this.IPConnectPanel);
            this.PortGroupBox.Controls.Add(this.ComportRadioButton);
            this.PortGroupBox.Controls.Add(this.checkBox_Forward);
            this.PortGroupBox.Controls.Add(this.ComportConnectPanel);
            this.PortGroupBox.Controls.Add(this.OpenClose);
            this.PortGroupBox.Controls.Add(this.IPRadioButton);
            this.PortGroupBox.Controls.Add(this.BytesReadNumber);
            this.PortGroupBox.Controls.Add(this.BytesReadLabel);
            this.PortGroupBox.Location = new System.Drawing.Point(17, 18);
            this.PortGroupBox.Margin = new System.Windows.Forms.Padding(4);
            this.PortGroupBox.Name = "PortGroupBox";
            this.PortGroupBox.Padding = new System.Windows.Forms.Padding(4);
            this.PortGroupBox.Size = new System.Drawing.Size(400, 417);
            this.PortGroupBox.TabIndex = 11;
            this.PortGroupBox.TabStop = false;
            this.PortGroupBox.Text = "Connection";
            // 
            // checkBox_Forward
            // 
            this.checkBox_Forward.AutoSize = true;
            this.checkBox_Forward.Checked = true;
            this.checkBox_Forward.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_Forward.Location = new System.Drawing.Point(46, 294);
            this.checkBox_Forward.Margin = new System.Windows.Forms.Padding(6);
            this.checkBox_Forward.Name = "checkBox_Forward";
            this.checkBox_Forward.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBox_Forward.Size = new System.Drawing.Size(188, 29);
            this.checkBox_Forward.TabIndex = 14;
            this.checkBox_Forward.Text = "Forward to app";
            this.checkBox_Forward.UseVisualStyleBackColor = true;
            // 
            // ComportConnectPanel
            // 
            this.ComportConnectPanel.Controls.Add(this.OpenPortComboList);
            this.ComportConnectPanel.Controls.Add(this.FindPorts);
            this.ComportConnectPanel.Location = new System.Drawing.Point(52, 157);
            this.ComportConnectPanel.Margin = new System.Windows.Forms.Padding(4);
            this.ComportConnectPanel.Name = "ComportConnectPanel";
            this.ComportConnectPanel.Size = new System.Drawing.Size(332, 79);
            this.ComportConnectPanel.TabIndex = 13;
            // 
            // ComportRadioButton
            // 
            this.ComportRadioButton.AutoSize = true;
            this.ComportRadioButton.Location = new System.Drawing.Point(1, 183);
            this.ComportRadioButton.Margin = new System.Windows.Forms.Padding(4);
            this.ComportRadioButton.Name = "ComportRadioButton";
            this.ComportRadioButton.Size = new System.Drawing.Size(27, 26);
            this.ComportRadioButton.TabIndex = 11;
            this.ComportRadioButton.TabStop = true;
            this.ComportRadioButton.UseVisualStyleBackColor = true;
            this.ComportRadioButton.CheckedChanged += new System.EventHandler(this.ComportRadioButton_CheckedChanged);
            // 
            // IPConnectPanel
            // 
            this.IPConnectPanel.Controls.Add(this.label2);
            this.IPConnectPanel.Controls.Add(this.Port);
            this.IPConnectPanel.Controls.Add(this.IPTextBox);
            this.IPConnectPanel.Controls.Add(this.PortTextBox);
            this.IPConnectPanel.Location = new System.Drawing.Point(52, 32);
            this.IPConnectPanel.Margin = new System.Windows.Forms.Padding(4);
            this.IPConnectPanel.Name = "IPConnectPanel";
            this.IPConnectPanel.Size = new System.Drawing.Size(332, 96);
            this.IPConnectPanel.TabIndex = 12;
            // 
            // IPTextBox
            // 
            this.IPTextBox.Location = new System.Drawing.Point(77, 17);
            this.IPTextBox.Name = "IPTextBox";
            this.IPTextBox.Size = new System.Drawing.Size(240, 31);
            this.IPTextBox.TabIndex = 10;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(37, 20);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 25);
            this.label2.TabIndex = 6;
            this.label2.Text = "IP";
            // 
            // PortTextBox
            // 
            this.PortTextBox.Location = new System.Drawing.Point(77, 55);
            this.PortTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.PortTextBox.Name = "PortTextBox";
            this.PortTextBox.Size = new System.Drawing.Size(110, 31);
            this.PortTextBox.TabIndex = 9;
            // 
            // Port
            // 
            this.Port.AutoSize = true;
            this.Port.Location = new System.Drawing.Point(18, 58);
            this.Port.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Port.Name = "Port";
            this.Port.Size = new System.Drawing.Size(51, 25);
            this.Port.TabIndex = 7;
            this.Port.Text = "Port";
            // 
            // IPRadioButton
            // 
            this.IPRadioButton.AutoSize = true;
            this.IPRadioButton.Location = new System.Drawing.Point(1, 64);
            this.IPRadioButton.Margin = new System.Windows.Forms.Padding(4);
            this.IPRadioButton.Name = "IPRadioButton";
            this.IPRadioButton.Size = new System.Drawing.Size(27, 26);
            this.IPRadioButton.TabIndex = 10;
            this.IPRadioButton.TabStop = true;
            this.IPRadioButton.UseVisualStyleBackColor = true;
            this.IPRadioButton.CheckedChanged += new System.EventHandler(this.IPRadioButton_CheckedChanged);
            // 
            // OpenRecordingDialog
            // 
            this.OpenRecordingDialog.FileName = "SomeRecording.dat";
            this.OpenRecordingDialog.Filter = "dat files *.dat|*.dat";
            // 
            // LogBox
            // 
            this.LogBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LogBox.Location = new System.Drawing.Point(0, 0);
            this.LogBox.Multiline = true;
            this.LogBox.Name = "LogBox";
            this.LogBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.LogBox.Size = new System.Drawing.Size(471, 291);
            this.LogBox.TabIndex = 12;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.PortGroupBox);
            this.splitContainer1.Panel1.Controls.Add(this.SimulationGroup);
            this.splitContainer1.Panel1.Controls.Add(this.OutputFileGroupBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.LogBox);
            this.splitContainer1.Size = new System.Drawing.Size(471, 1223);
            this.splitContainer1.SplitterDistance = 928;
            this.splitContainer1.TabIndex = 13;
            // 
            // Connection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(471, 1223);
            this.Controls.Add(this.splitContainer1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
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
            this.ComportConnectPanel.ResumeLayout(false);
            this.IPConnectPanel.ResumeLayout(false);
            this.IPConnectPanel.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
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
        private System.Windows.Forms.TextBox PortTextBox;
        private System.Windows.Forms.Label Port;
        private System.Windows.Forms.Panel ComportConnectPanel;
        private System.Windows.Forms.RadioButton ComportRadioButton;
        private System.Windows.Forms.Panel IPConnectPanel;
        private System.Windows.Forms.RadioButton IPRadioButton;
        private System.Windows.Forms.CheckBox checkBox_Forward;
        private System.Windows.Forms.CheckBox checkBox_WriteRaw;
        private System.Windows.Forms.TextBox LogBox;
        private System.Windows.Forms.TextBox IPTextBox;
        private System.Windows.Forms.CheckBox checkBox_Loop;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}