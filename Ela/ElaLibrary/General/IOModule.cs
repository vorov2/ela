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
            Add<String,FileWrapper,ElaObject>("writeString", WriteString);
            Add<FileWrapper,ElaObject>("readLine", ReadLine);
            Add<FileWrapper,ElaObject>("readLines", ReadLines);
            Add<String, String, ElaObject>("writeLine", WriteLine);
            Add<String, String, ElaObject>("writeText", WriteText);
            Add<String, ElaObject>("truncateFile", TruncateFile);
        }

        public ElaObject OpenFile(string file, string mode, string acc)
        {
            var fm = mode == "AppendMode" ? FileMode.Append :
                mode == "CreateMode" ? FileMode.Create :
                mode == "OpenMode" ? FileMode.Open :
                mode == "OpenCreateMode" ? FileMode.OpenOrCreate :
                FileMode.Truncate;
            var fa = acc == "ReadAccess" ? FileAccess.Read :
                acc == "WriteAccess" ? FileAccess.Write :
                FileAccess.ReadWrite;

            try
            {
                return Result(true, new FileWrapper(File.Open(file, fm, fa)));
            }
            catch (Exception ex)
            {
                return Result(false, new ElaString(ex.Message));
            }
        }

        public ElaUnit CloseFile(FileWrapper fs)
        {
            try
            {
                fs.Dispose();
            }
            catch { }
            return ElaUnit.Instance;
        }

        public ElaObject WriteString(string str, FileWrapper fs)
        {
            try
            {
                if (fs.Writer == null)
                    fs.Writer = new StreamWriter(fs.Stream);

                fs.Writer.Write(str);
                fs.Writer.Flush();
                return Result(true, ElaUnit.Instance);
            }
            catch (Exception ex)
            {
                return Result(false, new ElaString(ex.Message));
            }
        }
        
        public ElaObject ReadLine(FileWrapper fs)
        {
            try
            {
                if (fs.Reader == null)
                    fs.Reader = new StreamReader(fs.Stream);

                var line = fs.Reader.ReadLine();
                return Result(true, line != null ? (ElaObject)new ElaString(line) : ElaUnit.Instance);
            }
            catch (Exception ex)
            {
                return Result(false, new ElaString(ex.Message));
            }
        }

        public ElaObject ReadLines(FileWrapper fs)
        {
            try
            {
                fs.Reader = new StreamReader(fs.Stream);
                fs.Stream.Seek(0, SeekOrigin.Begin);
                return Result(true, new ElaString(fs.Reader.ReadToEnd() ?? String.Empty));
            }
            catch (Exception ex)
            {
                return Result(false, new ElaString(ex.Message));
            }
        }

        //Deprecated?
        public ElaObject WriteLine(string line, string file)
        {
            try
            {
                using (var sw = new StreamWriter(File.Open(file, FileMode.Append)))
                    sw.WriteLine(line);
                return Result(true, ElaUnit.Instance);
            }
            catch (Exception ex)
            {
                return Result(false, new ElaString(ex.Message));
            }
        }

        public ElaObject WriteText(string text, string file)
        {
            try
            {
                using (var sw = new StreamWriter(File.Open(file, FileMode.Create)))
                    sw.WriteLine(text);
                return Result(true, ElaUnit.Instance);
            }
            catch (Exception ex)
            {
                return Result(false, new ElaString(ex.Message));
            }
        }

        public ElaObject TruncateFile(string file)
        {
            try
            {
                File.Open(file, FileMode.Create).Close();
                return Result(true, ElaUnit.Instance);
            }
            catch (Exception ex)
            {
                return Result(false, new ElaString(ex.Message));
            }
        }
    }
}
