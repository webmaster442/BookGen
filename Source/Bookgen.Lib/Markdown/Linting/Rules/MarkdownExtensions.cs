using Markdig.Syntax;

namespace Bookgen.Lib.Markdown.Linting.Rules;

internal static class MarkdownExtensions
{
    public static IEnumerable<Block> AllBlocks(this MarkdownDocument document)
    {
        foreach (MarkdownObject block in document.Descendants())
        {
            if (block is Block b)
                yield return b;
        }
    }

    public static IEnumerable<TBlock> AllBlocks<TBlock>(this MarkdownDocument document)
        where TBlock: Block
    {
        foreach (MarkdownObject block in document.Descendants())
        {
            if (block is TBlock b)
                yield return b;
        }
    }

    public static IEnumerable<Block> Descendants(this Block block)
    {
        yield return block;

        if (block is ContainerBlock container)
        {
            foreach (Block child in container)
            {
                foreach (Block descendant in Descendants(child))
                {
                    yield return descendant;
                }
            }
        }
    }
}
