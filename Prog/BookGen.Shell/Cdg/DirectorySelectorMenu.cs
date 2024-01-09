//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Spectre.Console;

using TextResources = BookGen.Shell.Properties.Resources;

namespace BookGen.Shell.Cdg;

internal sealed class DirectorySelectorMenu
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
            Title = string.Format(TextResources.MenuTitle, DisplayPath(CurrentPath)),
            WrapAround = true,
            PageSize = Console.WindowHeight - 4,
            Mode = SelectionMode.Leaf,
        };
        selector.AddChoiceGroup(TextResources.GroupSpecials, GetSpecialItems());
        if (items.Any())
        {
            selector.AddChoiceGroup(TextResources.GroupDirectories, items);
        }
        return selector;
    }

    private static IEnumerable<string> GetSpecialItems()
    {
        yield return nameof(TextResources._MenuSelectorCurrentDir_10);
        yield return nameof(TextResources._MenuSelectorUpOneDir_20);
        yield return nameof(TextResources._MenuSelectorRootDir_30);
        yield return nameof(TextResources._MenuSelectorHomeDir_35);
        yield return nameof(TextResources._MenuSelectorKnownDirs_40);
    }

    private static string DisplayPath(string currentPath)
    {
        return currentPath switch
        {
            nameof(TextResources._MenuSelectorRootDir_30) => TextResources.PathNameRootDir,
            _ => currentPath,
        };
    }
}
