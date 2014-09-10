namespace NMEAViewer
{
    partial class MetaDataWindow
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.LegsTab = new System.Windows.Forms.TabPage();
            this.LegsDataGrid = new System.Windows.Forms.DataGridView();
            this.EventsTab = new System.Windows.Forms.TabPage();
            this.EventsDataGrid = new System.Windows.Forms.DataGridView();
            this.EventContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toCurrentTimeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LegContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.setStartTimeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setEndTimeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setTimeRangeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.highlightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LegName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StartTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EndTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LegDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TimeOfEvent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EventName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EventDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControl1.SuspendLayout();
            this.LegsTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LegsDataGrid)).BeginInit();
            this.EventsTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.EventsDataGrid)).BeginInit();
            this.EventContextMenuStrip.SuspendLayout();
            this.LegContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.LegsTab);
            this.tabControl1.Controls.Add(this.EventsTab);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(518, 329);
            this.tabControl1.TabIndex = 0;
            // 
            // LegsTab
            // 
            this.LegsTab.Controls.Add(this.LegsDataGrid);
            this.LegsTab.Location = new System.Drawing.Point(4, 25);
            this.LegsTab.Name = "LegsTab";
            this.LegsTab.Padding = new System.Windows.Forms.Padding(3);
            this.LegsTab.Size = new System.Drawing.Size(510, 300);
            this.LegsTab.TabIndex = 0;
            this.LegsTab.Text = "Legs";
            this.LegsTab.UseVisualStyleBackColor = true;
            // 
            // LegsDataGrid
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.LegsDataGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.LegsDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.LegsDataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.LegName,
            this.StartTime,
            this.EndTime,
            this.LegDescription});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.LegsDataGrid.DefaultCellStyle = dataGridViewCellStyle2;
            this.LegsDataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LegsDataGrid.Location = new System.Drawing.Point(3, 3);
            this.LegsDataGrid.Name = "LegsDataGrid";
            this.LegsDataGrid.RowTemplate.Height = 24;
            this.LegsDataGrid.Size = new System.Drawing.Size(504, 294);
            this.LegsDataGrid.TabIndex = 0;
            // 
            // EventsTab
            // 
            this.EventsTab.Controls.Add(this.EventsDataGrid);
            this.EventsTab.Location = new System.Drawing.Point(4, 25);
            this.EventsTab.Name = "EventsTab";
            this.EventsTab.Padding = new System.Windows.Forms.Padding(3);
            this.EventsTab.Size = new System.Drawing.Size(510, 300);
            this.EventsTab.TabIndex = 1;
            this.EventsTab.Text = "Events";
            this.EventsTab.UseVisualStyleBackColor = true;
            // 
            // EventsDataGrid
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.EventsDataGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.EventsDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.EventsDataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TimeOfEvent,
            this.EventName,
            this.EventDescription});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.EventsDataGrid.DefaultCellStyle = dataGridViewCellStyle4;
            this.EventsDataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EventsDataGrid.Location = new System.Drawing.Point(3, 3);
            this.EventsDataGrid.Name = "EventsDataGrid";
            this.EventsDataGrid.RowTemplate.Height = 24;
            this.EventsDataGrid.Size = new System.Drawing.Size(504, 294);
            this.EventsDataGrid.TabIndex = 0;
            // 
            // EventContextMenuStrip
            // 
            this.EventContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toCurrentTimeToolStripMenuItem});
            this.EventContextMenuStrip.Name = "EventContextMenuStrip";
            this.EventContextMenuStrip.Size = new System.Drawing.Size(180, 28);
            // 
            // toCurrentTimeToolStripMenuItem
            // 
            this.toCurrentTimeToolStripMenuItem.Name = "toCurrentTimeToolStripMenuItem";
            this.toCurrentTimeToolStripMenuItem.Size = new System.Drawing.Size(179, 24);
            this.toCurrentTimeToolStripMenuItem.Text = "To current time";
            this.toCurrentTimeToolStripMenuItem.Click += new System.EventHandler(this.toCurrentTimeToolStripMenuItem_Click);
            // 
            // LegContextMenuStrip
            // 
            this.LegContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setStartTimeToolStripMenuItem,
            this.setEndTimeToolStripMenuItem,
            this.setTimeRangeToolStripMenuItem,
            this.highlightToolStripMenuItem});
            this.LegContextMenuStrip.Name = "LegContextMenuStrip";
            this.LegContextMenuStrip.Size = new System.Drawing.Size(176, 100);
            // 
            // setStartTimeToolStripMenuItem
            // 
            this.setStartTimeToolStripMenuItem.Name = "setStartTimeToolStripMenuItem";
            this.setStartTimeToolStripMenuItem.Size = new System.Drawing.Size(175, 24);
            this.setStartTimeToolStripMenuItem.Text = "Set start time";
            this.setStartTimeToolStripMenuItem.Click += new System.EventHandler(this.setStartTimeToolStripMenuItem_Click);
            // 
            // setEndTimeToolStripMenuItem
            // 
            this.setEndTimeToolStripMenuItem.Name = "setEndTimeToolStripMenuItem";
            this.setEndTimeToolStripMenuItem.Size = new System.Drawing.Size(175, 24);
            this.setEndTimeToolStripMenuItem.Text = "Set end time";
            this.setEndTimeToolStripMenuItem.Click += new System.EventHandler(this.setEndTimeToolStripMenuItem_Click);
            // 
            // setTimeRangeToolStripMenuItem
            // 
            this.setTimeRangeToolStripMenuItem.Name = "setTimeRangeToolStripMenuItem";
            this.setTimeRangeToolStripMenuItem.Size = new System.Drawing.Size(175, 24);
            this.setTimeRangeToolStripMenuItem.Text = "Set time range";
            this.setTimeRangeToolStripMenuItem.Click += new System.EventHandler(this.setTimeRangeToolStripMenuItem_Click);
            // 
            // highlightToolStripMenuItem
            // 
            this.highlightToolStripMenuItem.Name = "highlightToolStripMenuItem";
            this.highlightToolStripMenuItem.Size = new System.Drawing.Size(175, 24);
            this.highlightToolStripMenuItem.Text = "Highlight";
            this.highlightToolStripMenuItem.Click += new System.EventHandler(this.highlightToolStripMenuItem_Click);
            // 
            // LegName
            // 
            this.LegName.HeaderText = "LegName";
            this.LegName.Name = "LegName";
            // 
            // StartTime
            // 
            this.StartTime.HeaderText = "StartTime";
            this.StartTime.Name = "StartTime";
            this.StartTime.ReadOnly = true;
            this.StartTime.ToolTipText = "Right click to set";
            // 
            // EndTime
            // 
            this.EndTime.HeaderText = "EndTime";
            this.EndTime.Name = "EndTime";
            this.EndTime.ReadOnly = true;
            this.EndTime.ToolTipText = "Right click to set";
            // 
            // LegDescription
            // 
            this.LegDescription.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.LegDescription.HeaderText = "Description";
            this.LegDescription.Name = "LegDescription";
            // 
            // TimeOfEvent
            // 
            this.TimeOfEvent.HeaderText = "TimeOfEvent";
            this.TimeOfEvent.Name = "TimeOfEvent";
            this.TimeOfEvent.ReadOnly = true;
            this.TimeOfEvent.ToolTipText = "Right click to set";
            // 
            // EventName
            // 
            this.EventName.HeaderText = "EventName";
            this.EventName.Name = "EventName";
            // 
            // EventDescription
            // 
            this.EventDescription.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.EventDescription.HeaderText = "Event Description";
            this.EventDescription.Name = "EventDescription";
            // 
            // MetaDataWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(518, 329);
            this.Controls.Add(this.tabControl1);
            this.Name = "MetaDataWindow";
            this.Text = "MetaDataWindow";
            this.tabControl1.ResumeLayout(false);
            this.LegsTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.LegsDataGrid)).EndInit();
            this.EventsTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.EventsDataGrid)).EndInit();
            this.EventContextMenuStrip.ResumeLayout(false);
            this.LegContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage LegsTab;
        private System.Windows.Forms.DataGridView LegsDataGrid;
        private System.Windows.Forms.TabPage EventsTab;
        private System.Windows.Forms.DataGridView EventsDataGrid;
        private System.Windows.Forms.ContextMenuStrip EventContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem toCurrentTimeToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip LegContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem setStartTimeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setEndTimeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setTimeRangeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem highlightToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn LegName;
        private System.Windows.Forms.DataGridViewTextBoxColumn StartTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn EndTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn LegDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn TimeOfEvent;
        private System.Windows.Forms.DataGridViewTextBoxColumn EventName;
        private System.Windows.Forms.DataGridViewTextBoxColumn EventDescription;

    }
}