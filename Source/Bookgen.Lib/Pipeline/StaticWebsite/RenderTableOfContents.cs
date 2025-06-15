using System.Diagnostics;
using System.Text;

using Bookgen.Lib.Domain;
using Bookgen.Lib.Domain.IO;
using Bookgen.Lib.Domain.IO.Configuration;

using BookGen.Vfs;

using Microsoft.Extensions.Logging;

namespace Bookgen.Lib.Pipeline.StaticWebsite;

internal class RenderTableOfContents : PipeLineStep<StaticWebState>
{
    public RenderTableOfContents(StaticWebState staticWebState) : base(staticWebState)
    {
    }

    public override Task<StepResult> ExecuteAsync(IBookEnvironment environment, ILogger logger, CancellationToken cancellationToken)
    {
        TocRenderer toc = new(environment.Configuration.StaticWebsiteConfig);
        toc.BeginContainer();
        foreach (var chapter in environment.TableOfContents.Chapters)
        {
            toc.BeginChapter(chapter);
            toc.BeginOuterItemContainer();
            foreach (var file in chapter.Files)
            {
                string renderedLink = toc.Render(file, environment.Output, State.SourceFiles[file]);
                State.TocLinks.Add(renderedLink);
            }
            toc.EndOuterItemContainer();
            toc.EndChapter();
        }
        toc.EndContainer();

        State.Toc = toc.ToString();

        return Task.FromResult(StepResult.Success);
    }

    internal sealed class TocRenderer
    {
        private readonly StringBuilder _buffer;
        private readonly StaticWebsiteConfig _configuration;

        public TocRenderer(StaticWebsiteConfig configuration)
        {
            _buffer = new StringBuilder(4096);
            _configuration = configuration;
        }

        public override string ToString()
            => _buffer.ToString();

        private static string ToHtml(ContainerElement element)
        {
            return Enum.GetName<ContainerElement>(element)?.ToLower()
                ?? throw new UnreachableException();
        }

        private static string ToHtml(ItemContainer itemContainer)
        {
            return itemContainer switch
            {
                ItemContainer.UnorderedList => "li",
                ItemContainer.OrderedList => "li",
                ItemContainer.Details => "",
                ItemContainer.Paragraph => "p",
                ItemContainer.Span => "span",
                _ => throw new UnreachableException()
            };
        }

        public void BeginContainer()
        {
            _buffer
            .Append('<')
                .Append(ToHtml(_configuration.TocConfiguration.ContainerElement));

            if (!string.IsNullOrEmpty(_configuration.TocConfiguration.ContainerId))
            {
                _buffer
                    .Append(" id=\"")
                    .Append(_configuration.TocConfiguration.ContainerId)
                    .Append('"');
            }

            if (!string.IsNullOrEmpty(_configuration.TocConfiguration.ContainerClass))
            {
                _buffer
                    .Append(" class=\"")
                    .Append(_configuration.TocConfiguration.ContainerClass)
                    .Append('"');
            }

            _buffer.Append('>').AppendLine();
        }

        public void BeginChapter(TocChapter chapter)
        {
            _buffer.Append('<')
                .Append(ToHtml(_configuration.TocConfiguration.ChapterContainer))
                .Append('>')
                .Append("<h1>")
                .Append(chapter.Title)
                .AppendLine("</h1>");

            if (!string.IsNullOrEmpty(chapter.SubTitle))
            {
                _buffer.Append("<h2>")
                    .Append(chapter.SubTitle)
                    .AppendLine("</h2>");
            }
        }

        public void BeginOuterItemContainer()
        {
            switch (_configuration.TocConfiguration.ItemContainer)
            {
                case ItemContainer.UnorderedList:
                    _buffer.AppendLine("<ul>");
                    break;
                case ItemContainer.OrderedList:
                    _buffer.AppendLine("<ol>");
                    break;
                case ItemContainer.Details:
                    _buffer.AppendLine("<details>");
                    break;
            }
        }

        public string Render(string file, IWritableFileSystem output, SourceFile sourceFile)
        {
            string linkTarget = Path.ChangeExtension(Path.Combine(output.Scope, file), ".html");
            linkTarget = Path.GetRelativePath(output.Scope, linkTarget).Replace("\\", "/");
            string linkTitle = sourceFile.FrontMatter.Title;

            string link = $"{_configuration.DeployHost}{linkTarget}";

            _buffer
                .Append('<')
                .Append(ToHtml(_configuration.TocConfiguration.ItemContainer))
                .Append('>')
                .Append($"<a href=\"{link}\">{linkTitle}</a>")
                .Append("</")
                .Append(ToHtml(_configuration.TocConfiguration.ItemContainer))
                .Append('>')
                .AppendLine();

            return link;
        }

        public void EndOuterItemContainer()
        {
            switch (_configuration.TocConfiguration.ItemContainer)
            {
                case ItemContainer.UnorderedList:
                    _buffer.AppendLine("</ul>");
                    break;
                case ItemContainer.OrderedList:
                    _buffer.AppendLine("</ol>");
                    break;
                case ItemContainer.Details:
                    _buffer.AppendLine("</details>");
                    break;
            }
        }


        public void EndChapter()
        {
            _buffer.Append("</")
                .Append(ToHtml(_configuration.TocConfiguration.ChapterContainer))
                .Append('>')
                .AppendLine();
        }

        public void EndContainer()
        {
            _buffer
                .Append("</")
                .Append(ToHtml(_configuration.TocConfiguration.ContainerElement))
                .Append('>')
                .AppendLine();
        }
    }
}
