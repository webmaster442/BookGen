//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ServiceModel.Syndication;

using Microsoft.Extensions.Logging;

namespace Bookgen.Lib.Pipeline.Feed;

internal sealed class CreateFeed : PipeLineStep<SyndicationFeedState>
{
    public CreateFeed(SyndicationFeedState state) : base(state)
    {
    }

    public override Task<StepResult> ExecuteAsync(IBookEnvironment environment, ILogger logger)
    {
        logger.LogInformation("Creating feed...");

        State.Feed.Title = new TextSyndicationContent(environment.Configuration.BookTitle);
        State.Feed.Description = new TextSyndicationContent(environment.Configuration.BookAuthor);
        State.Feed.Copyright = new TextSyndicationContent($"© {DateTime.UtcNow.Year} {environment.Configuration.BookAuthor}");
        State.Feed.Language = environment.Configuration.Book2LetterISO639Language;
        State.Feed.Generator = "BookGen";
        State.Feed.LastUpdatedTime = DateTime.UtcNow;

        return Task.FromResult(StepResult.Success);
    }
}
