﻿//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Gui;
using BookGen.Gui.MenuEnums;

namespace BookGen.ConsoleUi;

internal sealed class InitMenu : MenuBase
{
    private readonly ILogger _log;
    private readonly FsPath _workDir;
    private readonly ProgramInfo _programInfo;

    public InitMenu(ILogger log, FsPath WorkDir, ProgramInfo programInfo)
    {
        _log = log;
        _workDir = WorkDir;
        _programInfo = programInfo;
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
        if (createConfig)
        {
            renderer.PrintText("Creating and configuring config file...");
            bool configInYaml = configFormat == InitConfigFormat.ConfigYaml;

            InitializerMethods.CreateConfig(_log,
                                            _workDir,
                                            configInYaml,
                                            createMdFiles,
                                            createTemplates,
                                            _programInfo.ConfigVersion);
        }
    }
}
