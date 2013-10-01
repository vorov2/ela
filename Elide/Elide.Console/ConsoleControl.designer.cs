using Elide.Forms;
namespace Elide.Console
{
	partial class ConsoleControl
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
            this.panel = new Elide.Forms.SingleBorderPanel();
            this.cout = new Elide.Console.ConsoleTextBox();
            this.panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel
            // 
            this.panel.BottomBorder = false;
            this.panel.Controls.Add(this.cout);
            this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel.LeftBorder = false;
            this.panel.Location = new System.Drawing.Point(0, 0);
            this.panel.Name = "panel";
            this.panel.Padding = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.panel.RightBorder = false;
            this.panel.Size = new System.Drawing.Size(639, 333);
            this.panel.TabIndex = 0;
            // 
            // cout
            // 
            this.cout.CaretStyle = Elide.Scintilla.ObjectModel.CaretStyle.None;
            this.cout.CaretWidth = Elide.Scintilla.ObjectModel.CaretWidth.None;
            this.cout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cout.HistorySize = 100;
            this.cout.Location = new System.Drawing.Point(0, 1);
            this.cout.Name = "cout";
            this.cout.Size = new System.Drawing.Size(639, 332);
            this.cout.Styling = false;
            this.cout.TabIndex = 0;
            this.cout.Text = "";
            // 
            // ConsoleControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel);
            this.Name = "ConsoleControl";
            this.Size = new System.Drawing.Size(639, 333);
            this.panel.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

        private SingleBorderPanel panel;
        private ConsoleTextBox cout;


    }
}