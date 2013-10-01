using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Elide.Core;

namespace Elide.Environment.Editors
{
    public abstract class AbstractEditorService : Service, IEditorService
    {
        protected AbstractEditorService()
        {
            
        }

        public override void Initialize(IApp app)
        {
            base.Initialize(app);
            Editors.ForEach(e => e.Instance = TypeCreator.New<IEditor>(e.Type));
        }

        void IExecService.Run()
        {
            Editors.Select(e => e.Instance).ForEach(i => i.Initialize(App));
        }

        public IExtReader CreateExtReader(string section)
        {
            ValidateSection(section);
            return new EditorReader(this);
        }

        public virtual EditorInfo GetEditor(FileInfo fileInfo)
        {
            var editor = Editors.FirstOrDefault(e => e.Instance.TestDocumentType(fileInfo));

            if (editor == null)
                editor = GetEditor(EditorFlags.Default);

            return editor;
        }

        public virtual EditorInfo GetEditor(Type docType)
        {
            var editor = Editors.FirstOrDefault(e => e.DocumentType == docType);

            if (editor == null)
                editor = GetEditor(EditorFlags.Default);

            return editor;
        }
        
        public virtual EditorInfo GetEditor(EditorFlags flags)
        {
            var editor = Editors.FirstOrDefault(e => e.Flags.Set(flags));

            if (editor == null)
                throw new ElideException("No registered editor has flags '{0}'.", flags);

            return editor;
        }

        protected internal IEnumerable<EditorInfo> Editors { get; internal set; }


        public IEnumerable<ExtInfo> EnumerateInfos(string section)
        {
            ValidateSection(section);
            return Editors.OfType<ExtInfo>().ToList();
        }

        public ExtInfo GetInfo(string section, string key)
        {
            ValidateSection(section);
            return Editors.FirstOrDefault(e => e.Key == key);
        }

        private void ValidateSection(string section)
        {
            if (section != "editors")
                throw new ElideException("Section '{0}' is not supported by EditorService.", section);
        }
    }
}
