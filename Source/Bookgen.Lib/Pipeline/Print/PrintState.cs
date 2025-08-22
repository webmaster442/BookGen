//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text;

namespace Bookgen.Lib.Pipeline.Print;

internal class PrintState
{
    public StringBuilder Buffer { get; } = new StringBuilder(1024 * 1024);
}
