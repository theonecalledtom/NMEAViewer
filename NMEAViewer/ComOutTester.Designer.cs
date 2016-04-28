namespace NMEAViewer
{
    partial class ComOutTester
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
            this.button_ComCreateOrDestroy = new System.Windows.Forms.Button();
            this.PortNameTextBox = new System.Windows.Forms.TextBox();
            this.BaudRateTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.DataBitsTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ParityChoice = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.StopBits = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // serialPort1
            // 
            this.serialPort1.BaudRate = 4800;
            // 
            // button_ComCreateOrDestroy
            // 
            this.button_ComCreateOrDestroy.Location = new System.Drawing.Point(74, 189);
            this.button_ComCreateOrDestroy.Name = "button_ComCreateOrDestroy";
            this.button_ComCreateOrDestroy.Size = new System.Drawing.Size(75, 23);
            this.button_ComCreateOrDestroy.TabIndex = 0;
            this.button_ComCreateOrDestroy.Text = "Create";
            this.button_ComCreateOrDestroy.UseVisualStyleBackColor = true;
            this.button_ComCreateOrDestroy.Click += new System.EventHandler(this.button_ComCreateOrDestroy_Click);
            // 
            // PortNameTextBox
            // 
            this.PortNameTextBox.Location = new System.Drawing.Point(75, 20);
            this.PortNameTextBox.Name = "PortNameTextBox";
            this.PortNameTextBox.Size = new System.Drawing.Size(100, 20);
            this.PortNameTextBox.TabIndex = 1;
            this.PortNameTextBox.Text = "COM1";
            // 
            // BaudRateTextBox
            // 
            this.BaudRateTextBox.Location = new System.Drawing.Point(75, 47);
            this.BaudRateTextBox.Name = "BaudRateTextBox";
            this.BaudRateTextBox.Size = new System.Drawing.Size(100, 20);
            this.BaudRateTextBox.TabIndex = 2;
            this.BaudRateTextBox.Text = "4800";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "PortName";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "BaudRate";
            // 
            // DataBitsTextBox
            // 
            this.DataBitsTextBox.Location = new System.Drawing.Point(75, 74);
            this.DataBitsTextBox.Name = "DataBitsTextBox";
            this.DataBitsTextBox.Size = new System.Drawing.Size(100, 20);
            this.DataBitsTextBox.TabIndex = 6;
            this.DataBitsTextBox.Text = "8";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 77);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Data bits";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.StopBits);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.ParityChoice);
            this.groupBox1.Controls.Add(this.DataBitsTextBox);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.PortNameTextBox);
            this.groupBox1.Controls.Add(this.BaudRateTextBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 171);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Connection info";
            // 
            // ParityChoice
            // 
            this.ParityChoice.FormattingEnabled = true;
            this.ParityChoice.Items.AddRange(new object[] {
            "None",
            "Odd",
            "Even"});
            this.ParityChoice.Location = new System.Drawing.Point(75, 100);
            this.ParityChoice.Name = "ParityChoice";
            this.ParityChoice.Size = new System.Drawing.Size(100, 21);
            this.ParityChoice.TabIndex = 8;
            this.ParityChoice.Text = "None";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(36, 103);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Parity";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(25, 131);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(45, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Stopbits";
            // 
            // StopBits
            // 
            this.StopBits.FormattingEnabled = true;
            this.StopBits.Items.AddRange(new object[] {
            "None",
            "One",
            "Two",
            "OnePointFive"});
            this.StopBits.Location = new System.Drawing.Point(75, 128);
            this.StopBits.Name = "StopBits";
            this.StopBits.Size = new System.Drawing.Size(100, 21);
            this.StopBits.TabIndex = 12;
            this.StopBits.Text = "One";
            // 
            // ComOutTester
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(224, 224);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button_ComCreateOrDestroy);
            this.Name = "ComOutTester";
            this.Text = "ComOutTester";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.Button button_ComCreateOrDestroy;
        private System.Windows.Forms.TextBox PortNameTextBox;
        private System.Windows.Forms.TextBox BaudRateTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox DataBitsTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox StopBits;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox ParityChoice;
    }
}