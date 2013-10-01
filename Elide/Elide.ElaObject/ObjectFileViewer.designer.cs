namespace Elide.ElaObject
{
    partial class ObjectFileViewer
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
            this.header = new Elide.ElaObject.ObjectFileHeaderControl();
            this.panel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // header
            // 
            this.header.BackColor = System.Drawing.Color.White;
            this.header.Dock = System.Windows.Forms.DockStyle.Top;
            this.header.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.header.Location = new System.Drawing.Point(0, 0);
            this.header.Name = "header";
            this.header.Size = new System.Drawing.Size(693, 28);
            this.header.TabIndex = 0;
            this.header.Text = "[Text]";
            // 
            // panel
            // 
            this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel.Location = new System.Drawing.Point(0, 28);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(693, 494);
            this.panel.TabIndex = 1;
            // 
            // ObjectFileViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel);
            this.Controls.Add(this.header);
            this.Name = "ObjectFileViewer";
            this.Size = new System.Drawing.Size(693, 522);
            this.Load += new System.EventHandler(this.ObjectFileViewer_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private ObjectFileHeaderControl header;
        private System.Windows.Forms.Panel panel;
    }
}
