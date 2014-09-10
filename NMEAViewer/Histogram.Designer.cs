﻿namespace NMEAViewer
{
    partial class Histogram
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
            this.HistogramSurface = new System.Windows.Forms.PictureBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.rangeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toCurrentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.followCurrentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.numBucketsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.HistogramSurface)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // HistogramSurface
            // 
            this.HistogramSurface.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.HistogramSurface.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.HistogramSurface.Dock = System.Windows.Forms.DockStyle.Fill;
            this.HistogramSurface.Location = new System.Drawing.Point(0, 0);
            this.HistogramSurface.Name = "HistogramSurface";
            this.HistogramSurface.Size = new System.Drawing.Size(282, 235);
            this.HistogramSurface.TabIndex = 0;
            this.HistogramSurface.TabStop = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.AutoSize = false;
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.menuStrip1.Font = new System.Drawing.Font("Segoe UI", 4.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rangeToolStripMenuItem,
            this.numBucketsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 235);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(282, 20);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // rangeToolStripMenuItem
            // 
            this.rangeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toCurrentToolStripMenuItem,
            this.followCurrentToolStripMenuItem});
            this.rangeToolStripMenuItem.Name = "rangeToolStripMenuItem";
            this.rangeToolStripMenuItem.Size = new System.Drawing.Size(42, 16);
            this.rangeToolStripMenuItem.Text = "Range";
            // 
            // toCurrentToolStripMenuItem
            // 
            this.toCurrentToolStripMenuItem.Name = "toCurrentToolStripMenuItem";
            this.toCurrentToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.toCurrentToolStripMenuItem.Text = "ToCurrent";
            this.toCurrentToolStripMenuItem.Click += new System.EventHandler(this.toCurrentToolStripMenuItem_Click);
            // 
            // followCurrentToolStripMenuItem
            // 
            this.followCurrentToolStripMenuItem.Checked = true;
            this.followCurrentToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.followCurrentToolStripMenuItem.Name = "followCurrentToolStripMenuItem";
            this.followCurrentToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.followCurrentToolStripMenuItem.Text = "FollowCurrent";
            this.followCurrentToolStripMenuItem.Click += new System.EventHandler(this.followCurrentToolStripMenuItem_Click);
            // 
            // numBucketsToolStripMenuItem
            // 
            this.numBucketsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem8,
            this.toolStripMenuItem7,
            this.toolStripMenuItem6,
            this.toolStripMenuItem5,
            this.toolStripMenuItem4,
            this.toolStripMenuItem3,
            this.toolStripMenuItem2});
            this.numBucketsToolStripMenuItem.Name = "numBucketsToolStripMenuItem";
            this.numBucketsToolStripMenuItem.Size = new System.Drawing.Size(67, 16);
            this.numBucketsToolStripMenuItem.Text = "NumBuckets";
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(175, 22);
            this.toolStripMenuItem8.Text = "100";
            this.toolStripMenuItem8.Click += new System.EventHandler(this.toolStripNumBuckets);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(175, 22);
            this.toolStripMenuItem7.Text = "50";
            this.toolStripMenuItem7.Click += new System.EventHandler(this.toolStripNumBuckets);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(175, 22);
            this.toolStripMenuItem6.Text = "25";
            this.toolStripMenuItem6.Click += new System.EventHandler(this.toolStripNumBuckets);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(175, 22);
            this.toolStripMenuItem5.Text = "20";
            this.toolStripMenuItem5.Click += new System.EventHandler(this.toolStripNumBuckets);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(175, 22);
            this.toolStripMenuItem4.Text = "15";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.toolStripNumBuckets);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(175, 22);
            this.toolStripMenuItem3.Text = "10";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripNumBuckets);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(175, 22);
            this.toolStripMenuItem2.Text = "5";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripNumBuckets);
            // 
            // Histogram
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 255);
            this.Controls.Add(this.HistogramSurface);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Histogram";
            this.Text = "Histogram";
            this.Load += new System.EventHandler(this.Histogram_Load);
            ((System.ComponentModel.ISupportInitialize)(this.HistogramSurface)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox HistogramSurface;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem rangeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toCurrentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem followCurrentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem numBucketsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem8;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem7;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
    }
}