//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics;

using BookGen.Gui;
using BookGen.Gui.MenuEnums;
using BookGen.Infrastructure;

namespace BookGen.ConsoleUi;

internal sealed class MainMenu : MenuBase
{
    private readonly GeneratorRunner _runner;
    private readonly IModuleApi _api;
    private readonly IHelpProvider _helpProvider;
    private bool _inHelpMode;
    private readonly List<string> _helpItems;

    public MainMenu(GeneratorRunner runner, IModuleApi api, IHelpProvider helpProvider)
    {

        _runner = runner;
        _api = api;
        _helpProvider = helpProvider;
        _helpItems = helpProvider.HelpEntries.Order().ToList();
        _helpItems.Add("<- Back");

    }

    private bool ToggleHelp()
    {
        _inHelpMode = !_inHelpMode;
        return true;
    }

    protected override async Task OnRender(Renderer renderer)
    {
        bool shouldRun = true;
        while (shouldRun)
        {
            renderer.Clear();
            if (_inHelpMode)
            {
                renderer.FigletText("BookGen Help", ConsoleColor.Green);
                var selection = await renderer.SelectionMenu("Select a command to display it's usage: ", _helpItems);
                if (selection == "<- Back")
                {
                    ToggleHelp();
                }
                else
                {
                    renderer.Clear();
                    var help = _helpProvider.GetCommandHelp(selection);
                    await renderer.RenderHelpPages(help);
                }
            }
            else
            {
                renderer.FigletText("BookGen Gui", ConsoleColor.Green);
                renderer.DisplayPath("Work directory", _runner.WorkDirectory);
                renderer.BlankLine();

                var selection = await renderer.SelectionMenu("Select acion:", GetEnumItems<MainMenuAction>());
                shouldRun = DoAction(selection, renderer);
                if (shouldRun && !_inHelpMode)
                    await renderer.WaitKey();
            }
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
            MainMenuAction.Help => ToggleHelp(),
            MainMenuAction.Exit => false,
            _ => throw new InvalidOperationException("Unknown command"),
        };
    }
}
