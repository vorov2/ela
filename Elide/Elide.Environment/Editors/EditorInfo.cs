using System;
using Elide.Core;

namespace Elide.Environment.Editors
{
    public sealed class EditorInfo : ExtInfo
    {
        internal EditorInfo(string key, Type type, Type documentType, string displayName, string fileExtension, string fileExtensionDescription, EditorFlags flags) : base(key)
        {
            Type = type;
            DocumentType = documentType;
            DisplayName = displayName;
            FileExtension = fileExtension;
            FileExtensionDescription = fileExtensionDescription;
            Flags = flags;
        }

        public Type Type { get; private set; }

        public Type DocumentType { get; private set; }

        public string DisplayName { get; private set; }

        public string FileExtension { get; private set; }

        public string FileExtensionDescription { get; private set; }

        public EditorFlags Flags { get; private set; }

        public IEditor Instance { get; internal set; }
    }
}
