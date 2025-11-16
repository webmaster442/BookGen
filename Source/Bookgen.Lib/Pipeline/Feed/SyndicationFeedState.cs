//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ServiceModel.Syndication;

namespace Bookgen.Lib.Pipeline.Feed;

internal sealed class SyndicationFeedState
{
    public SyndicationFeed Feed { get; } = new SyndicationFeed();
}
