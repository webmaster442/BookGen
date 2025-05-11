//-----------------------------------------------------------------------------
// (c) 2023-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Shellprog.CommandCode;

internal sealed class SelectionItemAction : SelectionItemBase
{
    public required Action Action { get; init; }
}
