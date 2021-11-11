
namespace NMEAViewer
{
    partial class SetWindInputDialog
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.WindSpeedInput = new System.Windows.Forms.NumericUpDown();
            this.WindAngleInput = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.WindSpeedInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.WindAngleInput)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(67, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = "Wind Speed";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(172, 25);
            this.label2.TabIndex = 3;
            this.label2.Text = "True Wind Angle";
            // 
            // WindSpeedInput
            // 
            this.WindSpeedInput.Location = new System.Drawing.Point(229, 30);
            this.WindSpeedInput.Name = "WindSpeedInput";
            this.WindSpeedInput.Size = new System.Drawing.Size(143, 31);
            this.WindSpeedInput.TabIndex = 4;
            // 
            // WindAngleInput
            // 
            this.WindAngleInput.Location = new System.Drawing.Point(229, 82);
            this.WindAngleInput.Name = "WindAngleInput";
            this.WindAngleInput.Size = new System.Drawing.Size(143, 31);
            this.WindAngleInput.TabIndex = 5;
            // 
            // SetWindInputDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(436, 148);
            this.Controls.Add(this.WindAngleInput);
            this.Controls.Add(this.WindSpeedInput);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetWindInputDialog";
            this.Text = "Set wind input";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.WindSpeedInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.WindAngleInput)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.NumericUpDown WindSpeedInput;
        public System.Windows.Forms.NumericUpDown WindAngleInput;
    }
}