using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using Elide.CodeEditor;
using Elide.CodeEditor.Views;
using Elide.Core;
using Elide.Environment;
using Elide.Scintilla;
using Elide.Scintilla.ObjectModel;

namespace Elide.CodeWorkbench
{
    public sealed class OutlinerService : Service, IOutlinerService
    {
        private Dictionary<ScintillaControl,Object> scis;
    
        public OutlinerService()
        {
            scis = new Dictionary<ScintillaControl,Object>();
        }

        public void Outline(CodeDocument doc)
        {
            var ed = App.Editor();

            if (ed != null && doc.Messages != null)
            {
                var sci = ed.Control as ScintillaControl;

                if (sci != null)
                {
                    if (!scis.ContainsKey(sci))
                    {
                        sci.Indicators.Error.Style = IndicatorStyle.Squiggle;
                        sci.Indicators.Error.ForeColor = Color.Red;
                        sci.Indicators.Warning.Style = IndicatorStyle.Squiggle;
                        sci.Indicators.Warning.ForeColor = Color.Blue;
                        sci.MouseDwell += MouseDwell;
                        scis.Add(sci, null);
                    }

                    sci.ClearIndicator(sci.Indicators.Error);
                    sci.ClearIndicator(sci.Indicators.Warning);

                    doc.Messages.ForEachIndex((m,i) =>
                        {
                            var ln = m.Line - 1;
                            var col = m.Column - 1;

                            var indic = m.Type == MessageItemType.Error ? sci.Indicators.Error : sci.Indicators.Warning;
                            var sp = sci.GetPositionByColumn(ln, col);
                            var word = sci.GetWordAt(sp);
                            var len = word == null ? 3 : word.Length;

                            if (len < 3 && col > 1)
                            {
                                sp -= m.Column;
                                len += m.Column;
                            }

                            sci.SetIndicator(indic, i + 1, sp, len);
                        });
                }
            }
        }

        public void ClearOutline(CodeDocument doc)
        {
            var ed = App.Editor();

            if (ed != null && doc.Messages != null)
            {
                var sci = ed.Control as ScintillaControl;

                if (sci != null)
                {
                    sci.ClearIndicator(sci.Indicators.Error);
                    sci.ClearIndicator(sci.Indicators.Warning);
                }
            }
        }

        private void MouseDwell(object sender, DwellEventArgs e)
        {
            var doc = App.Document() as CodeDocument;

            if (doc == null)
                return;

            var sci = App.Editor().Control as ScintillaControl;
            var messages = doc.Messages;

            if (sci == null)
                return;

            var i = sci.GetIndicatorValue(sci.Indicators.Error, e.Position);

            if (i == 0)
                i = sci.GetIndicatorValue(sci.Indicators.Warning, e.Position);

            if (i > 0 && messages.Count() > i - 1)
                sci.ShowCallTip(e.Position, messages.ElementAt(i - 1).Message);
        }
    }
}
