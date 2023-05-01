using Cdg;

using Spectre.Console;

var selector = new DirectorySelectorMenu(Environment.CurrentDirectory);

while (true)
{
    try
    {
        AnsiConsole.Clear();
        IEnumerable<string> items = DirectoriesProvider.GetSubdirs(selector.CurrentPath, false);
        SelectionPrompt<string> menu = selector.CreateSelection(items);
        string selected = await menu.ShowAsync(AnsiConsole.Console, CancellationToken.None);

        if (DirectoriesProvider.PathIsCurrentDirString(selected))
        {
            AnsiConsole.WriteLine(selector.CurrentPath);
            Environment.Exit(0);
        }
        else if (DirectoriesProvider.PathIsHomeDirString(selected))
        {
            selector.CurrentPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        }
        else if (DirectoriesProvider.TryUpOneDir(selected, selector.CurrentPath, out string newPath))
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