//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using WordPressPCL;
using WpLoad.Domain;
using WpLoad.Infrastructure;
using WpLoad.Services;

namespace WpLoad.Commands
{
    internal class Up : IAsyncCommand
    {
        public string CommandName => nameof(Up);

        public async Task<ExitCode> Execute(ILog log, IReadOnlyList<string> arguments)
        {
            if (arguments.TryGetArgument(0, out string? site)
                && arguments.TryGetArgument(1, out string? folder))
            {
                if (!Directory.Exists(folder))
                {
                    log.Error($"{folder} doesn't exist");
                    return ExitCode.Fail;
                }

                log.Info("Configuring connection...");
                if (!TryConfifgureConnection(site, out WordPressClient? client))
                {
                    log.Error($"{site} profile doesn't exist");
                    return ExitCode.Fail;
                }


                var mediaFiles = FileServices.GetSupportedFilesInDirectory(folder);

                try
                {
                    await UploadMedia(log, client, mediaFiles.mediaFiles);
                    await UploadHtml(log, client, mediaFiles.htmls);
                }
                catch (Exception)
                {
                    log.Error("Upload failed");
                }

            }
            return ExitCode.BadParameters;
        }

        private static async Task UploadHtml(ILog log, WordPressClient client, IReadOnlyList<string> htmls)
        {
            foreach (var html in htmls)
            {
                string content = await File.ReadAllTextAsync(html);
                log.Info($"Uploading {Path.GetFileName(html)}...");
                await client.Posts.Create(new WordPressPCL.Models.Post
                {
                    Date = DateTime.Now,
                    DateGmt = DateTime.UtcNow,
                    Status = WordPressPCL.Models.Status.Draft,
                    Title = new WordPressPCL.Models.Title
                    {
                        Raw = Path.GetFileName(html),
                    },
                    Content = new WordPressPCL.Models.Content
                    {
                        Raw = content,
                    }
                });
            }
        }

        private static async Task UploadMedia(ILog log, WordPressClient client, IReadOnlyList<string> mediaFiles)
        {
            foreach (var media in mediaFiles)
            {
                log.Info($"Uploading {Path.GetFileName(media)}...");
                await client.Media.Create(media,
                                          Path.GetFileName(media),
                                          FileServices.GetMimeType(media));
            }
        }

        private static bool TryConfifgureConnection(string site, [NotNullWhen(true)] out WordPressClient? client)
        {
            if (SiteServices.TryReadSiteInfo(site, out SiteInfo? info))
            {
                client = new WordPressClient(info.Host);
                client.UserName = info.Username;
                client.SetApplicationPassword(info.Password);
                return true;
            }
            client = null;
            return false;
        }
    }
}
