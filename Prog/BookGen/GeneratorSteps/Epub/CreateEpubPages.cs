//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.DomainServices.Markdown;
using BookGen.Framework;

using Microsoft.CodeAnalysis;

namespace BookGen.GeneratorSteps.Epub;

internal sealed class CreateEpubPages : ITemplatedStep
{
    private readonly EpubSession _session;

    public CreateEpubPages(EpubSession session)
    {
        _session = session;
    }

    public ITemplateProcessor? Template { get; set; }
    public IContent? Content { get; set; }

    public void RunStep(IReadonlyRuntimeSettings settings, ILogger log)
    {
        DependencyException.ThrowIfNull(Content);
        DependencyException.ThrowIfNull(Template);

        log.LogInformation("Generating epub pages...");

        int index = 1;

        using var pipeline = new BookGenPipeline(BookGenPipeline.Epub);
        pipeline.InjectRuntimeConfig(settings);

        HtmlTidy tidy = new(log);

        Dictionary<FsPath, string> tidyCache = new();

        foreach (string? file in settings.TocContents.Files)
        {
            _session.GeneratedFiles.Add($"page_{index:D3}");

            FsPath? target = settings.OutputDirectory.Combine($"epubtemp\\OPS\\page_{index:D3}.xhtml");

            log.LogDebug("Processing file for epub output: {file}", file);
            FsPath? input = settings.SourceDirectory.Combine(file);

            string? inputContent = input.ReadFile(log);

            Content.Title = MarkdownUtils.GetDocumentTitle(inputContent, log, input);
            Content.Content = pipeline.RenderMarkdown(inputContent);

            tidyCache.Add(target, Template.Render());
            ++index;
        }

        ParallelOptions options = new ParallelOptions
        {
            MaxDegreeOfParallelism = 5,
        };

        if (tidy.IsInstalled())
        {
            Parallel.ForEach(tidyCache, options, toTidy =>
            {
                string replaced = tidy.ConvertHtml5TagsToXhtmlCompatible(toTidy.Value);
                string html = tidy.HtmlToXhtml(replaced);
                toTidy.Key.WriteFile(log, html);
            });
        }
        else
        {
            log.LogWarning("HtmlTidy not found, skipping tidy step");
            Parallel.ForEach(tidyCache, options, toTidy =>
            {
                string replaced = tidy.ConvertHtml5TagsToXhtmlCompatible(toTidy.Value);
                toTidy.Key.WriteFile(log, replaced);
            });

        }
    }
}