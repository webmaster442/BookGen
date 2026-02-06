using Markdig.Helpers;
using Markdig.Syntax;

namespace Bookgen.Lib.Markdown.Renderers.Terminal;

internal sealed class CodeBlockRenderer : TerminalObjectRenderer<CodeBlock>
{
    protected override void Write(TerminalRenderer renderer, CodeBlock obj)
    {
        if (obj?.Lines.Lines != null)
        {
            string begin = renderer.Builder
                .New()
                .WithBackgroundColor(renderer.RenderOptions.CodeBlockBackground)
                .WithForegroundColor(renderer.RenderOptions.CodeBlockColor)
                .ToString();

            renderer.Write(begin);

            for (int i = 0; i < obj.Lines.Count; i++)
            {
                StringLine codeLine = obj.Lines.Lines[i];
                if (!string.IsNullOrWhiteSpace(codeLine.ToString()))
                {
                    if (i == obj.Lines.Count - 1)
                    {
                        renderer.Write(codeLine.ToString()).WriteReset().WriteLine();
                    }
                    else
                    {
                        renderer.WriteLine(codeLine.ToString());
                    }

                }
            }

            renderer.WriteLine();
        }
    }
}
