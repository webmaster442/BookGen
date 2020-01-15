//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Contracts;
using BookGen.Core;
using BookGen.Core.Markdown;
using BookGen.Domain;
using BookGen.Utilities;

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
                    var title = $"{settings.Configuration.Metadata.Title} - {link.Text}";
                    var file = settings.SourceDirectory.Combine(link.Url);

                    var description = MarkdownRenderers.Markdown2Plain(file.ReadFile(log)).Replace('\n', ' ').Trim();
                    var limit = description.Length < 190 ? description.Length : 190;
                    description = description.Substring(0, limit) + "...";

                    var meta = new MetaTag().FillWithConfigDefaults(settings.Configuration);

                    meta.Title = title;
                    meta.Url = link.ConvertToLinkOnHost(settings.Configuration.HostName);
                    meta.Description = description;

                    if (settings.MetataCache.ContainsKey(link.Url))
                        settings.MetataCache[link.Url] = meta.GetHtmlMeta();
                    else
                        settings.MetataCache.Add(link.Url, meta.GetHtmlMeta());
                }
            }
        }
    }
}
