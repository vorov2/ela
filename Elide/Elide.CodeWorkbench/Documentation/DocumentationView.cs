using System;
using Elide.Environment.Views;
using Elide.Core;

namespace Elide.CodeWorkbench.Documentation
{
    public sealed class DocumentationView : AbstractView
    {
        private readonly DocControl control;
        private DocTreeBuilder builder;

        public DocumentationView()
        {
            control = new DocControl();
        }

        public override void Initialize(IApp app)
        {
            base.Initialize(app);

            var reader = new DocReader(app);
            var root = reader.Read();

            builder = new DocTreeBuilder(app, control);
            builder.BuildTree(root);
        }

        public override object Control
        {
            get { return control; }
        }
    }
}
