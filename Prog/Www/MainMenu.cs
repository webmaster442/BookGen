//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain.Www;

using Spectre.Console;

namespace Www;

internal sealed class MainMenu
{
    private readonly Dictionary<string, string> _favorites;
    private readonly WwwBang[] _bangs;
    private const string Exit = "Exit";

    public MainMenu(WwwUrl[] favorites, WwwBang[] bangs)
    {
        _favorites = favorites.ToDictionary(x => x.Value, x => x.Href);
        _bangs = bangs;
    }

    public async Task Run(IAnsiConsole console, Action<string> openUrl)
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
            openUrl.Invoke(url);
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