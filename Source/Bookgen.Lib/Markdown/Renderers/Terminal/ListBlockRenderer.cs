// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Markdig.Syntax;

namespace Bookgen.Lib.Markdown.Renderers.Terminal;

/// <summary>
/// Renderer for adding VT100 escape sequences for list blocks.
/// </summary>
internal class ListBlockRenderer : VT100ObjectRenderer<ListBlock>
{
    protected override void Write(VT100Renderer renderer, ListBlock obj)
    {
        // start index of a numbered block.
        int index = 1;

        foreach (var item in obj)
        {
            if (item is ListItemBlock listItem)
            {
                if (obj.IsOrdered)
                {
                    RenderNumberedList(renderer, listItem, index++);
                }
                else
                {
                    renderer.Write(listItem);
                }
            }
        }

        renderer.WriteLine();
    }

    private static void RenderNumberedList(VT100Renderer renderer, ListItemBlock block, int index)
    {
        // For a numbered list, we need to make sure the index is incremented.
        foreach (var line in block)
        {
            if (line is ParagraphBlock paragraphBlock && paragraphBlock.Inline != null)
            {
                renderer.Write(index.ToString()).Write(". ").Write(paragraphBlock.Inline);
            }
        }
    }
}
