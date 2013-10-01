using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ela.Runtime;
using Ela.CodeModel;
using Ela.Linking;
using System.IO;
using System.Web;
using Eladoc.Lexers;
using Ela.Runtime.ObjectModel;

namespace Eladoc
{
    public sealed class HtmGenerator
    {
        const string ELACODE = "<pre tab-width=\"2\">{0}</pre>";
        const string CODE = "<pre tab-width=\"2\">{0}</pre>";
        const string TEXT = "<div style=\"padding-bottom:5px\">{0}</div>";
        const string H1 = "<h1>{0}</h1>";
        const string H2 = "<h2>{0}</h2>";
        const string H3 = "<h3>{0}</h3>";
        const string H4 = "<h4>{0}</h4>";
        const string LI = "<li>{0}</li>";
        const string A = "<a href=\"#{0}\">{1}</a>";

        private readonly ElaProgram prog;
        private readonly Doc doc;
        private ElaMachine vm;
        private readonly ElaIncrementalLinker lnk;
        private IEnumerable<ElaExpression> types;
        private IEnumerable<ElaExpression> classes;
        private IEnumerable<ElaExpression> instances;
        private IEnumerable<ElaExpression> bindings;


        public HtmGenerator(ElaProgram prog, Doc doc, ElaIncrementalLinker lnk, ElaMachine vm)
        {
            this.prog = prog;
            this.doc = doc;
            this.vm = vm;
            this.lnk = lnk;
        }

        public string Generate()
        {
            var template = String.Empty;

            using (var sr = new StreamReader(typeof(HtmGenerator).Assembly.GetManifestResourceStream("Eladoc.Resources.Template.htm")))
                template = sr.ReadToEnd();

            var toc = new StringBuilder();
            var src = new StringBuilder();

            if (prog != null)
            {
                types = IterateTypes().ToList();
                classes = IterateClasses().ToList();
                instances = IterateInstances().ToList();
                bindings = IterateBindings().ToList();
            }

            WalkLines(doc.Lines, toc, src);

            return template
                .Replace("%TITLE%", doc.Title)
                .Replace("%CATEGORY%", doc.Category)
                .Replace("%TOC%", toc.ToString())
                .Replace("%BODY%", src.ToString());
        }

        private IEnumerable<ElaExpression> IterateTypes()
        {
            var dict = new Dictionary<String, String>();
            var t = prog.Types;
            var oldt = default(ElaNewtype);

            while (t != null)
            {
                if (t.Header)
                {
                    if (oldt != null)
                        oldt.Flags = t.Flags;
                }
                else
                    oldt = t;

                t = t.And;
            }

            t = prog.Types;

            while (t != null)
            {
                if ((t.Flags & ElaVariableFlags.Private) != ElaVariableFlags.Private)
                    yield return t;

                t = t.And;
            }
        }

        private IEnumerable<ElaExpression> IterateClasses()
        {
            var t = prog.Classes;

            while (t != null)
            {
                yield return t;
                t = t.And;
            }
        }

        private IEnumerable<ElaExpression> IterateInstances()
        {
            var t = prog.Instances;

            while (t != null)
            {
                yield return t;
                t = t.And;
            }
        }

        private IEnumerable<ElaExpression> IterateBindings()
        {
            var dict = new Dictionary<String, Byte>();

            foreach (var b in prog.TopLevel.Equations)
            {
                if (b.Left.Type == ElaNodeType.Header)
                {
                    var h = (ElaHeader)b.Left;

                    if ((h.Attributes & ElaVariableFlags.Private) == ElaVariableFlags.Private)
                    {
                        dict.Remove(h.Name);
                        dict.Add(h.Name, 0);
                    }
                }
                else if (b.Right != null)
                {
                    if (b.Left.Type == ElaNodeType.Juxtaposition)
                    {
                        var jx = (ElaJuxtaposition)b.Left;

                        if (jx.Target.Type == ElaNodeType.NameReference &&
                            !dict.ContainsKey(((ElaNameReference)jx.Target).Name))
                            yield return b;
                    }
                    else if (b.Left.Type == ElaNodeType.NameReference)
                    {
                        var nr = (ElaNameReference)b.Left;

                        if (!dict.ContainsKey(nr.Name))
                            yield return b;
                    }
                }
            }
        }

        private bool GenerateType(string name, StringBuilder sb)
        {
            var cls = types.OfType<ElaNewtype>().FirstOrDefault(t => t.Name == name);

            if (cls == null)
                return false;

            sb.AppendFormat(H3, cls.Name);
            
            var conCount = 0;

            foreach (var f in cls.ConstructorFlags)
                if ((f & ElaVariableFlags.Private) != ElaVariableFlags.Private)
                    conCount++;

            if (conCount > 0)
            {
                sb.AppendFormat(H4, "<span style='margin-left:20px'>Constructors</span>");
                var sbm = new StringBuilder();

                for (var i = 0; i < cls.Constructors.Count; i++)
                    if ((cls.ConstructorFlags[i] & ElaVariableFlags.Private) != ElaVariableFlags.Private)
                        sbm.AppendLine(cls.Constructors[i].ToString());

                sb.AppendFormat(ELACODE, sbm);
            }

            var seq = vm.Assembly.GetRootModule().Instances.Where(d => d.Type == cls.Name);

            if (seq.Count() > 0)
            {
                sb.AppendFormat(H4, "<span style='margin-left:20px'>Instances (defined in this module)</span>");
                var sbm2 = new StringBuilder();

                foreach (var i in seq)
                    sbm2.AppendLine(i.Class);

                sb.AppendFormat(ELACODE, sbm2);
            }

            return true;
        }

        private bool GenerateClass(string name, StringBuilder sb)
        {
            var cls = classes.OfType<ElaTypeClass>().FirstOrDefault(c => c.Name == name);

            if (cls == null)
                return false;

            sb.AppendFormat(H3, cls.Name);
            var sbm = new StringBuilder();
            cls.Members.ToList().ForEach(m => sbm.AppendLine(m.ToString()));
            sb.AppendFormat(ELACODE, sbm);

            return true;
        }

        private bool GenerateBinding(string fullName, StringBuilder sb)
        {
            var arr = fullName.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var name = arr[0];

            var fun = bindings.OfType<ElaEquation>().FirstOrDefault(f =>
                f.Left.Type == ElaNodeType.Juxtaposition ?
                    ((ElaNameReference)((ElaJuxtaposition)f.Left).Target).Name == name :
                    ((ElaNameReference)f.Left).Name == name);

            if (fun == null)
                return false;

            var isFun = fun.Left.Type == ElaNodeType.Juxtaposition;
            
            var sym = IsSymbolic(name);
            var headName = sym ? "(" + name + ")" : name;
            sb.AppendFormat(H3, headName);
            sb.AppendFormat("<a name=\"func_{0}\"></a>", name);

            if (arr.Length == 1)
            {
                if (isFun)
                {
                    var n = fun.Left.ToString();
                    sb.AppendFormat(ELACODE, n);
                }
            }
            else
            {
                var args = new string[arr.Length - 1];
                Array.Copy(arr, 1, args, 0, arr.Length - 1);

                if (sym && args.Length == 1)
                    sb.AppendFormat(ELACODE, name + args[0]);
                else if (sym && arr.Length == 3)
                    sb.AppendFormat(ELACODE, args[0] + name + args[1]);
                else if (sym)
                    sb.AppendFormat(ELACODE, "(" + name + ") " + String.Join(" ", args));
                else
                    sb.AppendFormat(ELACODE, name + " " + String.Join(" ", args));
            }

            return true;
        }

        private void GenerateCodeItem(string str, StringBuilder sb)
        {
            var _ = 
                GenerateBinding(str, sb) 
                || GenerateType(str, sb)
                || GenerateClass(str, sb);
        }

        private void WalkLines(IEnumerable<DocLine> lines, StringBuilder toc, StringBuilder sb)
        {
            int tocId = 0;

            WalkLines(lines,
               (str, type) =>
               {
                   if (type == LineType.CodeItem)
                   {
                       GenerateCodeItem(str, sb);
                   }
                   else if (type == LineType.Table)
                   {
                       str = HttpUtility.HtmlEncode(str);
                       var rows = str.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                       sb.Append("<table class='textTable'>");

                       foreach (var r in rows)
                       {
                           var cells = new List<String>();
                           var buffer = r.ToCharArray();
                           var pos = 0;
                           var l = '\0';

                           for (var i = 0; i < buffer.Length; i++)
                           {
                               var c = buffer[i];
                               var last = i == buffer.Length - 1;

                               if ((l == ' ' && c == '|') || last)
                               {
                                   cells.Add(new String(buffer, pos, i - pos + (last ? 1 : 0)));
                                   pos = i + 1;
                               }

                               l = c;
                           }

                           sb.Append("<tr>");

                           foreach (var c in cells)
                               sb.AppendFormat("<td class='textTd'>{0}</td>",
                                   MarkupParser.Parse(c));

                           sb.Append("</tr>");
                       }

                       sb.Append("</table>");
                   }
                   else if (type == LineType.ElaCode || type == LineType.EvalCode)
                   {
                       if (type == LineType.EvalCode)
                       {
                           lnk.SetSource(str);
                           var res = lnk.Build();

                           if (res.Success)
                           {
                               if (vm == null)
                                   vm = new ElaMachine(res.Assembly);
                               else
                                   vm.RefreshState();

                               try
                               {
                                   var rv = vm.Resume();

                                   if (!rv.ReturnValue.Is<ElaUnit>())
                                   {
                                       var val = vm.PrintValue(rv.ReturnValue);

                                       if (val.Contains('\r') || val.Length > 30)
                                           str += "\r\n/*\r\nThe result is:\r\n" + val + "\r\n*/";
                                       else
                                           str += " //The result is: " + val;
                                   }
                               }
                               catch (ElaCodeException ex)
                               {
                                   Error(ex.Message);
                               }
                           }
                           else
                               res.Messages.ToList().ForEach(m => Console.Write("\r\n" + m.ToString()));
                       }

                       sb.AppendFormat(ELACODE, Lex(str));
                   }
                   else if (type == LineType.Code)
                   {
                       str = HttpUtility.HtmlEncode(str);
                       sb.AppendFormat(CODE, str);
                   }
                   else if (type == LineType.Header1)
                   {
                       str = HttpUtility.HtmlEncode(str);
                       var id = (tocId++).ToString();
                       sb.AppendFormat("<a name=\"{0}\"></a>", id);
                       sb.AppendFormat(H1, str);
                       toc.Append("<b>");
                       toc.AppendFormat(A, id, str);
                       toc.Append("</b><br/>");
                   }
                   else if (type == LineType.Header2)
                   {
                       str = HttpUtility.HtmlEncode(str);
                       var id = (tocId++).ToString();
                       sb.AppendFormat("<a name=\"{0}\"></a>", id);
                       sb.AppendFormat(H2, str);
                       toc.Append("&nbsp;&nbsp;&nbsp;&nbsp;");
                       toc.AppendFormat(A, id, str);
                       toc.Append("<br/>");
                   }
                   else if (type == LineType.Header3)
                   {
                       str = HttpUtility.HtmlEncode(str);
                       sb.AppendFormat(H3, str);
                   }
                   else if (type == LineType.List)
                   {
                       str = HttpUtility.HtmlEncode(str);
                       sb.Append("<ul>");
                       var split = str.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                       split.ToList().ForEach(s => sb.AppendFormat(LI, MarkupParser.Parse(s)));
                       sb.Append("</ul>");
                   }
                   else
                   {
                       str = HttpUtility.HtmlEncode(str);
                       sb.AppendFormat(TEXT, MarkupParser.Parse(str));
                   }
               });
        }

        private void WalkLines(IEnumerable<DocLine> lines, Action<String, LineType> fun)
        {
            var type = LineType.None;
            var str = "";
            var len = lines.Count();

            foreach (var l in lines)
            {
                if (type == LineType.None)
                    type = l.Type;

                if (l.Type != type)
                {
                    fun(str, type);
                    type = l.Type;
                    str = l.Text;
                }
                else
                    str += (str.Length == 0 ? "" : "\r\n") + l.Text;
            }

            fun(str, type);
        }

        private string Lex(string src)
        {
            var lexer = new ElaLexer();
            var tokens = lexer.Parse(src);

            if (tokens != null)
            {
                var sb = new StringBuilder(src);
                var offset = 0;

                tokens.ToList().ForEach(t =>
                {
                    var span = String.Format("<span class='lexer_{0}'>", t.StyleKey);
                    var endSpan = "</span>";
                    sb.Insert(t.Position + offset, span);
                    offset += span.Length;
                    sb.Insert(t.Position + offset + t.Length, endSpan);
                    offset += endSpan.Length;
                });

                return sb.ToString();
            }

            return src;
        }

        private string Error(string message, params object[] args)
        {
            Console.Write("\r\nError: {0}", String.Format(message, args));
            return String.Empty;
        }

        private static readonly char[] ops = new char[] { '!', '%', '&', '*', '+', '-', '$', '.', ':', '/', '\\', '<', '=', '>', '?', '@', '^', '|', '~' };

        private bool IsSymbolic(string name)
        {
            return name.IndexOfAny(ops) != -1;
        }
    }
}
