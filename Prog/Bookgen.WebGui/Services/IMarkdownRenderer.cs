﻿//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.WebGui.Services;

public interface IMarkdownRenderer : IDisposable
{
    string RenderMarkdown(string content);
}