using Elide.Forms;
namespace Elide.CodeWorkbench.CodeSamples
{
    partial class SamplesControl
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
            this.split = new SplitContainerEx();
            this.treeView = new BufferedTreeView();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.desc = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.split)).BeginInit();
            this.split.Panel1.SuspendLayout();
            this.split.Panel2.SuspendLayout();
            this.split.SuspendLayout();
            this.SuspendLayout();
            // 
            // split
            // 
            this.split.Dock = System.Windows.Forms.DockStyle.Fill;
            this.split.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.split.Location = new System.Drawing.Point(0, 0);
            this.split.Name = "split";
            this.split.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // split.Panel1
            // 
            this.split.Panel1.Controls.Add(this.treeView);
            // 
            // split.Panel2
            // 
            this.split.Panel2.Controls.Add(this.desc);
            this.split.Size = new System.Drawing.Size(613, 486);
            this.split.SplitterDistance = 403;
            this.split.TabIndex = 0;
            // 
            // treeView
            // 
            this.treeView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.treeView.ImageIndex = 0;
            this.treeView.ImageList = this.imageList;
            this.treeView.ItemHeight = 18;
            this.treeView.Location = new System.Drawing.Point(0, 0);
            this.treeView.Name = "treeView";
            this.treeView.SelectedImageIndex = 0;
            this.treeView.ShowLines = false;
            this.treeView.Size = new System.Drawing.Size(613, 403);
            this.treeView.TabIndex = 0;
            // 
            // imageList
            // 
            this.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageList.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList.TransparentColor = System.Drawing.Color.Magenta;
            // 
            // desc
            // 
            this.desc.BackColor = System.Drawing.SystemColors.Window;
            this.desc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.desc.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.desc.Location = new System.Drawing.Point(0, 0);
            this.desc.Name = "desc";
            this.desc.Padding = new System.Windows.Forms.Padding(2);
            this.desc.Size = new System.Drawing.Size(613, 79);
            this.desc.TabIndex = 0;
            // 
            // SamplesControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.split);
            this.Name = "SamplesControl";
            this.Size = new System.Drawing.Size(613, 486);
            this.Load += new System.EventHandler(this.SamplesControl_Load);
            this.split.Panel1.ResumeLayout(false);
            this.split.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.split)).EndInit();
            this.split.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private SplitContainerEx split;
        private BufferedTreeView treeView;
        private System.Windows.Forms.Label desc;
        private System.Windows.Forms.ImageList imageList;
    }
}
