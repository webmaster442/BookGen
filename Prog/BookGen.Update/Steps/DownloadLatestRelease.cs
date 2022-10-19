using BookGen.Update.Infrastructure;

namespace BookGen.Update.Steps;

internal sealed class DownloadLatestRelease : IUpdateStepAsync
{
    public async Task<bool> Execute(GlobalState state)
    {
        if (string.IsNullOrEmpty(state.Latest.ZipPackageUrl))
        {
            state.Issues.Add("Don't know what release to download");
            return false;
        }

        using var client = new HttpClient();
        using HttpResponseMessage? response = await client.GetAsync(state.Latest.ZipPackageUrl);
        if (response.IsSuccessStatusCode)
        {
            using Stream stream = await response.Content.ReadAsStreamAsync();
            state.TempFile = CreateTempFileName();
            long? length = response.Content.Headers.ContentLength;
            using FileStream target = File.Create(state.TempFile);
            await CopyWithProgressAsync(stream, target, length);
        }

        state.Issues.Add($"Ar error occured during downloading update package: {response}");
        return false;
    }

    private static async Task CopyWithProgressAsync(Stream stream, Stream target, long? length)
    {
        byte[] buffer = new byte[4096];
        long progress = 0;
        int read = 0;
        do
        {
            read = await stream.ReadAsync(buffer);
            await target.WriteAsync(buffer.AsMemory(0, read));
            progress += read;
            Report(progress, length ?? read);
        }
        while (read > 0);
    }

    private static void Report(long progress, long length)
    {
        double percent = Math.Round(progress / (double)length * 100.0d, 3);
        Console.Write($"Dowloaded {progress} of {length} ({percent} %) ...\r");
    }

    private static string CreateTempFileName()
    {
        string tempFolder = Path.GetTempPath();
        return Path.Combine(tempFolder, "BookGenUpdate.zip");
    }
}
