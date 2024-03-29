﻿//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Interfaces
{
    public interface IMarkdownExtensionWithSyntaxToggle
    {
        bool SyntaxEnabled { get; set; }
    }
}
