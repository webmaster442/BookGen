//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;

using BookGen.Domain.Rss;
using BookGen.Launcher.Properties;

using CommunityToolkit.Mvvm.Messaging;

namespace BookGen.Launcher.Infrastructure;
internal sealed class NewsProvider
{
    public RssFeed? NewsRss { get; private set; }

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
            DateTime lastEntry = NewsRss.Channel.Item?.Max(x => !string.IsNullOrEmpty(x.PubDate) ? DateTime.Parse(x.PubDate) : DateTime.MinValue) ?? DateTime.MinValue;
            if (lastEntry > Settings.Default.NewsLastSeen)
            {
                Settings.Default.NewsLastSeen = lastEntry;
                Settings.Default.Save();
                WeakReferenceMessenger.Default.Send(NewsRss.Channel.Item ?? Array.Empty<Item>());
            }
        }
    }

    private async Task<RssFeed?> DowloadTask()
    {
        var xmlSerializer = new XmlSerializer(typeof(RssFeed));
        try
        {
            if (File.Exists(_cacheFile)
                && (DateTime.Now - File.GetLastWriteTime(_cacheFile)).TotalHours < 12)
            {
                using (var cache = File.OpenRead(_cacheFile))
                {
                    return xmlSerializer.Deserialize(cache) as RssFeed;
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
                    return xmlSerializer.Deserialize(cache) as RssFeed;
                }
            }
        }
        catch
        {
            return null;
        }
    }
}
