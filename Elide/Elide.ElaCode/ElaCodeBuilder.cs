using System;
using System.IO;
using System.Linq;
using Ela;
using Ela.Linking;
using Elide.CodeEditor.Infrastructure;
using Elide.CodeEditor.Views;
using Elide.Core;
using Elide.ElaCode.ObjectModel;
using Elide.Environment;

namespace Elide.ElaCode
{
    public sealed class ElaCodeBuilder : ICodeBuilder<CompiledAssembly>
    {
        //Extended build options
        public static readonly ExtendedOption ForceRecompile = new ExtendedOption(0x02, true);
        public static readonly ExtendedOption NoDebug = new ExtendedOption(0x04, true);
        public static readonly ExtendedOption NoWarnings = new ExtendedOption(0x08, true);

        public ElaCodeBuilder()
        {

        }

        public CompiledAssembly Build(string source, Document doc, IBuildLogger logger, params ExtendedOption[] options)
        {
            try
            {
                return InternalBuild(source, doc, logger, options);
            }
            catch (ElaException ex)
            {
                var msg = new MessageItem(MessageItemType.Error, ex.Message, doc, 0, 0);
                logger.WriteMessages(new MessageItem[] { msg }, m => true);
                return null;
            }
        }


        private CompiledAssembly InternalBuild(string source, Document doc, IBuildLogger logger, params ExtendedOption[] options)
        {
            logger.WriteBuildInfo("Ela", ElaVersionInfo.Version);

            var bo = new BuildOptionsManager(App);
            var lopt = bo.CreateLinkerOptions();
            var copt = bo.CreateCompilerOptions();

            if (options.Set(ForceRecompile.Code))
                lopt.ForceRecompile = true;

            if (!options.Set(NoDebug.Code))
                copt.GenerateDebugInfo = true;

            if (options.Set(NoWarnings.Code))
                copt.NoWarnings = true;

            logger.WriteBuildOptions("Compiler options: {0}", copt);
            logger.WriteBuildOptions("Linker options: {0}", lopt);
            logger.WriteBuildOptions("Module lookup directories:");
            lopt.CodeBase.Directories.ForEach(d => logger.WriteBuildOptions(d.FullName));

            var lnk = new ElaIncrementalLinker(lopt, copt, doc.FileInfo == null ? new FileInfo(doc.Title) : doc.FileInfo);
            lnk.ModuleResolve += (o, e) =>
            {
                //TBD
            };

            var res = lnk.Build(source);
            var messages = res.Messages.Take(100).ToList();
            logger.WriteMessages(messages.Select(m => 
                new MessageItem(
                    m.Type == MessageType.Error ? MessageItemType.Error : (m.Type == MessageType.Warning ? MessageItemType.Warning : MessageItemType.Information),
                    String.Format("ELA{0}: {1}", m.Code, m.Message),
                    m.File == null || !m.File.Exists ? doc : new VirtualDocument(m.File),
                    m.Line,
                    m.Column) { 
                    Tag = m.Code 
                    })
                    , m => (Int32)m.Tag < 600); //Exclude linker messages
            return res.Success ? new CompiledAssembly(doc, res.Assembly) : null;
        }

        public IApp App { get; set; }
    }
}
