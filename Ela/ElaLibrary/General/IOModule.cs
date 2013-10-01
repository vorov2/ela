using System;
using System.IO;
using System.Text;
using Ela.Linking;
using Ela.Runtime.ObjectModel;
using Ela.Runtime;

namespace Ela.Library.General
{
    public sealed class IOModule : ForeignModule
    {
        public sealed class FileWrapper : ElaObject, IDisposable
        {
            internal FileStream Stream;

            public FileWrapper(FileStream fs)
            {
                this.Stream = fs;
            }

            //~FileWrapper()
            //{
            //    Close();
            //}

            private void Close()
            {
                if (Stream != null)
                {
                    Stream.Close();
                    Stream = null;
                }
            }

            public void Dispose()
            {
                Close();
                GC.SuppressFinalize(this);
            }
        }

        public IOModule()
        {

        }
        
        public override void Initialize()
        {
            Add<String,String,String,ElaObject>("openFile", OpenFile);
            Add<FileWrapper,ElaUnit>("closeFile", CloseFile);
            Add<String,FileWrapper,ElaUnit>("writeString", WriteString);
            Add<FileWrapper,String>("readLine", ReadLine);
            Add<FileWrapper,String>("readAllLines", ReadAllLines);

            Add<ElaFunction,String,String>("readLines", ReadLines);
            Add<String,ElaUnit>("truncateFile", TruncateFile);
            Add<String,String,ElaUnit>("writeLine", WriteLine);
            Add<String,String,ElaUnit>("writeText", WriteText);
        }

        public FileWrapper OpenFile(string file, string mode, string acc)
        {
            var fm = mode == "AppendMode" ? FileMode.Append :
                mode == "CreateMode" ? FileMode.Create :
                mode == "OpenMode" ? FileMode.OpenOrCreate :
                mode == "TruncateMode" ? FileMode.Truncate :
                FileMode.Truncate;
            var fa = acc == "ReadMode" ? FileAccess.Read :
                acc == "WriteMode" ? FileAccess.Write :
                FileAccess.ReadWrite;
            return new FileWrapper(File.Open(file, fm, fa));
        }

        public ElaUnit CloseFile(FileWrapper fs)
        {
            fs.Dispose();
            return ElaUnit.Instance;
        }

        public ElaUnit WriteString(string str, FileWrapper fs)
        {
            var sw = new StreamWriter(fs.Stream);
            sw.Write(str);
            sw.Flush();
            return ElaUnit.Instance;
        }
        
        public string ReadLine(FileWrapper fs)
        {
            var sr = new StreamReader(fs.Stream);
            return sr.ReadLine();
        }

        public string ReadAllLines(FileWrapper fs)
        {
            var sr = new StreamReader(fs.Stream);
            return sr.ReadToEnd();
        }
        
        public string ReadLines(ElaFunction fun, string file)
        {
            using (var sr = File.OpenText(file))
            {
                var line = String.Empty;
                var sb = new StringBuilder();

                while ((line = sr.ReadLine()) != null)
                    sb.AppendLine((String)fun.Call(new ElaValue(line)).AsObject());

                return sb.ToString();
            }
        }

        public ElaUnit TruncateFile(string file)
        {
            File.Open(file, FileMode.Create).Close();
            return ElaUnit.Instance;
        }

        public ElaUnit WriteLine(string line, string file)
        {
            using (var sw = new StreamWriter(File.Open(file, FileMode.Append)))
                sw.WriteLine(line);

            return ElaUnit.Instance;
        }

        public ElaUnit WriteText(string text, string file)
        {
            using (var sw = new StreamWriter(File.Open(file, FileMode.Create)))
                sw.WriteLine(text);

            return ElaUnit.Instance;
        }
    }
}
