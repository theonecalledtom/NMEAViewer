namespace NMEAViewer
{
    partial class TimeBasedGraphDataTypes
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
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.DataTypeComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.DirectionArrowCheckBox = new System.Windows.Forms.CheckBox();
            this.LineThicknessComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.FieldColorPanel = new System.Windows.Forms.Panel();
            this.BackgroundColorPanel = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.InvertArrowCheckbox = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // DataTypeComboBox
            // 
            this.DataTypeComboBox.FormattingEnabled = true;
            this.DataTypeComboBox.Location = new System.Drawing.Point(56, 7);
            this.DataTypeComboBox.Name = "DataTypeComboBox";
            this.DataTypeComboBox.Size = new System.Drawing.Size(121, 24);
            this.DataTypeComboBox.TabIndex = 0;
            this.DataTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.DataTypeComboBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Field";
            // 
            // DirectionArrowCheckBox
            // 
            this.DirectionArrowCheckBox.AutoSize = true;
            this.DirectionArrowCheckBox.Location = new System.Drawing.Point(15, 37);
            this.DirectionArrowCheckBox.Name = "DirectionArrowCheckBox";
            this.DirectionArrowCheckBox.Size = new System.Drawing.Size(66, 21);
            this.DirectionArrowCheckBox.TabIndex = 2;
            this.DirectionArrowCheckBox.Text = "Arrow";
            this.DirectionArrowCheckBox.UseVisualStyleBackColor = true;
            this.DirectionArrowCheckBox.CheckedChanged += new System.EventHandler(this.DirectionArrowCheckBox_CheckedChanged);
            // 
            // LineThicknessComboBox
            // 
            this.LineThicknessComboBox.FormattingEnabled = true;
            this.LineThicknessComboBox.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4"});
            this.LineThicknessComboBox.Location = new System.Drawing.Point(56, 66);
            this.LineThicknessComboBox.Name = "LineThicknessComboBox";
            this.LineThicknessComboBox.Size = new System.Drawing.Size(121, 24);
            this.LineThicknessComboBox.TabIndex = 3;
            this.LineThicknessComboBox.SelectedIndexChanged += new System.EventHandler(this.LineThicknessComboBox_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Line";
            // 
            // FieldColorPanel
            // 
            this.FieldColorPanel.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.FieldColorPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.FieldColorPanel.Location = new System.Drawing.Point(3, 18);
            this.FieldColorPanel.Name = "FieldColorPanel";
            this.FieldColorPanel.Size = new System.Drawing.Size(159, 79);
            this.FieldColorPanel.TabIndex = 5;
            // 
            // BackgroundColorPanel
            // 
            this.BackgroundColorPanel.BackColor = System.Drawing.SystemColors.Info;
            this.BackgroundColorPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BackgroundColorPanel.Location = new System.Drawing.Point(3, 18);
            this.BackgroundColorPanel.Name = "BackgroundColorPanel";
            this.BackgroundColorPanel.Size = new System.Drawing.Size(159, 79);
            this.BackgroundColorPanel.TabIndex = 6;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.FieldColorPanel);
            this.groupBox1.Location = new System.Drawing.Point(12, 100);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(165, 100);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Field Color";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.BackgroundColorPanel);
            this.groupBox2.Location = new System.Drawing.Point(12, 207);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(165, 100);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "BackgroundColor";
            // 
            // InvertArrowCheckbox
            // 
            this.InvertArrowCheckbox.AutoSize = true;
            this.InvertArrowCheckbox.Location = new System.Drawing.Point(88, 37);
            this.InvertArrowCheckbox.Name = "InvertArrowCheckbox";
            this.InvertArrowCheckbox.Size = new System.Drawing.Size(65, 21);
            this.InvertArrowCheckbox.TabIndex = 9;
            this.InvertArrowCheckbox.Text = "Invert";
            this.InvertArrowCheckbox.UseVisualStyleBackColor = true;
            this.InvertArrowCheckbox.CheckedChanged += new System.EventHandler(this.InvertArrowCheckbox_CheckedChanged);
            // 
            // TimeBasedGraphDataTypes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(189, 315);
            this.Controls.Add(this.InvertArrowCheckbox);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.LineThicknessComboBox);
            this.Controls.Add(this.DirectionArrowCheckBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DataTypeComboBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TimeBasedGraphDataTypes";
            this.Text = "Graph style";
            this.TopMost = true;
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.ComboBox DataTypeComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox DirectionArrowCheckBox;
        private System.Windows.Forms.ComboBox LineThicknessComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel FieldColorPanel;
        private System.Windows.Forms.Panel BackgroundColorPanel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox InvertArrowCheckbox;

    }
}