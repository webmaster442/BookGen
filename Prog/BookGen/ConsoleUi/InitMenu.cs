using BookGen.Gui;
using BookGen.Gui.MenuEnums;
using BookGen.Interfaces;

namespace BookGen.ConsoleUi;

internal sealed class InitMenu : MenuBase
{
    private readonly ILog _log;
    private readonly FsPath _workDir;

    public InitMenu(ILog log, FsPath WorkDir)
    {
        _log = log;
        _workDir = WorkDir;
    }

    protected override async Task OnRender(Renderer renderer)
    {
        renderer.FigletText("BookGen init", ConsoleColor.Magenta);
        renderer.BlankLine();
        renderer.PrintText("To exit press CTRL+C");
        renderer.BlankLine();

        var options = await renderer.MultiSelectionMenu("Options", true, GetEnumItems<InitMenuAction>());
        var configFormat = await renderer.SelectionMenu("Config format", GetEnumItems<InitConfigFormat>());

        Init(options, configFormat, renderer);
    }

    private void Init(ISet<InitMenuAction> options, InitConfigFormat configFormat, Renderer renderer)
    {
        bool createConfig = options.Contains(InitMenuAction.CreateConfig);
        bool createMdFiles = options.Contains(InitMenuAction.CreateMdFiles);
        bool createTemplates = options.Contains(InitMenuAction.CreateTemplates);
        bool createScripts = options.Contains(InitMenuAction.CreateScripts);

        if (createMdFiles)
        {
            renderer.PrintText("Creating summary.md & index.md...");
            InitializerMethods.DoCreateMdFiles(_log, _workDir);
        }
        if (createTemplates)
        {
            renderer.PrintText("Extracting templates...");
            InitializerMethods.ExtractTemplates(_log, _workDir);
        }
        if (createScripts)
        {
            renderer.PrintText("Creating Script project...");
            InitializerMethods.CreateScriptProject(_log, _workDir, Program.CurrentState.ProgramDirectory);
        }
        if (createConfig)
        {
            renderer.PrintText("Creating and configuring config file...");
            bool configInYaml = configFormat == InitConfigFormat.ConfigYaml;
            InitializerMethods.CreateConfig(_log, _workDir, configInYaml, createMdFiles, createTemplates, createScripts);
        }

        if (options.Contains(InitMenuAction.CreateVsTasks))
        {
            renderer.PrintText("Creating VS Code Tasks");
            InitializerMethods.DoCreateTasks(_log, _workDir);
        }
    }
}
