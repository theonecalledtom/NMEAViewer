﻿namespace NMEAViewer
{
    partial class TimeBasedGraph
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
            this.GraphSurface = new System.Windows.Forms.PictureBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.overlayListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.trackSelectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.directionsAsArrowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.GraphSurface)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // GraphSurface
            // 
            this.GraphSurface.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.GraphSurface.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.GraphSurface.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GraphSurface.Location = new System.Drawing.Point(0, 0);
            this.GraphSurface.Name = "GraphSurface";
            this.GraphSurface.Size = new System.Drawing.Size(703, 207);
            this.GraphSurface.TabIndex = 0;
            this.GraphSurface.TabStop = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.overlayListToolStripMenuItem,
            this.timeToolStripMenuItem,
            this.optionsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 207);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(703, 28);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // overlayListToolStripMenuItem
            // 
            this.overlayListToolStripMenuItem.Name = "overlayListToolStripMenuItem";
            this.overlayListToolStripMenuItem.Size = new System.Drawing.Size(97, 24);
            this.overlayListToolStripMenuItem.Text = "Overlay List";
            // 
            // timeToolStripMenuItem
            // 
            this.timeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.trackSelectionToolStripMenuItem});
            this.timeToolStripMenuItem.Name = "timeToolStripMenuItem";
            this.timeToolStripMenuItem.Size = new System.Drawing.Size(54, 24);
            this.timeToolStripMenuItem.Text = "Time";
            // 
            // trackSelectionToolStripMenuItem
            // 
            this.trackSelectionToolStripMenuItem.Name = "trackSelectionToolStripMenuItem";
            this.trackSelectionToolStripMenuItem.Size = new System.Drawing.Size(176, 24);
            this.trackSelectionToolStripMenuItem.Text = "Track selection";
            this.trackSelectionToolStripMenuItem.Click += new System.EventHandler(this.trackSelectionToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.directionsAsArrowsToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(73, 24);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // directionsAsArrowsToolStripMenuItem
            // 
            this.directionsAsArrowsToolStripMenuItem.Name = "directionsAsArrowsToolStripMenuItem";
            this.directionsAsArrowsToolStripMenuItem.Size = new System.Drawing.Size(211, 24);
            this.directionsAsArrowsToolStripMenuItem.Text = "Directions as arrows";
            this.directionsAsArrowsToolStripMenuItem.Click += new System.EventHandler(this.directionsAsArrowsToolStripMenuItem_Click);
            // 
            // TimeBasedGraph
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(703, 235);
            this.Controls.Add(this.GraphSurface);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "TimeBasedGraph";
            this.Text = "TimeBasedGraph";
            ((System.ComponentModel.ISupportInitialize)(this.GraphSurface)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox GraphSurface;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem overlayListToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem timeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem trackSelectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem directionsAsArrowsToolStripMenuItem;
    }
}