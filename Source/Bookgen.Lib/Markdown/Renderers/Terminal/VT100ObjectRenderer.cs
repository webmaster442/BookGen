// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Markdig.Renderers;
using Markdig.Syntax;

namespace Bookgen.Lib.Markdown.Renderers.Terminal;

/// <summary>
/// Implement the MarkdownObjectRenderer with VT100Renderer.
/// </summary>
/// <typeparam name="T">The element type of the renderer.</typeparam>
public abstract class VT100ObjectRenderer<T> : MarkdownObjectRenderer<VT100Renderer, T> where T : MarkdownObject
{
}
