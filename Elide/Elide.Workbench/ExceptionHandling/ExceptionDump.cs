using System;
using System.Text;
using System.Windows.Forms;

namespace Elide.Workbench.ExceptionHandling
{
    public sealed class ExceptionDump
    {
        private const string HDR_DATETIME = "DateTime: {0}\r\n";
        private const string HDR_PRODUCT = "Product: {0}\r\n";
        private const string HDR_OS = "Windows: {0}\r\n";
        private const string HDR_WORKINGSET = "WorkingSet: {0}\r\n";
        private const string HDR_CMDLINE = "CommandLine: {0}\r\n";
        private const string HDR_TICKS = "Ticks: {0}\r\n";
        private const string HDR_RUNTIME = "Runtime: {0}\r\n";
        private const string HDR_DATA = "Data:\r\n {0}\r\n";
        
        public ExceptionDump(Exception ex)
        {
            Exception = ex;
        }
        
        public string GenerateDump(bool detailed)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat(HDR_DATETIME, DateTime.Now);

            if (detailed)
            {
                sb.AppendFormat(HDR_PRODUCT, Application.ProductVersion);
                sb.AppendFormat(HDR_OS, System.Environment.OSVersion);
                sb.AppendFormat(HDR_RUNTIME, System.Environment.Version);
                sb.AppendFormat(HDR_WORKINGSET, System.Environment.WorkingSet);
                sb.AppendFormat(HDR_TICKS, System.Environment.TickCount);
                sb.AppendFormat(HDR_CMDLINE, System.Environment.CommandLine);
            }

            if (Exception != null)
                sb.AppendFormat(HDR_DATA, Exception.ToString());

            return sb.ToString();
        }
        
        public Exception Exception { get; private set; }

        public Uri Url { get; private set; }
    }
}
