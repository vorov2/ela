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
                    g.DrawLine(UserPens.Border, 0, panel.ClientSize.Height - Dpi.ScaleY(1), panel.ClientSize.Width, panel.ClientSize.Height - Dpi.ScaleY(1));
                    g.DrawLine(UserPens.Border, panel.ClientSize.Width - Dpi.ScaleX(1), 0, panel.ClientSize.Width - Dpi.ScaleX(1), panel.ClientSize.Height);
                };
            switchBar.SelectedIndexChanged += SwitchBarSelectedIndexChanged;
            switchBar.CloseRequested += SwitchBarCloseRequested;
            panel.SetPadding(new Padding(Dpi.ScaleX(1), Dpi.ScaleY(5), Dpi.ScaleX(1), Dpi.ScaleX(2)));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.DrawRectangle(Pens.DarkGray, 0, switchBar.Height, ClientRectangle.Width - Dpi.ScaleX(1), ClientRectangle.Height - switchBar.Height - Dpi.ScaleY(1));
        }
        
        private void SwitchBarSelectedIndexChanged(object sender, SwitchBarEventArgs e)
        {
            OnSelectedIndexChanged(e);
        }

        private void SwitchBarCloseRequested(object sender, EventArgs e)
        {
            OnCloseRequested(e);
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

        public event EventHandler CloseRequested;
        private void OnCloseRequested(EventArgs e)
        {
            var h = CloseRequested;

            if (h != null)
                h(this, e);
        }
    }
}
