//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Text.Encodings.Web;
using System.Web;
using WordPressPCL;
using WordPressPCL.Models;
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
                if (!ClientService.TryConfifgureConnection(site, out WordPressClient? client))
                {
                    log.Error($"{site} profile doesn't exist");
                    return ExitCode.Fail;
                }


                var mediaFiles = FileServices.GetSupportedFilesInDirectory(folder);

                try
                {
                    await UploadMedia(log, client, mediaFiles.mediaFiles);
                    await UploadHtml(log, client, mediaFiles.htmls);
                    return ExitCode.Success;
                }
                catch (Exception ex)
                {
                    log.Error("Upload failed");
                    log.Error(ex);
                    return ExitCode.Fail;
                }

            }
            return ExitCode.BadParameters;
        }

        private static async Task UploadHtml(ILog log, WordPressClient client, IReadOnlyList<string> htmls)
        {
            ProgressReporter reporter = new ProgressReporter(log);
            reporter.Start();
            foreach (var html in htmls)
            {
                string content = await File.ReadAllTextAsync(html);
                log.Info($"Uploading {Path.GetFileName(html)}...");
                await client.Posts.Create(CreatePost(html, content));
                reporter.Report(html);

            }
            reporter.Stop();
        }

        private static async Task UploadMedia(ILog log, WordPressClient client, IReadOnlyList<string> mediaFiles)
        {
            ProgressReporter reporter = new ProgressReporter(log);
            reporter.Start();
            foreach (var media in mediaFiles)
            {
                log.Info($"Uploading {Path.GetFileName(media)}...");
                await client.Media.Create(media,
                                          HttpUtility.UrlEncode(Path.GetFileName(media)),
                                          FileServices.GetMimeType(media));
                reporter.Report(media);
            }
            reporter.Stop();
        }


        private static Post CreatePost(string html, string content)
        {
            return new Post
            {
                Date = DateTime.Now,
                DateGmt = DateTime.UtcNow,
                Status = Status.Draft,
                Title = new Title
                {
                    Raw = HttpUtility.UrlEncode(Path.GetFileName(html)),
                },
                Content = new Content
                {
                    Raw = content,
                }
            };
        }
    }
}
