//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ServiceModel.Syndication;

namespace BookGen.Lib.Pipeline.Feed;

internal sealed class SyndicationFeedState
{
    public SyndicationFeed Feed { get; } = new SyndicationFeed();
}
