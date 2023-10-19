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

    public void Clear() => _console.Clear();

    public void PrintText(string line)
    {
        _console.WriteLine(Localize(line));
    }

    public async Task RenderHelpPages(IEnumerable<string> helpContent)
    {
        var pages = HelpRenderer.GetPages(helpContent);
        int pageCounter = 0;
        foreach (var page in pages)
        {
            HelpRenderer.RenderPage(page);
            ++pageCounter;
            if (pageCounter < pages.Length)
            {
                await WaitKey("Press a key for next page");
                _console.Clear();
            }
        }
        await WaitKey();
    }

    public void FigletText(string text, ConsoleColor color)
    {
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

    public Task WaitKey(string message = "Press a key to continue...")
    {
        TextPrompt<string> confirm = new TextPrompt<string>(message).AllowEmpty();
        return confirm.ShowAsync(_console, _token);
    }

    public Task<string> Prompt(string message)
    {
        TextPrompt<string> prompt = new(message);
        return prompt.ShowAsync(_console, _token);
    }

    public Task<bool> Confirm(string message)
    {
        ConfirmationPrompt confirm = new(message);
        return confirm.ShowAsync(_console, _token);
    }

    private static int GetPageSize()
    {
        var (_, top) = Console.GetCursorPosition();
        var height = Console.WindowHeight;
        return height - top - 5;
    }

    public async Task<string> SelectionMenu(string title, IEnumerable<string> selectionItems)
    {
        SelectionPrompt<string> selector = new SelectionPrompt<string>()
            .Title(title)
            .PageSize(GetPageSize())
            .AddChoices(selectionItems);

        string selected = await selector.ShowAsync(_console, _token);

        return selected;
    }

    public async Task<T> SelectionMenu<T>(string title, IDictionary<string, T> selectionItems)
        where T : struct, Enum
    {
        SelectionPrompt<string> selector = new SelectionPrompt<string>()
            .Title(title)
            .PageSize(GetPageSize())
            .AddChoices(selectionItems.Keys.ToArray());

        string selected = await selector.ShowAsync(_console, _token);

        return selectionItems[selected];
    }

    public async Task<ISet<T>> MultiSelectionMenu<T>(string title,
                                         bool isRequred,
                                         IDictionary<string, T> selectionItems)
        where T : struct, Enum
    {
        MultiSelectionPrompt<string> selector = new MultiSelectionPrompt<string>()
            .Title(title)
            .PageSize(GetPageSize())
            .Required(isRequred)
            .AddChoices(selectionItems.Keys.ToArray());

        List<string> selections = await selector.ShowAsync(_console, _token);

        IEnumerable<T> results = from selection in selections
                                 from selectionItem in selectionItems
                                 where selectionItem.Key == selection
                                 select selectionItem.Value;

        return results.ToHashSet();
    }

    public void BlankLine(int numberOfLines = 1)
    {
        for (int i = 0; i < numberOfLines; i++)
        {
            _console.WriteLine("");
        }
    }

    public void DisplayPath(string title, string path)
    {
        _console.MarkupInterpolated($"{title} [green]{path}[/]{Environment.NewLine}");
    }

    public void DisplayImage(Stream img)
    {
        var markup = TextHelper.ImageStreamToMarkup(img,
                                                    Console.WindowWidth,
                                                    Console.WindowHeight);
        _console.Markup(markup);
    }
}
