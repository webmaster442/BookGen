//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics;
using System.Windows.Input;

using BookGen.Launcher.ViewModels.FileBrowser;

namespace BookGen.Launcher.ViewModels.Commands;
internal class TaskRunnerCommand : ICommand
{
    public event EventHandler? CanExecuteChanged;

    public string Folder { get; set; }

    public TaskRunnerCommand()
    {
        Folder = Environment.CurrentDirectory;
    }

    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public void Execute(object? parameter)
    {
        if (parameter is not BookGenTask task)
            return;

        using (var process = new Process())
        {
            var bookGenPath = Path.Combine(AppContext.BaseDirectory, "bookgen.exe");
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = $"/k {bookGenPath} {task.Command}";
            process.StartInfo.WorkingDirectory = Folder;
            process.StartInfo.UseShellExecute = false;
            process.Start();
        }
    }
}
