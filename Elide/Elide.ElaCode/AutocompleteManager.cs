using System;
using System.Collections.Generic;
using System.Linq;
using Ela.CodeModel;
using Ela.Compilation;
using Ela.Debug;
using Elide.CodeEditor;
using Elide.Core;
using Elide.Scintilla;
using Elide.ElaCode.ObjectModel;
using System.Text;
using System.IO;

namespace Elide.ElaCode
{
    public sealed class AutocompleteManager
    {
        private readonly ScintillaControl sci;
        private readonly IApp app;

        public AutocompleteManager(IApp app, ScintillaControl sci)
        {
            this.app = app;
            this.sci = sci;
        }

        private bool TestIfComments(int pos, bool checkStr)
        {
            var st = sci.GetStyleAt(pos);

            //exclude autocomplete in comments and strings
            var b = st == TextStyle.MultilineStyle1 || st == TextStyle.MultilineStyle2
                || st == TextStyle.Style6 || st == TextStyle.Style7;

            if (st == TextStyle.None && checkStr)
            {
                var lnn = sci.GetLineFromPosition(pos);
                var ln = sci.GetLine(lnn);
                var col = sci.GetColumnFromPosition(pos);

                for (var i = col; i > -1; i--)
                    if (sci.CharAt(sci.GetPositionByColumn(lnn, i)) == '"')
                        return true;
            }

            return b;
        }

        public void DoComplete(int pos, CodeDocument doc)
        {
            if (doc == null)
                return;

            if (TestIfComments(pos, true) || TestIfComments(pos - 1, false))
                return;

            if (sci.CurrentPosition > 0 && sci.CharAt(sci.CurrentPosition - 1) == '.')
            {
                ListModuleMembers(doc, pos - 2);
                return;
            }

            var names = default(List<AutocompleteSymbol>);

            if (doc.Unit != null)
            {
                var frame = ((CompiledUnit)doc.Unit).CodeFrame;
                var dr = new DebugReader(frame.Symbols);
                var col = sci.GetColumnFromPosition(sci.CurrentPosition);
                var ln = dr.FindClosestLineSym(sci.CurrentLine + 1, col + 1);

                var scope = col == 0 || col == 1 ? dr.GetScopeSymByIndex(0) :
                    dr.FindScopeSym(sci.CurrentLine + 1, col + 1);

                if (scope == null)
                    scope = dr.GetScopeSymByIndex(0);

                if (scope != null)
                {
                    var vars = LookVars(dr, ln != null ? ln.Offset : scope.EndOffset, scope.Index);
                    names = vars
                        .Where(v => v.Name[0] != '$')
                        .Select(v => 
                            {
                                var f = (ElaVariableFlags)v.Flags;
                                return new AutocompleteSymbol(v.Name,
                                    f.Set(ElaVariableFlags.Module) ? AutocompleteSymbolType.Module :
                                    f.Set(ElaVariableFlags.TypeFun) ? AutocompleteSymbolType.Type :
                                    f.Set(ElaVariableFlags.ClassFun) ? AutocompleteSymbolType.Member :
                                        AutocompleteSymbolType.Variable);                                
                            })
                        .ToList();
                }
            }

            var line = sci.GetLine(sci.CurrentLine).Text.Trim('\r', '\n', '\0');
            var tl = line.Trim();
            var keywords = new List<AutocompleteSymbol>();

            keywords.Add(Snippet("if"));

            if (tl.Length == 0)
            {
                keywords.Add(Snippet("open"));
                keywords.Add(Snippet("let"));

                if (line.Length > 0)
                {
                    keywords.Add(Snippet("et"));
                    keywords.Add(Snippet("where"));
                }
            }
            else if (tl.EndsWith("="))
                keywords.Add(Snippet("let"));
            else if (tl.EndsWith("let"))
            {
                keywords.Add(Snippet("private"));
                keywords.Add(Snippet("inline"));
            }

            if (names != null)
                keywords.AddRange(names);

            app.GetService<IAutocompleteService>().ShowAutocomplete(keywords);
        }

        public FileInfo FindModule(int pos, CodeDocument doc)
        {
            var sym = GetNameInfo(pos, doc);
            var unit = doc != null ? doc.Unit : null;
            var frame = unit != null ? ((CompiledUnit)unit).CodeFrame : null;

            if (sym != null && ((ElaVariableFlags)sym.Flags).Set(ElaVariableFlags.Module)
                && frame != null && frame.References.ContainsKey(sym.Name))
            {
                var mod = frame.References[sym.Name];
                var rr = new ElaReferenceResolver { App = app };
                var refUnit = rr.Resolve(new Reference(new CompiledUnit(doc, frame), mod), ElaReferenceResolver.NoBuild);

                if (refUnit != null)
                    return ((CompiledUnit)refUnit).Document.FileInfo;
            }

            return null;
        }

        private void ListModuleMembers(CodeDocument doc, int pos)
        {
            var sym = GetNameInfo(pos, doc);
            var unit = doc != null ? doc.Unit : null;
            var frame = unit != null ? ((CompiledUnit)unit).CodeFrame : null;

            if (sym != null && ((ElaVariableFlags)sym.Flags).Set(ElaVariableFlags.Module)
                && frame != null && frame.References.ContainsKey(sym.Name))
            {
                var mod = frame.References[sym.Name];
                var rr = new ElaReferenceResolver { App = app };
                var refUnit = rr.Resolve(new Reference(new CompiledUnit(doc, frame), mod));

                if (refUnit != null)
                {
                    var sb = new StringBuilder();
                    var list = new List<AutocompleteSymbol>();

                    foreach (var cn in ((CompiledUnit)refUnit).CodeFrame.GlobalScope.EnumerateVars()
                        .Where(v => !v.Value.VariableFlags.Set(ElaVariableFlags.Module) 
                            && !v.Value.VariableFlags.Set(ElaVariableFlags.Private)
                            && v.Key[0] != '$'))
                    {
                        list.Add(new AutocompleteSymbol(cn.Key,
                            cn.Value.VariableFlags.Set(ElaVariableFlags.Module) ? AutocompleteSymbolType.Module :
                            cn.Value.VariableFlags.Set(ElaVariableFlags.TypeFun) ? AutocompleteSymbolType.Type :
                            cn.Value.VariableFlags.Set(ElaVariableFlags.ClassFun) ? AutocompleteSymbolType.Member :
                                AutocompleteSymbolType.Variable));
                    }

                    list = list.OrderBy(c => c.Name).ToList();
                    app.GetService<IAutocompleteService>().ShowAutocomplete(list);
                }
            }
        }

        private VarSym GetNameInfo(int position, CodeDocument doc)
        {
            var word = sci.GetWordAt(position) ?? GetOperator(position, 0);
            var frame = doc != null ? doc.Unit : null;

            if (word != null && frame != null)
            {
                var dr = new DebugReader(((CompiledUnit)frame).CodeFrame.Symbols);
                var lineNum = sci.GetLineFromPosition(position);
                var colNum = sci.GetColumnFromPosition(position);

                var ln = dr.FindClosestLineSym(lineNum + 1, colNum + 1);
                var scope = default(ScopeSym);

                if (ln != null && (scope = (dr.FindScopeSym(lineNum + 1, colNum + 1) ?? dr.GetScopeSymByIndex(0))) != null)
                    return LookVar(dr, ln.Offset, scope.Index, word);
            }

            return null;
        }

        private AutocompleteSymbol Snippet(string text)
        {
            return new AutocompleteSymbol(text, AutocompleteSymbolType.Snippet);
        }
        
        private IEnumerable<VarSym> LookVars(DebugReader dr, int offset, int scopeIndex)
        {
            var scope = dr.GetScopeSymByIndex(scopeIndex);
            var list = new List<VarSym>();

            foreach (var vs in dr.FindVarSyms(offset, scope))
                list.Add(vs);

            if (scope.Index != 0 && scope.ParentIndex != scope.Index)
                list.AddRange(LookVars(dr, offset, scope.ParentIndex));

            return list;
        }

        private VarSym LookVar(DebugReader dr, int offset, int scopeIndex, string var)
        {
            var scope = dr.GetScopeSymByIndex(scopeIndex);

            foreach (var vs in dr.FindVarSyms(offset, scope))
                if (vs.Name == var)
                    return vs;

            if (scope.Index != 0)
                return LookVar(dr, offset, scope.ParentIndex, var);
            else
                return null;
        }

        private string GetOperator(int position, int mov)
        {
            var c = sci.CharAt(position);

            if (c == ' ' || c == '\r' || c == '\n' || c == '\0')
                return String.Empty;

            var ret = String.Empty;

            if (mov == 0 || mov == 1)
                ret += GetOperator(position - 1, 1);

            ret += c;

            if (mov == 0 || mov == 2)
                ret += GetOperator(position + 1, 2);

            return ret;
        }
    }
}
