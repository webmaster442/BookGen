//-----------------------------------------------------------------------------
// (c) 2019-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain.Epub.Ncx;
using BookGen.Framework;

namespace BookGen.GeneratorSteps.Epub;

internal sealed class CreateEpubToc : ITemplatedStep
{
    public ITemplateProcessor? Template { get; set; }
    public IContent? Content { get; set; }

    private void GenerateTocNcx(IReadonlyRuntimeSettings settings, ILog log)
    {
        log.Info("Creating epub toc.ncx...");
        FsPath? output = settings.OutputDirectory.Combine("epubtemp\\OPS\\toc.ncx");
        var toc = new Ncx
        {
            Version = "2005-1",
            Xmlns = "http://www.daisy.org/z3986/2005/ncx/",
            Lang = "en",
            Head = new Head
            {
                Meta = new List<Meta>
                {
                    new Meta
                    {
                        Name = "dtb:uid",
                        Content = "book"
                    },
                }
            },
            DocTitle = new DocTitle
            {
                Text = settings.Configuration.Metadata.Title
            },
            NavMap = new NavMap
            {
                NavPoint = new NavPoint
                {
                    Id = "root",
                    NavLabel = new NavLabel
                    {
                        Text = "Start"
                    },
                    Content = new Content
                    {
                        Src = "nav.xhtml",
                    },
                    NavPoints = FillNavPoints(settings),
                }
            }
        };

        var namespaces = new List<(string prefix, string namespac)>
        {
            ("", "http://www.daisy.org/z3986/2005/ncx/"),
            ("ncx", "http://www.daisy.org/z3986/2005/ncx/")
        };

        output.SerializeXml(toc, log, namespaces);
    }

    private List<NavPoint> FillNavPoints(IReadonlyRuntimeSettings settings)
    {
        var navPoint = new List<NavPoint>();
        int filecounter = 1;
        foreach (Link? link in settings.TocContents.GetLinksForChapter())
        {
            navPoint.Add(new NavPoint
            {
                Id = $"navpoint-{filecounter}",
                NavLabel = new NavLabel
                {
                    Text = link.Text
                },
                Content = new Content
                {
                    Src = $"page_{filecounter:D3}.xhtml"
                }

            });
            ++filecounter;
        }
        return navPoint;
    }

    private void GenerateHtmlToc(IReadonlyRuntimeSettings settings, ILog log)
    {
        log.Info("Generating epub TOC...");

        var buffer = new StringBuilder(4096);

        buffer.Append("<nav epub:type=\"toc\" id=\"toc\">\n");

        int index = 1;
        foreach (string? chapter in settings.TocContents.Chapters)
        {
            buffer.AppendFormat("<h1>{0}</h1>\n", chapter);
            buffer.Append("<ol>\n");
            foreach (Link? link in settings.TocContents.GetLinksForChapter(chapter))
            {
                buffer.AppendFormat("<li><a href=\"page_{0:D3}.xhtml\">{1}</a></li>\n", index, link.Text);
                ++index;
            }
            buffer.Append("</ol>\n");
        }

        buffer.Append("</nav>\n");

        FsPath? target = settings.OutputDirectory.Combine($"epubtemp\\OPS\\nav.xhtml");

        Template!.Content = buffer.ToString();
        Template!.Title = "";

        string? html = Template.Render();
        target.WriteFile(log, html);
    }

    public void RunStep(IReadonlyRuntimeSettings settings, ILog log)
    {
        if (Content == null)
            throw new DependencyException(nameof(Content));

        if (Template == null)
            throw new DependencyException(nameof(Template));

        GenerateTocNcx(settings, log);
        GenerateHtmlToc(settings, log);
    }
}