using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using Elide.Core;
using Elide.Environment.Views;
using Elide.Forms;
using Elide.Forms.State;
using Elide.TextEditor;
using System.Threading;
using Ela.Runtime;
using Ela.Runtime.ObjectModel;

namespace Elide.ElaCode.Views
{
    public partial class DebugViewGrid : StateUserControl
    {
        static readonly object stub = new Object();

        public DebugViewGrid()
        {
            InitializeComponent();

            grid.BackColor = UserColors.Window;
            grid.GridColor = UserColors.Border;
            grid.ColumnHeadersDefaultCellStyle.BackColor = UserColors.Button;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = UserColors.Text;
            grid.ColumnHeadersDefaultCellStyle.Font = Fonts.Menu;
            grid.DefaultCellStyle.Font = Fonts.Text;
        }

        public void AddItems(IEnumerable<TraceItem> items)
        {
            Application.OpenForms[0].Invoke(() =>
                {
                    items.ForEach(e =>
                    {
                        var ri = grid.Rows.Add(
                            String.Format("{0} ({1},{2})", e.TracePointName, e.LineInfo.Line, e.LineInfo.Column),
                            e.Var.Name,
                            PrintValue(e.Var.Value)
                            );
                        grid.Rows[ri].Tag = e.Var.Local ? stub : null;
                    });
                    RefreshView();
                });
        }

        public void RefreshView()
        {
            grid.Rows.OfType<DataGridViewRow>().ForEach(r => r.Visible = IsAllowed(r));
            grid.Refresh();
        }

        private bool IsAllowed(DataGridViewRow r)
        {
            return Show == TraceShowFlag.All ||
                (Show == TraceShowFlag.Locals && r.Tag != null) ||
                (Show == TraceShowFlag.Captured && r.Tag == null);
        }

        private string PrintValue(ElaValue value)
        {
            try
            {
                var result = String.Empty;
                var hdl = new AutoResetEvent(false);
                var th = new Thread(() =>
                {
                    var obj = value.AsObject();

                    if (obj is ElaLazy)
                        result = ((ElaLazy)obj).Force().AsObject().ToString();
                    else if (obj is ElaLazyList)
                    {
                        var lalist = (ElaLazyList)obj;
                        result = ElaList.FromEnumerable(lalist.Take(20)).ToString() + " (lazy)";
                    }
                    else
                        result = value.ToString();

                    hdl.Set();
                });
                th.Start();

                if (!hdl.WaitOne(500))
                {
                    th.Abort();
                    result = "<evaluation timeout>";
                }

                if (result == null)
                    return "_|_";
                else if (result.Trim().Length == 0)
                    result = "[" + value.GetTypeName() + "]";

                return result;
            }
            catch (Exception)
            {
                return "<evaluation error>";
            }
        }

        public void ClearItems()
        {
            Application.OpenForms[0].Invoke(grid.Rows.Clear);
        }
        
        public int ItemsCount
        {
            get { return grid.Rows.Count; }
        }

        public IApp App { get; set; }

        private TraceShowFlag _show;
        [StateItem]
        public new TraceShowFlag Show
        {
            get { return _show; }
            set
            {
                _show = value;
                RefreshView();
            }
        }
        
        #region State
        [StateItem]
        public int FirstColumnWidth
        {
            get { return grid.Columns[0].Width; }
            set { grid.Columns[0].Width = value; }
        }

        [StateItem]
        public int SecondColumnWidth
        {
            get { return grid.Columns[1].Width; }
            set { grid.Columns[1].Width = value; }
        }

        [StateItem]
        public int ThirdColumnWidth
        {
            get { return grid.Columns[2].Width; }
            set { grid.Columns[2].Width = value; }
        }
        #endregion
    }
}
