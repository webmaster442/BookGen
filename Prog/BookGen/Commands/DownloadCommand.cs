using BookGen.CommandArguments;
using BookGen.Infrastructure;

namespace BookGen.Commands;

[CommandName("download")]
internal class DownloadCommand : AsyncCommand<DownloadArguments>
{
    private readonly ILog _log;

    public DownloadCommand(ILog log)
    {
        _log = log;
    }

    public override async Task<int> Execute(DownloadArguments arguments, string[] context)
    {
        _log.EnableVerboseLogingIfRequested(arguments);

        using (var client = new BookGenHttpClient())
        {
            try
            {
                Uri uri = new(arguments.Url, UriKind.RelativeOrAbsolute);
                FsPath targetFile = GetFileName(arguments.Directory, uri);
                _log.Info("Downloading to {0}...", targetFile);
                await client.DownloadToFile(uri, targetFile, _log);

                return Constants.Succes;
            }
            catch (Exception ex) 
            {
                _log.Critical(ex);
                return Constants.GeneralError;
            }
        }
    }

    private static FsPath GetFileName(string directory, Uri uri)
    {
        var name = Path.GetFileName(uri.LocalPath);
        if (string.IsNullOrEmpty(name)) 
        {
            name = Path.ChangeExtension(uri.Host.Replace('.', '_'), ".html");
        }
        return new FsPath(directory, name);
    }
}
