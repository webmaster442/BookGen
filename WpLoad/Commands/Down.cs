//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Gui.ArgumentParser;
using WordPressPCL;
using WpLoad.Domain;
using WpLoad.Infrastructure;

namespace WpLoad.Commands
{
    internal class Down : LoadCommandBase
    {
        public override string CommandName => nameof(Down);

        public override async Task<ExitCode> Execute(ILog log, IReadOnlyList<string> arguments)
        {
            DownArguments args = new();
            ArgumentParser.ParseArguments(arguments, args);

            if (TryConfigureFolderAndClient(log, args, out var client))
            {
                if (args.Pages)
                {
                    if (!ConfigureFolder(args.Path, "pages")) return ExitCode.Fail;
                    await DownloadPages(log, client, args.Path);
                }
                if (args.Posts)
                {
                    if (!ConfigureFolder(args.Path, "posts")) return ExitCode.Fail;
                    await DownloadPosts(log, client, args.Path);
                }
                if (args.Media)
                {
                    if (!ConfigureFolder(args.Path, "media")) return ExitCode.Fail;
                    await DownloadMedia(log, client, args.Path);
                }
            }
            return ExitCode.BadParameters;
        }

        private bool ConfigureFolder(string baseFolder, string additionalName)
        {
            var path = Path.Combine(baseFolder, additionalName);
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                    return true;
                }
                catch (IOException) 
                {
                    return false;
                }
            }
            return true;
        }

        private async Task DownloadMedia(ILog log, WordPressClient client, string path)
        {
            log.Info("Searching media items...");
            var mediaItems = await client.Media.GetAllAsync();

            using (var HttpClient = new HttpClient())
            {
                foreach (var mediaItem in mediaItems)
                {
                    log.Info($"Downloading: {Path.GetFileName(mediaItem.SourceUrl)}...");
                    using var sourceStream = await HttpClient.GetStreamAsync(mediaItem.SourceUrl);
                    using var targetStream = File.Create(Path.Combine(path, Path.GetFileName(mediaItem.SourceUrl)));
                    await sourceStream.CopyToAsync(targetStream);
                }
            }
        }

        private async Task DownloadPosts(ILog log, WordPressClient client, string path)
        {
            log.Info("Searching posts...");
            var posts = await client.Posts.GetAllAsync();
            foreach (var post in posts)
            {
                string fleName = post.Slug + ".html";
                log.Info($"Writing {fleName}...");
                var targetFile = Path.Combine(path, fleName);
                File.WriteAllText(targetFile, post.Content.Raw);
            }
        }

        private Task DownloadPages(ILog log, WordPressClient client, string path)
        {
            throw new NotImplementedException();
        }
    }
}
