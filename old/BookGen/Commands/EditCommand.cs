//-----------------------------------------------------------------------------
// (c) 2019-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics;

namespace BookGen.Commands;

[CommandName("edit")]
internal class EditCommand : Command
{
    private readonly ILogger _log;
    private readonly IAppSetting _appSetting;

    public EditCommand(ILogger log, IAppSetting appSetting)
    {
        _log = log;
        _appSetting = appSetting;
    }

    public override int Execute(string[] context)
    {
        if (context.Length != 1)
        {
            _log.LogWarning("No file name given");
            return Constants.ArgumentsError;
        }

        if (string.IsNullOrEmpty(_appSetting.EditorPath))
        {
            _log.LogWarning("No Editor configured");
            return Constants.ArgumentsError;
        }

        string? file = System.IO.Path.GetFullPath(context[0]);

        if (!EditorHelper.IsSupportedFile(file))
        {
            _log.LogWarning("Unsupported file type");
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
            _log.LogCritical(ex, "Critical Error");
            return Constants.GeneralError;
        }
    }
}
