using System;
using System.Media;
using Elide.Core;
using Elide.Environment;
using Elide.Forms;

namespace Elide.Workbench
{
    public sealed class StatusBarService : Service, IStatusBarService
    {
        public StatusBarService()
        {

        }

        public void SetStatusString(StatusType type, string text, params object[] args)
        {
            var t = WB.Form.ActiveToolbar;
            t.Invoke(() =>
                {
                    t.HighlightStatusString = true;
                    t.StatusString = args != null && args.Length > 0 ? String.Format(text, args) : text;
                    t.StatusImage = type == StatusType.Error ? Bitmaps.Load("Error") :
                                    type == StatusType.Warning ? Bitmaps.Load("Warning") :
                                    Bitmaps.Load("Message");

                    if (type == StatusType.Error)
                        SystemSounds.Exclamation.Play();

                    if (type == StatusType.Warning)
                        SystemSounds.Beep.Play();

                    t.Refresh();
                });
        }

        public void ClearStatusString()
        {
            WB.Form.Invoke(() =>
                {
                    WB.Form.ActiveToolbar.HighlightStatusString = false;
                    WB.Form.ActiveToolbar.StatusString = null;
                });
            
        }
    }
}
