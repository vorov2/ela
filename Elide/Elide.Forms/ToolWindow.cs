using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Elide.Core;

namespace Elide.Forms
{
	public partial class ToolWindow : UserControl
	{
		public ToolWindow()
		{
			InitializeComponent();
			SetStyle(ControlStyles.OptimizedDoubleBuffer|ControlStyles.AllPaintingInWmPaint|ControlStyles.ResizeRedraw, true);

            panel.BackColor = Color.White;
			panel.Paint += (o,e) =>
				{
					var g = e.Graphics;
                    g.DrawLine(UserPens.Border, 0, 0, 0, panel.ClientSize.Height);
                    g.DrawLine(UserPens.Border, 0, panel.ClientSize.Height - 1, panel.ClientSize.Width, panel.ClientSize.Height - 1);
                    g.DrawLine(UserPens.Border, panel.ClientSize.Width - 1, 0, panel.ClientSize.Width - 1, panel.ClientSize.Height);
				};
            switchBar.SelectedIndexChanged += SwitchBarSelectedIndexChanged;
            panel.SetPadding(new Padding(1, 5, 1, 2));
		}

        protected override void OnPaint(PaintEventArgs e)
		{
			var g = e.Graphics;
			g.DrawRectangle(Pens.DarkGray, 0, switchBar.Height, ClientRectangle.Width - 1, ClientRectangle.Height - switchBar.Height - 1);
		}
        
		private void SwitchBarSelectedIndexChanged(object sender, SwitchBarEventArgs e)
		{
			OnSelectedIndexChanged(e);
		}
        
		public void AddHostedControl(Control control)
		{
            control.Dock = DockStyle.Fill;
            panel.Controls.Add(control);
            control.BringToFront();
		}
        
		public void RemoveHostedControl()
		{
			panel.Controls.Clear();
		}
        
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		public List<SwitchBarItem> Items
		{
			get { return switchBar.Items; }
		}

		public int SelectedIndex
		{
			get { return switchBar.SelectedIndex; }
			set 
            { 
                switchBar.SelectedIndex = value;
                Refresh();
            }
		}
        
		public event EventHandler<SwitchBarEventArgs> SelectedIndexChanged;
		private void OnSelectedIndexChanged(SwitchBarEventArgs e)
		{
			var h = SelectedIndexChanged;

			if (h != null)
				h(this, e);
		}
	}
}
