//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Lib.Domain.PostProcess;

namespace BookGen.Lib.Pipeline.PostProcess;

internal sealed class PostProcessState
{
    public PostProcessExport? Export { get; set; }
}
