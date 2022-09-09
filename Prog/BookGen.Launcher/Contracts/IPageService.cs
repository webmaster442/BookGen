using System.Windows.Controls;

namespace BookGen.Launcher.Contracts.Services;

public interface IPageService
{
    Type GetPageType(string key);

    Page GetPage(string key);
}
