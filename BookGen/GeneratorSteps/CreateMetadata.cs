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
    internal class CreateMetadata : IGeneratorStep
    {
        public void RunStep(RuntimeSettings settings, ILog log)
        {
            log.Info("Generating metadata for pages...");

            foreach (var chapter in settings.TocContents.Chapters)
            {
                foreach (var link in settings.TocContents.GetLinksForChapter(chapter))
                {
                    var title = $"{settings.Configruation.Metadata.Title} - {link.DisplayString}";
                    var file = settings.SourceDirectory.Combine(link.Link);

                    var description = MarkdownRenderers.Markdown2Plain(file.ReadFile()).Replace('\n', ' ').Trim();
                    var limit = description.Length < 190 ? description.Length : 190;
                    description = description.Substring(0, limit) + "...";

                    var metaData = new StringBuilder();
                    metaData.AppendFormat("<meta name=\"author\" content=\"{0}\">\n", settings.Configruation.Metadata.Author);
                    metaData.AppendFormat("<meta property=\"og:site_name\" content=\"{0}\">\n", settings.Configruation.Metadata.Title);
                    metaData.AppendFormat("<meta property=\"og:image\" content=\"{0}\">\n", settings.Configruation.Metadata.CoverImage);
                    metaData.AppendFormat("<meta property=\"og:title\" content=\"{0}\">\n", title);
                    metaData.AppendFormat("<meta property=\"og:url\" content=\"{0}\">\n", link.GetLinkOnHost(settings.Configruation.HostName));
                    metaData.AppendFormat("<meta property=\"og:description\" content=\"{0}\">\n", description);
                    metaData.AppendFormat("<meta property=\"og:type\" content=\"article\">\n");

                    settings.MetataCache.Add(link.Link, metaData.ToString());

                }
            }

        }
    }
}
