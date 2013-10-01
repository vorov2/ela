namespace Elide.Forms
{
	partial class ToolWindow
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
            this.panel = new Elide.Forms.FlatPanel();
            this.switchBar = new Elide.Forms.SwitchBar();
            this.SuspendLayout();
            // 
            // panel
            // 
            this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel.Location = new System.Drawing.Point(0, 20);
            this.panel.Name = "panel";
            this.panel.Padding = new System.Windows.Forms.Padding(2);
            this.panel.Size = new System.Drawing.Size(257, 299);
            this.panel.TabIndex = 1;
            this.panel.WideRendering = true;
            // 
            // switchBar
            // 
            this.switchBar.BackColor = System.Drawing.Color.DarkGray;
            this.switchBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.switchBar.Location = new System.Drawing.Point(0, 0);
            this.switchBar.Name = "switchBar";
            this.switchBar.SelectedIndex = -1;
            this.switchBar.Size = new System.Drawing.Size(257, 20);
            this.switchBar.TabIndex = 0;
            this.switchBar.SelectedIndexChanged += new System.EventHandler<Elide.Forms.SwitchBarEventArgs>(this.SwitchBarSelectedIndexChanged);
            // 
            // ToolWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel);
            this.Controls.Add(this.switchBar);
            this.Name = "ToolWindow";
            this.Size = new System.Drawing.Size(257, 319);
            this.ResumeLayout(false);

		}

		#endregion

        private SwitchBar switchBar;
		private FlatPanel panel;

	}
}
