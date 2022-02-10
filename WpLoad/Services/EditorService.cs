using System.Diagnostics;

namespace WpLoad.Services
{
    internal static class EditorService
    {
        private const int step = 100;

        internal static async Task<int> OpenAndWaitClose(string newTempFile)
        {
            using (var process = new Process())
            {
                process.StartInfo.FileName = newTempFile;
                process.StartInfo.UseShellExecute = true;
                process.Start();

                int time = 0;
                while (!process.HasExited)
                {
                    time += step;
                    await Task.Delay(time);
                }
            }

            return step;
        }
    }
}
