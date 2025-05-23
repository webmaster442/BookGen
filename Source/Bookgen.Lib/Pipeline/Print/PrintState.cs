using System.Text;

namespace Bookgen.Lib.Pipeline.Print;

internal class PrintState
{
    public StringBuilder Buffer { get; } = new StringBuilder(1024 * 1024);
}
