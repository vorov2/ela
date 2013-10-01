using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eladoc
{
    public sealed class DocParser
    {
        public Doc Parse(string[] lines)
        {
            var d = new Doc();

            for (var i = 0; i < lines.Length; i++)
            {
                var lt = lines[i].TrimStart();

                var pragma = String.Empty;

                if ((pragma = ReadPragma("#file", lt)) != null)
                    d.File = pragma.Trim();
                else if ((pragma = ReadPragma("#title", lt)) != null)
                    d.Title = pragma.Trim();
                else if ((pragma = ReadPragma("#category", lt)) != null)
                    d.Category = pragma.Trim();
                else if (lt.Length > 0)
                    d.Lines.Add(ReadLine(lt));
            }

            return d;
        }

        private DocItem ReadDocItem(string name, string[] lines, ref int idx)
        {
            var item = new DocItem { Header = name };

            for (; idx < lines.Length; idx++)
            {
                var lt = lines[idx].TrimStart();

                if (ReadPragma("#", lt) != null)
                {
                    idx--;
                    return item;
                }
                else
                    item.Lines.Add(ReadLine(lt));
            }

            return item;
        }

        private void ReadMembers(Doc d, string[] lines, int idx)
        {
            var item = default(DocItem);

            for (var i = idx; i < lines.Length; i++)
            {
                var lt = lines[i].TrimStart();

                if (lt.Length == 0)
                {
                    if (item != null)
                        d.Items.Add(item);

                    item = null;
                }
                else
                {
                    if (item == null)
                    {
                        item = new DocItem();
                        item.Header = lt;
                    }
                    else
                        item.Lines.Add(ReadLine(lt));
                }
            }

            if (item != null)
                d.Items.Add(item);
        }

        private DocLine ReadLine(string lt)
        {
            var dl = new DocLine();
            var str = String.Empty;

            if ((str = ReadPragma("#", lt)) != null)
            {
                dl.Text = str.TrimStart('#');
                dl.Type = LineType.CodeItem;
            }
            else if ((str = ReadPragma(">>>", lt)) != null)
            {
                dl.Text = str;
                dl.Type = LineType.EvalCode;
            }
            else if ((str = ReadPragma(">>", lt)) != null)
            {
                dl.Text = str;
                dl.Type = LineType.ElaCode;
            }
            else if ((str = ReadPragma(">", lt)) != null)
            {
                dl.Text = str;
                dl.Type = LineType.Code;
            }
            else if ((str = ReadPragma("|", lt)) != null)
            {
                dl.Text = str;
                dl.Type = LineType.Table;
            }
            else if ((str = ReadPragma("===", lt)) != null)
            {
                dl.Text = str;
                dl.Type = LineType.Header3;
            }
            else if ((str = ReadPragma("==", lt)) != null)
            {
                dl.Text = str;
                dl.Type = LineType.Header2;
            }
            else if ((str = ReadPragma("=", lt)) != null)
            {
                dl.Text = str;
                dl.Type = LineType.Header1;
            }
            else if ((str = ReadPragma("*", lt)) != null)
            {
                dl.Text = str;
                dl.Type = LineType.List;
            }
            else if ((str = ReadPragma("\\", lt)) != null)
            {
                dl.Text = str;
                dl.Type = LineType.Text;
            }
            else
            {
                dl.Text = lt;
                dl.Type = LineType.Text;
            }

            return dl;
        }

        private string ReadPragma(string pragma, string ln)
        {
            return ln.StartsWith(pragma) ? ln.Substring(pragma.Length) : null;
        }
    }    
}
