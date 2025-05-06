using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bookgen.Lib.ImageService;

using Markdig;
using Markdig.Renderers;
using Markdig.Syntax;

namespace Bookgen.Lib.Markdown;
public sealed class BookGenExtension : IMarkdownExtension, IDisposable
{
    private readonly IImgService _imgService;
    private MarkdownPipelineBuilder? _pipeline;

    public BookGenExtension(IImgService imgService)
    {
        _imgService = imgService;
    }

    public void Dispose()
    {
        if (_pipeline != null)
        {
            _pipeline.DocumentProcessed -= OnDocumentProcessed;
            _pipeline = null;
        }
    }

    public void Setup(MarkdownPipelineBuilder pipeline)
    {
        _pipeline = pipeline;
        _pipeline.DocumentProcessed += OnDocumentProcessed;
    }

    public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
    {
        throw new NotImplementedException();
    }

    private void OnDocumentProcessed(MarkdownDocument document)
    {
        throw new NotImplementedException();
    }
}
