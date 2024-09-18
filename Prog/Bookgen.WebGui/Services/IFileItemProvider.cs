
using BookGen.WebGui.Domain;

namespace BookGen.WebGui.Services;
public interface IFileItemProvider
{
    IList<BrowserItem> GetFiles(string id);
}