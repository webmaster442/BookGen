using System.ServiceModel.Syndication;

namespace Bookgen.Lib.Pipeline.Feed;

internal sealed class SyndicationFeedState
{
    public SyndicationFeed Feed { get; } = new SyndicationFeed();
}
