
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setInputsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.liveUpdateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadPolarsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openPolarFile = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.PolarDrawArea)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // PolarDrawArea
            // 
            this.PolarDrawArea.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.PolarDrawArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PolarDrawArea.Location = new System.Drawing.Point(0, 0);
            this.PolarDrawArea.Name = "PolarDrawArea";
            this.PolarDrawArea.Size = new System.Drawing.Size(800, 410);
            this.PolarDrawArea.TabIndex = 0;
            this.PolarDrawArea.TabStop = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 410);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 40);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setInputsToolStripMenuItem,
            this.liveUpdateToolStripMenuItem,
            this.loadPolarsToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(119, 38);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // setInputsToolStripMenuItem
            // 
            this.setInputsToolStripMenuItem.Name = "setInputsToolStripMenuItem";
            this.setInputsToolStripMenuItem.Size = new System.Drawing.Size(359, 44);
            this.setInputsToolStripMenuItem.Text = "Set inputs";
            this.setInputsToolStripMenuItem.Click += new System.EventHandler(this.setInputsToolStripMenuItem_Click);
            // 
            // liveUpdateToolStripMenuItem
            // 
            this.liveUpdateToolStripMenuItem.Checked = true;
            this.liveUpdateToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.liveUpdateToolStripMenuItem.Name = "liveUpdateToolStripMenuItem";
            this.liveUpdateToolStripMenuItem.Size = new System.Drawing.Size(359, 44);
            this.liveUpdateToolStripMenuItem.Text = "LiveUpdate";
            this.liveUpdateToolStripMenuItem.Click += new System.EventHandler(this.liveUpdateToolStripMenuItem_Click);
            // 
            // loadPolarsToolStripMenuItem
            // 
            this.loadPolarsToolStripMenuItem.Name = "loadPolarsToolStripMenuItem";
            this.loadPolarsToolStripMenuItem.Size = new System.Drawing.Size(359, 44);
            this.loadPolarsToolStripMenuItem.Text = "Load Polars";
            this.loadPolarsToolStripMenuItem.Click += new System.EventHandler(this.loadPolarsToolStripMenuItem_Click);
            // 
            // openPolarFile
            // 
            this.openPolarFile.FileName = "openPolarFile";
            // 
            // PolarDiagramWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.PolarDrawArea);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "PolarDiagramWindow";
            this.Text = "PolarDiagram";
            ((System.ComponentModel.ISupportInitialize)(this.PolarDrawArea)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox PolarDrawArea;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setInputsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem liveUpdateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadPolarsToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openPolarFile;
    }
}