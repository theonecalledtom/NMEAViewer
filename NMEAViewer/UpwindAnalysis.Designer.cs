namespace NMEAViewer
{
    partial class UpwindAnalysis
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
            this.UpdateData = new System.Windows.Forms.Button();
            this.TimeOnPort = new System.Windows.Forms.TextBox();
            this.TimeOnStarboard = new System.Windows.Forms.TextBox();
            this.AvAnglePort = new System.Windows.Forms.TextBox();
            this.AvAngleStarboard = new System.Windows.Forms.TextBox();
            this.AvSpdStarboard = new System.Windows.Forms.TextBox();
            this.AvSpdPort = new System.Windows.Forms.TextBox();
            this.AvVMGPort = new System.Windows.Forms.TextBox();
            this.AvVMGStarboard = new System.Windows.Forms.TextBox();
            this.Time = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textPolarVMGAverage = new System.Windows.Forms.TextBox();
            this.AverageVMG = new System.Windows.Forms.TextBox();
            this.AverageSpeed = new System.Windows.Forms.TextBox();
            this.AverageAngle = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.StartTimeText = new System.Windows.Forms.TextBox();
            this.EndTimeText = new System.Windows.Forms.TextBox();
            this.ResultsBox = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textTackingAngle = new System.Windows.Forms.TextBox();
            this.textPolarVMGStarboard = new System.Windows.Forms.TextBox();
            this.textPolarVMGPort = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.checkBox_UseGPS = new System.Windows.Forms.CheckBox();
            this.checkBox_FollowSelection = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.ResultsBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // UpdateData
            // 
            this.UpdateData.Location = new System.Drawing.Point(12, 12);
            this.UpdateData.Name = "UpdateData";
            this.UpdateData.Size = new System.Drawing.Size(75, 23);
            this.UpdateData.TabIndex = 0;
            this.UpdateData.Text = "Update";
            this.UpdateData.UseVisualStyleBackColor = true;
            this.UpdateData.Click += new System.EventHandler(this.Update_Click);
            // 
            // TimeOnPort
            // 
            this.TimeOnPort.Location = new System.Drawing.Point(93, 37);
            this.TimeOnPort.Name = "TimeOnPort";
            this.TimeOnPort.Size = new System.Drawing.Size(100, 22);
            this.TimeOnPort.TabIndex = 1;
            // 
            // TimeOnStarboard
            // 
            this.TimeOnStarboard.Location = new System.Drawing.Point(93, 65);
            this.TimeOnStarboard.Name = "TimeOnStarboard";
            this.TimeOnStarboard.Size = new System.Drawing.Size(100, 22);
            this.TimeOnStarboard.TabIndex = 2;
            // 
            // AvAnglePort
            // 
            this.AvAnglePort.Location = new System.Drawing.Point(199, 37);
            this.AvAnglePort.Name = "AvAnglePort";
            this.AvAnglePort.Size = new System.Drawing.Size(100, 22);
            this.AvAnglePort.TabIndex = 3;
            // 
            // AvAngleStarboard
            // 
            this.AvAngleStarboard.Location = new System.Drawing.Point(199, 65);
            this.AvAngleStarboard.Name = "AvAngleStarboard";
            this.AvAngleStarboard.Size = new System.Drawing.Size(100, 22);
            this.AvAngleStarboard.TabIndex = 5;
            // 
            // AvSpdStarboard
            // 
            this.AvSpdStarboard.Location = new System.Drawing.Point(305, 65);
            this.AvSpdStarboard.Name = "AvSpdStarboard";
            this.AvSpdStarboard.Size = new System.Drawing.Size(100, 22);
            this.AvSpdStarboard.TabIndex = 6;
            // 
            // AvSpdPort
            // 
            this.AvSpdPort.Location = new System.Drawing.Point(305, 37);
            this.AvSpdPort.Name = "AvSpdPort";
            this.AvSpdPort.Size = new System.Drawing.Size(100, 22);
            this.AvSpdPort.TabIndex = 7;
            // 
            // AvVMGPort
            // 
            this.AvVMGPort.Location = new System.Drawing.Point(408, 37);
            this.AvVMGPort.Name = "AvVMGPort";
            this.AvVMGPort.Size = new System.Drawing.Size(100, 22);
            this.AvVMGPort.TabIndex = 8;
            // 
            // AvVMGStarboard
            // 
            this.AvVMGStarboard.Location = new System.Drawing.Point(408, 65);
            this.AvVMGStarboard.Name = "AvVMGStarboard";
            this.AvVMGStarboard.Size = new System.Drawing.Size(100, 22);
            this.AvVMGStarboard.TabIndex = 9;
            // 
            // Time
            // 
            this.Time.AutoSize = true;
            this.Time.Location = new System.Drawing.Point(119, 12);
            this.Time.Name = "Time";
            this.Time.Size = new System.Drawing.Size(39, 17);
            this.Time.TabIndex = 10;
            this.Time.Text = "Time";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(222, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 17);
            this.label1.TabIndex = 11;
            this.label1.Text = "Angle";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(329, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 17);
            this.label2.TabIndex = 12;
            this.label2.Text = "Speed";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(435, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 17);
            this.label3.TabIndex = 13;
            this.label3.Text = "VMG";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(33, 41);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 17);
            this.label4.TabIndex = 14;
            this.label4.Text = "Port";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 68);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 17);
            this.label5.TabIndex = 15;
            this.label5.Text = "Starboard";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textPolarVMGAverage);
            this.groupBox1.Controls.Add(this.AverageVMG);
            this.groupBox1.Controls.Add(this.AverageSpeed);
            this.groupBox1.Controls.Add(this.AverageAngle);
            this.groupBox1.Location = new System.Drawing.Point(93, 93);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(528, 37);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Averages";
            // 
            // textPolarVMGAverage
            // 
            this.textPolarVMGAverage.Location = new System.Drawing.Point(420, 9);
            this.textPolarVMGAverage.Name = "textPolarVMGAverage";
            this.textPolarVMGAverage.Size = new System.Drawing.Size(100, 22);
            this.textPolarVMGAverage.TabIndex = 20;
            // 
            // AverageVMG
            // 
            this.AverageVMG.Location = new System.Drawing.Point(315, 9);
            this.AverageVMG.Name = "AverageVMG";
            this.AverageVMG.Size = new System.Drawing.Size(100, 22);
            this.AverageVMG.TabIndex = 2;
            // 
            // AverageSpeed
            // 
            this.AverageSpeed.Location = new System.Drawing.Point(212, 9);
            this.AverageSpeed.Name = "AverageSpeed";
            this.AverageSpeed.Size = new System.Drawing.Size(100, 22);
            this.AverageSpeed.TabIndex = 1;
            // 
            // AverageAngle
            // 
            this.AverageAngle.Location = new System.Drawing.Point(106, 9);
            this.AverageAngle.Name = "AverageAngle";
            this.AverageAngle.Size = new System.Drawing.Size(100, 22);
            this.AverageAngle.TabIndex = 0;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(189, 17);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(68, 17);
            this.label6.TabIndex = 17;
            this.label6.Text = "Start time";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(371, 15);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 17);
            this.label7.TabIndex = 18;
            this.label7.Text = "End time";
            // 
            // StartTimeText
            // 
            this.StartTimeText.Location = new System.Drawing.Point(263, 14);
            this.StartTimeText.Name = "StartTimeText";
            this.StartTimeText.Size = new System.Drawing.Size(100, 22);
            this.StartTimeText.TabIndex = 19;
            // 
            // EndTimeText
            // 
            this.EndTimeText.Location = new System.Drawing.Point(440, 12);
            this.EndTimeText.Name = "EndTimeText";
            this.EndTimeText.Size = new System.Drawing.Size(100, 22);
            this.EndTimeText.TabIndex = 20;
            // 
            // ResultsBox
            // 
            this.ResultsBox.Controls.Add(this.label9);
            this.ResultsBox.Controls.Add(this.textTackingAngle);
            this.ResultsBox.Controls.Add(this.textPolarVMGStarboard);
            this.ResultsBox.Controls.Add(this.textPolarVMGPort);
            this.ResultsBox.Controls.Add(this.label8);
            this.ResultsBox.Controls.Add(this.groupBox1);
            this.ResultsBox.Controls.Add(this.label5);
            this.ResultsBox.Controls.Add(this.label4);
            this.ResultsBox.Controls.Add(this.label3);
            this.ResultsBox.Controls.Add(this.label2);
            this.ResultsBox.Controls.Add(this.label1);
            this.ResultsBox.Controls.Add(this.Time);
            this.ResultsBox.Controls.Add(this.AvVMGStarboard);
            this.ResultsBox.Controls.Add(this.AvVMGPort);
            this.ResultsBox.Controls.Add(this.AvSpdPort);
            this.ResultsBox.Controls.Add(this.AvSpdStarboard);
            this.ResultsBox.Controls.Add(this.AvAngleStarboard);
            this.ResultsBox.Controls.Add(this.AvAnglePort);
            this.ResultsBox.Controls.Add(this.TimeOnStarboard);
            this.ResultsBox.Controls.Add(this.TimeOnPort);
            this.ResultsBox.Location = new System.Drawing.Point(12, 94);
            this.ResultsBox.Name = "ResultsBox";
            this.ResultsBox.Size = new System.Drawing.Size(627, 193);
            this.ResultsBox.TabIndex = 21;
            this.ResultsBox.TabStop = false;
            this.ResultsBox.Text = "Results";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(95, 140);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(98, 17);
            this.label9.TabIndex = 21;
            this.label9.Text = "Tacking Angle";
            // 
            // textTackingAngle
            // 
            this.textTackingAngle.Location = new System.Drawing.Point(199, 137);
            this.textTackingAngle.Name = "textTackingAngle";
            this.textTackingAngle.Size = new System.Drawing.Size(100, 22);
            this.textTackingAngle.TabIndex = 20;
            // 
            // textPolarVMGStarboard
            // 
            this.textPolarVMGStarboard.Location = new System.Drawing.Point(513, 65);
            this.textPolarVMGStarboard.Name = "textPolarVMGStarboard";
            this.textPolarVMGStarboard.Size = new System.Drawing.Size(100, 22);
            this.textPolarVMGStarboard.TabIndex = 19;
            // 
            // textPolarVMGPort
            // 
            this.textPolarVMGPort.Location = new System.Drawing.Point(513, 37);
            this.textPolarVMGPort.Name = "textPolarVMGPort";
            this.textPolarVMGPort.Size = new System.Drawing.Size(100, 22);
            this.textPolarVMGPort.TabIndex = 18;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(525, 12);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 17);
            this.label8.TabIndex = 17;
            this.label8.Text = "Polar%";
            // 
            // checkBox_UseGPS
            // 
            this.checkBox_UseGPS.AutoSize = true;
            this.checkBox_UseGPS.Location = new System.Drawing.Point(13, 51);
            this.checkBox_UseGPS.Name = "checkBox_UseGPS";
            this.checkBox_UseGPS.Size = new System.Drawing.Size(120, 21);
            this.checkBox_UseGPS.TabIndex = 22;
            this.checkBox_UseGPS.Text = "Use GPS data";
            this.checkBox_UseGPS.UseVisualStyleBackColor = true;
            this.checkBox_UseGPS.CheckedChanged += new System.EventHandler(this.checkBox_UseGPS_CheckedChanged);
            // 
            // checkBox_FollowSelection
            // 
            this.checkBox_FollowSelection.AutoSize = true;
            this.checkBox_FollowSelection.Location = new System.Drawing.Point(160, 51);
            this.checkBox_FollowSelection.Name = "checkBox_FollowSelection";
            this.checkBox_FollowSelection.Size = new System.Drawing.Size(129, 21);
            this.checkBox_FollowSelection.TabIndex = 23;
            this.checkBox_FollowSelection.Text = "Follow selection";
            this.checkBox_FollowSelection.UseVisualStyleBackColor = true;
            this.checkBox_FollowSelection.CheckedChanged += new System.EventHandler(this.checkBox_FollowSelection_CheckedChanged);
            // 
            // UpwindAnalysis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(651, 299);
            this.Controls.Add(this.checkBox_FollowSelection);
            this.Controls.Add(this.checkBox_UseGPS);
            this.Controls.Add(this.ResultsBox);
            this.Controls.Add(this.EndTimeText);
            this.Controls.Add(this.StartTimeText);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.UpdateData);
            this.Name = "UpwindAnalysis";
            this.Text = "UpwindAnalysis";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResultsBox.ResumeLayout(false);
            this.ResultsBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button UpdateData;
        private System.Windows.Forms.TextBox TimeOnPort;
        private System.Windows.Forms.TextBox TimeOnStarboard;
        private System.Windows.Forms.TextBox AvAnglePort;
        private System.Windows.Forms.TextBox AvAngleStarboard;
        private System.Windows.Forms.TextBox AvSpdStarboard;
        private System.Windows.Forms.TextBox AvSpdPort;
        private System.Windows.Forms.TextBox AvVMGPort;
        private System.Windows.Forms.TextBox AvVMGStarboard;
        private System.Windows.Forms.Label Time;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox AverageAngle;
        private System.Windows.Forms.TextBox AverageVMG;
        private System.Windows.Forms.TextBox AverageSpeed;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox StartTimeText;
        private System.Windows.Forms.TextBox EndTimeText;
        private System.Windows.Forms.GroupBox ResultsBox;
        private System.Windows.Forms.CheckBox checkBox_UseGPS;
        private System.Windows.Forms.CheckBox checkBox_FollowSelection;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textPolarVMGAverage;
        private System.Windows.Forms.TextBox textPolarVMGStarboard;
        private System.Windows.Forms.TextBox textPolarVMGPort;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textTackingAngle;

    }
}