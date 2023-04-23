//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

using BookGen.CommandArguments;
using BookGen.ConsoleUi;
using BookGen.Domain.TaskRunner;
using BookGen.ProjectHandling;

namespace BookGen.Commands;

internal class TasksCommand : AsyncCommand<TasksArgument>
{
    private readonly BookGenTaskRunner _taskRunner;
    private readonly ILog _log;

    public TasksCommand(BookGenTaskRunner taskRunner, ILog log)
    {
        _taskRunner = taskRunner;
        _log = log;
    }

    public override async Task<int> Execute(TasksArgument arguments, string[] context)
    {
        var files = ProjectFilesLocator.Locate(new FsPath(arguments.Directory));
        if (!files.tasks.IsExisting)
        {
            _log.Warning("project doesn't contain a tasks.xml");
            return Constants.GeneralError;
        }

        using var stream = files.tasks.OpenStreamRead();

        XmlSerializer serializer = new XmlSerializer(typeof(Task[]));
        BookGenTask[]? tasks = serializer.Deserialize(stream) as BookGenTask[];
        if (tasks == null)
        {
            _log.Warning("tasks.xml is empty or corrupted");
            return Constants.GeneralError;
        }

        TasksMenu menu = new TasksMenu(_taskRunner, tasks);
        await menu.Run();

        return Constants.Succes;
    }
}
