//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics;

namespace BookGen.Web;
internal static class ErrorPageFactory
{
    public static string GetErrorPage(int code, string message)
    {
        using var stream = typeof(ErrorPageFactory).Assembly.GetManifestResourceStream("BookGen.Web.ErrorPageTemplate.html")
            ?? throw new UnreachableException("Error page template was null");

        using var reader = new StreamReader(stream);
        var page = reader.ReadToEnd();
        
        return page.Replace("{{code}}", code.ToString()).Replace("{{message}}", message);
    }
}
