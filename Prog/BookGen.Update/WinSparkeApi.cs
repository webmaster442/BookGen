using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;

namespace BookGen.Update;
internal partial class WinSparkeApi
{
    [LibraryImport("WinSparkle.dll", EntryPoint = "win_sparkle_init")]
    public static partial void Init();

    [LibraryImport("WinSparkle.dll", EntryPoint = "win_sparkle_check_update_with_ui")]
    public static partial void CheckUpdateWithUi();

    [LibraryImport("WinSparkle.dll", EntryPoint = "win_sparkle_cleanup")]
    public static partial void Cleanup();

    [LibraryImport("WinSparkle.dll", EntryPoint = "win_sparkle_set_appcast_url", StringMarshalling = StringMarshalling.Custom, StringMarshallingCustomType = typeof(AnsiStringMarshaller))]
    public static partial void SetAppcastUrl(string url);

    [LibraryImport("WinSparkle.dll", EntryPoint = "win_sparkle_set_dsa_pub_pem", StringMarshalling = StringMarshalling.Custom, StringMarshallingCustomType = typeof(AnsiStringMarshaller))]
    public static partial void SetDsaPubPem(string dsaPubPem);

    [LibraryImport("WinSparkle.dll", EntryPoint = "win_sparkle_set_app_details", StringMarshalling = StringMarshalling.Utf16)]
    public static partial void SetAppDetails(string companyName, string appName, string AppVersion);
}
