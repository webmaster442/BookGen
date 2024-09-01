//-----------------------------------------------------------------------------
// (c) 2019-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.DomainServices.Markdown;

namespace BookGen.GeneratorSteps;

internal sealed class CreateMetadata : IGeneratorStep
{
    public void RunStep(IReadonlyRuntimeSettings settings, ILogger log)
    {
        log.LogInformation("Generating metadata for pages...");

        foreach (string? chapter in settings.TocContents.Chapters)
        {
            foreach (Link? link in settings.TocContents.GetLinksForChapter(chapter))
            {
                string title = $"{settings.Configuration.Metadata.Title} - {link.Text}";
                FsPath file = settings.SourceDirectory.Combine(link.Url);

                string description = GetDescription(log, file);

                MetaTag meta = CreateMetaTag(settings, link, title, description);

                settings.MetataCache[link.Url] = meta.GetHtmlMeta();
            }
        }
    }

    private static MetaTag CreateMetaTag(IReadonlyRuntimeSettings settings, Link link, string title, string description)
    {
        MetaTag? meta = new MetaTag().FillWithConfigDefaults(settings.Configuration);

        meta.Title = title;
        meta.Url = link.ConvertToLinkOnHost(settings.Configuration.HostName);
        meta.Description = description;
        return meta;
    }

    private static string GetDescription(ILogger log, FsPath file)
    {
        using (var pipeline = new BookGenPipeline(BookGenPipeline.Plain))
        {
            string? content = file.ReadFile(log).Replace('\n', ' ').Trim();
            string? description = pipeline.RenderMarkdown(content);

            int limit = description.Length < 190 ? description.Length : 190;
            return description[..limit] + "...";
        }
    }
}
