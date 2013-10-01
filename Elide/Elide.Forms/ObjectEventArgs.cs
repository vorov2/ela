using System;

namespace Elide.Forms
{
    public sealed class ObjectEventArgs : EventArgs
    {
        public ObjectEventArgs(object data)
        {
            Data = data;
        }

        public object Data { get; set; }
    }
}
