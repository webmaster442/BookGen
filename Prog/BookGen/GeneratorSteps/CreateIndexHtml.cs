//-----------------------------------------------------------------------------
// (c) 2019-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

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

            Content.Content = pipeline.RenderMarkdown(input.ReadFile(log));
        }

        string? html = Template.Render();

        target.WriteFile(log, html);
    }
}
