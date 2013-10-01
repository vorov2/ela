namespace Elide.Workbench.Configuration
{
    partial class AddFolderDialog
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
            this.folder = new System.Windows.Forms.Button();
            this.dirInputText = new System.Windows.Forms.TextBox();
            this.pathLabel = new System.Windows.Forms.Label();
            this.maskLabel = new System.Windows.Forms.Label();
            this.maskTextBox = new System.Windows.Forms.TextBox();
            this.nameLabel = new System.Windows.Forms.Label();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.accept = new System.Windows.Forms.Button();
            this.cancel = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // folder
            // 
            this.folder.CausesValidation = false;
            this.folder.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.folder.Location = new System.Drawing.Point(307, 74);
            this.folder.Name = "folder";
            this.folder.Size = new System.Drawing.Size(25, 24);
            this.folder.TabIndex = 2;
            this.folder.Text = "...";
            this.folder.UseVisualStyleBackColor = true;
            this.folder.Click += new System.EventHandler(this.folder_Click);
            // 
            // dirInputText
            // 
            this.dirInputText.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.dirInputText.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories;
            this.dirInputText.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.errorProvider.SetIconPadding(this.dirInputText, -20);
            this.dirInputText.Location = new System.Drawing.Point(15, 75);
            this.dirInputText.Margin = new System.Windows.Forms.Padding(0);
            this.dirInputText.Name = "dirInputText";
            this.dirInputText.Size = new System.Drawing.Size(286, 22);
            this.dirInputText.TabIndex = 1;
            this.dirInputText.WordWrap = false;
            this.dirInputText.TextChanged += new System.EventHandler(this.dirInputText_TextChanged);
            this.dirInputText.Validating += new System.ComponentModel.CancelEventHandler(this.dirInputText_Validating);
            // 
            // pathLabel
            // 
            this.pathLabel.AutoSize = true;
            this.pathLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.pathLabel.Location = new System.Drawing.Point(12, 58);
            this.pathLabel.Name = "pathLabel";
            this.pathLabel.Size = new System.Drawing.Size(72, 13);
            this.pathLabel.TabIndex = 0;
            this.pathLabel.Text = "System &path:";
            // 
            // maskLabel
            // 
            this.maskLabel.AutoSize = true;
            this.maskLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.maskLabel.Location = new System.Drawing.Point(12, 107);
            this.maskLabel.Name = "maskLabel";
            this.maskLabel.Size = new System.Drawing.Size(37, 13);
            this.maskLabel.TabIndex = 3;
            this.maskLabel.Text = "&Mask:";
            // 
            // maskTextBox
            // 
            this.maskTextBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.maskTextBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories;
            this.maskTextBox.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.maskTextBox.Location = new System.Drawing.Point(15, 124);
            this.maskTextBox.Margin = new System.Windows.Forms.Padding(0);
            this.maskTextBox.Name = "maskTextBox";
            this.maskTextBox.Size = new System.Drawing.Size(317, 22);
            this.maskTextBox.TabIndex = 4;
            this.maskTextBox.Text = "*.*";
            this.maskTextBox.WordWrap = false;
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.nameLabel.Location = new System.Drawing.Point(12, 9);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(78, 13);
            this.nameLabel.TabIndex = 5;
            this.nameLabel.Text = "&Display name:";
            // 
            // nameTextBox
            // 
            this.nameTextBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.nameTextBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories;
            this.nameTextBox.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.nameTextBox.Location = new System.Drawing.Point(15, 26);
            this.nameTextBox.Margin = new System.Windows.Forms.Padding(0);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(317, 22);
            this.nameTextBox.TabIndex = 6;
            this.nameTextBox.WordWrap = false;
            // 
            // accept
            // 
            this.accept.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.accept.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.accept.Location = new System.Drawing.Point(176, 165);
            this.accept.Name = "accept";
            this.accept.Size = new System.Drawing.Size(75, 23);
            this.accept.TabIndex = 7;
            this.accept.Text = "OK";
            this.accept.UseVisualStyleBackColor = true;
            // 
            // cancel
            // 
            this.cancel.CausesValidation = false;
            this.cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancel.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.cancel.Location = new System.Drawing.Point(257, 165);
            this.cancel.Name = "cancel";
            this.cancel.Size = new System.Drawing.Size(75, 23);
            this.cancel.TabIndex = 8;
            this.cancel.Text = "Cancel";
            this.cancel.UseVisualStyleBackColor = true;
            // 
            // errorProvider
            // 
            this.errorProvider.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.AlwaysBlink;
            this.errorProvider.ContainerControl = this;
            // 
            // AddFolderDialog
            // 
            this.AcceptButton = this.accept;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancel;
            this.ClientSize = new System.Drawing.Size(349, 199);
            this.Controls.Add(this.cancel);
            this.Controls.Add(this.accept);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.maskLabel);
            this.Controls.Add(this.maskTextBox);
            this.Controls.Add(this.pathLabel);
            this.Controls.Add(this.folder);
            this.Controls.Add(this.dirInputText);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddFolderDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Favourite Folder";
            this.Load += new System.EventHandler(this.AddFolderDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button folder;
        private System.Windows.Forms.TextBox dirInputText;
        private System.Windows.Forms.Label pathLabel;
        private System.Windows.Forms.Label maskLabel;
        private System.Windows.Forms.TextBox maskTextBox;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.Button accept;
        private System.Windows.Forms.Button cancel;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}