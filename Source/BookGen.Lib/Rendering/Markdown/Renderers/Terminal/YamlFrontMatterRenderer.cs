//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Markdig.Extensions.Yaml;

namespace BookGen.Lib.Rendering.Markdown.Renderers.Terminal;

internal sealed class YamlFrontMatterRenderer : TerminalObjectRenderer<YamlFrontMatterBlock>
{
    protected override void Write(TerminalRenderer renderer, YamlFrontMatterBlock obj)
    {
        // Do not render anything for YAML front matter in terminal output
    }
}
