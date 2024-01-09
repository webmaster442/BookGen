//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain.Www;

using Spectre.Console;

namespace BookGen.Shell.Www;

internal sealed class FavoritesMenu
{
    private readonly Dictionary<string, string> _favorites;
    private const string Exit = "Exit";

    public FavoritesMenu(WwwUrl[] favorites)
    {
        _favorites = favorites.ToDictionary(x => x.Value, x => x.Href);
    }

    public async Task Run(IAnsiConsole console, Action<string> urlOpenAction)
    {
        while (true)
        {
            console.Clear();
            SelectionPrompt<string> urlSelector = CreateSelector();
            string selected = await urlSelector.ShowAsync(console, CancellationToken.None);
            if (selected == Exit)
            {
                break;
            }
            string url = _favorites[selected];
            urlOpenAction.Invoke(url);
        }
    }

    private SelectionPrompt<string> CreateSelector()
    {
        SelectionPrompt<string> selector = new()
        {
            Title = "WWW - A web tool",
            WrapAround = true,
            PageSize = Console.WindowHeight - 4,
            Mode = SelectionMode.Leaf
        };
        selector.AddChoiceGroup("Favorites", _favorites.Keys);
        selector.AddChoiceGroup("Program", Exit);
        return selector;
    }
}