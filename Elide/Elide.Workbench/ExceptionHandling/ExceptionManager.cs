using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Net.Mail;
using System.Net;
using Elide.Core;

namespace Elide.Workbench.ExceptionHandling
{
	public static class ExceptionManager
	{
		private static object syncRoot = new Object();
                
        public static void Process(IApp app, Exception exception)
		{
			if (exception is ThreadAbortException ||
                exception == null)
				return;

            lock (syncRoot)
            {
                var d = new ExceptionDialog();
                d.ExceptionMessage = exception.Message;
                d.ExceptionDetails = exception.ToString();
                d.ShowDialog(WB.Form);
                
                if (d.SendMail)
                {
                    var dump = new ExceptionDump(exception);
                    var sender = new DumpSender
                    {
                        To = SenderData.Recipient,
                        Credentials = new NetworkCredential(SenderData.Login, SenderData.Password)
                    };

                    if (!sender.SendDump(dump.GenerateDump(true)))
                        MessageBox.Show(sender.Error, Application.ProductName, 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                Log(exception);
                app.Unload();
            }
		}
        
        public static void Log(Exception exception)
        {
            if (ErrorLogPath != null)
            {
                lock (syncRoot)
                {
                    try
                    {
                        var dump = new ExceptionDump(exception);

                        using (StreamWriter writer = new StreamWriter(
                            File.Open(ErrorLogPath.FullName, FileMode.Append)))
                            writer.Write(dump.GenerateDump(false));
                    }
                    catch (Exception) { }
                }
            }
        }
		
		public static FileInfo ErrorLogPath { get; set; }
	}
}
