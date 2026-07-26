//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Lib;

public static class BundledAssets
{
    public const string PrismJs = "prism.js";
    public const string BookGenCss = "bookgen.min.css";
    public const string TemplateSinglePage = "Md2Html.html";
    public const string ProtectHtml = "protect.html";
    public const string TemplateStaticWeb = "Static.html";
    public const string TemplatePrint = "Print.html";
    public const string TemplateEpub = "Epub.html";
    public const string TemplateBlank = "Blank.html";
    public const string TemplatePreview = "Preview.html";
    public const string QrCodeJs = "qrcode.min.js";
    public const string JsPageToc = "PageToc.js";
    public const string NomnomlJs = "nomnoml.js";
    public const string GraphreJs = "graphre.js";
    public const string WordTemplate = "Template.docx";

    public const string Md2HtmlTemplateAir = "Md2HtmlAir.html";
    public const string Md2HtmlTemplateRetro = "Md2HtmlRetro.html";
    public const string Md2HtmlTemplatteTinyDark = "Md2HtmlTinyDark.html";
    public const string Md2HtmlTemplatteTinyLight = "Md2HtmlTinyLight.html";
    public const string Md2HtmlTemplatteMvp = "Md2HtmlMvp.html";

    public static IEnumerable<string> Md2HtmlTemplates
    {
        get
        {
            yield return TemplateSinglePage;
            yield return Md2HtmlTemplateAir;
            yield return Md2HtmlTemplateRetro;
            yield return Md2HtmlTemplatteTinyDark;
            yield return Md2HtmlTemplatteTinyLight;
            yield return Md2HtmlTemplatteMvp;
        }
    }
}
