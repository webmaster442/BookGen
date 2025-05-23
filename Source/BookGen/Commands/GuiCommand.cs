using BookGen.Cli;
using BookGen.Cli.Annotations;
using BookGen.Infrastructure.Terminal;
using BookGen.Vfs;

using Spectre.Console;

namespace BookGen.Commands;

[CommandName("gui")]
internal class GuiCommand : AsyncCommand<BookGenArgumentBase>
{
    private readonly IWritableFileSystem _fileSystem;
    private readonly ICommandRunnerProxy _commandRunnerProxy;

    private BookGenArgumentBase? _currentArgs;

    public GuiCommand(IWritableFileSystem writableFileSystem, ICommandRunnerProxy commandRunnerProxy)
    {
        _fileSystem = writableFileSystem;
        _commandRunnerProxy = commandRunnerProxy;
    }

    public override async Task<int> ExecuteAsync(BookGenArgumentBase arguments, IReadOnlyList<string> context)
    {
        AnsiConsole.Clear();

        _currentArgs = arguments;
        _fileSystem.Scope = arguments.Directory;

        var figlet = new FigletText("BookGen");
        figlet.Justification = Justify.Center;
        AnsiConsole.Write(figlet);

        var path = new TextPath(_fileSystem.Scope)
            .RootColor(Color.Red)
            .SeparatorColor(Color.Green)
            .StemColor(Color.Blue)
            .LeafColor(Color.Yellow);

        path.Justification = Justify.Left;

        AnsiConsole.Write("Work directory: ");
        AnsiConsole.Write(path);
        AnsiConsole.WriteLine();
        AnsiConsole.WriteLine();

        MenuItem[] menu =
        [
            new(Emoji.Known.RedQuestionMark, "Validate current configuration", OnValidate),
            new(Emoji.Known.SpiderWeb, " Start a webserver in curent directory", OnServe),
            new(Emoji.Known.Door, "Exit", OnExit)
        ];

        var selector = new SelectionPrompt<MenuItem>()
            .Title("Select an action:")
            .PageSize(10)
            .AddChoices(menu)
            .UseConverter(mi => mi.ToString());

        var selected = AnsiConsole.Prompt(selector);
        return await selected.ExecuteAsync();
    }

    private async Task<int> OnValidate()
    {
        if (_currentArgs == null)
            throw new InvalidOperationException("Command not initialized");

        return await _commandRunnerProxy.RunCommand("validate", ["-d", _currentArgs.Directory]);
    }

    private async Task<int> OnServe()
    {
        if (_currentArgs == null)
            throw new InvalidOperationException("Command not initialized");

        return await _commandRunnerProxy.RunCommand("serve", ["-d", _currentArgs.Directory]);
    }
    private Task<int> OnExit()
    {
        Environment.Exit(ExitCodes.Succes);
        return Task.FromResult(ExitCodes.Succes);
    }
}
