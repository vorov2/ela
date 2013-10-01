using System;
using System.Collections.Generic;
using System.Linq;
using Ela.Compilation;
using Ela.Debug;
using Ela.Parsing;
using Elide.CodeEditor;
using Elide.Core;
using Elide.Environment;
using Elide.TextEditor;
using Ela.CodeModel;

namespace Elide.ElaCode
{
    public sealed class SymbolFinder : ISymbolFinder
    {
        private IApp app;

        public SymbolFinder(IApp app)
        {
            this.app = app;
        }

        public IEnumerable<SymbolLocation> FindSymbols(string name, bool allFiles)
        {
            if (!allFiles)
            {
                var doc = (CodeDocument)app.Document();
                return ProcessFile(name, doc);
            }
            else
            {
                return app.GetService<IDocumentService>().EnumerateDocuments()
                    .Where(d => d is CodeDocument)
                    .OfType<CodeDocument>()
                    .Select(d => ProcessFile(name, d))
                    .SelectMany(en => en);
            }

        }

        private IEnumerable<SymbolLocation> ProcessFile(string name, CodeDocument doc)
        {
            var editor = app.Editor(doc.GetType());
            var list = new List<SymbolLocation>();

            if (editor is ElaEditor)
            {
                var src = ((ITextEditor)editor).GetContent(doc);
                var p = new ElaParser();
                var res = p.Parse(src);

                if (res.Success)
                {
                    res.Program.Includes.ForEach(i => FindName(name, doc, i, list));
                    FindName(name, doc, res.Program.Instances, list);
                    FindName(name, doc, res.Program.Classes, list);
                    FindName(name, doc, res.Program.Types, list);
                    FindName(name, doc, res.Program.TopLevel, list);
                }
            }

            return list;
        }


        private void FindName(string name, CodeDocument doc, ElaExpression expr, List<SymbolLocation> syms)
        {
            if (expr == null)
                return;

            switch (expr.Type)
            {
                case ElaNodeType.As:
                    {
                        var a = (ElaAs)expr;

                        if (a.Name == name)
                            syms.Add(new SymbolLocation(doc, a.Line, a.Column));

                        if (a.Expression != null)
                            FindName(name, doc, a.Expression, syms);
                    }
                    break;
                case ElaNodeType.EquationSet:
                    {
                        var b = (ElaEquationSet)expr;

                        foreach (var e in b.Equations)
                            FindName(name, doc, e, syms);
                    }
                    break;
                case ElaNodeType.Builtin:
                    break;
                case ElaNodeType.Newtype:
                    {
                        var b = (ElaNewtype)expr;

                        foreach (var e in b.Constructors)
                            FindName(name, doc, e, syms);
                    }
                    break;
                case ElaNodeType.TypeClass:
                    {
                        var b = (ElaTypeClass)expr;

                        if (b.Members != null)
                            foreach (var m in b.Members)
                                FindName(name, doc, m, syms);

                        if (b.And != null)
                            FindName(name, doc, b.And, syms);
                    }
                    break;
                case ElaNodeType.ClassMember:
                    {
                        var b = (ElaClassMember)expr;
                        
                        if (b.Name == name)
                            syms.Add(new SymbolLocation(doc, b.Line, b.Column));
                    }
                    break;
                case ElaNodeType.ClassInstance:
                    {
                        var b = (ElaClassInstance)expr;

                        if (b.Where != null)
                            FindName(name, doc, b.Where, syms);

                        if (b.And != null)
                            FindName(name, doc, b.And, syms);
                    }
                    break;
                case ElaNodeType.Equation:
                    {
                        var b = (ElaEquation)expr;

                        if (b.Left != null)
                            FindName(name, doc, b.Left, syms);
                        
                        if (b.Right != null)
                            FindName(name, doc, b.Right, syms);
                        
                        if (b.Next != null)
                            FindName(name, doc, b.Next, syms);
                    }
                    break;
                case ElaNodeType.LetBinding:
                    {
                        var b = (ElaLetBinding)expr;

                        if (b.Equations != null)
                            FindName(name, doc, b.Equations, syms);

                        if (b.Expression != null)
                            FindName(name, doc, b.Expression, syms);
                    }
                    break;
                case ElaNodeType.Comprehension:
                    {
                        var c = (ElaComprehension)expr;

                        if (c.Generator != null)
                            FindName(name, doc, c.Generator, syms);
                    }
                    break;
                case ElaNodeType.Condition:
                    {
                        var c = (ElaCondition)expr;

                        if (c.Condition != null)
                            FindName(name, doc, c.Condition, syms);

                        if (c.True != null)
                            FindName(name, doc, c.True, syms);

                        if (c.False != null)
                            FindName(name, doc, c.False, syms);
                    }
                    break;
                case ElaNodeType.Placeholder:
                    break;
                case ElaNodeType.FieldDeclaration:
                    {
                        var f = (ElaFieldDeclaration)expr;

                        if (f.FieldValue != null)
                            FindName(name, doc, f.FieldValue, syms);
                    }
                    break;
                case ElaNodeType.FieldReference:
                    {
                        var r = (ElaFieldReference)expr;

                        if (r.TargetObject != null)
                            FindName(name, doc, r.TargetObject, syms);
                    }
                    break;
                case ElaNodeType.Juxtaposition:
                    {
                        var c = (ElaJuxtaposition)expr;
                        FindName(name, doc, c.Target, syms);

                        foreach (var p in c.Parameters)
                            FindName(name, doc, p, syms);
                    }
                    break;
                case ElaNodeType.Lambda:
                    {
                        var f = (ElaLambda)expr;

                        if (f.Left != null)
                            FindName(name, doc, f.Left, syms);

                        if (f.Right != null)
                            FindName(name, doc, f.Right, syms);
                    }
                    break;
                case ElaNodeType.Generator:
                    {
                        var g = (ElaGenerator)expr;

                        if (g.Target != null)
                            FindName(name, doc, g.Target, syms);

                        if (g.Pattern != null)
                            FindName(name, doc, g.Pattern, syms);

                        if (g.Guard != null)
                            FindName(name, doc, g.Guard, syms);

                        if (g.Body != null)
                            FindName(name, doc, g.Body, syms);
                    }
                    break;
                case ElaNodeType.ImportedVariable:
                    {
                        var v = (ElaImportedVariable)expr;

                        if (v.LocalName == name || v.Name == name)
                            syms.Add(new SymbolLocation(doc, v.Line, v.Column));
                    }
                    break;
                case ElaNodeType.TypeCheck:
                    {
                        var i = (ElaTypeCheck)expr;

                        if (i.Expression != null)
                            FindName(name, doc, i.Expression, syms);
                    }
                    break;
                case ElaNodeType.LazyLiteral:
                    {
                        var l = (ElaLazyLiteral)expr;

                        if (l.Expression != null)
                            FindName(name, doc, l.Expression, syms);
                    }
                    break;
                case ElaNodeType.ListLiteral:
                    {
                        var l = (ElaListLiteral)expr;

                        if (l.Values != null)
                            foreach (var v in l.Values)
                                FindName(name, doc, v, syms);
                    }
                    break;
                case ElaNodeType.Match:
                    {
                        var m = (ElaMatch)expr;

                        if (m.Expression != null)
                            FindName(name, doc, m.Expression, syms);

                        if (m.Entries != null)
                            FindName(name, doc, m.Entries, syms);
                    }
                    break;
                case ElaNodeType.ModuleInclude:
                    {
                        var m = (ElaModuleInclude)expr;

                        if (m.Alias == name)
                            syms.Add(new SymbolLocation(doc, m.Line, m.Column));

                        if (m.HasImportList)
                            foreach (var i in m.ImportList)
                                FindName(name, doc, i, syms);
                    }
                    break;
                case ElaNodeType.Primitive:
                    break;
                case ElaNodeType.Raise:
                    {
                        var r = (ElaRaise)expr;

                        if (r.Expression != null)
                            FindName(name, doc, r.Expression, syms);
                    }
                    break;
                case ElaNodeType.Range:
                    {
                        var r = (ElaRange)expr;

                        if (r.First != null)
                            FindName(name, doc, r.First, syms);

                        if (r.Second != null)
                            FindName(name, doc, r.Second, syms);

                        if (r.Last != null)
                            FindName(name, doc, r.Last, syms);
                    }
                    break;
                case ElaNodeType.RecordLiteral:
                    {
                        var r = (ElaRecordLiteral)expr;

                        if (r.Fields != null)
                            foreach (var f in r.Fields)
                                FindName(name, doc, f, syms);
                    }
                    break;
                case ElaNodeType.Try:
                    {
                        var t = (ElaTry)expr;

                        if (t.Expression != null)
                            FindName(name, doc, t.Expression, syms);

                        if (t.Entries != null)
                            FindName(name, doc, t.Entries, syms);
                    }
                    break;
                case ElaNodeType.TupleLiteral:
                    {
                        var t = (ElaTupleLiteral)expr;

                        if (t.Parameters != null)
                            foreach (var p in t.Parameters)
                                FindName(name, doc, p, syms);
                    }
                    break;
                case ElaNodeType.NameReference:
                    {
                        var r = (ElaNameReference)expr;

                        if (r.Name == name)
                            syms.Add(new SymbolLocation(doc, r.Line, r.Column));
                    }
                    break;
            }
        }
    }
}
