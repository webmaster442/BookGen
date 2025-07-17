using System.Text;

using Bookgen.Lib.Domain.Epub;
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

        var tocHtml = new EpubTocRenderer();
        tocHtml.BeginNav();
        tocHtml.AddTitle(environment.Configuration.BookTitle);
        tocHtml.BeginOl(display: false);
        foreach (var chapter in State.TocData)
        {
            tocHtml.BeginChapter(chapter.Key);
            tocHtml.BeginOl();
            foreach (var item in chapter.Value)
            {
                tocHtml.AddItem(item.Title, item.FileName.Replace("content/", ""));
                string id = IdGenerator.Generate32BitDeterministicId(item.FileName);
                ncx.NavMap.Add(new NcxNavPoint
                {
                    Id = $"id-{id}",
                    NavLabel = new NcxNavInfoType
                    {
                        Text = item.Title,
                    },
                    Content = new NcxNavPointContent
                    {
                        Src = item.FileName
                    },
                });
                State.Spine.Itemref.Add(new PackageSpineItemref
                {
                    Idref = $"id-{id}",
                    Linear = State.Spine.Itemref.Count == 0 ? "yes" : null,
                });
            }
            tocHtml.EndOl();
            tocHtml.EndChapter();
        }
        tocHtml.EndOl();
        tocHtml.EndNav();


        var renderer = new TemplateEngine(logger, environment);
        string template = environment.GetAsset("Epub.html");

        var viewData = new ViewData
        {
            Content = tocHtml.ToString().MakeSelfClosingTagsXmlCompatible(),
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
                    Content = "1"
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
