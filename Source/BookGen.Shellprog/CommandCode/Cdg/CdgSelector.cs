//-----------------------------------------------------------------------------
// (c) 2023-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Shell.Cdg;

using Spectre.Console;

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace BookGen.Shellprog.CommandCode.Cdg;

internal sealed class CdgSelector
{
    private readonly SelectionItemAction[] _menuItems;
    private readonly List<SelectionItemDirectory> _directories;
    private bool _canRun;

    public string _currentPath;
    private bool _showHidden;

    private void SetDirectories(IEnumerable<SelectionItemDirectory> items)
    {
        _directories.Clear();
        _directories.AddRange(items);
    }

    public CdgSelector(string startDirectory, bool showHidden)
    {
        _currentPath = startDirectory;
        _showHidden = showHidden;
        _directories = new List<SelectionItemDirectory>();
        _menuItems =
        [
            new SelectionItemAction
            {
                DisplayString = "Select current directory",
                Icon = ":right_arrow:",
                Action = () =>
                {
                    Environment.SetEnvironmentVariable("cdgPath", _currentPath, EnvironmentVariableTarget.User);
                    _canRun = false;
                },
                Color = Color.Red,
            },
            new SelectionItemAction 
            {
                DisplayString = "Open in file manager",
                Icon = ":right_arrow:",
                Action = OpenInFileExplorer,
                Color = Color.Red,
            },
            new SelectionItemAction
            {
                DisplayString = "Up one directory",
                Icon = ":upwards_button:",
                Action = () =>
                {
                    if (TryGetUpOneDirectory(_currentPath, out string newPath))
                    {
                        _currentPath = newPath;
                        SetDirectories(SelectionItemFactory.CreateFromDirectories(Directory.GetDirectories(newPath)));
                    }
                },
                Color = Color.Olive,
            },
            new SelectionItemAction
            {
                DisplayString = "Jump home",
                Icon = ":house:",
                Action = () =>
                {
                    _currentPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                    SetDirectories(SelectionItemFactory.CreateFromDirectories(Directory.GetDirectories(_currentPath)));
                },
                Color = Color.Yellow,
            },
            new SelectionItemAction
            {
                DisplayString = "Known folders",
                Icon = ":file_cabinet: ",
                Action = () =>
                {
                    _currentPath = "Special folders";
                    SetDirectories(SelectionItemFactory.GetSpecialFolders());
                },
                Color = Color.Lime,
            },
            new SelectionItemAction
            {
                DisplayString = "Path folders",
                Icon = ":file_cabinet: ",
                Action = () =>
                {
                    _currentPath = "Path folders";
                    SetDirectories(SelectionItemFactory.GetPathDirectories());
                },
                Color = Color.Lime,
            },
            new SelectionItemAction
            {
                DisplayString = "Drives",
                Icon = ":desktop_computer: ",
                Action = () =>
                {
                    _currentPath = "Drives";
                    SetDirectories(SelectionItemFactory.GetDrives());
                },
                Color = Color.Fuchsia,
            },
            new SelectionItemAction
            {
                Id = "ShowHide",
                DisplayString =  _showHidden ? "Hide hidden files" : "Toggle hidden files",
                Icon = ":eye: ",
                Action = () => 
                {
                    var showHide = _menuItems?.First(m => m.Id == "ShowHide") ?? throw new InvalidOperationException(); 
                    showHide.DisplayString = _showHidden ? "Show hidden files" : "Hide hidden files";
                    _showHidden = !_showHidden;
                },
                Color = Color.Blue,
            },
        ];
    }

    private void OpenInFileExplorer()
    {
        using var process = Process.Start(new ProcessStartInfo
        {
            FileName = _currentPath,
            UseShellExecute = true,
            Verb = "open"
        });
        _canRun = false;
    }

    private bool CanAccess(string path, [NotNullWhen(true)] out string[]? subdirs)
    {
        try
        {
            var items = new DirectoryInfo(path).GetDirectories();
            if (_showHidden)
            {
                items = items.Where(x => !x.Attributes.HasFlag(FileAttributes.Hidden)).ToArray();
            }
            subdirs = items.Select(x => x.FullName).ToArray();
            return true;
        }
        catch (Exception)
        {
            subdirs = null;
            return false;
        }
    }

    private static bool TryGetUpOneDirectory(string currentPath, out string newPath)
    {
        if (string.IsNullOrEmpty(currentPath))
        {
            newPath = string.Empty;
            return false;
        }

        var directory = new DirectoryInfo(currentPath);
        if (directory.Parent is null)
        {
            newPath = string.Empty;
            return false;
        }

        newPath = directory.Parent.FullName;
        return true;
    }

    private static string Render(SelectionItemBase item)
    {
        string seperator = "  ";
        if (item.IsMenuHeader)
            seperator = " ";

        return $"{item.Icon}{seperator}[{item.Color.ToMarkup()}]{item.DisplayString.EscapeMarkup()}[/]";
    }

    private static SelectionItemBase CreateGroup(string groupName, string icon)
    {
        return new SelectionItemBase
        {
            DisplayString = groupName,
            Icon = icon,
            IsMenuHeader = true,
            Color = Color.Red
        };
    }

    private SelectionPrompt<SelectionItemBase> CreateSelection()
    {
        SelectionPrompt<SelectionItemBase> selector = new()
        {
            Converter = Render,
            Title = $"Location: {_currentPath}",
            WrapAround = true,
            PageSize = Console.WindowHeight - 4,
            Mode = SelectionMode.Leaf
        };

        selector.AddChoiceGroup(CreateGroup("Menu", ":fire:"), _menuItems);
        if (_directories.Count > 0)
        {
            selector.AddChoiceGroup(CreateGroup("Directories", ":file_folder:"), _directories);
        }

        return selector;
    }

    public async Task ShowMenu()
    {
        SetDirectories(SelectionItemFactory.CreateFromDirectories(Directory.GetDirectories(_currentPath)));
        _canRun = true;
        while (_canRun)
        {
            try
            {
                AnsiConsole.Clear();
                var menu = CreateSelection();
                var selected = await menu.ShowAsync(AnsiConsole.Console, CancellationToken.None);
                if (selected is SelectionItemDirectory directory)
                {
                    if (CanAccess(directory.Path, out string[]? subdirs))
                    {
                        _currentPath = directory.Path;
                        SetDirectories(SelectionItemFactory.CreateFromDirectories(subdirs));
                    }
                    else
                    {
                        AnsiConsole.Clear();
                        AnsiConsole.MarkupInterpolated($"[red]Cannot access {directory.Path}[/]");
                    }
                }
                else if (selected is SelectionItemAction action)
                {
                    action.Action();
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
    }
}