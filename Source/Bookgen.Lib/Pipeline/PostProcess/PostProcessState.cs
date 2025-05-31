using Bookgen.Lib.Domain.PostProcess;

namespace Bookgen.Lib.Pipeline.PostProcess;

internal sealed class PostProcessState
{
    public PostProcessExport? Export { get; set; }
}
