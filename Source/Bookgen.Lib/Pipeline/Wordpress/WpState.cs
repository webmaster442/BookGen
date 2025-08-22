//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------


using Bookgen.Lib.Domain.Wordpress;

namespace Bookgen.Lib.Pipeline.Wordpress;

internal sealed class WpState
{
    public Channel? CurrentChannel { get; set; }
}
