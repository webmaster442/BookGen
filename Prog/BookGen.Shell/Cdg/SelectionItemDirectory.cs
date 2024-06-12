﻿//-----------------------------------------------------------------------------
// (c) 2023-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Shell.Cdg;

internal sealed class SelectionItemDirectory : SelectionItemBase
{
    public required string Path { get; init; }
}
