using Cdg.Properties;

using Spectre.Console;

namespace Cdg;
internal class DirectorySelectorMenu
{
    private readonly SelectorNameConverter _converter;
    public string CurrentPath { get; set; }

    public DirectorySelectorMenu(string startDir)
    {
        CurrentPath = startDir;
        _converter = new SelectorNameConverter();
    }

    public SelectionPrompt<string> CreateSelection(IEnumerable<string> items)
    {
        SelectionPrompt<string> selector = new()
        {
            Converter = _converter.Convert,
            Title = string.Format(Resources.MenuTitle, DisplayPath(CurrentPath)),
            WrapAround = true,
            PageSize = Console.WindowHeight - 4,
            Mode = SelectionMode.Leaf,
        };
        selector.AddChoiceGroup(Resources.GroupSpecials, GetSpecialItems());
        if (items.Any())
        {
            selector.AddChoiceGroup(Resources.GroupDirectories, items);
        }
        return selector;
    }

    private static IEnumerable<string> GetSpecialItems()
    {
        yield return nameof(Resources._MenuSelectorCurrentDir_10);
        yield return nameof(Resources._MenuSelectorUpOneDir_20);
        yield return nameof(Resources._MenuSelectorRootDir_30);
        yield return nameof(Resources._MenuSelectorHomeDir_35);
        yield return nameof(Resources._MenuSelectorKnownDirs_40);
    }


    private string DisplayPath(string currentPath)
    {
        return currentPath switch
        {
            nameof(Resources._MenuSelectorRootDir_30) => Resources.PathNameRootDir,
            _ => currentPath,
        };
    }
}
