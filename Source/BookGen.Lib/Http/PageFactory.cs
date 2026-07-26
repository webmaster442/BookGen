//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics;
using System.Text;
using System.Web;

namespace BookGen.Lib.Http;

internal static class PageFactory
{
    private static string GetResource(string resoruceName)
    {
        using Stream stream = typeof(PageFactory).Assembly.GetManifestResourceStream(resoruceName)
            ?? throw new UnreachableException("Error page template was null");

        using var reader = new StreamReader(stream);
        var content = reader.ReadToEnd();

        return content;
    }

    public static string GetFiles(List<string> allowedFiles)
    {
        var files = new StringBuilder();
        IEnumerable<IGrouping<string, string>> groups = allowedFiles.GroupBy(f => Path.GetDirectoryName(f) ?? string.Empty);
        foreach (IGrouping<string, string> group in groups)
        {
            files.AppendLine($"<h2>{group.Key}</h2>");
            files.AppendLine("<ul>");
            foreach (var file in group)
            {
                files.AppendLine($"<li><a href=\"/preview?file={HttpUtility.UrlEncode(file)}\">{file}</a></li>");
            }
            files.AppendLine("</ul>");
        }
        return files.ToString();
    }

    public static string GetErrorPage(int code, string message)
    {
        var page = GetResource($"BookGen.Lib.Http.ErrorPageTemplate.html");
        return page.Replace("{{Code}}", code.ToString()).Replace("{{Message}}", message);
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

        return GetResource("BookGen.Lib.Http.QRCodeTemplate.html").Replace("{{Links}}", qrcodes.ToString());
    }
}
