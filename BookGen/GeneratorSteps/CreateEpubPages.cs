//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Contracts;
using BookGen.Core;
using BookGen.Core.Contracts;
using BookGen.Core.Markdown;
using BookGen.Domain;
using BookGen.Utilities;
using System.Text;

namespace BookGen.GeneratorSteps
{
    internal class CreateEpubPages : IGeneratorStep
    {
        public void RunStep(RuntimeSettings settings, ILog log)
        {
            log.Info("Generating epub pages...");

            int chaptercounter = 1;
            foreach (var chapter in settings.TocContents.Chapters)
            {
                int index = 1;
                var output = settings.OutputDirectory.Combine($"OEBPS\\chapter_{chaptercounter:D2}.html");
                StringBuilder buffer = new StringBuilder();
                buffer.AppendFormat("<h1>{0}</h1>\r\n\r\n", chapter);

                foreach (var file in settings.TocContents.GetFilesForChapter(chapter))
                {
                    log.Detail("Processing file for epub output: {0}", file);
                    var input = settings.SourceDirectory.Combine(file);

                    var inputContent = input.ReadFile();

                    inputContent = MarkdownUtils.Reindex(inputContent, ref index);
                    buffer.AppendLine(inputContent);
                }

                var rendered = MarkdownRenderers.Markdown2EpubHtml(buffer.ToString(), settings);
                log.Info("Writing epub chapter: {0}", chaptercounter);
                output.WriteFile(Properties.Resources.html5header, rendered, "</body></html>");
                ++chaptercounter;
            }
        }
    }
}
