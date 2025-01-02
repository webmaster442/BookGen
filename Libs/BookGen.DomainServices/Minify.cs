//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

using BookGen.Interfaces;

using Microsoft.Extensions.Logging;

using NUglify;
using NUglify.Css;
using NUglify.Html;
using NUglify.JavaScript;

namespace BookGen.DomainServices;

public static class Minify
{
    private static readonly CssSettings CssSettings = new()
    {
        CommentMode = CssComment.None,
        ColorNames = CssColor.Strict,
        LineTerminator = "\n",
        RemoveEmptyBlocks = true,
        MinifyExpressions = true,
        OutputMode = OutputMode.SingleLine,
    };

    private static readonly CodeSettings JavaScriptSettings = new()
    {
        OutputMode = OutputMode.SingleLine,
        PreserveImportantComments = false,
        StripDebugStatements = true,
        LineTerminator = "\n",
        MinifyCode = true,
    };

    private static readonly HtmlSettings HtmlSettings = new()
    {
        MinifyCss = true,
        MinifyJs = true,
        JsSettings = JavaScriptSettings,
        CssSettings = CssSettings,
        CollapseWhitespaces = true,
        RemoveComments = true,
    };

    public static bool TryMinify(FsPath file, ILogger log, [NotNullWhen(true)]out string? result)
    {
        static void LogIssues(List<UglifyError> errors, ILogger log)
        {
            foreach (var error in errors)
            {
                log.LogWarning("Uglify errror: {uglyerror}", error.ToString());
            }
        }

        UglifyResult uglifyResult;
        var ext = file.Extension.ToLower();

        switch (ext)
        {
            case ".css":
                uglifyResult = Uglify.Css(file.ReadFile(log), CssSettings);
                break;
            case ".js":
                uglifyResult = Uglify.Js(file.ReadFile(log), JavaScriptSettings);
                break;
            case ".html":
            case ".htm":
                uglifyResult = Uglify.Html(file.ReadFile(log), HtmlSettings);
                break;
            default:
                log.LogWarning("Unsupported file extension: {extension}", file.Extension);
                result = null;
                return false;
        }

        LogIssues(uglifyResult.Errors, log);
        result = uglifyResult.Code;
        return uglifyResult.HasErrors;
    }
}
