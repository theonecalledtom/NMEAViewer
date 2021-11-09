namespace NMEAViewer
{
    partial class MapWindow
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
            this.gMapControl1 = new GMap.NET.WindowsForms.GMapControl();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.locationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.centerOnMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.trackCurrentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mapTypeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vS2015DarkTheme1 = new WeifenLuo.WinFormsUI.Docking.VS2015DarkTheme();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gMapControl1
            // 
            this.gMapControl1.Bearing = 0F;
            this.gMapControl1.CanDragMap = true;
            this.gMapControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gMapControl1.EmptyTileColor = System.Drawing.Color.Navy;
            this.gMapControl1.GrayScaleMode = false;
            this.gMapControl1.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow;
            this.gMapControl1.LevelsKeepInMemory = 5;
            this.gMapControl1.Location = new System.Drawing.Point(0, 0);
            this.gMapControl1.Margin = new System.Windows.Forms.Padding(4);
            this.gMapControl1.MarkersEnabled = true;
            this.gMapControl1.MaxZoom = 2;
            this.gMapControl1.MinZoom = 2;
            this.gMapControl1.MouseWheelZoomEnabled = true;
            this.gMapControl1.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            this.gMapControl1.Name = "gMapControl1";
            this.gMapControl1.NegativeMode = false;
            this.gMapControl1.PolygonsEnabled = true;
            this.gMapControl1.RetryLoadTile = 0;
            this.gMapControl1.RoutesEnabled = true;
            this.gMapControl1.ScaleMode = GMap.NET.WindowsForms.ScaleModes.Fractional;
            this.gMapControl1.SelectedAreaFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(65)))), ((int)(((byte)(105)))), ((int)(((byte)(225)))));
            this.gMapControl1.ShowTileGridLines = false;
            this.gMapControl1.Size = new System.Drawing.Size(464, 342);
            this.gMapControl1.TabIndex = 0;
            this.gMapControl1.Zoom = 0D;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.menuStrip1.Font = new System.Drawing.Font("Segoe UI", 7.000003F);
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.locationToolStripMenuItem,
            this.mapTypeToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 342);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(8, 4, 0, 4);
            this.menuStrip1.Size = new System.Drawing.Size(464, 48);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // locationToolStripMenuItem
            // 
            this.locationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.centerOnMapToolStripMenuItem,
            this.trackCurrentToolStripMenuItem,
            this.selectionToolStripMenuItem});
            this.locationToolStripMenuItem.Name = "locationToolStripMenuItem";
            this.locationToolStripMenuItem.Size = new System.Drawing.Size(104, 40);
            this.locationToolStripMenuItem.Text = "Location";
            // 
            // centerOnMapToolStripMenuItem
            // 
            this.centerOnMapToolStripMenuItem.Name = "centerOnMapToolStripMenuItem";
            this.centerOnMapToolStripMenuItem.Size = new System.Drawing.Size(276, 44);
            this.centerOnMapToolStripMenuItem.Text = "Center on route";
            this.centerOnMapToolStripMenuItem.Click += new System.EventHandler(this.centerOnMapToolStripMenuItem_Click);
            // 
            // trackCurrentToolStripMenuItem
            // 
            this.trackCurrentToolStripMenuItem.Checked = true;
            this.trackCurrentToolStripMenuItem.CheckOnClick = true;
            this.trackCurrentToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.trackCurrentToolStripMenuItem.Name = "trackCurrentToolStripMenuItem";
            this.trackCurrentToolStripMenuItem.Size = new System.Drawing.Size(276, 44);
            this.trackCurrentToolStripMenuItem.Text = "TrackCurrent";
            // 
            // selectionToolStripMenuItem
            // 
            this.selectionToolStripMenuItem.Name = "selectionToolStripMenuItem";
            this.selectionToolStripMenuItem.Size = new System.Drawing.Size(276, 44);
            this.selectionToolStripMenuItem.Text = "Selection";
            this.selectionToolStripMenuItem.Click += new System.EventHandler(this.selectionToolStripMenuItem_Click);
            // 
            // mapTypeToolStripMenuItem
            // 
            this.mapTypeToolStripMenuItem.Name = "mapTypeToolStripMenuItem";
            this.mapTypeToolStripMenuItem.Size = new System.Drawing.Size(109, 40);
            this.mapTypeToolStripMenuItem.Text = "MapType";
            this.mapTypeToolStripMenuItem.Click += new System.EventHandler(this.mapTypeToolStripMenuItem_Click);
            // 
            // MapWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(464, 390);
            this.Controls.Add(this.gMapControl1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MapWindow";
            this.Text = "MapWindow";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GMap.NET.WindowsForms.GMapControl gMapControl1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem locationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem centerOnMapToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem trackCurrentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mapTypeToolStripMenuItem;
        private WeifenLuo.WinFormsUI.Docking.VS2015DarkTheme vS2015DarkTheme1;
    }
}