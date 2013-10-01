using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Elide.Core;

namespace Elide.Main
{
    public sealed class DaemonService : Service, IDaemonService
    {
        private readonly object syncRoot = new Object();
        private readonly List<IDaemon> daemons;
        
        public DaemonService()
        {
            daemons = new List<IDaemon>();
            Application.Idle += Idle;       
        }
        
        public void RegisterDaemon(IDaemon daemon)
        {
            lock (syncRoot)
                daemons.Add(daemon);
        }

        public void RemoveDaemon(IDaemon daemon)
        {
            lock (syncRoot)
                daemons.Remove(daemon);
        }

        private void Idle(object sender, EventArgs e)
        {
            daemons.ForEach(s => s.Execute());
        }
    }
}
