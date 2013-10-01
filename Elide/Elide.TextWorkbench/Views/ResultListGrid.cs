using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Elide.Core;
using Elide.Environment.Views;
using Elide.Forms;
using Elide.Forms.State;
using Elide.TextEditor;

namespace Elide.TextWorkbench.Views
{
    public partial class ResultListGrid : StateUserControl
    {
        public ResultListGrid()
        {
            InitializeComponent();

            grid.BackColor = UserColors.Window;
            grid.GridColor = UserColors.Border;
            grid.ColumnHeadersDefaultCellStyle.BackColor = UserColors.Button;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = UserColors.Text;
            grid.ColumnHeadersDefaultCellStyle.Font = Fonts.Menu;
            grid.DefaultCellStyle.Font = Fonts.Text;
        }

        public void AddItems(IEnumerable<ResultItem> items)
        {
            items.ForEachIndex((e,i) =>
            {
                var ri = grid.Rows.Add(i + 1, String.Format("{0} ({1},{2})", e.Document.Title, e.Line + 1, e.Column + 1), e.Text);
                grid.Rows[ri].Tag = (Action)(() =>
                    {
                        var svc = App.GetService<IDocumentNavigatorService>();
                        svc.Navigate(e.Document, e.Line, e.Column, e.Length, true);
                    });
            });
        }

        public void ClearItems()
        {
            grid.Rows.Clear();
        }

        private void grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var row = e.RowIndex != -1 ? grid.Rows[e.RowIndex] : null;

            if (row != null && row.Tag != null)
            {
                var fun = row.Tag as Action;
                fun();
            }
        }

        public int ItemsCount
        {
            get { return grid.Rows.Count; }
        }

        public IApp App { get; set; }
        
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
