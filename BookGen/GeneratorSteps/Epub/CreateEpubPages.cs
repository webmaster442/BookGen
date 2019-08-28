//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Contracts;
using BookGen.Core;
using BookGen.Core.Contracts;
using BookGen.Core.Markdown;
using BookGen.Domain;
using BookGen.Framework;
using BookGen.Utilities;
using System.Text;

namespace BookGen.GeneratorSteps.Epub
{
    internal class CreateEpubPages : ITemplatedStep
    {
        public Template Template { get; set; }
        public IContent Content { get; set; }

        public void RunStep(RuntimeSettings settings, ILog log)
        {
            log.Info("Generating epub pages...");

            int chaptercounter = 1;
            foreach (var chapter in settings.TocContents.Chapters)
            {
                int index = 1;
                var output = settings.OutputDirectory.Combine($"epubtemp\\OEBPS\\chapter_{chaptercounter:D2}.html");
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

                log.Info("Writing epub chapter: {0}", chaptercounter);

                Content.Title = chapter;
                Content.Content = MarkdownRenderers.Markdown2EpubHtml(buffer.ToString(), settings);

                var html = Template.Render();

                output.WriteFile(html);
                ++chaptercounter;
            }
        }
    }
}
