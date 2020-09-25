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
                    string title = $"{settings.Configuration.Metadata.Title} - {link.Text}";
                    FsPath file = settings.SourceDirectory.Combine(link.Url);

                    string description = GetDescription(log, file);

                    MetaTag meta = CreateMetaTag(settings, link, title, description);

                    settings.MetataCache[link.Url] = meta.GetHtmlMeta();
                }
            }
        }

        private static MetaTag CreateMetaTag(RuntimeSettings settings, Link link, string title, string description)
        {
            var meta = new MetaTag().FillWithConfigDefaults(settings.Configuration);

            meta.Title = title;
            meta.Url = link.ConvertToLinkOnHost(settings.Configuration.HostName);
            meta.Description = description;
            return meta;
        }

        private static string GetDescription(ILog log, FsPath file)
        {
            using (var pipeline = new BookGenPipeline(BookGenPipeline.Plain))
            {
                string? content = file.ReadFile(log).Replace('\n', ' ').Trim();
                string? description = pipeline.RenderMarkdown(content);

                var limit = description.Length < 190 ? description.Length : 190;
                return description.Substring(0, limit) + "...";
            }
        }
    }
}
