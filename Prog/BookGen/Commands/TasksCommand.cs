//-----------------------------------------------------------------------------
// (c) 2023-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

using BookGen.CommandArguments;
using BookGen.ConsoleUi;
using BookGen.Domain.TaskRunner;
using BookGen.Infrastructure;
using BookGen.ProjectHandling;

namespace BookGen.Commands;

[CommandName("tasks")]
internal class TasksCommand : AsyncCommand<TasksArgument>
{
    private readonly BookGenTaskRunner _taskRunner;
    private readonly ILogger _log;

    public TasksCommand(BookGenTaskRunner taskRunner, ILogger log)
    {
        _taskRunner = taskRunner;
        _log = log;
    }

    public override async Task<int> Execute(TasksArgument arguments, string[] context)
    {
        _log.EnableVerboseLogingIfRequested(arguments);

        ProjectFiles files = ProjectFilesLocator.Locate(new FsPath(arguments.Directory));
        if (!files.TasksXml.IsExisting)
        {
            if (arguments.Create)
            {
                var items = BookGenTaskFactory.CreateSample();
                files.TasksXml.SerializeXml(items, _log);
            }
            else
            {
                _log.LogWarning("project doesn't contain a tasks.xml. Exiting");
                _log.LogInformation("To create a sample tasks.xml run this command with the -c option");
                return Constants.GeneralError;
            }
        }

        using var stream = files.TasksXml.OpenStreamRead();

        XmlSerializer serializer = new XmlSerializer(typeof(BookGenTasks));
        BookGenTasks? tasks = serializer.Deserialize(stream) as BookGenTasks;
        if (tasks == null)
        {
            _log.LogWarning("tasks.xml is empty or corrupted");
            return Constants.GeneralError;
        }

        TasksMenu menu = new TasksMenu(_taskRunner, arguments.Directory, tasks.Tasks);
        await menu.Run();

        return Constants.Succes;
    }
}
