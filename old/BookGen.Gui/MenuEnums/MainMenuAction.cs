//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Gui.MenuEnums;

public enum MainMenuAction
{
    [Text("ID_ValidateConfig")]
    ValidateConfig,
    [Text("ID_ClearOutputDirectory")]
    ClearOutputDirectory,
    [Text("ID_BuildTest")]
    BuildTest,
    [Text("ID_BuildRelease")]
    BuildRelease,
    [Text("ID_BuildPrint")]
    BuildPrint,
    [Text("ID_BuildEpub")]
    BuildEpub,
    [Text("ID_BuildWordpress")]
    BuildWordpress,
    [Text("ID_BuildPostProc")]
    BuildPostProc,
    [Text("ID_Serve")]
    Serve,
    [Text("ID_PreviewServer")]
    PreviewServer,
    [Text("ID_Stat")]
    Stat,
    [Text("ID_Help")]
    Help,
    [Text("ID_Exit")]
    Exit,
}
