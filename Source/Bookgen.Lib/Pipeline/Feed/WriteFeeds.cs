//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ServiceModel.Syndication;

using BookGen.Vfs;

using Microsoft.Extensions.Logging;

namespace Bookgen.Lib.Pipeline.Feed;

internal sealed class WriteFeeds : PipeLineStep<SyndicationFeedState>
{
    public WriteFeeds(SyndicationFeedState state) : base(state)
    {
    }

    public override Task<StepResult> ExecuteAsync(IBookEnvironment environment, ILogger logger)
    {
        logger.LogInformation("Writing RSS feed...");
        using (var rss = environment.Output.CreateXmlWriter("rss.xml"))
        {
            Rss20FeedFormatter rssFormatter = new Rss20FeedFormatter(State.Feed);
            rssFormatter.WriteTo(rss);
        }

        logger.LogInformation("Writing Atom feed...");
        using (var atom = environment.Output.CreateXmlWriter("atom.xml"))
        {
            Atom10FeedFormatter atomFormatter = new Atom10FeedFormatter(State.Feed);
            atomFormatter.WriteTo(atom);
        }

        return Task.FromResult(StepResult.Success);
    }
}


/*
 using System.ServiceModel.Syndication;
using System.Xml;

// Create a new syndication feed
SyndicationFeed feed = new SyndicationFeed(
    "Feed Title",
    "Feed Description",
    new Uri("http://example.com"),
    "FeedID",
    DateTime.Now);

// Add items to the feed
SyndicationItem item1 = new SyndicationItem(
    "Title1",
    "Content1",
    new Uri("http://example.com/item1"));
feed.Items = new List<SyndicationItem> { item1 };

// Serialize the feed to RSS format
using (XmlWriter writer = XmlWriter.Create("rss.xml"))
{
    Rss20FeedFormatter rssFormatter = new Rss20FeedFormatter(feed);
    rssFormatter.WriteTo(writer);
}
 */
