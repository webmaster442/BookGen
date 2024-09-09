//-----------------------------------------------------------------------------
// (c) 2019-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.DomainServices.Markdown;
using BookGen.Framework;

namespace BookGen.GeneratorSteps;

internal sealed class CreatePages : ITemplatedStep
{
    public IContent? Content { get; set; }
    public ITemplateProcessor? Template { get; set; }

    public void RunStep(IReadonlyRuntimeSettings settings, ILogger log)
    {
        if (Content == null)
            throw new DependencyException(nameof(Content));

        if (Template == null)
            throw new DependencyException(nameof(Template));

        log.LogInformation("Generating Sub Markdown Files...");

        using var pipeline = new BookGenPipeline(BookGenPipeline.Web);
        pipeline.InjectRuntimeConfig(settings);
        pipeline.SetSvgPasstroughTo(settings.Configuration.TargetWeb.ImageOptions.SvgPassthru);

        var bag = new ConcurrentBag<(string source, FsPath target, string title, string content)>();

        Parallel.ForEach(settings.TocContents.Files, file =>
        {

            (string source, FsPath target, string title, string content) result;

            FsPath? input = settings.SourceDirectory.Combine(file);
            result.target = settings.OutputDirectory.Combine(Path.ChangeExtension(file, ".html"));

            log.LogDebug("Processing file: {file}", input);

            string? inputContent = input.ReadFile(log);

            result.title = MarkdownUtils.GetDocumentTitle(inputContent, log, input);

            if (string.IsNullOrEmpty(result.title))
            {
                log.LogWarning("No title found in document: {file}", file);
                result.title = file;
            }

            result.source = file;
            result.content = pipeline.RenderMarkdown(inputContent);

            bag.Add(result);

        });

        log.LogInformation("Writing files to disk...");
        foreach ((string source, FsPath target, string title, string content) in bag)
        {
            Content.Title = title;
            Content.Metadata = GetMetaData(settings, source);
            Content.Content = content;
            target.WriteFile(log, Template.Render());
        }
    }

    private static string GetMetaData(IReadonlyRuntimeSettings settings, string fileName)
    {
        if (settings.MetataCache.ContainsKey(fileName))
            return settings.MetataCache[fileName];
        return string.Empty;
    }
}
