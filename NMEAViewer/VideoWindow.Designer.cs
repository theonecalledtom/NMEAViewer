namespace NMEAViewer
{
    partial class VideoWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VideoWindow));
            this.WMPPlayer = new AxWMPLib.AxWindowsMediaPlayer();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.setFirstSyncPointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setSecondSyncPointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.WMPPlayer)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // WMPPlayer
            // 
            this.WMPPlayer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WMPPlayer.Enabled = true;
            this.WMPPlayer.Location = new System.Drawing.Point(0, 0);
            this.WMPPlayer.Name = "WMPPlayer";
            this.WMPPlayer.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("WMPPlayer.OcxState")));
            this.WMPPlayer.Size = new System.Drawing.Size(282, 255);
            this.WMPPlayer.TabIndex = 0;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setFirstSyncPointToolStripMenuItem,
            this.setSecondSyncPointToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(213, 80);
            // 
            // setFirstSyncPointToolStripMenuItem
            // 
            this.setFirstSyncPointToolStripMenuItem.Name = "setFirstSyncPointToolStripMenuItem";
            this.setFirstSyncPointToolStripMenuItem.Size = new System.Drawing.Size(212, 24);
            this.setFirstSyncPointToolStripMenuItem.Text = "SetFirstSyncPoint";
            this.setFirstSyncPointToolStripMenuItem.Click += new System.EventHandler(this.setFirstSyncPointToolStripMenuItem_Click);
            // 
            // setSecondSyncPointToolStripMenuItem
            // 
            this.setSecondSyncPointToolStripMenuItem.Name = "setSecondSyncPointToolStripMenuItem";
            this.setSecondSyncPointToolStripMenuItem.Size = new System.Drawing.Size(212, 24);
            this.setSecondSyncPointToolStripMenuItem.Text = "SetSecondSyncPoint";
            this.setSecondSyncPointToolStripMenuItem.Click += new System.EventHandler(this.setSecondSyncPointToolStripMenuItem_Click);
            // 
            // VideoWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 255);
            this.Controls.Add(this.WMPPlayer);
            this.Name = "VideoWindow";
            this.Text = "VideoWindow";
            ((System.ComponentModel.ISupportInitialize)(this.WMPPlayer)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private AxWMPLib.AxWindowsMediaPlayer WMPPlayer;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem setFirstSyncPointToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setSecondSyncPointToolStripMenuItem;
    }
}