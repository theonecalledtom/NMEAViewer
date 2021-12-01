
namespace NMEAViewer
{
    partial class GPXLoader
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.inject = new System.Windows.Forms.Button();
            this.RemoveTWD = new System.Windows.Forms.Button();
            this.AddTWD = new System.Windows.Forms.Button();
            this.TWDTable = new System.Windows.Forms.DataGridView();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.LoadGPX = new System.Windows.Forms.Button();
            this.Map = new NMEAViewer.GMapComponent(this.components);
            this.PathOffset = new System.Windows.Forms.TrackBar();
            this.gMapComponent1 = new NMEAViewer.GMapComponent(this.components);
            this.openGPXDialog = new System.Windows.Forms.OpenFileDialog();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.Time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TWD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TWDTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PathOffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.AutoScroll = true;
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            this.splitContainer1.Panel1.Controls.Add(this.splitter2);
            this.splitContainer1.Panel1.Controls.Add(this.splitter1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.Map);
            this.splitContainer1.Panel2.Controls.Add(this.PathOffset);
            this.splitContainer1.Panel2.Controls.Add(this.gMapComponent1);
            this.splitContainer1.Size = new System.Drawing.Size(1696, 868);
            this.splitContainer1.SplitterDistance = 563;
            this.splitContainer1.TabIndex = 0;
            // 
            // inject
            // 
            this.inject.Location = new System.Drawing.Point(21, 227);
            this.inject.Name = "inject";
            this.inject.Size = new System.Drawing.Size(212, 78);
            this.inject.TabIndex = 6;
            this.inject.Text = "INJECT";
            this.inject.UseVisualStyleBackColor = true;
            this.inject.Click += new System.EventHandler(this.inject_Click);
            // 
            // RemoveTWD
            // 
            this.RemoveTWD.Location = new System.Drawing.Point(21, 160);
            this.RemoveTWD.Name = "RemoveTWD";
            this.RemoveTWD.Size = new System.Drawing.Size(212, 51);
            this.RemoveTWD.TabIndex = 5;
            this.RemoveTWD.Text = "Remove TWD";
            this.RemoveTWD.UseVisualStyleBackColor = true;
            this.RemoveTWD.Click += new System.EventHandler(this.RemoveTWD_Click);
            // 
            // AddTWD
            // 
            this.AddTWD.Location = new System.Drawing.Point(21, 86);
            this.AddTWD.Name = "AddTWD";
            this.AddTWD.Size = new System.Drawing.Size(212, 51);
            this.AddTWD.TabIndex = 4;
            this.AddTWD.Text = "Add TWD";
            this.AddTWD.UseVisualStyleBackColor = true;
            this.AddTWD.Click += new System.EventHandler(this.AddTWD_Click);
            // 
            // TWDTable
            // 
            this.TWDTable.AllowUserToAddRows = false;
            this.TWDTable.AllowUserToDeleteRows = false;
            this.TWDTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.TWDTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Time,
            this.TWD});
            this.TWDTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TWDTable.Location = new System.Drawing.Point(0, 0);
            this.TWDTable.Name = "TWDTable";
            this.TWDTable.RowHeadersWidth = 82;
            this.TWDTable.RowTemplate.Height = 33;
            this.TWDTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.TWDTable.Size = new System.Drawing.Size(557, 464);
            this.TWDTable.TabIndex = 3;
            // 
            // splitter2
            // 
            this.splitter2.Location = new System.Drawing.Point(3, 0);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(3, 868);
            this.splitter2.TabIndex = 2;
            this.splitter2.TabStop = false;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(0, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 868);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            // 
            // LoadGPX
            // 
            this.LoadGPX.Location = new System.Drawing.Point(21, 19);
            this.LoadGPX.Name = "LoadGPX";
            this.LoadGPX.Size = new System.Drawing.Size(212, 51);
            this.LoadGPX.TabIndex = 0;
            this.LoadGPX.Text = "Load GPX";
            this.LoadGPX.UseVisualStyleBackColor = true;
            this.LoadGPX.Click += new System.EventHandler(this.LoadGPX_Click);
            // 
            // Map
            // 
            this.Map.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.Map.Bearing = 0F;
            this.Map.CanDragMap = true;
            this.Map.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Map.EmptyTileColor = System.Drawing.Color.Navy;
            this.Map.GrayScaleMode = false;
            this.Map.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow;
            this.Map.LevelsKeepInMemory = 5;
            this.Map.Location = new System.Drawing.Point(0, 0);
            this.Map.MarkersEnabled = true;
            this.Map.MaxZoom = 2;
            this.Map.MinZoom = 2;
            this.Map.MouseWheelZoomEnabled = false;
            this.Map.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            this.Map.Name = "Map";
            this.Map.NegativeMode = false;
            this.Map.PolygonsEnabled = true;
            this.Map.RetryLoadTile = 0;
            this.Map.RoutesEnabled = true;
            this.Map.ScaleMode = GMap.NET.WindowsForms.ScaleModes.Fractional;
            this.Map.SelectedAreaFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(65)))), ((int)(((byte)(105)))), ((int)(((byte)(225)))));
            this.Map.ShowTileGridLines = false;
            this.Map.Size = new System.Drawing.Size(1129, 778);
            this.Map.TabIndex = 2;
            this.Map.Zoom = 0D;
            // 
            // PathOffset
            // 
            this.PathOffset.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.PathOffset.Location = new System.Drawing.Point(0, 778);
            this.PathOffset.Maximum = 100;
            this.PathOffset.Name = "PathOffset";
            this.PathOffset.Size = new System.Drawing.Size(1129, 90);
            this.PathOffset.TabIndex = 1;
            this.PathOffset.Scroll += new System.EventHandler(this.PathOffset_Scroll);
            // 
            // gMapComponent1
            // 
            this.gMapComponent1.Bearing = 0F;
            this.gMapComponent1.CanDragMap = true;
            this.gMapComponent1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gMapComponent1.EmptyTileColor = System.Drawing.Color.Navy;
            this.gMapComponent1.GrayScaleMode = false;
            this.gMapComponent1.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow;
            this.gMapComponent1.LevelsKeepInMemory = 5;
            this.gMapComponent1.Location = new System.Drawing.Point(0, 0);
            this.gMapComponent1.MarkersEnabled = true;
            this.gMapComponent1.MaxZoom = 2;
            this.gMapComponent1.MinZoom = 2;
            this.gMapComponent1.MouseWheelZoomEnabled = true;
            this.gMapComponent1.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            this.gMapComponent1.Name = "gMapComponent1";
            this.gMapComponent1.NegativeMode = false;
            this.gMapComponent1.PolygonsEnabled = true;
            this.gMapComponent1.RetryLoadTile = 0;
            this.gMapComponent1.RoutesEnabled = true;
            this.gMapComponent1.ScaleMode = GMap.NET.WindowsForms.ScaleModes.Integer;
            this.gMapComponent1.SelectedAreaFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(65)))), ((int)(((byte)(105)))), ((int)(((byte)(225)))));
            this.gMapComponent1.ShowTileGridLines = false;
            this.gMapComponent1.Size = new System.Drawing.Size(1129, 868);
            this.gMapComponent1.TabIndex = 0;
            this.gMapComponent1.Zoom = 0D;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(6, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.LoadGPX);
            this.splitContainer2.Panel1.Controls.Add(this.inject);
            this.splitContainer2.Panel1.Controls.Add(this.AddTWD);
            this.splitContainer2.Panel1.Controls.Add(this.RemoveTWD);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.TWDTable);
            this.splitContainer2.Size = new System.Drawing.Size(557, 868);
            this.splitContainer2.SplitterDistance = 400;
            this.splitContainer2.TabIndex = 7;
            // 
            // Time
            // 
            this.Time.HeaderText = "Time";
            this.Time.MinimumWidth = 10;
            this.Time.Name = "Time";
            this.Time.Width = 200;
            // 
            // TWD
            // 
            this.TWD.HeaderText = "TWD";
            this.TWD.MinimumWidth = 10;
            this.TWD.Name = "TWD";
            this.TWD.Width = 200;
            // 
            // GPXLoader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1696, 868);
            this.Controls.Add(this.splitContainer1);
            this.Name = "GPXLoader";
            this.Text = "GPXLoader";
            this.Load += new System.EventHandler(this.GPXLoader_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.TWDTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PathOffset)).EndInit();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private GMapComponent gMapComponent1;
        private System.Windows.Forms.Button LoadGPX;
        private System.Windows.Forms.TrackBar PathOffset;
        private System.Windows.Forms.Button RemoveTWD;
        private System.Windows.Forms.Button AddTWD;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.OpenFileDialog openGPXDialog;
        private System.Windows.Forms.Button inject;
        private GMapComponent Map;
        private System.Windows.Forms.DataGridView TWDTable;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Time;
        private System.Windows.Forms.DataGridViewTextBoxColumn TWD;
    }
}