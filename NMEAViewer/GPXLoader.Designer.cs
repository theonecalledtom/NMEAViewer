
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
            this.gMapComponent1 = new NMEAViewer.GMapComponent(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.gMapComponent1);
            this.splitContainer1.Size = new System.Drawing.Size(800, 450);
            this.splitContainer1.SplitterDistance = 266;
            this.splitContainer1.TabIndex = 0;
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
            this.gMapComponent1.Size = new System.Drawing.Size(530, 450);
            this.gMapComponent1.TabIndex = 0;
            this.gMapComponent1.Zoom = 0D;
            // 
            // GPXLoader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.splitContainer1);
            this.Name = "GPXLoader";
            this.Text = "GPXLoader";
            this.Load += new System.EventHandler(this.GPXLoader_Load);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private GMapComponent gMapComponent1;
    }
}