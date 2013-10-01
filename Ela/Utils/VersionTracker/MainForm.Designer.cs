namespace VersionTracker
{
    partial class MainForm
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
			this.menuStrip = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.newChangeListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openChangeListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.viedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.allItemsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.currentMinorRevisionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.currentMajorRevisionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.currentMinorVersionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.currentMajorVersionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.statusStrip = new System.Windows.Forms.StatusStrip();
			this.viewChangeLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.viewNewLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.viewFixLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.viewSep = new System.Windows.Forms.ToolStripSeparator();
			this.viewModeLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.incrementMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.incrementThirdRevisionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.incrementSecondRevisionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.incrementFirstRevisionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.revertChangeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.grid = new VersionTracker.BufferedDataGrid();
			this.VersionColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.TypeColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
			this.DescriptionColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.menuStrip.SuspendLayout();
			this.statusStrip.SuspendLayout();
			this.incrementMenu.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
			this.SuspendLayout();
			// 
			// menuStrip
			// 
			this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.viedToolStripMenuItem});
			this.menuStrip.Location = new System.Drawing.Point(0, 0);
			this.menuStrip.Name = "menuStrip";
			this.menuStrip.Size = new System.Drawing.Size(917, 24);
			this.menuStrip.TabIndex = 0;
			this.menuStrip.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newChangeListToolStripMenuItem,
            this.openChangeListToolStripMenuItem,
            this.toolStripMenuItem1,
            this.exitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
			this.fileToolStripMenuItem.Text = "&File";
			// 
			// newChangeListToolStripMenuItem
			// 
			this.newChangeListToolStripMenuItem.Name = "newChangeListToolStripMenuItem";
			this.newChangeListToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
			this.newChangeListToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
			this.newChangeListToolStripMenuItem.Text = "&New Change List";
			this.newChangeListToolStripMenuItem.Click += new System.EventHandler(this.newChangeListToolStripMenuItem_Click);
			// 
			// openChangeListToolStripMenuItem
			// 
			this.openChangeListToolStripMenuItem.Name = "openChangeListToolStripMenuItem";
			this.openChangeListToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.openChangeListToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
			this.openChangeListToolStripMenuItem.Text = "&Open Change List";
			this.openChangeListToolStripMenuItem.Click += new System.EventHandler(this.openChangeListToolStripMenuItem_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(207, 6);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
			this.exitToolStripMenuItem.Text = "E&xit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
			// 
			// viedToolStripMenuItem
			// 
			this.viedToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.allItemsToolStripMenuItem,
            this.currentMinorRevisionToolStripMenuItem,
            this.currentMajorRevisionToolStripMenuItem,
            this.currentMinorVersionToolStripMenuItem,
            this.currentMajorVersionToolStripMenuItem});
			this.viedToolStripMenuItem.Name = "viedToolStripMenuItem";
			this.viedToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
			this.viedToolStripMenuItem.Text = "&View";
			this.viedToolStripMenuItem.DropDownOpening += new System.EventHandler(this.viedToolStripMenuItem_DropDownOpening);
			// 
			// allItemsToolStripMenuItem
			// 
			this.allItemsToolStripMenuItem.Name = "allItemsToolStripMenuItem";
			this.allItemsToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
			this.allItemsToolStripMenuItem.Text = "&All items";
			this.allItemsToolStripMenuItem.Click += new System.EventHandler(this.allItemsToolStripMenuItem_Click);
			// 
			// currentMinorRevisionToolStripMenuItem
			// 
			this.currentMinorRevisionToolStripMenuItem.Name = "currentMinorRevisionToolStripMenuItem";
			this.currentMinorRevisionToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
			this.currentMinorRevisionToolStripMenuItem.Text = "Current minor &revision";
			this.currentMinorRevisionToolStripMenuItem.Click += new System.EventHandler(this.currentMinorRevisionToolStripMenuItem_Click);
			// 
			// currentMajorRevisionToolStripMenuItem
			// 
			this.currentMajorRevisionToolStripMenuItem.Name = "currentMajorRevisionToolStripMenuItem";
			this.currentMajorRevisionToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
			this.currentMajorRevisionToolStripMenuItem.Text = "Current &major revision";
			this.currentMajorRevisionToolStripMenuItem.Click += new System.EventHandler(this.currentMajorRevisionToolStripMenuItem_Click);
			// 
			// currentMinorVersionToolStripMenuItem
			// 
			this.currentMinorVersionToolStripMenuItem.Name = "currentMinorVersionToolStripMenuItem";
			this.currentMinorVersionToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
			this.currentMinorVersionToolStripMenuItem.Text = "Current minor &version";
			this.currentMinorVersionToolStripMenuItem.Click += new System.EventHandler(this.currentMinorVersionToolStripMenuItem_Click);
			// 
			// currentMajorVersionToolStripMenuItem
			// 
			this.currentMajorVersionToolStripMenuItem.Name = "currentMajorVersionToolStripMenuItem";
			this.currentMajorVersionToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
			this.currentMajorVersionToolStripMenuItem.Text = "Current ma&jor version";
			this.currentMajorVersionToolStripMenuItem.Click += new System.EventHandler(this.currentMajorVersionToolStripMenuItem_Click);
			// 
			// statusStrip
			// 
			this.statusStrip.BackColor = System.Drawing.SystemColors.Control;
			this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewChangeLabel,
            this.viewNewLabel,
            this.viewFixLabel,
            this.viewSep,
            this.viewModeLabel});
			this.statusStrip.Location = new System.Drawing.Point(0, 551);
			this.statusStrip.Name = "statusStrip";
			this.statusStrip.Size = new System.Drawing.Size(917, 23);
			this.statusStrip.TabIndex = 1;
			this.statusStrip.Text = "statusStrip1";
			// 
			// viewChangeLabel
			// 
			this.viewChangeLabel.AutoSize = false;
			this.viewChangeLabel.BackColor = System.Drawing.SystemColors.Control;
			this.viewChangeLabel.Name = "viewChangeLabel";
			this.viewChangeLabel.Size = new System.Drawing.Size(120, 18);
			this.viewChangeLabel.Text = "Changes: {0}";
			this.viewChangeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// viewNewLabel
			// 
			this.viewNewLabel.AutoSize = false;
			this.viewNewLabel.BackColor = System.Drawing.SystemColors.Control;
			this.viewNewLabel.Name = "viewNewLabel";
			this.viewNewLabel.Size = new System.Drawing.Size(90, 18);
			this.viewNewLabel.Text = "New: {0}";
			this.viewNewLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// viewFixLabel
			// 
			this.viewFixLabel.AutoSize = false;
			this.viewFixLabel.BackColor = System.Drawing.SystemColors.Control;
			this.viewFixLabel.Name = "viewFixLabel";
			this.viewFixLabel.Size = new System.Drawing.Size(100, 18);
			this.viewFixLabel.Text = "Fixes: {0}";
			this.viewFixLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// viewSep
			// 
			this.viewSep.Name = "viewSep";
			this.viewSep.Size = new System.Drawing.Size(6, 23);
			// 
			// viewModeLabel
			// 
			this.viewModeLabel.AutoSize = false;
			this.viewModeLabel.BackColor = System.Drawing.SystemColors.Control;
			this.viewModeLabel.Margin = new System.Windows.Forms.Padding(10, 3, 0, 2);
			this.viewModeLabel.Name = "viewModeLabel";
			this.viewModeLabel.Size = new System.Drawing.Size(576, 18);
			this.viewModeLabel.Spring = true;
			this.viewModeLabel.Text = "View: {0}";
			this.viewModeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// openFileDialog
			// 
			this.openFileDialog.Filter = "All files (*.*)|*.*|Change lists (*.txt)|*.txt";
			this.openFileDialog.FilterIndex = 2;
			this.openFileDialog.Title = "Open Change List";
			// 
			// incrementMenu
			// 
			this.incrementMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.incrementThirdRevisionToolStripMenuItem,
            this.incrementSecondRevisionToolStripMenuItem,
            this.incrementFirstRevisionToolStripMenuItem,
            this.toolStripMenuItem2,
            this.revertChangeToolStripMenuItem});
			this.incrementMenu.Name = "incrementMenu";
			this.incrementMenu.Size = new System.Drawing.Size(208, 98);
			this.incrementMenu.Opening += new System.ComponentModel.CancelEventHandler(this.incrementMenu_Opening);
			// 
			// incrementThirdRevisionToolStripMenuItem
			// 
			this.incrementThirdRevisionToolStripMenuItem.Name = "incrementThirdRevisionToolStripMenuItem";
			this.incrementThirdRevisionToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
			this.incrementThirdRevisionToolStripMenuItem.Tag = "3";
			this.incrementThirdRevisionToolStripMenuItem.Text = "Increment &Major Revision";
			this.incrementThirdRevisionToolStripMenuItem.Click += new System.EventHandler(this.incrementRevisionToolStripMenuItem_Click);
			// 
			// incrementSecondRevisionToolStripMenuItem
			// 
			this.incrementSecondRevisionToolStripMenuItem.Name = "incrementSecondRevisionToolStripMenuItem";
			this.incrementSecondRevisionToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
			this.incrementSecondRevisionToolStripMenuItem.Tag = "2";
			this.incrementSecondRevisionToolStripMenuItem.Text = "Increment Minor &Version";
			this.incrementSecondRevisionToolStripMenuItem.Click += new System.EventHandler(this.incrementRevisionToolStripMenuItem_Click);
			// 
			// incrementFirstRevisionToolStripMenuItem
			// 
			this.incrementFirstRevisionToolStripMenuItem.Name = "incrementFirstRevisionToolStripMenuItem";
			this.incrementFirstRevisionToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
			this.incrementFirstRevisionToolStripMenuItem.Tag = "1";
			this.incrementFirstRevisionToolStripMenuItem.Text = "Increment Ma&jor Version";
			this.incrementFirstRevisionToolStripMenuItem.Click += new System.EventHandler(this.incrementRevisionToolStripMenuItem_Click);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(204, 6);
			// 
			// revertChangeToolStripMenuItem
			// 
			this.revertChangeToolStripMenuItem.Name = "revertChangeToolStripMenuItem";
			this.revertChangeToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
			this.revertChangeToolStripMenuItem.Text = "&Revert Change";
			this.revertChangeToolStripMenuItem.Click += new System.EventHandler(this.revertChangeToolStripMenuItem_Click);
			// 
			// grid
			// 
			this.grid.AllowUserToResizeColumns = false;
			this.grid.AllowUserToResizeRows = false;
			this.grid.BackgroundColor = System.Drawing.SystemColors.Window;
			this.grid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.VersionColumn,
            this.TypeColumn,
            this.DescriptionColumn});
			this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grid.Location = new System.Drawing.Point(0, 24);
			this.grid.MultiSelect = false;
			this.grid.Name = "grid";
			this.grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.grid.Size = new System.Drawing.Size(917, 527);
			this.grid.TabIndex = 2;
			this.grid.Visible = false;
			this.grid.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.grid_UserDeletingRow);
			this.grid.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.grid_SortCompare);
			this.grid.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.grid_CellMouseDown);
			this.grid.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.grid_RowPostPaint);
			this.grid.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.grid_DefaultValuesNeeded);
			// 
			// VersionColumn
			// 
			this.VersionColumn.ContextMenuStrip = this.incrementMenu;
			this.VersionColumn.HeaderText = "Version";
			this.VersionColumn.Name = "VersionColumn";
			this.VersionColumn.ReadOnly = true;
			this.VersionColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			// 
			// TypeColumn
			// 
			this.TypeColumn.HeaderText = "Type";
			this.TypeColumn.Items.AddRange(new object[] {
            "New",
            "Fix",
            "Change",
            "Release"});
			this.TypeColumn.Name = "TypeColumn";
			this.TypeColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			// 
			// DescriptionColumn
			// 
			this.DescriptionColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.DescriptionColumn.HeaderText = "Description";
			this.DescriptionColumn.Name = "DescriptionColumn";
			this.DescriptionColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.AppWorkspace;
			this.ClientSize = new System.Drawing.Size(917, 574);
			this.Controls.Add(this.grid);
			this.Controls.Add(this.statusStrip);
			this.Controls.Add(this.menuStrip);
			this.MainMenuStrip = this.menuStrip;
			this.Name = "MainForm";
			this.Text = "Version Tracker";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			this.menuStrip.ResumeLayout(false);
			this.menuStrip.PerformLayout();
			this.statusStrip.ResumeLayout(false);
			this.statusStrip.PerformLayout();
			this.incrementMenu.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private BufferedDataGrid grid;
        private System.Windows.Forms.ToolStripMenuItem newChangeListToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openChangeListToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.ContextMenuStrip incrementMenu;
		private System.Windows.Forms.ToolStripMenuItem incrementThirdRevisionToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem incrementSecondRevisionToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem incrementFirstRevisionToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem revertChangeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem viedToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem allItemsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem currentMinorRevisionToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem currentMajorRevisionToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem currentMinorVersionToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem currentMajorVersionToolStripMenuItem;
		private System.Windows.Forms.ToolStripStatusLabel viewModeLabel;
		private System.Windows.Forms.ToolStripStatusLabel viewNewLabel;
		private System.Windows.Forms.ToolStripStatusLabel viewChangeLabel;
		private System.Windows.Forms.ToolStripStatusLabel viewFixLabel;
		private System.Windows.Forms.ToolStripSeparator viewSep;
		private System.Windows.Forms.DataGridViewTextBoxColumn VersionColumn;
		private System.Windows.Forms.DataGridViewComboBoxColumn TypeColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn DescriptionColumn;
    }
}

