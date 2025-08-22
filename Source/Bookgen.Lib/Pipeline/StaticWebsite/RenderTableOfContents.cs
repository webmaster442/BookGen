//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

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
        TocRenderer toc = new(environment.Configuration.StaticWebsiteConfig.TocConfiguration);
        toc.BeginContainer();
        foreach (var chapter in environment.TableOfContents.Chapters)
        {
            toc.BeginChapter(chapter.Title);
            toc.BeginOuterItemContainer();
            foreach (var file in chapter.Files)
            {
                string renderedLink = toc.Add(file: file,
                                              output: environment.Output,
                                              title: State.SourceFiles[file].FrontMatter.Title,
                                              host: environment.Configuration.StaticWebsiteConfig.DeployHost);

                State.TocLinks.Add(renderedLink);
            }
            toc.EndOuterItemContainer();
            toc.EndChapter();
        }
        toc.EndContainer();

        State.Toc = toc.ToString();

        return Task.FromResult(StepResult.Success);
    }
}
