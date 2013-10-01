using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Elide.Forms.State
{
    public partial class StateUserControl : UserControl
    {
        public StateUserControl()
        {
            
        }

        protected override void OnLoad(EventArgs e)
        {
            LoadState();
            base.OnLoad(e);            
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            base.OnHandleDestroyed(e);
            SaveState();
        }

        private void LoadState()
        {
            try
            {
                var ser = new BinarySerializer();
                var stat = ser.Deserialize(StateFileName) as Dictionary<String, Object>;

                if (stat != null)
                {
                    foreach (var p in GetType().GetProperties())
                    {
                        if (Attribute.IsDefined(p, typeof(StateItemAttribute)))
                            p.SetValue(this, stat[p.Name], null);
                    }
                }
            }
            catch { }
        }

        private void SaveState()
        {
            var dict = new Dictionary<String, Object>();
            
            foreach (var p in GetType().GetProperties())
            {
                if (Attribute.IsDefined(p, typeof(StateItemAttribute)))
                    dict.Add(p.Name, p.GetValue(this, null));
            }

            var ser = new BinarySerializer();
            ser.Serialize(dict, StateFileName);
        }

        protected virtual string StateFileName
        {
            get { return GetType().FullName; }
        }
    }
}
