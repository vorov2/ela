using System;
using Elide.Core;
using Elide.Environment;
using Elide.Environment.Configuration;
using Elide.Workbench.Configuration;

namespace Elide.Workbench
{
    [DependsFrom(typeof(IConfigService))]
    [DependsFrom(typeof(IDocumentService))]
    [ExecOrder(10)]
    public sealed class EnvironmentService : Service, IEnvironmentService
    {
        public EnvironmentService()
        {
            WB.Form = new MainForm();
        }

        public override void Initialize(IApp app)
        {
            base.Initialize(app);
            WB.Form.Initialize(app);

            var cs = app.GetService<IConfigService>();
            cs.ConfigUpdated += ConfigUpdated;
        }

        public object GetMainWindow()
        {
            return WB.Form;
        }

        public void Run()
        {
            WB.Form.Show();
        }

        private void ConfigUpdated(object sender, ConfigEventArg e)
        {
            var con = e.Config as WorkbenchConfig;

            if (con != null)
            {
                WB.Form.DocumentContainer.Refresh();
                WB.Form.UpdateWindowHeader();
            }
        }
    }
}
