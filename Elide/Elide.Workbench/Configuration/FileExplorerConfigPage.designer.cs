namespace Elide.Workbench.Configuration
{
    partial class FileExplorerConfigPage
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dirFirst = new System.Windows.Forms.CheckBox();
            this.sortAsc = new System.Windows.Forms.CheckBox();
            this.sortingSettings = new System.Windows.Forms.Label();
            this.favorites = new System.Windows.Forms.Label();
            this.remove = new System.Windows.Forms.Button();
            this.add = new System.Windows.Forms.Button();
            this.dirList = new System.Windows.Forms.ListBox();
            this.edit = new System.Windows.Forms.Button();
            this.hidden = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // dirFirst
            // 
            this.dirFirst.AutoSize = true;
            this.dirFirst.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.dirFirst.Location = new System.Drawing.Point(15, 27);
            this.dirFirst.Name = "dirFirst";
            this.dirFirst.Size = new System.Drawing.Size(171, 17);
            this.dirFirst.TabIndex = 0;
            this.dirFirst.Text = "&Directories always come first";
            this.dirFirst.UseVisualStyleBackColor = true;
            this.dirFirst.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // sortAsc
            // 
            this.sortAsc.AutoSize = true;
            this.sortAsc.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.sortAsc.Location = new System.Drawing.Point(15, 46);
            this.sortAsc.Name = "sortAsc";
            this.sortAsc.Size = new System.Drawing.Size(233, 17);
            this.sortAsc.TabIndex = 1;
            this.sortAsc.Text = "Sort file system items in &ascending order";
            this.sortAsc.UseVisualStyleBackColor = true;
            this.sortAsc.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // sortingSettings
            // 
            this.sortingSettings.AutoSize = true;
            this.sortingSettings.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.sortingSettings.Location = new System.Drawing.Point(12, 10);
            this.sortingSettings.Name = "sortingSettings";
            this.sortingSettings.Size = new System.Drawing.Size(115, 13);
            this.sortingSettings.TabIndex = 3;
            this.sortingSettings.Text = "Sorting and filtering:";
            // 
            // favorites
            // 
            this.favorites.AutoSize = true;
            this.favorites.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.favorites.Location = new System.Drawing.Point(12, 92);
            this.favorites.Name = "favorites";
            this.favorites.Size = new System.Drawing.Size(91, 13);
            this.favorites.TabIndex = 4;
            this.favorites.Text = "Favorite folders:";
            // 
            // remove
            // 
            this.remove.Enabled = false;
            this.remove.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.remove.Location = new System.Drawing.Point(401, 206);
            this.remove.Name = "remove";
            this.remove.Size = new System.Drawing.Size(60, 24);
            this.remove.TabIndex = 14;
            this.remove.Text = "&Remove";
            this.remove.UseVisualStyleBackColor = true;
            this.remove.Click += new System.EventHandler(this.remove_Click);
            // 
            // add
            // 
            this.add.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.add.Location = new System.Drawing.Point(269, 206);
            this.add.Name = "add";
            this.add.Size = new System.Drawing.Size(60, 24);
            this.add.TabIndex = 12;
            this.add.Text = "&Add";
            this.add.UseVisualStyleBackColor = true;
            this.add.Click += new System.EventHandler(this.add_Click);
            // 
            // dirList
            // 
            this.dirList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.dirList.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.dirList.FormattingEnabled = true;
            this.dirList.IntegralHeight = false;
            this.dirList.Location = new System.Drawing.Point(15, 108);
            this.dirList.Name = "dirList";
            this.dirList.Size = new System.Drawing.Size(446, 91);
            this.dirList.TabIndex = 10;
            this.dirList.SelectedIndexChanged += new System.EventHandler(this.dirList_SelectedIndexChanged);
            // 
            // edit
            // 
            this.edit.Enabled = false;
            this.edit.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.edit.Location = new System.Drawing.Point(335, 206);
            this.edit.Name = "edit";
            this.edit.Size = new System.Drawing.Size(60, 24);
            this.edit.TabIndex = 15;
            this.edit.Text = "&Edit";
            this.edit.UseVisualStyleBackColor = true;
            this.edit.Click += new System.EventHandler(this.edit_Click);
            // 
            // hidden
            // 
            this.hidden.AutoSize = true;
            this.hidden.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.hidden.Location = new System.Drawing.Point(15, 65);
            this.hidden.Name = "hidden";
            this.hidden.Size = new System.Drawing.Size(181, 17);
            this.hidden.TabIndex = 16;
            this.hidden.Text = "Show &hidden files and folders";
            this.hidden.UseVisualStyleBackColor = true;
            this.hidden.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // FileExplorerConfigPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.hidden);
            this.Controls.Add(this.edit);
            this.Controls.Add(this.remove);
            this.Controls.Add(this.add);
            this.Controls.Add(this.dirList);
            this.Controls.Add(this.favorites);
            this.Controls.Add(this.sortingSettings);
            this.Controls.Add(this.sortAsc);
            this.Controls.Add(this.dirFirst);
            this.Name = "FileExplorerConfigPage";
            this.Size = new System.Drawing.Size(475, 321);
            this.Load += new System.EventHandler(this.PlainTextConfigPage_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox dirFirst;
        private System.Windows.Forms.CheckBox sortAsc;
        private System.Windows.Forms.Label sortingSettings;
        private System.Windows.Forms.Label favorites;
        private System.Windows.Forms.Button remove;
        private System.Windows.Forms.Button add;
        private System.Windows.Forms.ListBox dirList;
        private System.Windows.Forms.Button edit;
        private System.Windows.Forms.CheckBox hidden;
    }
}
