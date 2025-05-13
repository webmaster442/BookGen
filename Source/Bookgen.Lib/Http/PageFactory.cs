//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics;
using System.Text;
using System.Web;

namespace Bookgen.Lib.Http;

internal static class PageFactory
{
    private static string GetResource(string resoruceName)
    {
        using var stream = typeof(PageFactory).Assembly.GetManifestResourceStream(resoruceName)
            ?? throw new UnreachableException("Error page template was null");

        using var reader = new StreamReader(stream);
        var content = reader.ReadToEnd();

        return content;
    }

    public static string GetErrorPage(int code, string message)
    {
        var page = GetResource($"Bookgen.Lib.Http.ErrorPageTemplate.html");
        return page.Replace("{{code}}", code.ToString()).Replace("{{message}}", message);
    }

    public static string GetQrCodePage(IEnumerable<string> urls)
    {
        StringBuilder qrcodes = new StringBuilder(4096);
        foreach (var url in urls)
        {
            qrcodes.AppendLine("<figure>");
            qrcodes.AppendLine($"<img src=\"https://api.qrserver.com/v1/create-qr-code/?data={HttpUtility.UrlEncode(url)}&size=300x300\" width=\"300\" height=\"300\"/>");
            qrcodes.AppendLine($"<figcaption>{url}</figcaption>");
            qrcodes.AppendLine("</figure>");
        }

        return GetResource("Bookgen.Lib.Http.QRCodeTemplate.html").Replace("{{links}}", qrcodes.ToString());
    }
}
