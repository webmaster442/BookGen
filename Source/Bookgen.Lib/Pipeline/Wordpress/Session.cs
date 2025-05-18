
using Bookgen.Lib.Domain.Wordpress;

namespace Bookgen.Lib.Pipeline.Wordpress;

internal sealed class Session
{
    public Channel? CurrentChannel { get; set; }
}
