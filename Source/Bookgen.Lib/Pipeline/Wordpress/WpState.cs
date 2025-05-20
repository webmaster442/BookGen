
using Bookgen.Lib.Domain.Wordpress;

namespace Bookgen.Lib.Pipeline.Wordpress;

internal sealed class WpState
{
    public Channel? CurrentChannel { get; set; }
}
