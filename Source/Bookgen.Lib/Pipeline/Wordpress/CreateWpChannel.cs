//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Bookgen.Lib.Domain.Wordpress;
using Bookgen.Lib.Internals;

using Microsoft.Extensions.Logging;

namespace Bookgen.Lib.Pipeline.Wordpress;

internal sealed class CreateWpChannel : PipeLineStep<WpState>
{

    public CreateWpChannel(WpState state) : base(state)
    {
    }

    public override Task<StepResult> ExecuteAsync(IBookEnvironment environment, ILogger logger)
    {
        logger.LogInformation("Creating channel...");
        State.CurrentChannel = new Channel
        {
            Title = environment.Configuration.BookTitle,
            Link = environment.Configuration.WordpressConfig.DeployHost,
            PubDate = DateTime.UtcNow.ToWordpressTime(),
            Language = environment.Configuration.Book2LetterISO639Language,
            WxrVersion = "1.2",
            BaseSiteUrl = environment.Configuration.WordpressConfig.DeployHost,
            BaseBlogUrl = environment.Configuration.WordpressConfig.DeployHost,
            Generator = "BookGen",
            Description = string.Empty,
            Author = new Author
            {
                AuthorDisplayName = environment.Configuration.BookAuthor,
                AuthorEmail = "bookgen@github.com",
                AuthorFirstName = "Generator",
                AuthorLastName = "BookGen",
                AuthorId = "1",
                AuthorLogin = "bookgen@github.com"
            },
            Item = new List<Item>()
        };

        return Task.FromResult(StepResult.Success);
    }
}
