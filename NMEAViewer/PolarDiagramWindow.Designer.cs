
namespace NMEAViewer
{
    partial class PolarDiagramWindow
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
            this.PolarDrawArea = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.PolarDrawArea)).BeginInit();
            this.SuspendLayout();
            // 
            // PolarDrawArea
            // 
            this.PolarDrawArea.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.PolarDrawArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PolarDrawArea.Location = new System.Drawing.Point(0, 0);
            this.PolarDrawArea.Name = "PolarDrawArea";
            this.PolarDrawArea.Size = new System.Drawing.Size(800, 450);
            this.PolarDrawArea.TabIndex = 0;
            this.PolarDrawArea.TabStop = false;
            // 
            // PolarDiagramWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.PolarDrawArea);
            this.Name = "PolarDiagramWindow";
            this.Text = "PolarDiagram";
            ((System.ComponentModel.ISupportInitialize)(this.PolarDrawArea)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox PolarDrawArea;
    }
}