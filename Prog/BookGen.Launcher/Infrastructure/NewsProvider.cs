//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;

using BookGen.Launcher.Properties;
using BookGen.Launcher.ViewModels.Rss;

using CommunityToolkit.Mvvm.Messaging;

namespace BookGen.Launcher.Infrastructure;
internal sealed class NewsProvider
{
    public RSS? NewsRss { get; private set; }

    private const string NewsChannel = "https://raw.githubusercontent.com/webmaster442/BookGen/development/.github/releases.xml";
    
    private readonly string _cacheFile;

    public NewsProvider()
    {
        _cacheFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal),"bookgennews.xml");
    }

    public async void StartDownload()
    {
        NewsRss = await DowloadTask();
        if (NewsRss != null)
        {
            DateTime lastEntry = NewsRss.Channel.Item.Max(x => DateTime.Parse(x.PubDate));
            if (lastEntry > Settings.Default.NewsLastSeen)
            {
                Settings.Default.NewsLastSeen = lastEntry;
                Settings.Default.Save();
                WeakReferenceMessenger.Default.Send(NewsRss.Channel.Item);
            }
        }
    }

    private async Task<RSS?> DowloadTask()
    {
        var xmlSerializer = new XmlSerializer(typeof(RSS));
        try
        {
            if (File.Exists(_cacheFile)
                && (DateTime.Now - File.GetLastWriteTime(_cacheFile)).TotalHours < 12)
            {
                using (var cache = File.OpenRead(_cacheFile))
                {
                    return xmlSerializer.Deserialize(cache) as RSS;
                }
            }
            else
            {
                using (var client = new HttpClient())
                {
                    using var stream = await client.GetStreamAsync(NewsChannel);
                    using var cache = File.Create(_cacheFile);
                    stream.CopyTo(cache);
                    cache.Flush();
                    cache.Seek(0, SeekOrigin.Begin);
                    return xmlSerializer.Deserialize(cache) as RSS;
                }
            }
        }
        catch
        {
            return null;
        }
    }
}
