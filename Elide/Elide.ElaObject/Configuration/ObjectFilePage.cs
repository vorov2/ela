using System;
using System.Windows.Forms;
using Elide.Core;
using Elide.Environment.Configuration;

namespace Elide.ElaObject.Configuration
{
    public partial class ObjectFilePage : UserControl, IOptionPage
    {
        private bool noevents;

        public ObjectFilePage()
        {
            InitializeComponent();
        }

        private void ObjectFilePage_Load(object sender, EventArgs e)
        {
            noevents = true;
            expand.Checked = Con().ExpandAllNodes;
            header.Checked = Con().DisplayHeader;
            format.Checked = Con().UseCustomHeaderFormat;
            formatBox.Text = Con().CustomHeaderFormat;
            offsets.Checked = Con().DisplayOffset;
            frameOpcodes.Checked = Con().DisplayFrameOpCodes;
            opcodes.Checked = Con().DisplayFlatOpCodes;
            limit.Checked = Con().LimitOpCodes;
            limitTextBox.Text = Con().OpCodeLimit.ToString();
            noevents = false;
        }

        private void CheckedChanged(object sender, EventArgs e)
        {
            format.Enabled = formatBox.Enabled = formatLabel.Enabled = header.Checked;
            limit.Enabled = limitTextBox.Enabled = opcodes.Checked;
            formatBox.Enabled = formatLabel.Enabled = format.Checked;

            if (noevents)
                return;

            Con().ExpandAllNodes = expand.Checked;
            Con().DisplayHeader = header.Checked;
            Con().UseCustomHeaderFormat = format.Checked;
            Con().DisplayOffset = offsets.Checked;
            Con().DisplayFrameOpCodes = frameOpcodes.Checked;
            Con().DisplayFlatOpCodes = opcodes.Checked;
            Con().LimitOpCodes = limit.Checked;
        }

        private void formatBox_TextChanged(object sender, EventArgs e)
        {
            if (noevents)
                return;

            Con().CustomHeaderFormat = formatBox.Text;
        }

        private void limitTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsNumber(e.KeyChar) && e.KeyChar != '\b')
            {
                errorProvider.SetError(limitTextBox, "Only numbers are allowed.");
                e.Handled = true;
            }
            else
                errorProvider.SetError(limitTextBox, String.Empty);
        }

        private void limitTextBox_TextChanged(object sender, EventArgs e)
        {
            if (noevents)
                return;

            var i = default(Int32);

            if (Int32.TryParse(limitTextBox.Text, out i))
                Con().OpCodeLimit = i;
        }

        private ElaObjectConfig Con()
        {
            return (ElaObjectConfig)Config;
        }

        public IApp App { get; set; }

        public Config Config { get; set; }
    }
}
