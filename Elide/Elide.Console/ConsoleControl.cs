using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using Elide.Forms;
using Elide.Forms.State;
using Elide.Core;

namespace Elide.Console
{
	public partial class ConsoleControl : StateUserControl
	{
        public ConsoleControl()
		{
			InitializeComponent();
        }

        public ConsoleTextBox Cout
        {
            get { return cout; }
        }

        [StateItem]
        public HistoryList<String> History
        {
            get { return cout.History; }
            set { cout.History = value; }
        }

        public IApp App
        {
            get { return cout.App; }
            set { cout.App = value; }
        }
	}
}
