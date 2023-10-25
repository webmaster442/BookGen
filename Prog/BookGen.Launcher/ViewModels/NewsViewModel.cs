//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections.ObjectModel;
using System.Diagnostics;

using BookGen.Domain.Rss;

namespace BookGen.Launcher.ViewModels;
internal partial class NewsViewModel : ObservableObject
{
    public ObservableCollection<Item> Items { get; }

    public NewsViewModel(Item[]? items)
    {
        Items = items != null
            ? new ObservableCollection<Item>(items)
            : new ObservableCollection<Item>();
    }

    [RelayCommand]
    private static void OpenLink(string url)
    {
        if (!string.IsNullOrEmpty(url))
        {
            using (var process = new Process())
            {
                process.StartInfo.FileName = url;
                process.StartInfo.UseShellExecute = true;
                process.Start();
            }
        }
    }
}
