//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Spectre.Console;
using System.Resources;

namespace BookGen.Gui;

public sealed class Renderer
{
    private readonly IAnsiConsole _console;
    private readonly ResourceManager _resourceManager;
    private readonly CancellationToken _token;

    public Renderer(IAnsiConsole ansiConsole, ResourceManager resourceManager, CancellationToken token)
    {
        _console = ansiConsole;
        _resourceManager = resourceManager;
        _token = token;
    }

    private string Localize(string text)
    {
        if (text == null)
            ArgumentNullException.ThrowIfNull(text);

        string? translated = _resourceManager.GetString(text);
        return translated ?? text;
    }

    public Position GlobalPosition
    {
        get
        {
            var (Left, Top) = Console.GetCursorPosition();
            return new Position
            {
                X = Left,
                Y = Top,
            };
        }
        set
        {
            Console.SetCursorPosition(value.X, value.Y);
        }
    }

    public void Clear() => _console.Clear();

    public void AlternateScreen(Action alternateScreenAction)
    {
        _console.AlternateScreen(alternateScreenAction);
    }

    public void PrintText(IEnumerable<string> lines)
    {
        foreach (var line in lines)
        {
            _console.WriteLine(Localize(line));
        }
    }

    public void FigletText(string text, ConsoleColor color)
    {
        //todo: localize
        var figlet = new FigletText(Localize(text))
        {
            Justification = Justify.Left,
            Color = Color.FromConsoleColor(color),
        };
        _console.Write(figlet);
    }

    public void Rule(string text = "")
    {
        var rule = new Rule(Localize(text));
        _console.Write(rule);
    }

    public async Task<T> SelectionMenu<T>(string title, IDictionary<string, T> selectionItems)
        where T : struct, Enum
    {
        var selector = new SelectionPrompt<string>()
            .Title(title)
            .PageSize(12)
            .AddChoices(selectionItems.Keys.ToArray());

        var selected = await selector.ShowAsync(_console, _token);

        return selectionItems[selected];
    }

    public async Task<ISet<T>> MultiSelectionMenu<T>(string title,
                                         bool isRequred,
                                         IDictionary<string, T> selectionItems) 
        where T : struct, Enum
    {
        var selector = new MultiSelectionPrompt<string>()
            .Title(title)
            .PageSize(12)
            .Required(isRequred)
            .AddChoices(selectionItems.Keys.ToArray());

        var selections = await selector.ShowAsync(_console, _token);

        var results = from selection in selections
                      from selectionItem in selectionItems
                      where selectionItem.Key == selection
                      select selectionItem.Value;

        return results.ToHashSet();
    }

    public void BlankLine(int numberOfLines)
    {
        for (int i=0; i<numberOfLines; i++)
        {
            _console.WriteLine("");
        }
    }
}
