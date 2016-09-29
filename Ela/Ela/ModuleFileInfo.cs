using System;
using System.Collections.Generic;
using System.Text;

namespace Ela
{
    public sealed class ModuleFileInfo
    {
        public ModuleFileInfo(string fileName)
        {
            if (fileName == null)
                throw new ArgumentNullException("fileName");

            FullName = fileName;
        }

        public string FullName { get; private set; }

        public string Extension
        {
            get
            {
                var idx = FullName.LastIndexOf('.');
                if (idx != -1)
                    return FullName.Substring(idx);
                else
                    return String.Empty;
            }
        }
        
        public string Directory
        {
            get
            {
                var arr = GetComponents();
                if (arr.Length > 1)
                    return FullName.Replace(arr[arr.Length - 1], String.Empty);
                else
                    return String.Empty;
            }
        }

        public string Name
        {
            get
            {
                var arr = GetComponents();
                if (arr.Length > 1)
                    return arr[arr.Length - 1];
                else
                    return FullName;
            }
        }

        public string GetFileNameWithoutExtension()
        {
            return !String.IsNullOrEmpty(FullName) && !String.IsNullOrEmpty(Extension) ? FullName.Replace(Extension, String.Empty) : FullName;
        }

        public override string ToString()
        {
            return FullName;
        }

        private string[] GetComponents()
        {
            var sep = '\\';

            if (FullName.IndexOf('\\') == -1)
                sep = '/';

            return FullName.Split(new char[] { sep }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
