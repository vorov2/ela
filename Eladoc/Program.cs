using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using Ela.CodeModel;
using Ela.Compilation;
using Ela.Linking;
using Ela.Parsing;
using Ela.Runtime;
using Eladoc.Lexers;
using System.Web;
using Ela.Runtime.ObjectModel;
using System.Reflection;
using System.Diagnostics;

namespace Eladoc
{
    class Program
    {
        static int Main(string[] args)
        {
            var file = new FileInfo(args[0].Trim('"'));
            var outputFile = new FileInfo(args[1].Trim('"'));

            if (!file.Exists && !outputFile.Exists)
            {
                var dir = new DirectoryInfo(file.FullName);
                var targetDir = new DirectoryInfo(outputFile.FullName);
                var count = ProcessDir(dir, targetDir);
                Console.Write("\r\n{0} files generated.\r\n", count);
                return 0;
            }
            else
            {
                var ret = Run(file.FullName, outputFile.FullName);
                Console.WriteLine();
                return ret;
            }
        }


        static int ProcessDir(DirectoryInfo dir, DirectoryInfo outDir)
        {
            var count = 0;

            foreach (var fin in dir.GetFileSystemInfos())
            {
                if (fin is DirectoryInfo)
                {
                    var newOutDir = new DirectoryInfo(Path.Combine(outDir.FullName, fin.Name));
                    count += ProcessDir((DirectoryInfo)fin, newOutDir);
                }
                else if (fin is FileInfo)
                {
                    var fi = (FileInfo)fin;

                    if (fi.Extension.ToLower() == ".eladoc")
                    {
                        Run(fi.FullName, Path.Combine(outDir.FullName, fi.Name.ToLower().Replace(fi.Extension, "") + ".htm"));
                        count++;
                    }
                }
            }

            return count;
        }


        static int Run(string file, string outputFile)
        {
            Console.Write("\r\nGenerating documentation file: {0}...", outputFile);
            
            var fi = new FileInfo(file);
            var doc = default(Doc);

            using (var sr = new StreamReader(file))
            {
                var lst = new List<String>();
                var line = String.Empty;

                while ((line = sr.ReadLine()) != null)
                    lst.Add(line);

                var p = new DocParser();
                doc = p.Parse(lst.ToArray());
            }

            var gen = default(HtmGenerator);
            var lopt = new LinkerOptions();
            var copt = new CompilerOptions { Prelude = "prelude" };
            ConfigurationManager.AppSettings["refs"].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                .ToList().ForEach(s => lopt.CodeBase.Directories.Add(new DirectoryInfo(s)));

            if (!String.IsNullOrEmpty(doc.File) && doc.File.Trim(' ').Length != 0)
            {
                var elaFile = new FileInfo(Path.Combine(fi.DirectoryName, doc.File));
              
                if (!elaFile.Exists)
                {
                    foreach (var d in lopt.CodeBase.Directories)
                    {
                        elaFile = new FileInfo(Path.Combine(d.FullName, doc.File));

                        if (elaFile.Exists)
                            break;
                    }
                } 
                
                var elap = new ElaParser();
                var res = elap.Parse(elaFile);

                if (!res.Success)
                {
                    res.Messages.ToList().ForEach(m => Console.WriteLine("\r\n" + m));
                    return -1;
                }

                lopt.CodeBase.Directories.Add(elaFile.Directory);
                var lnk = new ElaIncrementalLinker(lopt, copt, elaFile);
                var lres = lnk.Build();

                if (!lres.Success)
                {
                    lres.Messages.ToList().ForEach(m => Console.Write("\r\n" + m));
                    return -1;
                }

                var vm = new ElaMachine(lres.Assembly);
                vm.Run();
                gen = new HtmGenerator(res.Program, doc, lnk, vm);
            }
            else
            {
                var lnk = new ElaIncrementalLinker(lopt, copt);
                gen = new HtmGenerator(null, doc, lnk, null);
            }

            var src = gen.Generate();

            var outFi = new FileInfo(outputFile);

            if (!outFi.Directory.Exists)
                outFi.Directory.Create();

            using (var fs = new StreamWriter(File.Create(outputFile)))
                fs.Write(src);

            Console.Write(" Done");
            return 0;
        }
    }
}
