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

            internal StreamReader Reader { get; set; }

            internal StreamWriter Writer { get; set; }

            private void Close()
            {
                if (Stream != null)
                {
                    Stream.Close();
                    Stream = null;
                }

                Writer = null;
                Reader = null;
            }

            ~FileWrapper()
            {
                Close();
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
            Add<FileWrapper,ElaValue>("readLine", ReadLine);
            Add<FileWrapper, String>("readLines", ReadLines);
            Add<String, String, ElaUnit>("writeLine", WriteLine);
            Add<String, String, ElaUnit>("writeText", WriteText);
            Add<String, ElaUnit>("truncateFile", TruncateFile);
        }

        public FileWrapper OpenFile(string file, string mode, string acc)
        {
            var fm = mode == "AppendMode" ? FileMode.Append :
                mode == "CreateMode" ? FileMode.Create :
                mode == "OpenMode" ? FileMode.Open :
                mode == "OpenCreateMode" ? FileMode.OpenOrCreate :
                FileMode.Truncate;
            var fa = acc == "ReadAccess" ? FileAccess.Read :
                acc == "WriteAccess" ? FileAccess.Write :
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
            if (fs.Writer == null)
                fs.Writer = new StreamWriter(fs.Stream);

            fs.Writer.Write(str);
            fs.Writer.Flush();
            return ElaUnit.Instance;
        }
        
        public ElaValue ReadLine(FileWrapper fs)
        {
            if (fs.Reader == null)
                fs.Reader = new StreamReader(fs.Stream);

            var line = fs.Reader.ReadLine();
            return line != null ? new ElaValue(line) : new ElaValue(ElaUnit.Instance);
        }

        public string ReadLines(FileWrapper fs)
        {
            fs.Reader = new StreamReader(fs.Stream);
            fs.Stream.Seek(0, SeekOrigin.Begin);
            return fs.Reader.ReadToEnd() ?? String.Empty;
        }

        //Deprecated?
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

        public ElaUnit TruncateFile(string file)
        {
            File.Open(file, FileMode.Create).Close();
            return ElaUnit.Instance;
        }
    }
}
