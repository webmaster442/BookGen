using System.Diagnostics;

namespace BookGen.Commands;

[CommandName("edit")]
internal class EditCommand : Command
{
    private readonly ILog _log;
    private readonly IAppSetting _appSetting;

    public EditCommand(ILog log, IAppSetting appSetting)
    {
        _log = log;
        _appSetting = appSetting;
    }

    public override int Execute(string[] context)
    {
        if (context.Length != 1)
        {
            _log.Warning("No file name given");
            return Constants.ArgumentsError;
        }

        if (string.IsNullOrEmpty(_appSetting.EditorPath))
        {
            _log.Warning("No Editor configured");
            return Constants.ArgumentsError;
        }

        string? file = System.IO.Path.GetFullPath(context[0]);

        if (!EditorHelper.IsSupportedFile(file))
        {
            _log.Warning("Unsupported file type");
            return Constants.ArgumentsError;
        }

        try
        {
            using var p = new Process();
            p.StartInfo.FileName = _appSetting.EditorPath;
            p.StartInfo.Arguments = $"\"{file}\"";
            p.StartInfo.UseShellExecute = false;
            p.Start();
            return Constants.Succes;
        }
        catch (Exception ex)
        {
            _log.Critical(ex);
            return Constants.GeneralError;
        }
    }
}
