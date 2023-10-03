using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BookGen.Launcher.ViewModels.Rss;
internal partial class NewsViewModel : ObservableObject
{
    private const string NewsChannel = "https://raw.githubusercontent.com/webmaster442/BookGen/development/.github/releases.xml";

    public ObservableCollection<RssChannelItem> Items { get; }

    public NewsViewModel()
    {
        Items = new ObservableCollection<RssChannelItem>();
    }

    private async Task<RSS?> Download()
    {
        var xmlSerializer = new XmlSerializer(typeof(RSS));
        try
        {
            using (var client = new HttpClient())
            {
                using var stream = await client.GetStreamAsync(NewsChannel);
                if (xmlSerializer.Deserialize(stream) is RSS rss)
                {
                    return rss;
                }
            }
        }
        catch
        {

            return null;
        }
        return null;
    }

    [RelayCommand]
    private async Task LoadItems()
    {
        if (Items.Count < 1)
        {
            var rss = await Download();
            if (rss != null)
            {
                foreach (var item in rss.Channel.Item)
                {
                    Items.Add(item);
                }
            }
            else
            {
                Dialog.ShowMessageBox("Error loading news", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    [RelayCommand]
    private void OpenLink(string url)
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
