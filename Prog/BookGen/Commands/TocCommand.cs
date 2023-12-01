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
        if (!loader.LoadProject())
        {
            return Constants.GeneralError;
        }

        var files = TocUtils.GetLinks(new FsPath(arguments.Directory), _log);

        HashSet<Link> tocLinks = new HashSet<Link>(loader.Toc, TocUtils.LinkTargetComparer);
        HashSet<Link> fileLinks = new HashSet<Link>(files, TocUtils.LinkTargetComparer);

        var items = fileLinks.Except(tocLinks, TocUtils.LinkTargetComparer).ToArray();

        return Constants.GeneralError;
    }
}
