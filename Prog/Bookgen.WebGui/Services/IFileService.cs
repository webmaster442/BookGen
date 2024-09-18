
using BookGen.WebGui.Domain;

namespace BookGen.WebGui.Services;
public interface IFileService
{
    IList<BrowserItem> GetFiles(string fullPath);
}