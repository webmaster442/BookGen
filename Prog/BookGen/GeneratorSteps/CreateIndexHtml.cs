//-----------------------------------------------------------------------------
// (c) 2019-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain.Epub;
using BookGen.Domain.Rss;
using BookGen.DomainServices.Markdown;
using BookGen.Framework;

namespace BookGen.GeneratorSteps;

internal sealed class CreateIndexHtml : ITemplatedStep
{
    public IContent? Content { get; set; }
    public ITemplateProcessor? Template { get; set; }

    public void RunStep(IReadonlyRuntimeSettings settings, ILogger log)
    {
        if (Content == null)
            throw new DependencyException(nameof(Content));

        if (Template == null)
            throw new DependencyException(nameof(Template));

        log.LogInformation("Generating Index file...");
        FsPath? input = settings.SourceDirectory.Combine(settings.Configuration.Index);
        FsPath? target = settings.OutputDirectory.Combine("index.html");

        using (var pipeline = new BookGenPipeline(BookGenPipeline.Web))
        {
            pipeline.InjectRuntimeConfig(settings);
            pipeline.SetSvgPasstroughTo(settings.Configuration.TargetWeb.ImageOptions.SvgPassthru);

            Content.Metadata = GetMetaData(settings, settings.Configuration.Index);
            Content.Title = $"{settings.Configuration.Metadata.Title}";
            Content.Content = pipeline.RenderMarkdown(input.ReadFile(log));
        }

        string? html = Template.Render();

        target.WriteFile(log, html);
    }

    private static string GetMetaData(IReadonlyRuntimeSettings settings, string fileName)
    {
        if (settings.MetataCache.ContainsKey(fileName))
            return settings.MetataCache[fileName];
        return string.Empty;
    }
}
