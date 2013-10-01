using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Elide
{
    public sealed class BinarySerializer
    {
        public BinarySerializer()
        {

        }

        public void Serialize(object graph, string fileName)
        {
            var fi = GetFullPath(fileName);
            var ser = new BinaryFormatter();

            using (var stream = fi.Open(FileMode.Create, FileAccess.Write))
                ser.Serialize(stream, graph);
        }
        
        public object Deserialize(string fileName)
        {
            var fi = GetFullPath(fileName);

            if (!fi.Exists)
                return null;

            var ser = new BinaryFormatter();

            using (var stream = fi.Open(FileMode.Open, FileAccess.Read))
                return stream.Length > 0 ? ser.Deserialize(stream) : null;
        }
        
        public FileInfo GetFullPath(string file)
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var dir = new DirectoryInfo(Path.Combine(path, "Elide"));

            if (!dir.Exists)
                dir.Create();

            return new FileInfo(Path.Combine(dir.FullName, file));
        }
    }
}
