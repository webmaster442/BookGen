//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;

using Bookgen.Lib.ImageService;
using Bookgen.Lib.Templates;

using BookGen.Vfs;

using Microsoft.Extensions.Logging;

using PreMailer.Net;

namespace Bookgen.Lib.Pipeline.Print;

internal sealed class WriteXHtml : PipeLineStep<PrintState>
{
    private readonly Dictionary<string, string> _tagreplacements;

    public WriteXHtml(PrintState state) : base(state)
    {
        _tagreplacements = new Dictionary<string, string>()
        {
            { "abbr", "span" },
            { "acronym", "span" },
            { "address", "div" },
            { "article", "div" },
            { "aside", "div" },
            { "canvas", "div" },
            { "cite", "span" },
            { "dd", "span" },
            { "details", "div" },
            { "dfn", "span" },
            { "dl", "div" },
            { "dt", "span" },
            { "figcaption", "p" },
            { "figure", "div" },
            { "footer", "div" },
            { "header", "div" },
            { "kbd", "span" },
            { "nav", "div" },
            { "samp", "span" },
            { "section", "div" },
            { "var", "span" },
        };
    }

    private void ReplaceHtml5ElementsWithXhtmlCompatible(IHtmlDocument document)
    {
        foreach (KeyValuePair<string, string> elementToReplace in _tagreplacements)
        {
            IHtmlCollection<IElement> elements = document.QuerySelectorAll(elementToReplace.Key);
            foreach (IElement element in elements)
            {
                IElement newElement = document.CreateElement(elementToReplace.Value);
                newElement.InnerHtml = element.InnerHtml;
                CopyAttributesAndAddCssClass(element, newElement, $".{elementToReplace.Key}");
                element.Replace(newElement);
            }
        }
    }

    private static async Task ExtractImages(IHtmlDocument document, IWritableFileSystem output)
    {
        IHtmlCollection<IElement> imageElements = document.QuerySelectorAll("img");
        foreach (IElement imageElement in imageElements)
        {
            string? src = imageElement.GetAttribute("src");
            
            if (string.IsNullOrEmpty(src))
                continue;

            if (ImageResult.TryParse(src, out ImageResult? parsed))
            {
                await output.WiteBase64EncodedFile(parsed.OriginalName, parsed.Data);
                imageElement.SetAttribute("src", parsed.OriginalName);
            }
        }
    }

    private static void CopyAttributesAndAddCssClass(IElement source, IElement target, string cssClass)
    {
        foreach (var attribute in source.Attributes)
        {
            target.SetAttribute(attribute.Name, attribute.Value);
        }
        target.ClassList.Add(cssClass);
    }

    public override async Task<StepResult> ExecuteAsync(IBookEnvironment environment, ILogger logger)
    {

        logger.LogInformation("Rendering print template...");

        string tempate = await environment.GetTemplate(frontMatterTemplate: null,
                                                       fallbackTemplate: BundledAssets.TemplatePrint,
                                                       defaultTemplateSelector: cfg => cfg.PrintConfig.DefaultTempate);

        var renderer = new TemplateEngine(logger, environment);

        var viewData = new ViewData
        {
            Host = "",
            Content = State.Buffer.ToString(),
            LastModified = DateTime.UtcNow,
            Title = environment.Configuration.BookTitle,
            AdditionalData = new(),
        };


        var rendered = new HtmlParser().ParseDocument(renderer.Render(tempate, viewData));

        logger.LogInformation("Replacing html5 tags with xhtml compatible ones...");
        ReplaceHtml5ElementsWithXhtmlCompatible(rendered);

        logger.LogInformation("Extracting images...");
        await ExtractImages(rendered, environment.Output);

        logger.LogInformation("Moving css into inline atttibutes...");
        
        using var pm = new PreMailer.Net.PreMailer(rendered);
        InlineResult result = pm.MoveCssInline(removeStyleElements: false, preserveMediaQueries: true);

        if (result.Warnings.Count > 0)
        {
            foreach (string warning in result.Warnings)
            {
                logger.LogWarning("Xhtml: {Warning}", warning);
            }
        }

        logger.LogInformation("Writing xhtml file...");
        await environment.Output.WriteAllTextAsync("print.xhtml.html", result.Html);

        return StepResult.Success;
    }
}
