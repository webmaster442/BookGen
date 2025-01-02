//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.DomainServices.Markdown;
using BookGen.Framework;

namespace BookGen.GeneratorSteps.Print;

internal sealed class CreatePrintableHtml : ITemplatedStep
{
    private const string NewPage = "<p style=\"page-break-before: always\"></p>\r\n";

    public ITemplateProcessor? Template { get; set; }
    public IContent? Content { get; set; }

    public void RunStep(IReadonlyRuntimeSettings settings, ILogger log)
    {
        if (Content == null)
            throw new DependencyException(nameof(Content));

        if (Template == null)
            throw new DependencyException(nameof(Template));

        log.LogInformation("Generating Printable html...");
        FsPath target = settings.OutputDirectory.Combine("print.html");

        using var pipeline = new BookGenPipeline(BookGenPipeline.Print);
        pipeline.InjectRuntimeConfig(settings);
        pipeline.SetSvgPasstroughTo(settings.CurrentBuildConfig.ImageOptions.SvgPassthru);

        FootNoteReindexer reindexer = new(log, appendLineBreakbeforeDefs: true);

        FsPath indexFile = settings.SourceDirectory.Combine(settings.Configuration.Index);
        reindexer.AddMarkdown(indexFile.ReadFile(log, true));

        foreach (string? chapter in settings.TocContents.Chapters)
        {
            log.LogInformation("Processing: {chapter}...", chapter);
            reindexer.AddHtml($"<h1>{chapter}</h1>\r\n\r\n");
            foreach (string? file in settings.TocContents.GetLinksForChapter(chapter).Select(l => l.Url))
            {
                log.LogDebug("Processing file for print output: {file}", file);
                FsPath? input = settings.SourceDirectory.Combine(file);

                string? inputContent = input.ReadFile(log, true);
                reindexer.AddMarkdown(inputContent);
            }
            reindexer.AddHtml(NewPage);
        }

        Content.Content = pipeline.RenderMarkdown(reindexer.ToString());

        target.WriteFile(log, Template.Render());
    }
}
