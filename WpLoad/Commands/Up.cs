//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Gui.ArgumentParser;
using System.Web;
using WordPressPCL;
using WordPressPCL.Models;
using WpLoad.Domain;
using WpLoad.Infrastructure;
using WpLoad.Services;

namespace WpLoad.Commands
{
    internal class Up : LoadCommandBase
    {
        public override string CommandName => nameof(Up);

        public override async Task<ExitCode> Execute(ILog log, IReadOnlyList<string> arguments)
        {
            UpArguments args = new();
            ArgumentParser.ParseArguments(arguments, args);

            if (TryConfigureFolderAndClient(log, args, out var client))
            {
                var mediaFiles = FileServices.GetSupportedFilesInDirectory(args.Path);
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
            await Parallel.ForEachAsync(htmls, async (html, ct) =>
            {
                string content = await File.ReadAllTextAsync(html);
                log.Info($"Uploading {Path.GetFileName(html)}...");
                await client.Posts.CreateAsync(CreatePost(html, content));
                reporter.Report(html);

            });
            reporter.Stop();
        }

        private static async Task UploadMedia(ILog log, WordPressClient client, IReadOnlyList<string> mediaFiles)
        {
            ProgressReporter reporter = new ProgressReporter(log);
            reporter.Start();
            await Parallel.ForEachAsync(mediaFiles, async (media, ct) =>
            {
                log.Info($"Uploading {Path.GetFileName(media)}...");
                await client.Media.CreateAsync(media,
                          HttpUtility.UrlEncode(Path.GetFileName(media)),
                          FileServices.GetMimeType(media));
                reporter.Report(media);
            });
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
