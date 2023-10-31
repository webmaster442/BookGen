//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Cli;

using Cdg;

using Spectre.Console;

var arguments = new ArgumentParser<CdgArguments>().Parse(args);
var directories = new DirectoriesProvider();
var selector = new DirectorySelectorMenu(arguments.Folder);

var validation = arguments.Validate();
if (!validation.IsOk)
{
    AnsiConsole.WriteLine(validation.ToString());
    return;
}

while (true)
{
    try
    {
        AnsiConsole.Clear();
        IEnumerable<string> items = directories.GetSubdirs(selector.CurrentPath, arguments.ShowHidden);
        SelectionPrompt<string> menu = selector.CreateSelection(items);
        string selected = await menu.ShowAsync(AnsiConsole.Console, CancellationToken.None);

        if (DirectoriesProvider.PathIsCurrentDirString(selected))
        {
            Environment.SetEnvironmentVariable("cdgPath", selector.CurrentPath, EnvironmentVariableTarget.User);
            Environment.Exit(0);
        }
        else if (DirectoriesProvider.PathIsHomeDirString(selected))
        {
            selector.CurrentPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        }
        else if (directories.TryKnownFolder(selected, out string newPath))
        {
            selector.CurrentPath = newPath;
        }
        else if (DirectoriesProvider.TryUpOneDir(selected, selector.CurrentPath, out newPath))
        {
            selector.CurrentPath = newPath;
        }
        else
        {
            selector.CurrentPath = selected;
        }

    }
    catch (Exception ex)
    {
#if DEBUG
        AnsiConsole.WriteException(ex);
#endif
        AnsiConsole.WriteLine(ex.Message);
        var confirm = new ConfirmationPrompt("Press a key to continue").HideChoices();
        await confirm.ShowAsync(AnsiConsole.Console, CancellationToken.None);
    }
}