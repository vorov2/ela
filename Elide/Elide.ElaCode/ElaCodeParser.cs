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
using Ela.Parsing;

namespace Elide.ElaCode
{
    public sealed class ElaCodeParser : ICodeParser<ElaAst>
    {
        public ElaCodeParser()
        {

        }

        public ElaAst Parse(string source, Document doc, IBuildLogger logger)
        {
            try
            {
                return InternalParse(source, doc, logger);
            }
            catch (ElaException ex)
            {
                var msg = new MessageItem(MessageItemType.Error, ex.Message, doc, 0, 0);
                logger.WriteMessages(new MessageItem[] { msg }, m => true);
                return null;
            }
        }

        private ElaAst InternalParse(string source, Document doc, IBuildLogger logger)
        {
            logger.WriteBuildInfo("Ela", ElaVersionInfo.Version);

            var parser = new ElaParser();
            var res = parser.Parse(source);//doc.FileInfo == null ? new ModuleFileInfo(doc.Title) : doc.FileInfo.ToModuleFileInfo());
            var messages = res.Messages.Take(100).ToList();
            logger.WriteMessages(messages.Select(m =>
                new MessageItem(
                    m.Type == MessageType.Error ? MessageItemType.Error : (m.Type == MessageType.Warning ? MessageItemType.Warning : MessageItemType.Information),
                    String.Format("ELA{0}: {1}", m.Code, m.Message),
                    m.File == null || !new FileInfo(m.File.FullName).Exists ? doc : new VirtualDocument(new FileInfo(m.File.FullName)),
                    m.Line,
                    m.Column)
                    {
                        Tag = m.Code
                    })
                    , m => true);
            return res.Success ? new ElaAst(res.Program) : null;
        }

        public IApp App { get; set; }
    }
}
