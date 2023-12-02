//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.CommandArguments;
using BookGen.Infrastructure;
using BookGen.ProjectHandling;

namespace BookGen.Commands;

[CommandName("toc")]
internal sealed class TocCommand : Command<BookGenArgumentBase>
{
    private readonly ILog _log;
    private readonly ProgramInfo _programInfo;

    public TocCommand(ILog log, ProgramInfo programInfo)
    {
        _log = log;
        _programInfo = programInfo;
    }

    public override int Execute(BookGenArgumentBase arguments, string[] context)
    {
        _log.EnableVerboseLogingIfRequested(arguments);

        ProjectLoader loader = new(arguments.Directory, _log, _programInfo);
        bool hasToc = true;

        if (!loader.LoadProject())
        {
            hasToc = false;
            _log.Warning("Bookgen project load failed, defaulting to file based search");
        }

        var filePath = new FsPath(arguments.Directory, "toc_scratch.md");

        IEnumerable<Link> files = TocUtils.GetLinks(new FsPath(arguments.Directory), _log);

        if (hasToc)
        {
            HashSet<Link> tocLinks = new HashSet<Link>(loader.Toc, TocUtils.LinkTargetComparer);
            HashSet<Link> fileLinks = new HashSet<Link>(files, TocUtils.LinkTargetComparer);

            IEnumerable<Link> byTarget = fileLinks
                .Except(tocLinks, TocUtils.LinkTargetComparer)
                .OrderBy(l => l.Url);

            IEnumerable<Link> byTitle = fileLinks
                .Except(tocLinks, TocUtils.LinkTitleComparer)
                .OrderBy(l => l.Url);

            using (var stream = filePath.CreateStreamWriter(_log))
            {
                _log.Info("Writing toc Info to: {0}", filePath);
                WriteItems(stream, byTarget, "Not found in toc:");
                WriteItems(stream, byTitle, "Found, but with differtent title");
            }
        }
        else
        {
            using (var stream = filePath.CreateStreamWriter(_log))
            {
                _log.Info("Writing toc Info to: {0}", filePath);
                WriteItems(stream, files, "Not found in toc:");
            }
        }

        return Constants.Succes;
    }

    private static void WriteItems(StreamWriter stream, IEnumerable<Link> items, string title)
    {
        stream.WriteLine(title);
        stream.WriteLine();
        foreach (var item in items)
        {
            stream.WriteLine(TocUtils.ToMarkdownLink(item));
        }
    }
}
