﻿//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.DomainServices.Markdown;
using BookGen.Framework;

namespace BookGen.GeneratorSteps;

internal sealed class CreateSubpageIndexes : ITemplatedStep
{
    public IContent? Content { get; set; }
    public ITemplateProcessor? Template { get; set; }
    public List<Link>? Chapters { get; private set; }

    public void RunStep(IReadonlyRuntimeSettings settings, ILogger log)
    {
        DependencyException.ThrowIfNull(Content);
        DependencyException.ThrowIfNull(Template);

        log.LogInformation("Generating index files for sub content folders...");

        using var pipeline = new BookGenPipeline(BookGenPipeline.Web);
        pipeline.InjectRuntimeConfig(settings);

        foreach (string? file in settings.TocContents.Files)
        {
            string? dir = Path.GetDirectoryName(file);

            if (dir == null) continue;

            FsPath? target = settings.OutputDirectory.Combine(dir).Combine("index.html");
            if (!target.IsExisting)
            {
                string? mdcontent = CreateContentLinks(settings, dir);

                Content.Title = dir;
                Content.Content = pipeline.RenderMarkdown(mdcontent);
                Content.Metadata = "";
                string? html = Template.Render();

                log.LogDebug("Creating file: {target}", target);
                target.WriteFile(log, html);
            }
        }
    }

    private string CreateContentLinks(IReadonlyRuntimeSettings settings, string dir)
    {
        if (Chapters == null)
            Chapters = settings.TocContents.GetLinksForChapter().ToList();

        IEnumerable<Link>? links = from link in Chapters
                                   where link.Url.Contains(dir)
                                   select link;

        var sb = new StringBuilder();

        foreach (Link? link in links)
        {
            string? flink = link.Url.Replace(".md", ".html").Replace(dir, ".");
            sb.AppendFormat("## [{0}]({1})\r\n", link.Text, flink);
        }

        return sb.ToString();
    }
}
