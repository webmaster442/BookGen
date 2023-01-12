using BookGen.Gui;
using BookGen.Gui.MenuEnums;
using BookGen.Infrastructure;
using System.Diagnostics;

namespace BookGen.ConsoleUi;

internal sealed class MainMenu : MenuBase
{
    private readonly GeneratorRunner _runner;
    private readonly IModuleApi _api;

    public MainMenu(GeneratorRunner runner, IModuleApi api)
    {
        _runner = runner;
        _api = api;
    }

    protected override async Task OnRender(Renderer renderer)
    {
        bool shouldRun = true;
        while (shouldRun)
        {
            renderer.Clear();
            renderer.FigletText("BookGen Gui", ConsoleColor.Green);
            renderer.BlankLine(2);

            var selection = await renderer.SelectionMenu("Select acion", GetEnumItems<MainMenuAction>());
            shouldRun = DoAction(selection, renderer);
        }
    }

    private bool StartModuleInWorkdir(string name)
    {
        _api.ExecuteModule(name, new string[]
        {
                "-d",
                _runner.WorkDirectory
        });
        return true;
    }

    private bool LaunchUpdater()
    {
        using (var p = new Process())
        {
            p.StartInfo.WorkingDirectory = AppContext.BaseDirectory;
            p.StartInfo.FileName = "BookGen.Update.exe";
            p.Start();
        }
        return true;
    }

    private bool DoAction(MainMenuAction selection, Renderer renderer)
    {
        renderer.Clear();
        return selection switch
        {
            MainMenuAction.ValidateConfig => _runner.Initialize(),
            MainMenuAction.ClearOutputDirectory => _runner.InitializeAndExecute(x => x.DoClean()),
            MainMenuAction.BuildTest => _runner.InitializeAndExecute(x => x.DoTest()),
            MainMenuAction.BuildRelease => _runner.InitializeAndExecute(x => x.DoBuild()),
            MainMenuAction.BuildPrint => _runner.InitializeAndExecute(x => x.DoPrint()),
            MainMenuAction.BuildEpub => _runner.InitializeAndExecute(x => x.DoEpub()),
            MainMenuAction.BuildWordpress => _runner.InitializeAndExecute(x => x.DoWordpress()),
            MainMenuAction.BuildPostProc => _runner.InitializeAndExecute(x => x.DoPostProcess()),
            MainMenuAction.Serve => StartModuleInWorkdir("serve"),
            MainMenuAction.PreviewServer => StartModuleInWorkdir("preview"),
            MainMenuAction.Stat => StartModuleInWorkdir("stat"),
            MainMenuAction.Update => LaunchUpdater(),
            MainMenuAction.Help or MainMenuAction.Exit => false,
            _ => throw new InvalidOperationException("Unknown command"),
        };
    }


}
