namespace Elide.Workbench.Views
{
    partial class OutputControl
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
            this.panel = new Elide.Forms.SingleBorderPanel();
            this.SuspendLayout();
            // 
            // panel
            // 
            this.panel.BottomBorder = false;
            this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel.LeftBorder = false;
            this.panel.Location = new System.Drawing.Point(0, 0);
            this.panel.Name = "panel";
            this.panel.Padding = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.panel.RightBorder = false;
            this.panel.Size = new System.Drawing.Size(482, 391);
            this.panel.TabIndex = 0;
            // 
            // OutputView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel);
            this.Name = "OutputView";
            this.Size = new System.Drawing.Size(482, 391);
            this.ResumeLayout(false);

        }

        #endregion

        private Elide.Forms.SingleBorderPanel panel;
    }
}
