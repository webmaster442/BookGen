using System.Data;
using System.Reflection;

namespace BookGen.Update;

internal sealed class Updater : IDisposable
{
    private const string CompanyName = "Webmaster442";
    private const string AppName = "BookGen";

    private static string GetVersion()
    {
        var version = typeof(Updater).Assembly.GetName().Version;
        return version != null
            ? version.ToString()
            : $"{DateTime.Now.Year}.{DateTime.Now.Month}.{DateTime.Now.Day}.0";
    }

    private string GetDsaContents()
    {
        return string.Empty;
    }

    public Updater()
    {
        WinSparkeApi.Init();
        WinSparkeApi.SetAppDetails(CompanyName, AppName, GetVersion());
        WinSparkeApi.SetAppcastUrl("");
        WinSparkeApi.SetDsaPubPem(GetDsaContents());
    }

    public void CheckUpdate()
        => WinSparkeApi.CheckUpdateWithUi();

    public void Dispose()
    {
        WinSparkeApi.Cleanup();
    }
}
