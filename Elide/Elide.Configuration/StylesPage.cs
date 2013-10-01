using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Elide.Forms;
using Elide.Core;
using Elide.Environment.Configuration;

namespace Elide.Configuration
{
    public partial class StylesPage : UserControl, IOptionPage
    {
        private bool noevents;

        public StylesPage()
        {
            InitializeComponent();
            InitializeControls();
        }

        private void InitializeControls()
        {
            fontCombo.Populate();
            sizeCombo.Populate(6, 18);
            forePicker.Populate();
            backPicker.Populate();            
        }

        private void StylesPage_Load(object sender, EventArgs e)
        {
            var srv = App.GetService<IStylesConfigService>();
            groupsCombo.Items.Clear();
            srv.EnumerateInfos("styles").OfType<StyleGroupInfo>().ForEach(g => groupsCombo.Items.Add(g));
            
            if (groupsCombo.Items.Count > 0)
                groupsCombo.SelectedIndex = 0;
        }
        
        private void groupsCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            var group = (StyleGroupInfo)groupsCombo.SelectedItem;
            itemsList.Items.Clear();

            var con = (StylesConfig)Config;
            con.Styles[group.Key].ForEach(i => itemsList.Items.Add(i));

            if (itemsList.Items.Count > 0)
                itemsList.SelectedIndex = 0;
        }

        private void itemsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                noevents = true;
                
                var defItem = GetDefaultItem();
                var item = (StyleItemConfig)itemsList.SelectedItem;
                EnableDefault(defItem != item, defItem);
                
                fontCombo.SelectedItem = String.IsNullOrEmpty(item.FontName) ? fontCombo.DefaultItem : (Object)item.FontName;
                sizeCombo.SelectedItem = item.FontSize == 0 ? sizeCombo.DefaultItem : (Object)item.FontSize;
                
                if (item.ForeColor == null)
                    forePicker.SelectedIndex = 0;
                else
                    forePicker.SelectedColor = item.ForeColor.Value;

                forePicker.DefaultColor = defItem.ForeColor.Value;

                if (item.BackColor == null)
                    backPicker.SelectedIndex = 0;
                else
                    backPicker.SelectedColor = item.BackColor.Value;
                
                backPicker.DefaultColor = defItem.BackColor.Value;

                checkBold.CheckState = item.Bold == null ? CheckState.Indeterminate : (item.Bold.Value ? CheckState.Checked : CheckState.Unchecked);
                checkItalic.CheckState = item.Italic == null ? CheckState.Indeterminate : (item.Italic.Value ? CheckState.Checked : CheckState.Unchecked);
                checkUnderline.CheckState = item.Underline == null ? CheckState.Indeterminate : (item.Underline.Value ? CheckState.Checked : CheckState.Unchecked);

                UpdateSampleView();
            }
            finally
            {
                noevents = false;
            }
        }

        private void UpdateSampleView()
        {
            var def = GetDefaultItem();

            //Path for bug reproducible on XP (?)
            if (fontCombo.SelectedItem == null || sizeCombo.SelectedItem == null)
                return;

            var font = fontCombo.SelectedIndex == 0 ? def.FontName : fontCombo.SelectedItem.ToString();
            var size = sizeCombo.SelectedIndex == 0 ? def.FontSize : (Int32)sizeCombo.SelectedItem;
            
            sample.Font = new Font(font, size,
                FontStyle.Regular 
                | (checkBold.CheckState == CheckState.Indeterminate ? (def.Bold != null && def.Bold.Value ? FontStyle.Bold : FontStyle.Regular) : (checkBold.CheckState == CheckState.Checked ? FontStyle.Bold : FontStyle.Regular))
                | (checkItalic.CheckState == CheckState.Indeterminate ? (def.Italic != null && def.Bold.Value ? FontStyle.Italic : FontStyle.Regular) : (checkItalic.CheckState == CheckState.Checked ? FontStyle.Italic : FontStyle.Regular))
                | (checkUnderline.CheckState == CheckState.Indeterminate ? (def.Underline != null && def.Bold.Value ? FontStyle.Underline : FontStyle.Regular) : (checkUnderline.CheckState == CheckState.Checked ? FontStyle.Underline : FontStyle.Regular)) 
                );
            sample.ForeColor = Color.FromKnownColor(forePicker.SelectedIndex == 0 ? def.ForeColor.Value : forePicker.SelectedColor.Value);
            sample.BackColor = Color.FromKnownColor(backPicker.SelectedIndex == 0 ? def.BackColor.Value : backPicker.SelectedColor.Value);
        }

        private void ControlUpdated(object sender, EventArgs e)
        {
            if (noevents)
                return;

            var item = (StyleItemConfig)itemsList.SelectedItem;
            item.FontName = GetFont();
            item.FontSize = GetFontSize();
            item.ForeColor = forePicker.SelectedColor;
            item.BackColor = backPicker.SelectedColor;
            item.Bold = GetBool(checkBold);
            item.Italic = GetBool(checkItalic);
            item.Underline = GetBool(checkUnderline);
            UpdateSampleView();
        }

        private StyleItemConfig GetDefaultItem()
        {
            return itemsList.Items.OfType<StyleItemConfig>().First(i => i.Type == "Default");
        }

        private void EnableDefault(bool enable, StyleItemConfig def)
        {
            var seq = Controls.OfType<Control>();

            seq.Where(c => c is CheckBox).OfType<CheckBox>()
               .ForEach(c => c.ThreeState = enable);

            seq.Where(c => c is ColorPicker).OfType<ColorPicker>()
               .ForEach(c => c.SetShowDefault(enable));

            if (enable)
            {
                fontCombo.AddDefault(def.FontName);
                sizeCombo.AddDefault(def.FontSize.ToString());
            }
            else
            {
                fontCombo.RemoveDefault();
                sizeCombo.RemoveDefault();
            }
        }

        private bool? GetBool(CheckBox checkBox)
        {
            return checkBox.CheckState == CheckState.Indeterminate ? (Boolean?)null :
                checkBox.CheckState == CheckState.Checked;
        }
                
        private string GetFont()
        {
            if (fontCombo.SelectedIndex != 0)
                return fontCombo.SelectedFont;
            else
                return null;
        }

        private int GetFontSize()
        {
            if (sizeCombo.SelectedIndex != 0)
                return (Int32)sizeCombo.SelectedItem;
            else
                return 0;
        }

        public IApp App { get; set; }

        public Config Config { get; set; }
    }
}
