//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Bookgen.Lib.Domain.PostProcess;

namespace Bookgen.Lib.Pipeline.PostProcess;

internal sealed class PostProcessState
{
    public PostProcessExport? Export { get; set; }
}
