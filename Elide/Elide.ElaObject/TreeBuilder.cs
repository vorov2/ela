using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Elide.ElaObject.Configuration;
using Elide.ElaObject.ObjectModel;
using Ela.Compilation;
using Ela.CodeModel;

namespace Elide.ElaObject
{
    internal sealed class TreeBuilder
    {
        private readonly ElaObjectFile objectFile;
        private readonly ElaObjectConfig config;

        internal TreeBuilder(ElaObjectFile objectFile, ElaObjectConfig config)
        {
            this.objectFile = objectFile;
            this.config = config;
        }

        public void Build(TreeView tree)
        {
            tree.BeginUpdate();
            tree.Nodes.Clear();
            tree.Nodes.Add(ReadHeader(objectFile.Header));
            tree.Nodes.Add(ReadReferences(objectFile.References));
            tree.Nodes.Add(ReadGlobals(objectFile.Globals));
            tree.Nodes.Add(ReadLateBounds(objectFile.LateBounds));
            tree.Nodes.Add(ReadLayouts(objectFile.Layouts));
            tree.Nodes.Add(ReadStrings(objectFile.Strings));
            tree.Nodes.Add(ReadTypes(objectFile.Types));
            tree.Nodes.Add(ReadClasses(objectFile.Classes));
            tree.Nodes.Add(ReadInstances(objectFile.Instances));
            tree.Nodes.Add(ReadConstructors(objectFile.Constructors));

            if (config.DisplayFlatOpCodes)
                tree.Nodes.Add(ReadCode(objectFile.OpCodes));

            tree.EndUpdate();
        }

        private TreeNode ReadReferences(IEnumerable<Reference> refs)
        {
            var par = Group("References");
            refs.ForEach(r =>
                {
                    var n = par.Element(r.ModuleName, null, "Module");
                    n.Element("Alias", r.Alias);

                    if (r.Dll != null)
                        n.Element("DLL", r.Dll);

                    if (!String.IsNullOrEmpty(r.Path))
                        n.Element("Path", r.Path);

                    n.Element("Require Qualified", r.RequireQualified);
                });

            return par;
        }

        private TreeNode ReadLateBounds(IEnumerable<LateBound> lateBounds)
        {
            var par = Group("Late Bound Symbols");
            lateBounds.ForEach(g =>
                {
                    var n = par.Element(g.Name, null, "Symbol");
                    n.Element("Address", g.Address);
                    n.Element("Data", g.Data);
                    n.Element("Line", g.Line);
                    n.Element("Column", g.Column);
                });

            return par;
        }

        private TreeNode ReadGlobals(IEnumerable<Global> globals)
        {
            var par = Group("Globals");
            globals.Where(g => !g.Name.StartsWith("$")).ForEach(g =>
                {
                    var n = par.Element(g.Name, null, 
                        g.Flags.Set(ElaVariableFlags.Private) ? "PrivateVariable" : "Variable");
                    n.Element("Flags", g.Flags);
                    n.Element("Address", g.Address);
                    n.Element("Data", g.Data);
                });

            return par;
        }

        private TreeNode ReadLayouts(IEnumerable<Layout> layouts)
        {
            var par = Group("Memory Layouts");
            layouts.ForEachIndex((o,i) =>
                {
                    var max = objectFile.OpCodes.ElementAt(o.Address - 1).Argument.Value;
                    var co = objectFile.OpCodes;
                    var title = "Layout " + i;

                    if (i != 0 && co.Count() >= max + 2 &&
                        co.ElementAt(max).Op == Op.PushI4 &&
                        co.ElementAt(max + 1).Op == Op.Newfun &&
                        co.ElementAt(max + 2).Op == Op.Popvar)
                    {
                        var addr = co.ElementAt(max + 2).Argument >> 8;
                        var glo = default(Global);

                        if ((glo = objectFile.Globals.FirstOrDefault(g => g.Address == addr)) != null)
                            title += ": " + glo.Name;
                    }
                    else if (i == 0)
                        title += " [global scope]";

                    var n = par.Element(title, null, "Layout");
                    n.Element("Size", o.Size);
                    n.Element("Stack Size", o.StackSize);
                    n.Element("Address", o.Address);

                    if (i > 0 && config.DisplayFrameOpCodes)
                    {
                        var opn = n.Element("Code", null, "Folder");
                        
                        objectFile.OpCodes
                            .Skip(o.Address)
                            .Take(max - o.Address)
                            .ForEach(op => GenOpCode(opn, op));
                    }
                });
            return par;
        }

        private TreeNode ReadStrings(IEnumerable<String> strings)
        {
            var par = Group("String Table");
            strings.ForEach(s => par.Element("String", "\"" + s.Replace("\"", "\\\"") + "\""));
            return par;
        }

        private TreeNode ReadTypes(IEnumerable<String> strings)
        {
            var par = Group("Types");
            strings.ForEach(s => par.Element(s, null, "Type"));
            return par;
        }

        private TreeNode ReadClasses(IEnumerable<Class> classes)
        {
            var par = Group("Classes");
            classes.ForEach(c =>
                {
                    var n = par.Element(c.Name, null, "Interface");
                    c.Members.ForEach(m => n.Element(m.ToString(), null, "Member"));
                });
            return par;
        }

        private TreeNode ReadInstances(IEnumerable<Instance> instances)
        {
            var par = Group("Instances");
            instances.ForEach(c =>
            {
                var n = par.Element(c.Class + " " + c.Type, null, "Instance");
                n.Element("Class", c.Class);
                n.Element("Class Module ID", c.ClassModuleId < 0 ? "(local)" : c.ClassModuleId.ToString());
                n.Element("Type", c.Type);
                n.Element("Type Module ID", c.TypeModuleId < 0 ? "(local)" : c.TypeModuleId.ToString());
                n.Element("Line", c.Line);
                n.Element("Column", c.Column);
            });
            return par;
        }

        private TreeNode ReadConstructors(IEnumerable<String> strings)
        {
            var par = Group("Constructors");
            strings.ForEach(s => par.Element(s, null, "Type"));
            return par;
        }

        private TreeNode ReadCode(IEnumerable<OpCode> ops)
        {
            var count = ops.Count();
            var max = config.LimitOpCodes && count > config.OpCodeLimit ? config.OpCodeLimit : count;            
            var par = Group(count != max ? String.Format("Byte Code (0 - {0} from {1})", max, count) : 
                String.Format("Byte Code ({0})", count));
            ops.Take(max).ForEach(o => GenOpCode(par, o));
            return par;
        }

        private TreeNode ReadHeader(Header h)
        {
            var par = Group("Header");
            par.Element("Version", h.Version);
            par.Element("Ela version", h.ElaVersion);
            par.Element("Date", h.Date);
            return par;
        }

        private void GenOpCode(TreeNode n, OpCode o)
        {
            var title = config.DisplayOffset ? String.Format("[{0}] {1}", o.Offset, o.Op) : o.Op.ToString();

            n.Element(title, o.Argument, "Op", "{0}.{1}");
        }

        private TreeNode Group(string title)
        {
            return new TreeNode(title) { ImageKey = "Folder", SelectedImageKey = "Folder" };
        }
    }

    internal static class TreeNodeHelpers
    {
        public static TreeNode Element(this TreeNode par, string title, object data)
        {
            return Element(par, title, data, "Literal");
        }

        public static TreeNode Element(this TreeNode par, string title, object data, string image)
        {
            return Element(par, title, data, image, "{0}: {1}");
        }

        public static TreeNode Element(this TreeNode par, string title, object data, string image, string format)
        {
            var n = new TreeNode(
                data != null ? String.Format(format, title, data) : title) { ImageKey = image, SelectedImageKey = image, Tag = data };
            par.Nodes.Add(n);
            return n;
        }
    }
}
