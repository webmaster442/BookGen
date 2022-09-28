//-----------------------------------------------------------------------------
// (c) 2019-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Framework;
using BookGen.Interfaces;

namespace BookGen.GeneratorSteps
{
    internal sealed class CreateToCForWebsite : IGeneratorContentFillStep
    {
        public IContent? Content { get; set; }

        public void RunStep(IReadonlyRuntimeSettings settings, ILog log)
        {
            if (Content == null)
                throw new DependencyException(nameof(Content));

            log.Info("Generating Table of Contents...");
            var toc = new StringBuilder();
            foreach (string? chapter in settings.TocContents.Chapters)
            {
                toc.Append("<details open=\"true\">");
                toc.AppendFormat("<summary>{0}</summary>", chapter);
                toc.Append("<ul>");
                foreach (Link? link in settings.TocContents.GetLinksForChapter(chapter))
                {
                    toc.AppendFormat("<li><a href=\"{0}\">{1}</a></li>", link.ConvertToLinkOnHost(settings.Configuration.HostName), link.Text);
                }
                toc.Append("</ul>");
                toc.Append("</details>");
            }
            Content.TableOfContents = toc.ToString();
        }
    }
}
