using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Elide.Forms.State
{
    public class StateForm : Form
    {
        public StateForm()
        {
            
        }

        protected override void OnLoad(EventArgs e)
        {
            LoadState(); 
            base.OnLoad(e);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            SaveState();
            base.OnFormClosing(e);
        }

        private void LoadState()
        {
            try
            {
                var ser = new BinarySerializer();
                var stat = ser.Deserialize(StateFileName) as Dictionary<String, Object>;

                if (stat != null)
                {
                    var w = (Int32)stat["Width"];
                    var h = (Int32)stat["Height"];

                    if (w > MinimumSize.Width)
                        Width = w;

                    if (h > MinimumSize.Height)
                        Height = h;

                    var x = (Int32)stat["X"];
                    var y = (Int32)stat["Y"];

                    if (x > 0 && y > 0)
                        Location = new Point(x, y);

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
            var dict = new Dictionary<String,Object>();
            dict.Add("Width", Width);
            dict.Add("Height", Height);
            dict.Add("X", Location.X);
            dict.Add("Y", Location.Y);

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
