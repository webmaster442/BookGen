using Bookgen.Lib.Domain.VsCode;

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Vfs;

namespace BookGen.Commands;

[CommandName("vstasks")]
internal sealed class VsTasksCommand : AsyncCommand<BookGenArgumentBase>
{
    private readonly IWritableFileSystem _writableFileSystem;

    private sealed class VsCodeTaskBuilder
    {
        private readonly VsCodeTasks _tasks;

        public VsCodeTaskBuilder()
        {
            _tasks = new VsCodeTasks
            {
                Tasks = new List<VsCodeTask>()
            };
        }

        public VsCodeTaskBuilder AddBookgenTask(string name, string description, string[] args)
        {
            string exePath = Environment.ProcessPath
                ?? throw new InvalidOperationException("Path is not available");

            _tasks.Tasks.Add(new VsCodeTask
            {
                Label = name,
                Detail = description,
                Command = exePath,
                Args = args,
                Group = Group.Build,
                Type = TaskType.Shell,
                Presentation = new()
                {
                    Reveal = Reveal.Always,
                    Panel = PresentationPanel.Dedicated,
                    Clear = true,
                    ShowReuseMessage = true,
                }
            });

            return this;
        }

        public VsCodeTasks Build()
            => _tasks;
    }

    public VsTasksCommand(IWritableFileSystem writableFileSystem)
    {
        _writableFileSystem = writableFileSystem;
    }

    public override async Task<int> ExecuteAsync(BookGenArgumentBase arguments, IReadOnlyList<string> context)
    {
        var file = Path.Combine(arguments.Directory, ".vscode", "tasks.json");

        VsCodeTaskBuilder taskBuilder = new();

        taskBuilder
            .AddBookgenTask("Md2html", "Render to html", ["md2html", "-i", VsCodeVars.File, "-o", $"{VsCodeVars.File}.html"])
            .AddBookgenTask("Get statistics", "Statistics information", ["stats", "-d", VsCodeVars.WorkspaceFolder])
            .AddBookgenTask("Build web", "Build static website", ["buildweb", "-d", VsCodeVars.WorkspaceFolder, "-o", "Output/Web"])
            .AddBookgenTask("Bild print", "Build printable html", ["buildprint", "-d", VsCodeVars.WorkspaceFolder, "-o", "Output/Print"])
            .AddBookgenTask("Build wordpress", "Build wordpress export xml", ["buildwp", "-d", VsCodeVars.WorkspaceFolder,  "-o", "Output/Wp"]);

        await _writableFileSystem.SerializeAsync(file, taskBuilder.Build(), writeSchema: false);

        return ExitCodes.Succes;
    }
}
