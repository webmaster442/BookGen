using System.Diagnostics;
using System.Globalization;
using System.Text;

using Bookgen.Lib.Domain.Wordpress;
using Bookgen.Lib.ImageService;
using Bookgen.Lib.Internals;
using Bookgen.Lib.Markdown;
using Bookgen.Lib.Templates;

using Microsoft.Extensions.Logging;

namespace Bookgen.Lib.Pipeline.Wordpress;

internal sealed class CreateWpPages : IPipeLineStep<Session>
{
    public Session State { get; }

#if DEBUG
    private readonly HashSet<int> _usedids;
#endif

    public CreateWpPages(Session state)
    {
        State = state;
#if DEBUG
        _usedids = [];
#endif
    }

    private static string EncodeTitle(string title)
    {
        string? normalizedString = title.Trim().Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder();

        foreach (char c in normalizedString)
        {
            UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(c);
            }
        }
        return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
    }

    private Item CreateItem(int uid,
                            int parent,
                            int order,
                            string content,
                            string title,
                            string path,
                            IBookEnvironment environment)
    {
#if DEBUG
        if (_usedids.Contains(uid))
        {
            //UID mismatch. Some kind of generator error
            Debugger.Break();
        }
        _usedids.Add(uid);
#endif

        var result = new Item
        {
            Content = content,
            Title = title,
            PubDate = DateTime.Now.ToWordpressTime(),
            Post_date = DateTime.Now.ToWordpressPostDate(),
            Post_date_gmt = DateTime.UtcNow.ToWordpressPostDate(),
            Menu_order = order,
            Ping_status = "closed",
            Comment_status = environment.Configuration.WordpressConfig.AllowComments ? "open" : "closed",
            Is_sticky = "0",
            Postmeta =
                    [
                        new Postmeta { Meta_key = "", Meta_value = "" }
                    ],
            Post_password = "",
            Status = "publish",
            Post_name = EncodeTitle(title),
            Post_id = uid,
            Post_parent = parent,
            Post_type = environment.Configuration.WordpressConfig.ItemType,
            Link = path,
            Creator = "bookgen@github.com",
            Description = "",
            Guid = new Domain.Wordpress.Guid
            {
                IsPermaLink = false,
                Text = $"{environment.Configuration.WordpressConfig.DeployHost}?page_id={uid}",
            },
        };
        return result;
    }

    public Task<StepResult> ExecuteAsync(IBookEnvironment environment, ILogger logger, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating pages...");

        var imgService = new ImgService(environment.Source, environment.Configuration.StaticWebsiteConfig.Images);
        var cached = new CachedImageService(imgService);
        var renderer = new TemplateEngine();

        using var settings = new RenderSettings
        {
            CssClasses = environment.Configuration.StaticWebsiteConfig.CssClasses,
            DeleteFirstH1 = false,
            HostUrl = environment.Configuration.StaticWebsiteConfig.DeployHost,
            PrismJsInterop = null,
        };

        using var markdown = new MarkdownToHtml(cached, settings);

        var files = environment.TableOfContents.Chapters.SelectMany(x => x.Files);

        int mainorder = 0;
        int uid = 2000;
        int globalparent = 0;

        string fillerPage = "";
        string title = environment.Configuration.BookTitle;
        string path = $"{environment.Configuration.WordpressConfig.DeployHost}{EncodeTitle(title)}";
        Item parent = CreateItem(uid, 0, mainorder, fillerPage, title, path, environment);
        State.CurrentChannel!.Item.Add(parent);
        globalparent = uid;
        ++uid;

        foreach (var chapter in environment.TableOfContents.Chapters)
        {
            foreach (var file in chapter.Files)
            {
                logger.LogDebug("Processing file {File}...", file);
                if (cancellationToken.IsCancellationRequested)
                {
                    logger.LogWarning("Cancellation requested. Stoping...");
                    return Task.FromResult(StepResult.Failure);
                }

                var sourceData = environment.Source.GetSourceFile(file, logger);



            }
        }


        return Task.FromResult(StepResult.Success);
    }
}
