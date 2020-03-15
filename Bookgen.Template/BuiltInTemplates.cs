//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;

namespace BookGen.Template
{
    public sealed class BuiltInTemplates
    {
        private BuiltInTemplates() { }

        public static string IndexMd => ResourceLocator.GetResourceFile<BuiltInTemplates>("/TemplateMarkdown/index.md");

        public static string SummaryMd => ResourceLocator.GetResourceFile<BuiltInTemplates>("/TemplateMarkdown/summary.md");

        public static string Epub => ResourceLocator.GetResourceFile<BuiltInTemplates>("/TemplateEpub/TemplateEpub.html");

        public static string Print => ResourceLocator.GetResourceFile<BuiltInTemplates>("/TemplatePrint/TemplatePrint.html");

        public static string TemplateWeb => ResourceLocator.GetResourceFile<BuiltInTemplates>("/TemplateWeb/TemplateWeb.html");

        public static string Searchform => ResourceLocator.GetResourceFile<BuiltInTemplates>("/TemplateWeb/Searchform.html");

        public static string CookieWarningCode => ResourceLocator.GetResourceFile<BuiltInTemplates>("/TemplateWeb/CookieWarning.html");

        public static string AssetPrismCss => ResourceLocator.GetResourceFile<BuiltInTemplates>("/TemplateWeb/Assets/prism.css");

        public static string AssetPrismJs => ResourceLocator.GetResourceFile<BuiltInTemplates>("/TemplateWeb/Assets/prism.js");

        public static string AssetBootstrapCSS => ResourceLocator.GetResourceFile<BuiltInTemplates>("/TemplateWeb/Assets/bootstrap.min.css");

        public static string AssetBootstrapJs => ResourceLocator.GetResourceFile<BuiltInTemplates>("/TemplateWeb/Assets/bootstrap.min.js");

        public static string AssetJqueryJs => ResourceLocator.GetResourceFile<BuiltInTemplates>("/TemplateWeb/Assets/jquery.min.js");

        public static string AssetPopperJs => ResourceLocator.GetResourceFile<BuiltInTemplates>("/TemplateWeb/Assets/popper.min.js");

        public static string AssetTurbolinksJs => ResourceLocator.GetResourceFile<BuiltInTemplates>("/TemplateWeb/Assets/turbolinks.js");

        public static string ScriptTemplate => ResourceLocator.GetResourceFile<BuiltInTemplates>("/TemplateScript/ScriptTemplate.cs");
    }
}
