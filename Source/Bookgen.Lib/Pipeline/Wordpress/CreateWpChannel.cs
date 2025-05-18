
using Bookgen.Lib.Domain.Wordpress;
using Bookgen.Lib.Internals;

using Microsoft.Extensions.Logging;

namespace Bookgen.Lib.Pipeline.Wordpress;

internal sealed class CreateWpChannel : IPipeLineStep<Session>
{
    public Session State { get; }

    public CreateWpChannel(Session state)
    {
        State = state;
    }

    public Task<StepResult> ExecuteAsync(IBookEnvironment environment, ILogger logger, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating channel...");
        State.CurrentChannel = new Channel
        {
            Title = environment.Configuration.BookTitle,
            Link = environment.Configuration.WordpressConfig.DeployHost,
            PubDate = DateTime.UtcNow.ToWordpressTime(),
            Language = "hu",
            WxrVersion = "1.2",
            BaseSiteUrl = environment.Configuration.WordpressConfig.DeployHost,
            BaseBlogUrl = environment.Configuration.WordpressConfig.DeployHost,
            Generator = "BookGen",
            Description = string.Empty,
            Author = new Author
            {
                AuthorDisplayName = "Bookgen generator",
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