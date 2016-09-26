using Ela;
using Ela.CodeModel;
using Elide.ElaCode.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Elide.ElaCode
{
    internal sealed class AstTreeBuilder
    {
        private ElaProgram root;
        private TreeView tree;

        internal AstTreeBuilder(ElaProgram root)
        {
            this.root = root;
        }

        public void Build(AstControl astControl)
        {
            try
            {
                this.tree = astControl.TreeView;
                tree.BeginUpdate();
                tree.Nodes.Clear();

                astControl.ShowWorking();
                var rootNode = new TreeNode("root") { ImageKey = "Module", SelectedImageKey = "Module" };
                tree.Nodes.Add(rootNode);
                PopulateNodes(rootNode, root.TopLevel);
                astControl.HideInfo();
            }
            finally
            {
                tree.EndUpdate();
            }
        }

        private void PopulateNodes(TreeNode par, ElaExpression exp)
        {
            if (exp == null)
                return;

            switch (exp.Type)
            {
                case ElaNodeType.DebugPoint:
                    var @deb = (ElaDebugPoint)exp;
                    var debNode = par.Simple(exp, String.Format("trace: {0}", @deb.Data));
                    if (@deb.Expression != null)
                        PopulateNodes(debNode, @deb.Expression);
                    break;
                case ElaNodeType.DoNotation:
                    var @do = (ElaDoNotation)exp;
                    var doNode = par.Simple(@do, "do");

                    foreach (var st in @do.Statements)
                    {
                        var stNode = doNode.Simple(st, "statement");
                        PopulateNodes(stNode, st);
                    }
                    break;
                case ElaNodeType.DoAssignment:
                    var @ass = (ElaDoAssignment)exp;
                    par.Simple(@ass, String.Format("{0} <- {1}", @ass.Left, @ass.Right));
                    break;
                case ElaNodeType.As:
                    var @as = (ElaAs)exp;
                    par.Control(@as, "as", @as.Name);
                    break;
                case ElaNodeType.Builtin:
                    var @built = (ElaBuiltin)exp;
                    par.Control(@built, "built-in", @built.Kind.ToString());
                    break;
                case ElaNodeType.ClassInstance:
                    var @inst = (ElaClassInstance)exp;

                    while (@inst != null)
                    {
                        var instNode = par.Simple(@inst, "instance " +
                            (String.IsNullOrEmpty(@inst.TypeClassPrefix) ? @inst.TypeClassName : String.Format("{0}.{1}", @inst.TypeClassName, @inst.TypeClassPrefix)) + "( " +
                            (String.IsNullOrEmpty(@inst.TypePrefix) ? @inst.TypeName : String.Format("{0}.{1}", @inst.TypePrefix, @inst.TypeName)) + " )");
                        PopulateNodes(instNode, @inst.Where);
                        @inst = @inst.And;
                    }
                    break;
                case ElaNodeType.ClassMember:
                    var @mem = (ElaClassMember)exp;
                    par.Plain(@mem);
                    break;
                case ElaNodeType.Comprehension:
                    var @comp = (ElaComprehension)exp;
                    var compNode = par.Simple(@comp, @comp.Lazy ? "lazy comprehension" : "comprehension");
                    PopulateNodes(compNode, @comp.Generator);
                    break;
                case ElaNodeType.Condition:
                    var @cond = (ElaCondition)exp;
                    var condNode = par.Simple(@cond, "if");
                    var condExpNode = condNode.Simple(@cond.Condition, "condition");
                    PopulateNodes(condExpNode, @cond.Condition);
                    var trueExpNode = condNode.Simple(@cond.True, "true");
                    PopulateNodes(trueExpNode, @cond.True);
                    var falseExpNode = condNode.Simple(@cond.False, "false");
                    PopulateNodes(falseExpNode, @cond.False);
                    break;
                case ElaNodeType.Context:
                    var @ctx = (ElaContext)exp;
                    var ctxNode = par.Simple(@ctx, "context");
                    var ctxSetNode = ctxNode.Simple(@ctx, "set context");
                    PopulateNodes(ctxSetNode, @ctx.Context);
                    if (@ctx.Expression != null)
                    {
                        var ctxObjNode = ctxNode.Simple(@ctx, "expression");
                        PopulateNodes(ctxObjNode, @ctx.Expression);
                    }
                    break;
                case ElaNodeType.Equation:
                    var @eq = (ElaEquation)exp;
                    var eqNode = par.Simple(@eq, "equation");
                    var eqLeftNode = eqNode.Simple(@eq.Left, "left");
                    PopulateNodes(eqLeftNode, @eq.Left);
                    if (@eq.Right != null)
                    {
                        var eqRightNode = eqNode.Simple(@eq.Right, "right");
                        PopulateNodes(eqRightNode, @eq.Right);
                    }
                    break;
                case ElaNodeType.EquationSet:
                    var @eqs = (ElaEquationSet)exp;
                    var eqsNode = par.Simple(@eqs, "equation set");
                    foreach (var e in @eqs.Equations)
                        PopulateNodes(eqsNode, e);
                    break;
                case ElaNodeType.FieldDeclaration:
                    var @fldDec = (ElaFieldDeclaration)exp;
                    var fldDecNode = par.Simple(@fldDec, "field " + @fldDec.FieldName);
                    PopulateNodes(fldDecNode, @fldDec.FieldValue);
                    break;
                case ElaNodeType.FieldReference:
                    var @fld = (ElaFieldReference)exp;
                    var fldNode = par.Simple(@fld, "field " + @fld.FieldName);
                    PopulateNodes(fldNode, @fld.TargetObject);
                    break;
                case ElaNodeType.Generator:
                    var @gen = (ElaGenerator)exp;
                    var genNode = par.Simple(@gen, "generator");
                    var genTargetNode = genNode.Simple(@gen.Target, "target");
                    PopulateNodes(genTargetNode, @gen.Target);
                    var genPatternNode = genNode.Simple(@gen.Pattern, "pattern");
                    PopulateNodes(genPatternNode, @gen.Pattern);
                    var genGuardNode = genNode.Simple(@gen.Guard, "guard");
                    PopulateNodes(genGuardNode, @gen.Guard);
                    var genBodyNode = genNode.Simple(@gen.Body, "body");
                    PopulateNodes(genBodyNode, @gen.Body);
                    break;
                case ElaNodeType.Header:
                    var @head = (ElaHeader)exp;
                    var attrs = String.Empty;
                    if ((@head.Attributes & ElaVariableFlags.Private) == ElaVariableFlags.Private)
                        attrs += "private ";
                    if ((@head.Attributes & ElaVariableFlags.Qualified) == ElaVariableFlags.Qualified)
                        attrs += "qualified ";
                    par.Name(@head, attrs + @head.Name, "header");
                    break;
                case ElaNodeType.ImportedVariable:
                    var @imp = (ElaImportedVariable)exp;
                    par.Name(@imp, (@imp.Private ? "private " : String.Empty) + @imp.LocalName + " = " + @imp.Name, "import");
                    break;
                case ElaNodeType.Juxtaposition:
                    var @juxta = (ElaJuxtaposition)exp;
                    var juxtaNode = par.Simple(@juxta, "juxtaposition");
                    var juxtaTargetNode = juxtaNode.Simple(@juxta.Target, "target");
                    PopulateNodes(juxtaTargetNode, @juxta.Target);
                    var juxtaParsNode = juxtaNode.Simple(@juxta, "positioned");
                    foreach (var jp in @juxta.Parameters)
                        PopulateNodes(juxtaParsNode, jp);
                    break;
                case ElaNodeType.Lambda:
                    var @lam = (ElaLambda)exp;
                    var lamNode = par.Simple(@lam, "lambda");
                    var lamParNode = lamNode.Simple(@lam.Left, "left");
                    PopulateNodes(lamParNode, @lam.Left);
                    var lamRightNode = lamNode.Simple(@lam.Right, "right");
                    PopulateNodes(lamRightNode, @lam.Right);
                    break;
                case ElaNodeType.LazyLiteral:
                    var @laz = (ElaLazyLiteral)exp;
                    var lazNode = par.Simple(@laz, "lazy");
                    PopulateNodes(lazNode, @laz.Expression);
                    break;
                case ElaNodeType.LetBinding:
                    var @let = (ElaLetBinding)exp;
                    var letNode = par.Simple(@let, "let");

                    if (@let.Expression != null)
                    {
                        var letExprNode = letNode.Simple(@let.Expression, "expression");
                        PopulateNodes(letExprNode, @let.Expression);
                    }

                    var letEqNode = letNode.Simple(@let.Equations, "equations");
                    PopulateNodes(letEqNode, @let.Equations);
                    break;
                case ElaNodeType.ListLiteral:
                    var @list = (ElaListLiteral)exp;
                    var listNode = par.Simple(@list, "list");
                    foreach (var le in @list.Values)
                        PopulateNodes(listNode, le);
                    break;
                case ElaNodeType.Match:
                    var @mt = (ElaMatch)exp;
                    var mtNode = par.Simple(@mt, "match");
                    var mtExprNode = mtNode.Simple(@mt.Expression, "expression");
                    PopulateNodes(mtExprNode, @mt.Expression);
                    var mtEntriesNode = mtNode.Simple(@mt.Entries, "entries");
                    PopulateNodes(mtEntriesNode, @mt.Entries);
                    break;
                case ElaNodeType.ModuleInclude:
                    var @inc = (ElaModuleInclude)exp;
                    var incNode = par.Simple(@inc, "include " + (String.IsNullOrEmpty(@inc.Alias) ? @inc.Name : @inc.Name + "@" + @inc.Alias) +
                        (String.IsNullOrEmpty(@inc.DllName) ? String.Empty : "#" + @inc.DllName));
                    foreach (var incNm in @inc.ImportList)
                        PopulateNodes(incNode, incNm);
                    break;
                case ElaNodeType.NameReference:
                    var @nam = (ElaNameReference)exp;
                    par.Name(@nam, (@nam.Bang ? "!" : String.Empty) + @nam.Name);
                    break;
                case ElaNodeType.Newtype:
                    var @newt = (ElaNewtype)exp;
                    while (@newt != null)
                    {
                        var newtNode = par.Simple(@newt, (@newt.Extends ? "data " : (@newt.Opened ? "opentype " : "type ")) +
                            (@newt.Prefix != null ? @newt.Prefix + "." : String.Empty) + @newt.Name);
                        foreach (var newtCons in @newt.Constructors)
                            PopulateNodes(newtNode, newtCons);
                        @newt = @newt.And;
                    }
                    break;
                case ElaNodeType.Placeholder:
                    par.Name(exp, "_");
                    break;
                case ElaNodeType.Primitive:
                    var @prim = (ElaPrimitive)exp;
                    par.Literal(@prim, @prim.Value.LiteralType, @prim.Value.ToString());
                    break;
                case ElaNodeType.Range:
                    var @rng = (ElaRange)exp;
                    var rngNode = par.Simple(@rng, "range");
                    var rngFirstNode = rngNode.Simple(@rng.First, "start");
                    PopulateNodes(rngFirstNode, @rng.First);
                    if (@rng.Second != null)
                    {
                        var rngSecondNode = rngNode.Simple(@rng.Second, "step");
                        PopulateNodes(rngSecondNode, @rng.Second);
                    }
                    if (@rng.Last != null)
                    {
                        var rngLastNode = rngNode.Simple(@rng.Last, "end");
                        PopulateNodes(rngLastNode, @rng.Last);
                    }
                    break;
                case ElaNodeType.RecordLiteral:
                    var @rec = (ElaRecordLiteral)exp;
                    var recNode = par.Simple(@rec, "record");
                    foreach (var recFld in @rec.Fields)
                        PopulateNodes(recNode, recFld);
                    break;
                case ElaNodeType.Try:
                    var @try = (ElaTry)exp;
                    var tryNode = par.Simple(@try, "try");
                    var tryExprNode = tryNode.Simple(@try.Expression, "expression");
                    PopulateNodes(tryExprNode, @try.Expression);
                    var tryEntriesNode = tryNode.Simple(@try.Entries, "entries");
                    PopulateNodes(tryEntriesNode, @try.Entries);
                    break;
                case ElaNodeType.TupleLiteral:
                    var @tup = (ElaTupleLiteral)exp;
                    var tupNode = par.Simple(@tup, "tuple");
                    foreach (var tupEl in @tup.Parameters)
                        PopulateNodes(tupNode, tupEl);
                    break;
                case ElaNodeType.TypeCheck:
                    var @typc = (ElaTypeCheck)exp;
                    var typcNode = par.Simple(@typc, "type check");
                    var traits = typcNode.Simple(@typc, "traits");
                    foreach (var typcTra in @typc.Traits)
                        typcNode.Name(@typc, (typcTra.Prefix != null ? typcTra.Prefix + "." : String.Empty) + typcTra.Name, "trait");
                    var typcExprNode = typcNode.Simple(@typc, "expression");
                    PopulateNodes(typcExprNode, @typc.Expression);
                    break;
                case ElaNodeType.TypeClass:
                    var @class = (ElaTypeClass)exp;
                    while (@class != null)
                    {
                        var classNode = par.Simple(@class, "class " + @class.Name);
                        foreach (var classMem in @class.Members)
                            PopulateNodes(classNode, classMem);
                        @class = @class.And;
                    }
                    break;
                case ElaNodeType.UnitLiteral:
                    par.Literal(exp, ElaTypeCode.Unit, "()");
                    break;
            }
        }
    }

    internal static class TreeNodeHelpers
    {
        public static TreeNode Literal(this TreeNode par, ElaExpression exp, ElaTypeCode typeCode, object data)
        {
            return Element(par, exp, TCF.GetShortForm(typeCode), data, "Literal", "{1} ({0})");
        }

        public static TreeNode Control(this TreeNode par, ElaExpression exp, string title, object data)
        {
            return Element(par, exp, title, data, "Function", "{0} {1}");
        }

        public static TreeNode Plain(this TreeNode par, ElaExpression exp)
        {
            return Element(par, exp, "", exp.ToString(), "Function", "{0}{1}");
        }

        public static TreeNode Simple(this TreeNode par, ElaExpression exp, string str)
        {
            return Element(par, exp, "", str, "Arrow", "{0}{1}");
        }

        public static TreeNode Name(this TreeNode par, ElaExpression exp, object data, string title = "name")
        {
            return Element(par, exp, title, data, "Field", "{1} ({0})");
        }

        private static TreeNode Element(this TreeNode par, ElaExpression exp, string title, object data, string image, string format)
        {
            var n = default(TreeNode);
            par.TreeView.Invoke((MethodInvoker)(() =>
                {
                    n = new TreeNode(
                        data != null ? String.Format(format, title, data) : title) { 
                        ImageKey = image, SelectedImageKey = image, 
                        ToolTipText = String.Format("Ln: {0}; Col: {1}", exp.Line, exp.Column) };
                    par.Nodes.Add(n);
                }));
            return n;
        }
    }
}
