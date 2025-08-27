//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Formats.Tar;
using System.IO.Compression;
using System.Text;

namespace BookGen.Infrastructure.Tools;

internal static class Extractor
{
    public static async Task Copy(IDownloadUi ui, Stream stream, string folderName, string fileName)
    {
        ui.BeginNew("Copying...", stream.Length);
        string outputPath = Path.Combine(AppContext.BaseDirectory, "tools", folderName, fileName);

        string directory = Path.GetDirectoryName(outputPath) 
            ?? throw new InvalidOperationException("Invalid output path");

        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        await using var targetStream = File.Create(outputPath);
        await stream.CopyToAsync(targetStream, CancellationToken.None);
        ui.Report(stream.Length);
    }

    public static async Task ExtractTarGz(IDownloadUi ui, Stream stream, string targetFolder)
    {
        static string GetEntryOutputPath(string targetFolder, TarEntry entry)
            => Path.Combine(AppContext.BaseDirectory, "tools", targetFolder, entry.Name);

        await using var gzipStream = new GZipStream(stream, CompressionMode.Decompress, leaveOpen: true);
        await using TarReader tarReader = new TarReader(gzipStream);

        TarEntry? entry;

        ui.BeginNew("Extracting...", int.MaxValue);

        while ((entry = tarReader.GetNextEntry()) != null)
        {
            if (string.IsNullOrEmpty(entry.Name))
            {
                // Skip directories
                continue;
            }

            string outputPath = GetEntryOutputPath(targetFolder, entry);
            string? directory = Path.GetDirectoryName(outputPath);
            if (directory != null && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            await using var targetStream = File.Create(outputPath);
            if (entry.DataStream != null)
            {
                await entry.DataStream.CopyToAsync(targetStream, CancellationToken.None);
            }

            ui.Report(entry.Length);
        }
    }

    public static async Task ExtractZip(IDownloadUi ui, Stream stream, string targetFolder)
    {
        static string GetEntryOutputPath(string targetFolder, ZipArchiveEntry entry)
            => Path.Combine(AppContext.BaseDirectory, "tools", targetFolder, entry.Name);

        using var archive = new ZipArchive(stream, ZipArchiveMode.Read, true, Encoding.UTF8);

        ui.BeginNew("Extracting...", archive.Entries.Sum(e => e.Length));

        foreach (var entry in archive.Entries)
        {
            if (string.IsNullOrEmpty(entry.Name))
            {
                // Skip directories
                continue;
            }

            string outputPath = GetEntryOutputPath(targetFolder, entry);
            string? directory = Path.GetDirectoryName(outputPath);
            if (directory != null && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            await using var targetStream = File.Create(outputPath);
            await using var source = entry.Open();
            await source.CopyToAsync(targetStream, CancellationToken.None);

            ui.Report(entry.Length);
        }
    }
}