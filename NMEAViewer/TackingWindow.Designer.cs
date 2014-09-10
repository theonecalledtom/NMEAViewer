namespace NMEAViewer
{
    partial class TackingWindow
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.UseSelection = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDown_Downwind = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_Upwind = new System.Windows.Forms.NumericUpDown();
            this.checkBox2_RestrictByAWA = new System.Windows.Forms.CheckBox();
            this.UseSOG = new System.Windows.Forms.CheckBox();
            this.Scan = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.UpwindTab = new System.Windows.Forms.TabPage();
            this.UpwindDataGrid = new System.Windows.Forms.DataGridView();
            this.DownwindTab = new System.Windows.Forms.TabPage();
            this.DownwindDataGrid = new System.Windows.Forms.DataGridView();
            this.DataSelectionTab = new System.Windows.Forms.TabPage();
            this.VisibleColumns = new System.Windows.Forms.CheckedListBox();
            this.RestrictToLegs = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Downwind)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Upwind)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.UpwindTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UpwindDataGrid)).BeginInit();
            this.DownwindTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DownwindDataGrid)).BeginInit();
            this.DataSelectionTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.RestrictToLegs);
            this.splitContainer1.Panel1.Controls.Add(this.UseSelection);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel1.Controls.Add(this.UseSOG);
            this.splitContainer1.Panel1.Controls.Add(this.Scan);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(522, 305);
            this.splitContainer1.SplitterDistance = 150;
            this.splitContainer1.TabIndex = 0;
            // 
            // UseSelection
            // 
            this.UseSelection.AutoSize = true;
            this.UseSelection.Checked = true;
            this.UseSelection.CheckState = System.Windows.Forms.CheckState.Checked;
            this.UseSelection.Location = new System.Drawing.Point(12, 41);
            this.UseSelection.Name = "UseSelection";
            this.UseSelection.Size = new System.Drawing.Size(117, 21);
            this.UseSelection.TabIndex = 3;
            this.UseSelection.Text = "Use Selection";
            this.UseSelection.UseVisualStyleBackColor = true;
            this.UseSelection.CheckedChanged += new System.EventHandler(this.UseSelection_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.numericUpDown_Downwind);
            this.groupBox1.Controls.Add(this.numericUpDown_Upwind);
            this.groupBox1.Controls.Add(this.checkBox2_RestrictByAWA);
            this.groupBox1.Location = new System.Drawing.Point(12, 104);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(124, 110);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "AWA";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 17);
            this.label2.TabIndex = 18;
            this.label2.Text = "MinDown";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 17);
            this.label1.TabIndex = 17;
            this.label1.Text = "MaxUp";
            // 
            // numericUpDown_Downwind
            // 
            this.numericUpDown_Downwind.Location = new System.Drawing.Point(76, 72);
            this.numericUpDown_Downwind.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.numericUpDown_Downwind.Minimum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numericUpDown_Downwind.Name = "numericUpDown_Downwind";
            this.numericUpDown_Downwind.Size = new System.Drawing.Size(45, 22);
            this.numericUpDown_Downwind.TabIndex = 16;
            this.numericUpDown_Downwind.Value = new decimal(new int[] {
            80,
            0,
            0,
            0});
            // 
            // numericUpDown_Upwind
            // 
            this.numericUpDown_Upwind.Location = new System.Drawing.Point(74, 43);
            this.numericUpDown_Upwind.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.numericUpDown_Upwind.Name = "numericUpDown_Upwind";
            this.numericUpDown_Upwind.Size = new System.Drawing.Size(47, 22);
            this.numericUpDown_Upwind.TabIndex = 14;
            this.numericUpDown_Upwind.Value = new decimal(new int[] {
            45,
            0,
            0,
            0});
            // 
            // checkBox2_RestrictByAWA
            // 
            this.checkBox2_RestrictByAWA.AutoSize = true;
            this.checkBox2_RestrictByAWA.Checked = true;
            this.checkBox2_RestrictByAWA.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox2_RestrictByAWA.Location = new System.Drawing.Point(6, 21);
            this.checkBox2_RestrictByAWA.Name = "checkBox2_RestrictByAWA";
            this.checkBox2_RestrictByAWA.Size = new System.Drawing.Size(78, 21);
            this.checkBox2_RestrictByAWA.TabIndex = 12;
            this.checkBox2_RestrictByAWA.Text = "Restrict";
            this.checkBox2_RestrictByAWA.UseMnemonic = false;
            this.checkBox2_RestrictByAWA.UseVisualStyleBackColor = true;
            this.checkBox2_RestrictByAWA.CheckedChanged += new System.EventHandler(this.checkBox2_RestrictByAWA_CheckedChanged);
            // 
            // UseSOG
            // 
            this.UseSOG.AutoSize = true;
            this.UseSOG.Location = new System.Drawing.Point(10, 231);
            this.UseSOG.Name = "UseSOG";
            this.UseSOG.Size = new System.Drawing.Size(86, 21);
            this.UseSOG.TabIndex = 1;
            this.UseSOG.Text = "UseSOG";
            this.UseSOG.UseVisualStyleBackColor = true;
            // 
            // Scan
            // 
            this.Scan.Location = new System.Drawing.Point(36, 12);
            this.Scan.Name = "Scan";
            this.Scan.Size = new System.Drawing.Size(75, 23);
            this.Scan.TabIndex = 0;
            this.Scan.Text = "Scan";
            this.Scan.UseVisualStyleBackColor = true;
            this.Scan.Click += new System.EventHandler(this.Scan_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.UpwindTab);
            this.tabControl1.Controls.Add(this.DownwindTab);
            this.tabControl1.Controls.Add(this.DataSelectionTab);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(368, 305);
            this.tabControl1.TabIndex = 0;
            // 
            // UpwindTab
            // 
            this.UpwindTab.Controls.Add(this.UpwindDataGrid);
            this.UpwindTab.Location = new System.Drawing.Point(4, 25);
            this.UpwindTab.Name = "UpwindTab";
            this.UpwindTab.Padding = new System.Windows.Forms.Padding(3);
            this.UpwindTab.Size = new System.Drawing.Size(360, 276);
            this.UpwindTab.TabIndex = 0;
            this.UpwindTab.Text = "Upwind";
            this.UpwindTab.UseVisualStyleBackColor = true;
            // 
            // UpwindDataGrid
            // 
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle13.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle13.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle13.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle13.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle13.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.UpwindDataGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle13;
            this.UpwindDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle14.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle14.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle14.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.UpwindDataGrid.DefaultCellStyle = dataGridViewCellStyle14;
            this.UpwindDataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UpwindDataGrid.Location = new System.Drawing.Point(3, 3);
            this.UpwindDataGrid.Name = "UpwindDataGrid";
            this.UpwindDataGrid.ReadOnly = true;
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle15.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle15.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle15.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle15.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle15.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle15.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.UpwindDataGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle15;
            this.UpwindDataGrid.RowTemplate.Height = 24;
            this.UpwindDataGrid.Size = new System.Drawing.Size(354, 270);
            this.UpwindDataGrid.TabIndex = 0;
            // 
            // DownwindTab
            // 
            this.DownwindTab.Controls.Add(this.DownwindDataGrid);
            this.DownwindTab.Location = new System.Drawing.Point(4, 25);
            this.DownwindTab.Name = "DownwindTab";
            this.DownwindTab.Padding = new System.Windows.Forms.Padding(3);
            this.DownwindTab.Size = new System.Drawing.Size(360, 276);
            this.DownwindTab.TabIndex = 1;
            this.DownwindTab.Text = "Downwind";
            this.DownwindTab.UseVisualStyleBackColor = true;
            // 
            // DownwindDataGrid
            // 
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle16.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle16.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle16.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle16.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle16.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle16.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DownwindDataGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle16;
            this.DownwindDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle17.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle17.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle17.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle17.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle17.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle17.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DownwindDataGrid.DefaultCellStyle = dataGridViewCellStyle17;
            this.DownwindDataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DownwindDataGrid.Location = new System.Drawing.Point(3, 3);
            this.DownwindDataGrid.Name = "DownwindDataGrid";
            this.DownwindDataGrid.ReadOnly = true;
            dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle18.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle18.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle18.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle18.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle18.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle18.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DownwindDataGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle18;
            this.DownwindDataGrid.RowTemplate.Height = 24;
            this.DownwindDataGrid.Size = new System.Drawing.Size(354, 270);
            this.DownwindDataGrid.TabIndex = 0;
            // 
            // DataSelectionTab
            // 
            this.DataSelectionTab.Controls.Add(this.VisibleColumns);
            this.DataSelectionTab.Location = new System.Drawing.Point(4, 25);
            this.DataSelectionTab.Name = "DataSelectionTab";
            this.DataSelectionTab.Padding = new System.Windows.Forms.Padding(3);
            this.DataSelectionTab.Size = new System.Drawing.Size(360, 276);
            this.DataSelectionTab.TabIndex = 2;
            this.DataSelectionTab.Text = "DataSelection";
            this.DataSelectionTab.UseVisualStyleBackColor = true;
            // 
            // VisibleColumns
            // 
            this.VisibleColumns.Dock = System.Windows.Forms.DockStyle.Fill;
            this.VisibleColumns.FormattingEnabled = true;
            this.VisibleColumns.Location = new System.Drawing.Point(3, 3);
            this.VisibleColumns.Name = "VisibleColumns";
            this.VisibleColumns.Size = new System.Drawing.Size(354, 270);
            this.VisibleColumns.TabIndex = 0;
            // 
            // RestrictToLegs
            // 
            this.RestrictToLegs.AutoSize = true;
            this.RestrictToLegs.Location = new System.Drawing.Point(12, 68);
            this.RestrictToLegs.Name = "RestrictToLegs";
            this.RestrictToLegs.Size = new System.Drawing.Size(124, 21);
            this.RestrictToLegs.TabIndex = 4;
            this.RestrictToLegs.Text = "Restrict to legs";
            this.RestrictToLegs.UseVisualStyleBackColor = true;
            this.RestrictToLegs.CheckedChanged += new System.EventHandler(this.RestrictToLegs_CheckedChanged);
            // 
            // TackingWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(522, 305);
            this.Controls.Add(this.splitContainer1);
            this.Name = "TackingWindow";
            this.Text = "TackingWindow";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Downwind)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_Upwind)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.UpwindTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.UpwindDataGrid)).EndInit();
            this.DownwindTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DownwindDataGrid)).EndInit();
            this.DataSelectionTab.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage UpwindTab;
        private System.Windows.Forms.DataGridView UpwindDataGrid;
        private System.Windows.Forms.TabPage DownwindTab;
        private System.Windows.Forms.DataGridView DownwindDataGrid;
        private System.Windows.Forms.Button Scan;
        private System.Windows.Forms.CheckBox UseSOG;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDown_Downwind;
        private System.Windows.Forms.NumericUpDown numericUpDown_Upwind;
        private System.Windows.Forms.CheckBox checkBox2_RestrictByAWA;
        private System.Windows.Forms.CheckBox UseSelection;
        private System.Windows.Forms.TabPage DataSelectionTab;
        private System.Windows.Forms.CheckedListBox VisibleColumns;
        private System.Windows.Forms.CheckBox RestrictToLegs;
    }
}