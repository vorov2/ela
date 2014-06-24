namespace Elide.ElaCode.Views
{
    partial class AstControl
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
            this.components = new System.ComponentModel.Container();
            this.treeView = new Elide.Forms.BufferedTreeView();
            this.workLabel = new System.Windows.Forms.Label();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // treeView
            // 
            this.treeView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.treeView.Location = new System.Drawing.Point(0, 0);
            this.treeView.Name = "treeView";
            this.treeView.Size = new System.Drawing.Size(600, 600);
            this.treeView.TabIndex = 0;
            // 
            // workLabel
            // 
            this.workLabel.AutoSize = true;
            this.workLabel.BackColor = System.Drawing.SystemColors.Window;
            this.workLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.workLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.workLabel.Location = new System.Drawing.Point(3, 3);
            this.workLabel.Name = "workLabel";
            this.workLabel.Size = new System.Drawing.Size(101, 13);
            this.workLabel.TabIndex = 1;
            this.workLabel.Tag = "Wait, while generating AST...";
            this.workLabel.Text = "No data to display";
            // 
            // imageList
            // 
            this.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageList.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList.TransparentColor = System.Drawing.Color.Magenta;
            // 
            // AstControl
            // 
            this.Controls.Add(this.workLabel);
            this.Controls.Add(this.treeView);
            this.Name = "AstControl";
            this.Size = new System.Drawing.Size(600, 600);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Forms.BufferedTreeView treeView;
        private System.Windows.Forms.Label workLabel;
        private System.Windows.Forms.ImageList imageList;
    }
}
