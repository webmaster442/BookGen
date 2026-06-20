using Markdig.Syntax;

namespace Bookgen.Lib.Rendering.Markdown.Renderers.Terminal;

internal sealed class ListRenderer : TerminalObjectRenderer<ListBlock>
{
    protected override void Write(TerminalRenderer renderer, ListBlock obj)
    {
        int indent = 0;
        string sub = "";
        Write(renderer, obj, sub, ref indent);
    }

    private static void Write(TerminalRenderer renderer, ListBlock obj, string sub, ref int indent)
    {
        const int indentSize = 2;
        if (!obj.IsOrdered)
        {
            foreach (ListItemBlock item in obj.Cast<ListItemBlock>())
            {
                renderer.Write(new string(' ', indent * indentSize));
                renderer.Write("* ");
                for (int i = 0; i < item.Count; i++)
                {
                    Block subBlock = item[i];
                    if (subBlock is ListBlock subListBlock)
                    {
                        indent++;
                        Write(renderer, subListBlock, "", ref indent);
                    }
                    else
                    {
                        renderer.Render(subBlock);
                    }
                }
            }
        }
        else
        {
            int number = 1;
            foreach (ListItemBlock item in obj.Cast<ListItemBlock>())
            {
                renderer.Write(new string(' ', indent * indentSize))
                    .Write(sub)
                    .Write(number.ToString())
                    .Write(". ");

                for (int i = 0; i < item.Count; i++)
                {
                    Block subBlock = item[i];
                    if (subBlock is ListBlock subListBlock)
                    {
                        indent++;
                        var nsub = $"{sub} {number}.";
                        Write(renderer, subListBlock, nsub, ref indent);
                    }
                    else
                    {
                        renderer.Render(subBlock);
                    }
                }
                ++number;
            }
        }
    }
}
