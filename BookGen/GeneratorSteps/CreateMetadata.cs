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
                    var title = $"{settings.Configuration.Metadata.Title} - {link.DisplayString}";
                    var file = settings.SourceDirectory.Combine(link.Link);

                    var description = MarkdownRenderers.Markdown2Plain(file.ReadFile(log)).Replace('\n', ' ').Trim();
                    var limit = description.Length < 190 ? description.Length : 190;
                    description = description.Substring(0, limit) + "...";

                    var meta = new MetaTag().FillWithConfigDefaults(settings.Configuration);

                    meta.Title = title;
                    meta.Url = link.GetLinkOnHost(settings.Configuration.HostName);
                    meta.Description = description;

                    if (settings.MetataCache.ContainsKey(link.Link))
                        settings.MetataCache[link.Link] = meta.GetHtmlMeta();
                    else
                        settings.MetataCache.Add(link.Link, meta.GetHtmlMeta());
                }
            }
        }
    }
}
