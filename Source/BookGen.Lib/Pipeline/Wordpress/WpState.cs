//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Lib.Domain.Wordpress;

namespace BookGen.Lib.Pipeline.Wordpress;

internal sealed class WpState
{
    public Channel? CurrentChannel { get; set; }
}
