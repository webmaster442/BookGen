using System.Collections;

namespace BookGen.Launcher.ViewModels.FileBrowser
{
    internal sealed class FileBrowserTasks : IEnumerable<FileBrowserTask>
    {
        public IEnumerator<FileBrowserTask> GetEnumerator()
        {
            yield return new FileBrowserTask
            {
                Name = "Update tags...",
                ProgramName = "BookGen.exe",
                Arguments = "tags"
            };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
