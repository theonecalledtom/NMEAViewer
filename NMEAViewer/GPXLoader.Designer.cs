
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
            this.RemoveTWD = new System.Windows.Forms.Button();
            this.AddTWD = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.TWD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.LoadGPX = new System.Windows.Forms.Button();
            this.PathOffset = new System.Windows.Forms.TrackBar();
            this.gMapComponent1 = new NMEAViewer.GMapComponent(this.components);
            this.openGPXDialog = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PathOffset)).BeginInit();
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
            this.splitContainer1.Panel1.Controls.Add(this.RemoveTWD);
            this.splitContainer1.Panel1.Controls.Add(this.AddTWD);
            this.splitContainer1.Panel1.Controls.Add(this.dataGridView1);
            this.splitContainer1.Panel1.Controls.Add(this.splitter2);
            this.splitContainer1.Panel1.Controls.Add(this.splitter1);
            this.splitContainer1.Panel1.Controls.Add(this.LoadGPX);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.PathOffset);
            this.splitContainer1.Panel2.Controls.Add(this.gMapComponent1);
            this.splitContainer1.Size = new System.Drawing.Size(1400, 705);
            this.splitContainer1.SplitterDistance = 465;
            this.splitContainer1.TabIndex = 0;
            // 
            // RemoveTWD
            // 
            this.RemoveTWD.Location = new System.Drawing.Point(37, 192);
            this.RemoveTWD.Name = "RemoveTWD";
            this.RemoveTWD.Size = new System.Drawing.Size(212, 51);
            this.RemoveTWD.TabIndex = 5;
            this.RemoveTWD.Text = "Remove TWD";
            this.RemoveTWD.UseVisualStyleBackColor = true;
            // 
            // AddTWD
            // 
            this.AddTWD.Location = new System.Drawing.Point(37, 105);
            this.AddTWD.Name = "AddTWD";
            this.AddTWD.Size = new System.Drawing.Size(212, 51);
            this.AddTWD.TabIndex = 4;
            this.AddTWD.Text = "Add TWD";
            this.AddTWD.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TWD,
            this.Time});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dataGridView1.Location = new System.Drawing.Point(6, 398);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 82;
            this.dataGridView1.RowTemplate.Height = 33;
            this.dataGridView1.Size = new System.Drawing.Size(459, 307);
            this.dataGridView1.TabIndex = 3;
            // 
            // TWD
            // 
            this.TWD.HeaderText = "TWD";
            this.TWD.MinimumWidth = 10;
            this.TWD.Name = "TWD";
            this.TWD.Width = 200;
            // 
            // Time
            // 
            this.Time.HeaderText = "Time";
            this.Time.MinimumWidth = 10;
            this.Time.Name = "Time";
            this.Time.Width = 200;
            // 
            // splitter2
            // 
            this.splitter2.Location = new System.Drawing.Point(3, 0);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(3, 705);
            this.splitter2.TabIndex = 2;
            this.splitter2.TabStop = false;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(0, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 705);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            // 
            // LoadGPX
            // 
            this.LoadGPX.Location = new System.Drawing.Point(37, 23);
            this.LoadGPX.Name = "LoadGPX";
            this.LoadGPX.Size = new System.Drawing.Size(212, 51);
            this.LoadGPX.TabIndex = 0;
            this.LoadGPX.Text = "Load GPX";
            this.LoadGPX.UseVisualStyleBackColor = true;
            // 
            // PathOffset
            // 
            this.PathOffset.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.PathOffset.Location = new System.Drawing.Point(0, 615);
            this.PathOffset.Maximum = 100;
            this.PathOffset.Name = "PathOffset";
            this.PathOffset.Size = new System.Drawing.Size(931, 90);
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
            this.gMapComponent1.Size = new System.Drawing.Size(931, 705);
            this.gMapComponent1.TabIndex = 0;
            this.gMapComponent1.Zoom = 0D;
            // 
            // GPXLoader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1400, 705);
            this.Controls.Add(this.splitContainer1);
            this.Name = "GPXLoader";
            this.Text = "GPXLoader";
            this.Load += new System.EventHandler(this.GPXLoader_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PathOffset)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private GMapComponent gMapComponent1;
        private System.Windows.Forms.Button LoadGPX;
        private System.Windows.Forms.TrackBar PathOffset;
        private System.Windows.Forms.Button RemoveTWD;
        private System.Windows.Forms.Button AddTWD;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn TWD;
        private System.Windows.Forms.DataGridViewTextBoxColumn Time;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.OpenFileDialog openGPXDialog;
    }
}