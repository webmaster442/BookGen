using BookGen.WebGui.Domain;

namespace BookGen.WebGui.Services;

internal class FileService : IFileService
{
    public IList<BrowserItem> GetFiles(string fullPath)
    {
        List<BrowserItem> list = new List<BrowserItem>();
        DirectoryInfo parent = new DirectoryInfo(fullPath);
        foreach (var directory in parent.GetDirectories())
        {
            list.Add(new BrowserItem
            {
                Extension = "DIR",
                FullPath = directory.FullName,
                LastModified = directory.LastWriteTime,
                Name = directory.Name,
                Size = 0,
            });
        }
        foreach (var file in parent.GetFiles())
        {
            list.Add(new BrowserItem
            {
                Extension = file.Extension.ToUpper(),
                FullPath = file.FullName,
                LastModified = file.LastWriteTime,
                Name = file.Name,
                Size = file.Length,
            });
        }
        return list;
    }
}
