using System;
using System.Collections.Generic;
using System.IO;
using Ela.CodeModel;
using Ela.Debug;

namespace Ela.Compilation
{
	public class CodeFrame
	{
		#region Construction
		public static readonly CodeFrame Empty = new CodeFrame(Op.Stop);
        internal FastList<Int32> HandleMap;
		
		private CodeFrame(Op op) : this()
		{
			Ops.Add(op);
			OpData.Add(0);
			Layouts.Add(new MemoryLayout(0, 0, 0));
		}


		internal CodeFrame()
		{
			Layouts = new FastList<MemoryLayout>();
			Ops = new FastList<Op>();
			OpData = new FastList<Int32>();
			Strings = new FastList<String>();
			_references = new ReferenceMap();
            HandleMap = new FastList<Int32>();
            InternalTypes = new Dictionary<String,Int32>();
            _internalClasses = new ClassMap();
            InternalInstances = new FastList<InstanceData>();
            LateBounds = new FastList<LateBoundSymbol>();
            InternalConstructors = new FastList<ConstructorData>();
       	}
		#endregion


		#region Nested classes
		private sealed class ReferenceMap : Dictionary<String,ModuleReference>, IReadOnlyMap<String,ModuleReference>
		{
			internal ReferenceMap() { }
			internal ReferenceMap(ReferenceMap copy) : base(copy) { }
		}

        private sealed class ClassMap : Dictionary<String,ClassData>, IReadOnlyMap<String,ClassData>
        {
            internal ClassMap() { }
			internal ClassMap(ClassMap copy) : base(copy) { }
        }
		#endregion


		#region Methods
		public CodeFrame Clone()
		{
			var copy = new CodeFrame();
			copy.Layouts = Layouts.Clone();
			copy.Strings = Strings.Clone();
			copy.GlobalScope = GlobalScope.Clone();
			copy.Ops = Ops.Clone();
			copy.OpData = OpData.Clone();
			copy._references = new ReferenceMap(_references);
			copy.Symbols = Symbols != null ? Symbols.Clone() : null;
            copy.HandleMap = HandleMap.Clone();
            copy.InternalTypes = new Dictionary<String,Int32>(InternalTypes);
            copy._internalClasses = new ClassMap(_internalClasses);
            copy.InternalInstances = InternalInstances.Clone();
            copy.LateBounds = LateBounds.Clone();
            copy.InternalConstructors = InternalConstructors.Clone();
         	return copy;
		}


		public void AddReference(string alias, ModuleReference mr)
		{
			if (_references.ContainsKey(alias))
				_references.Remove(alias);

			_references.Add(alias, mr);
		}


        public override string ToString()
        {
            return File != null ? File.ToString() : "";
        }
		#endregion


		#region Properties
        private ReferenceMap _references;
		public IReadOnlyMap<String,ModuleReference> References
		{
			get { return _references; }
		}

        public IReadOnlyMap<String,ClassData> Classes
        {
            get { return _internalClasses; }
        }

        public IEnumerable<String> Types
        {
            get { return InternalTypes.Keys; }
        }

        public IEnumerable<InstanceData> Instances
        {
            get { return InternalInstances; }
        }

		public FastList<Op> Ops { get; private set; }

		public FastList<Int32> OpData { get; private set; }

		public DebugInfo Symbols { get; internal set; }

		public Scope GlobalScope { get; internal set; }

		public FileInfo File { get; set; }
        
        internal FastList<MemoryLayout> Layouts { get; private set; }

        internal FastList<String> Strings { get; private set; }

        internal Dictionary<String,Int32> InternalTypes { get; private set; }

        internal FastList<ConstructorData> InternalConstructors { get; private set; }

        private ClassMap _internalClasses;
        internal Dictionary<String,ClassData> InternalClasses
        {
            get { return _internalClasses; }
        }

        internal FastList<InstanceData> InternalInstances { get; private set; }

        public FastList<LateBoundSymbol> LateBounds { get; private set; }
        #endregion
	}
}