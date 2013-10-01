using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Elide.CodeEditor.Views;
using Elide.Core;
using Elide.Forms;
using Elide.Forms.State;
using Elide.TextEditor;

namespace Elide.CodeWorkbench.Views
{
    public partial class ErrorListGrid : StateUserControl
    {
        public ErrorListGrid()
        {
            InitializeComponent();

            grid.BackColor = UserColors.Window;
            grid.GridColor = UserColors.Border;
            grid.ColumnHeadersDefaultCellStyle.BackColor = UserColors.Button;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = UserColors.Text;
            grid.ColumnHeadersDefaultCellStyle.Font = Fonts.Menu;
            grid.DefaultCellStyle.Font = Fonts.Text;

            ShowErrors = true;
            ShowMessages = true;
            ShowWarnings = true;
        }

        public void AddItems(IEnumerable<MessageItem> items)
        {
            items.ForEachIndex((e, i) =>
            {
                var ri = grid.Rows.Add(GetImage(e.Type), i + 1, e.Message, e.Document.Title, e.Line, e.Column);
                grid.Rows[ri].Tag = e;
            });
        }

        public void ClearItems()
        {
            grid.Rows.Clear();
        }

        private Image GetImage(MessageItemType type)
        {
            return type == MessageItemType.Error ? Bitmaps.Load("Error") :
                type == MessageItemType.Warning ? Bitmaps.Load("Warning") :
                Bitmaps.Load("Message");
        }


        private void grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var row = e.RowIndex != -1 ? grid.Rows[e.RowIndex] : null;

            if (row != null && row.Tag != null)
            {
                var msg = row.Tag as MessageItem;

                if (msg != null)
                {
                    var svc = App.GetService<IDocumentNavigatorService>();
                    svc.Navigate(msg.Document, msg.Line - 1, msg.Column - 1, 0, true);
                }
            }
        }

        public void RefreshView()
        {
            grid.Rows.OfType<DataGridViewRow>().ForEach(r => r.Visible = IsAllowed(r));
            grid.Refresh();
        }

        public int ItemsCount
        {
            get { return grid.Rows.Count; }
        }

        private bool IsAllowed(DataGridViewRow r)
        {
            var mt = ((MessageItem)r.Tag).Type;

            return (mt == MessageItemType.Error && ShowErrors)
                || (mt == MessageItemType.Warning && ShowWarnings)
                || (mt == MessageItemType.Information && ShowMessages);
        }

        internal IApp App { get; set; }

        #region State
        private bool _showErrors;
        [StateItem]
        public bool ShowErrors
        {
            get { return _showErrors; }
            set
            {
                _showErrors = value;
                RefreshView();
            }
        }

        private bool _showWarnings;
        [StateItem]
        public bool ShowWarnings
        {
            get { return _showWarnings; }
            set
            {
                _showWarnings = value;
                RefreshView();
            }
        }

        private bool _showMessages;
        [StateItem]
        public bool ShowMessages
        {
            get { return _showMessages; }
            set
            {
                _showMessages = value;
                RefreshView();
            }
        }

        [StateItem]
        public int Column1
        {
            get { return grid.Columns[0].Width; }
            set { grid.Columns[0].Width = value; }
        }

        [StateItem]
        public int Column2
        {
            get { return grid.Columns[1].Width; }
            set { grid.Columns[1].Width = value; }
        }

        [StateItem]
        public int Column3
        {
            get { return grid.Columns[2].Width; }
            set { grid.Columns[2].Width = value; }
        }

        [StateItem]
        public int Column4
        {
            get { return grid.Columns[3].Width; }
            set { grid.Columns[3].Width = value; }
        }

        [StateItem]
        public int Column5
        {
            get { return grid.Columns[4].Width; }
            set { grid.Columns[4].Width = value; }
        }

        [StateItem]
        public int Column6
        {
            get { return grid.Columns[5].Width; }
            set { grid.Columns[5].Width = value; }
        }
        #endregion
    }
}
