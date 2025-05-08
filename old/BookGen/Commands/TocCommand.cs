//-----------------------------------------------------------------------------
// (c) 2023-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.CommandArguments;
using BookGen.Infrastructure;
using BookGen.ProjectHandling;

namespace BookGen.Commands;

[CommandName("toc")]
internal sealed class TocCommand : Command<BookGenArgumentBase>
{
    private readonly ILogger _log;
    private readonly ProgramInfo _programInfo;

    public TocCommand(ILogger log, ProgramInfo programInfo)
    {
        _log = log;
        _programInfo = programInfo;
    }

    public override int Execute(BookGenArgumentBase arguments, string[] context)
    {
        _programInfo.EnableVerboseLogingIfRequested(arguments);

        ProjectLoader loader = new(arguments.Directory, _log, _programInfo);
        bool hasToc = true;

        if (!loader.LoadProject())
        {
            hasToc = false;
            _log.LogWarning("Bookgen project load failed, defaulting to file based search");
        }

        var filePath = new FsPath(arguments.Directory, "toc_scratch.md");

        string[] exclude = Array.Empty<string>();

        if (hasToc)
        {
            exclude =
            [
                loader.Configuration.TOCFile,
                loader.Configuration.Index,
            ];
        }

        IEnumerable<Link> files = TocUtils.GetLinks(new FsPath(arguments.Directory), exclude, _log);

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
                _log.LogInformation("Writing toc Info to: {filePath}", filePath);
                WriteItems(stream, byTarget, _log, "Not found in toc:");
                WriteItems(stream, byTitle, _log, "\r\nFound, but with differtent title:");
            }
        }
        else
        {
            using (var stream = filePath.CreateStreamWriter(_log))
            {
                _log.LogInformation("Writing toc Info to: {file}", filePath);
                WriteItems(stream, files, _log, "Not found in toc:");
            }
        }

        return Constants.Succes;
    }

    private static void WriteItems(StreamWriter stream,
                                   IEnumerable<Link> items,
                                   ILogger _log,
                                   string title)
    {
        int count = 0;
        stream.WriteLine(title);
        stream.WriteLine();
        foreach (var item in items)
        {
            stream.WriteLine(TocUtils.ToMarkdownLink(item));
            ++count;
        }
        _log.LogInformation("Wrote {count} items into file", count);
    }
}
