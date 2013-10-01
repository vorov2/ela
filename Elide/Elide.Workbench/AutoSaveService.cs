using System;
using System.Linq;
using System.Threading;
using Elide.Core;
using Elide.Environment;
using Elide.Workbench.Configuration;

namespace Elide.Workbench
{
    [ExecOrder(100)]
    public sealed class AutoSaveService : Service, IAutoSaveService
    {
        private readonly object syncRoot = new Object();
        private Thread thread;

        public AutoSaveService()
        {
            
        }

        public void Run()
        {
            thread = new Thread(Execute);
            thread.IsBackground = true;
            thread.Start();
        }

        public override void Unload()
        {
            if (thread != null)
            {
                lock (syncRoot)
                {
                    if (thread != null)
                    {
                        try
                        {
                            thread.Abort();
                            thread = null;
                        }
                        catch { }
                    }
                }
            }
        }

        private void Execute()
        {
            for (;;)
            {
                var con = App.Config<WorkbenchConfig>();
                Thread.Sleep(con.AutoSavePeriod);            
                
                if (con.EnableAutoSave)
                {
                    lock (syncRoot)
                    {
                        try
                        {
                            WB.Form.Invoke(() =>
                                {
                                    var fs = App.GetService<IFileService>();
                                    App.GetService<IDocumentService>()
                                        .EnumerateDocuments()
                                        .Where(d => d.FileInfo != null)
                                        .ForEach(d => fs.Save(d, d.FileInfo));
                                });
                            App.GetService<IStatusBarService>().SetStatusString(StatusType.Information, "File(s) autosaved");
                        }
                        catch { }
                    }
                }
            }
        }
    }
}
