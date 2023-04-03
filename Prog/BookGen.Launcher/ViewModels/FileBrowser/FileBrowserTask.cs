using System.Diagnostics;

namespace BookGen.Launcher.ViewModels.FileBrowser
{
    internal sealed record class FileBrowserTask
    {
        public required string Name { get; init; }
        public required string ProgramName { get; init; }
        public string Arguments { get; init; }

        public RelayCommand<string> Command { get; }

        public FileBrowserTask()
        {
            Arguments = string.Empty;
            Command = new RelayCommand<string>(OnCommand);
        }

        private void OnCommand(string? obj)
        {
            try
            {
                using var process = new Process();
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.FileName = ProgramName;
                process.StartInfo.Arguments = Arguments;
                process.StartInfo.WorkingDirectory = obj ?? Environment.CurrentDirectory;
                process.Start();
            }
            catch (Win32Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
