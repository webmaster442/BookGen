//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Shellprog.CommandCode.Cdg;

internal sealed class SelectionItemAction : SelectionItemBase
{
    public required Action Action { get; init; }
}
