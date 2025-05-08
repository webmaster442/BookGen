//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.DomainServices.Markdown;

namespace BookGen.WebGui.Services;

internal sealed class MarkdownRenderer : IMarkdownRenderer
{
    private readonly ICurrentSession _currentSession;
    private readonly BookGenPipeline _pipeline;

    public MarkdownRenderer(ICurrentSession currentSession)
    {
        _currentSession = currentSession;
        _pipeline = new BookGenPipeline(BookGenPipeline.Preview);
        _pipeline.InjectPath(_currentSession.StartDirectory);
        _pipeline.SetSyntaxHighlightTo(false);
    }

    public void Dispose()
    {
        _pipeline.Dispose();
    }

    public string RenderMarkdown(string content)
    {
        return _pipeline.RenderMarkdown(content);
    }
}
