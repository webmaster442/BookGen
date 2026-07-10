//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel;
using System.Globalization;
using System.Reflection;

using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Commands.Book;
using BookGen.Commands.Build;
using BookGen.Commands.Convert;
using BookGen.Lib.Domain.VsCode;
using BookGen.Vfs;

namespace BookGen.Commands.Folder;

[CommandName("vscode")]
[Description("Creates Vs Code task and extension files")]
[ExitCode(ExitCodes.Success, "The command completed successfully.")]
internal sealed class VsCodeCommand : AsyncCommand<VsCodeCommand.Arguments>
{
    internal sealed class Arguments : BookGenArgumentBase
    {
        [Switch("t", "tasks", Required = false)]
        [Description("Create tasks.json file for VS Code")]
        public bool CreateTasks { get; set; }

        [Switch("e", "extensions", Required = false)]
        [Description("Create extensions.json file for VS Code")]
        public bool CreateExtensions { get; set; }
    }

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

        public VsCodeTaskBuilder AddBookgenTask<TCommand, TArguments>(string name, string description, TArguments arguments)
            where TCommand: ICommand
            where TArguments : ArgumentsBase
        {
            string exePath = Environment.ProcessPath
                ?? throw new InvalidOperationException("Path is not available");

            _tasks.Tasks.Add(new VsCodeTask
            {
                Label = name,
                Detail = description,
                Command = exePath,
                Args = GetArguments<TCommand, TArguments>(arguments),
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

        private string[] GetArguments<TCommand, TArguments>(TArguments arguments) 
            where TCommand : ICommand 
            where TArguments : ArgumentsBase
        {
            var args = new List<string>();

            var name = (typeof(TCommand).GetCustomAttribute<CommandNameAttribute>()?.Name)
                ?? throw new InvalidOperationException("Command name is not available");

            args.Add(name);

            PropertyInfo[] properties = typeof(TArguments).GetProperties();
            
            List<(string @switch, string value)> switches = new();
            List<(int position, string value)> positional = new();
            
            foreach (PropertyInfo property in properties)
            {
                SwitchAttribute? switchAttribute = property.GetCustomAttribute<SwitchAttribute>();
                ArgumentAttribute? argumentAttribute = property.GetCustomAttribute<ArgumentAttribute>();

                if (switchAttribute != null)
                {
                    object? value = property.GetValue(arguments);
                    if (value != null)
                    {
                        if (value is bool)
                            switches.Add((switchAttribute.LongName, ""));
                        else if (value is IFormattable formattable)
                            switches.Add((switchAttribute.LongName, formattable.ToString(null, CultureInfo.InvariantCulture)));
                        else
                            switches.Add((switchAttribute.LongName, value.ToString() ?? ""));

                    }
                }
                if (argumentAttribute != null)
                {
                    var value = property.GetValue(arguments);
                    if (value != null)
                    {
                        if (value is IFormattable formattable)
                            positional.Add((argumentAttribute.Index, formattable.ToString(null, CultureInfo.InvariantCulture)));
                        else
                            positional.Add((argumentAttribute.Index, value.ToString() ?? ""));
                    }
                }
            }

            foreach ((string @switch, string value) @switch in switches)
            {
                args.Add($"--{@switch.@switch}");

                if (!string.IsNullOrEmpty(@switch.value))
                    args.Add($"\"{@switch.value}\"");
            }

            foreach (var position in positional.OrderBy(p => p.position))
            {
                args.Add($"\"{position.value}\"");
            }

            return args.ToArray();
        }

        public VsCodeTasks Build()
            => _tasks;
    }

    private readonly IWritableFileSystem _writableFileSystem;

    public VsCodeCommand(IWritableFileSystem writableFileSystem)
    {
        _writableFileSystem = writableFileSystem;
    }

    public override async Task<int> ExecuteAsync(Arguments arguments, IReadOnlyList<string> context, CancellationToken token)
    {
        if (!arguments.CreateTasks &&
            !arguments.CreateExtensions)
        {
            arguments.CreateTasks = true;
            arguments.CreateExtensions = true;
        }

        if (arguments.CreateExtensions)
        {
            var extensionFile = Path.Combine(arguments.Directory, ".vscode", "extensions.json");
            RecommendedExtensions extensions = new()
            {
                Recommendations = new[]
                {
                    "yzhang.markdown-all-in-one",
                    "DavidAnson.vscode-markdownlint",
                }
            };
            await _writableFileSystem.SerializeAsync(extensionFile, extensions, writeSchema: false);
        }

        if (arguments.CreateTasks)
        {
            var file = Path.Combine(arguments.Directory, ".vscode", "tasks.json");
            VsCodeTaskBuilder taskBuilder = new();

            taskBuilder
                .AddBookgenTask<Md2HtmlCommand, Md2HtmlCommand.Arguments>("Md2html", "Render to html", new()
                {
                    InputFiles = [VsCodeVars.File],
                    OutputFile = $"{VsCodeVars.File}.html"
                });

            taskBuilder
                .AddBookgenTask<StatsCommand, BookGenArgumentBase>("Get statistics", "Statistics information", new()
                {
                    Directory = VsCodeVars.WorkspaceFolder
                });

            taskBuilder
                .AddBookgenTask<BuildWebCommand, BuildArguments>("Build web", "Build static website", new()
                {
                    Directory = VsCodeVars.WorkspaceFolder,
                    OutputDirectory = "Output/Web"
                });

            taskBuilder
                .AddBookgenTask<BuildPrintCommand, BuildArguments>("Build print", "Build printable html", new()
                {
                    Directory = VsCodeVars.WorkspaceFolder,
                    OutputDirectory = "Output/Print"
                });

            taskBuilder
                .AddBookgenTask<BuildWordpressCommand, BuildArguments>("Build wordpress", "Build wordpress export xml", new()
                {
                    Directory = VsCodeVars.WorkspaceFolder,
                    OutputDirectory = "Output/Wp"
                });

            taskBuilder
                .AddBookgenTask<BuildEpub, BuildArguments>("Build e-pub", "Build e-pub export", new()
                {
                    Directory = VsCodeVars.WorkspaceFolder,
                    OutputDirectory = "Output/Epub"
                });

            await _writableFileSystem.SerializeAsync(file, taskBuilder.Build(), writeSchema: false);
        }

        return ExitCodes.Success;
    }
}
