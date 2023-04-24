//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.CommandArguments;

internal class TasksArgument : BookGenArgumentBase
{
    [Switch("c", "create")]
    public bool Create { get; set; }
}
