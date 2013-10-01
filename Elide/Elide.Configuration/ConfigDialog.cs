using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Elide.Core;
using Elide.Environment.Configuration;
using Elide.Forms;

namespace Elide.Configuration
{
    public partial class ConfigDialog : Form
    {
        private List<Config> updatedConfigs;

        public ConfigDialog()
        {
            updatedConfigs = new List<Config>();
            InitializeComponent();
        }

        private void ConfigDialog_Load(object sender, EventArgs e)
        {
            var srv = App.GetService<IConfigService>();
            var map = new Dictionary<String,List<ConfigInfo>>();

            foreach (var ci in srv.EnumerateInfos("configs").OfType<ConfigInfo>()
                .Where(c => c.Widget != null))
            {
                var list = default(List<ConfigInfo>);

                if (!map.TryGetValue(ci.Category, out list))
                {
                    list = new List<ConfigInfo>();
                    map.Add(ci.Category, list);
                }

                list.Add(ci);
            }

            foreach (var kv in map)
            {
                configList.Items.Add(new ListGroup(kv.Key));
                kv.Value.OrderBy(c => c.Position).ForEach(c => configList.Items.Add(new ListItem(c.DisplayName, c)));
            }

            configList.SelectedIndex = 1;
        }

        private void configList_SelectedIndexChanged(object sender, EventArgs e)
        {
            var li = configList.SelectedItem as ListItem;

            if (li == null)
                return;

            var ci = (ConfigInfo)li.Value;
            
            var ctl = (UserControl)Activator.CreateInstance(ci.Widget);
            ctl.BackColor = UserColors.Window;
            ctl.BorderStyle = BorderStyle.None;
            ctl.Dock = DockStyle.Fill;
            ctl.Width = 476;
            ctl.Height = 362;

            var newConf = updatedConfigs.FirstOrDefault(c => c.GetType() == ci.Type);

            if (newConf == null)
            {
                var conf = App.GetService<IConfigService>().QueryConfig(ci.Type);
                newConf = conf.Clone();
                updatedConfigs.Add(newConf);
            }

            ((IOptionPage)ctl).Config = newConf;
            ((IOptionPage)ctl).App = App;
            
            panel.Controls.Clear();
            panel.Controls.Add(ctl);
        }

        private void accept_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;

            var srv = App.GetService<IConfigService>();
            updatedConfigs.ForEach(srv.UpdateConfig);
            
            Close();
        }

        private void cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        public IApp App { get; set; }
    }
}
