//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections.ObjectModel;
using System.Diagnostics;

namespace BookGen.Launcher.ViewModels.Rss;
internal partial class NewsViewModel : ObservableObject
{
    public ObservableCollection<RssChannelItem> Items { get; }

    public NewsViewModel(RssChannelItem[]? items)
    {
        Items = items != null
            ? new ObservableCollection<RssChannelItem>(items)
            : new ObservableCollection<RssChannelItem>();
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
