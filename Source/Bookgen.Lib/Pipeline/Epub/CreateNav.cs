using System.Text;

using Bookgen.Lib.Domain.Epub;
using Bookgen.Lib.Domain.IO.Configuration;
using Bookgen.Lib.Internals;
using Bookgen.Lib.Templates;

using Microsoft.Extensions.Logging;

namespace Bookgen.Lib.Pipeline.Epub;
internal sealed class CreateNav : PipeLineStep<EpubState>
{
    public CreateNav(EpubState state) : base(state)
    {
    }

    public override Task<StepResult> ExecuteAsync(IBookEnvironment environment, ILogger logger, CancellationToken cancellationToken)
    {
        var ncx = CreateNcx(environment);

        TocRenderer toc = new(new TableOfContentsConfiguration
        {
            ChapterContainer = ContainerElement.None,
        });
        toc.BeginContainer(("epub:type", "toc"));
        foreach (var chapter in State.TocData)
        {
            toc.BeginChapter(chapter.Key);
            toc.BeginOuterItemContainer();
            foreach (var item in chapter.Value)
            {
                toc.AddEpubLink(file: item.FileName.Replace("content/", ""), title: item.Title);
                ncx.NavMap.Add(new NcxNavPoint
                {
                    Id = $"id-{IdGenerator.Generate32BitDeterministicId(item.Title)}",
                    NavLabel = new NcxNavInfoType
                    {
                        Text = item.Title,
                    },
                    Content = new NcxNavPointContent
                    {
                        Src = item.FileName
                    },
                });
            }
            toc.EndOuterItemContainer();
            toc.EndChapter();
        }
        toc.EndContainer();


        var renderer = new TemplateEngine(logger, environment);
        string template = environment.GetAsset("Epub.html");

        var viewData = new ViewData
        {
            Content = toc.ToString().MakeSelfClosingTagsXmlCompatible(),
            Title = environment.Configuration.BookTitle,
            Host = string.Empty,
            LastModified = DateTime.UtcNow,
        };

        string html = renderer.Render(template, viewData);

        State.EpubFile.Add("EPUB/content/nav.xhtml", html, Encoding.UTF8);
        State.EpubFile.AddXml("EPUB/toc.ncx", ncx);

        return Task.FromResult(StepResult.Success);
    }

    private Ncx CreateNcx(IBookEnvironment environment)
    {
        return new Ncx
        {
            DocTitle = new NcxNavInfoType
            {
                Text = environment.Configuration.BookTitle
            },
            Version = "2005-1",
            Head = new List<NcxMeta>
            {
                new()
                {
                    Name = "dtb:uid",
                    Content = $"urn:uuid:{State.BookId}"
                },
                new()
                {
                    Name = "dtb:depth",
                    Content = State.TocData.Count.ToString()
                },
                new()
                {
                    Name = "dtb:totalPageCount",
                    Content = "0"
                },
                new()
                {
                    Name = "dtb:maxPageNumber",
                    Content = "0"
                }
            },
            NavMap = new List<NcxNavPoint>()
        };
    }
}
