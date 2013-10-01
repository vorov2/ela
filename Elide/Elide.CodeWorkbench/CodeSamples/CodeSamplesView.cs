using System;
using Elide.Environment.Views;
using Elide.Core;

namespace Elide.CodeWorkbench.CodeSamples
{
    public sealed class CodeSamplesView : AbstractView
    {
        private readonly SamplesControl control;
        private CodeSampleTreeBuilder builder;

        public CodeSamplesView()
        {
            control = new SamplesControl();
        }

        public override void Initialize(IApp app)
        {
            base.Initialize(app);

            var reader = new CodeSamplesReader(app);
            var root = reader.Read();

            builder = new CodeSampleTreeBuilder(app, control);
            builder.BuildTree(root);
        }

        public override object Control
        {
            get { return control; }
        }
    }
}
