using System;
using System.Collections.Generic;
using System.IO;

namespace Elide.Environment
{
    public abstract class Document : IDisposable
    {
        protected Document(FileInfo fileInfo)
        {
            FileInfo = fileInfo;
            _isAlive = true;
        }

        protected Document(string title)
        {
            _title = title;
            _isAlive = true;
        }

        public override string ToString()
        {
            return Title + (IsDirty?"*":String.Empty);
        }

        public virtual void Dispose()
        {
            _isAlive = false;
        }

        private FileInfo _fileInfo;
        public FileInfo FileInfo
        {
            get { return _fileInfo; }
            protected set
            {
                _fileInfo = value;
                _title = null;
                OnFileChanged();
            }
        }

        public abstract bool IsDirty { get; set;  }
        
        private string _title;
        public virtual string Title
        {
            get { return _fileInfo == null ? _title : _fileInfo.FullName; }
        }

        public object Tag { get; set; }

        private volatile bool _isAlive;
        public virtual bool IsAlive
        {
            get { return _isAlive; }
        }

        public event EventHandler FileChanged;
        protected virtual void OnFileChanged()
        {
            var h = FileChanged;

            if (h != null)
                h(this, EventArgs.Empty);
        }
    }
}
