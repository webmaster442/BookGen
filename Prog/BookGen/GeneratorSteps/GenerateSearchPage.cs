//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain.Configuration;
using BookGen.DomainServices.Markdown;
using BookGen.Framework;
using BookGen.Interfaces.Configuration;
using BookGen.Resources;

namespace BookGen.GeneratorSteps;

internal sealed class GenerateSearchPage : ITemplatedStep
{
    public ITemplateProcessor? Template { get; set; }
    public IContent? Content { get; set; }

    private readonly StringBuilder _buffer;
    private readonly Regex _spaces;

    public GenerateSearchPage()
    {
        _buffer = new StringBuilder();
        _spaces = new Regex(@"\s+", RegexOptions.Compiled);
    }

    public void RunStep(IReadonlyRuntimeSettings settings, ILog log)
    {
        if (Content == null)
            throw new DependencyException(nameof(Content));

        if (Template == null)
            throw new DependencyException(nameof(Template));

        Content.Metadata = FillMeta(settings.Configuration);

        log.Info("Generating search page...");
        GenerateSearchContents(settings, log);
        _buffer.Append(ResourceHandler.GetFile(KnownFile.SearchformHtml));

        FsPath? target = settings.OutputDirectory.Combine("search.html");
        Content.Title = settings.Configuration.Translations[Translations.SearchPageTitle];
        Content.Content = _buffer.ToString();

        string? html = Template.Render();
        target.WriteFile(log, html);
    }

    private string FillMeta(IReadOnlyConfig configruation)
    {
        MetaTag? meta = new MetaTag().FillWithConfigDefaults(configruation);
        meta.Title = $"{configruation.Metadata.Title} - {configruation.Translations[Translations.SearchPageTitle]}";
        meta.Description = configruation.Translations[Translations.SearchPageTitle];
        meta.Url = $"{configruation.HostName}search.html";
        return meta.GetHtmlMeta();
    }

    private void GenerateSearchContents(IReadonlyRuntimeSettings settings, ILog log)
    {
        _buffer.WriteElement(HtmlElement.Div, "searchcontents", "nodisplay");

        using var pipeline = new BookGenPipeline(BookGenPipeline.Plain);

        foreach (string? chapter in settings.TocContents.Chapters)
        {
            foreach (Link? link in settings.TocContents.GetLinksForChapter(chapter))
            {
                log.Detail("Processing file for search index: {0}", link.Url);
                string? fileContent = settings.SourceDirectory.Combine(link.Url).ReadFile(log);

                string? rendered = pipeline.RenderMarkdown(fileContent);

                string? file = Path.ChangeExtension(link.Url, ".html");
                string? fullpath = $"{settings.Configuration.HostName}{file}";

                _buffer.AppendFormat("<div title=\"{0}\" data-link=\"{1}\">", link.Text, fullpath);
                _buffer.Append(_spaces.Replace(rendered, " "));
                _buffer.Append("</div>\n");
            }
        }
        _buffer.CloseElement(HtmlElement.Div);
    }
}
