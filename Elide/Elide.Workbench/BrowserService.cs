using System;
using System.Diagnostics;
using Elide.Core;
using Elide.Environment;

namespace Elide.Workbench
{
    public sealed class BrowserService : Service, IBrowserService
    {
        public BrowserService()
        {
            
        }

        public bool OpenLink(Uri link)
        {
            try
            {
                Process.Start(link.ToString());
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
