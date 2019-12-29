//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;

namespace Bookgen.Template
{
    public sealed class BuiltInTemplates
    {
        private BuiltInTemplates() { }

        public static string IndexMd
        {
            get { return ResourceLocator.GetResourceFile<BuiltInTemplates>("/TemplateMarkdown/index.md"); }
        }

        public static string SummaryMd
        {
            get { return ResourceLocator.GetResourceFile<BuiltInTemplates>("/TemplateMarkdown/summary.md"); }
        }

        public static string Epub
        {
            get { return ResourceLocator.GetResourceFile<BuiltInTemplates>("/TemplateEpub/TemplateEpub.html"); }
        }

        public static string Print
        {
            get { return ResourceLocator.GetResourceFile<BuiltInTemplates>("/TemplatePrint/TemplatePrint.html"); }
        }

        public static string TemplateWeb
        {
            get { return ResourceLocator.GetResourceFile<BuiltInTemplates>("/TemplateWeb/TemplateWeb.html"); }
        }

        public static string Searchform
        {
            get { return ResourceLocator.GetResourceFile<BuiltInTemplates>("/TemplateWeb/Searchform.html"); }
        }

        public static string CookieWarningCode
        {
            get { return ResourceLocator.GetResourceFile<BuiltInTemplates>("/TemplateWeb/CookieWarning.html"); }
        }

        public static string AssetPrismCss
        {
            get { return ResourceLocator.GetResourceFile<BuiltInTemplates>("/TemplateWeb/Assets/prism.css"); }
        }

        public static string AssetPrismJs
        {
            get { return ResourceLocator.GetResourceFile<BuiltInTemplates>("/TemplateWeb/Assets/prism.js"); }
        }

        public static string AssetBootstrapCSS
        {
            get { return ResourceLocator.GetResourceFile<BuiltInTemplates>("/TemplateWeb/Assets/bootstrap.min.css"); }
        }

        public static string AssetBootstrapJs
        {
            get { return ResourceLocator.GetResourceFile<BuiltInTemplates>("/TemplateWeb/Assets/bootstrap.min.js"); }
        }

        public static string AssetJqueryJs
        {
            get { return ResourceLocator.GetResourceFile<BuiltInTemplates>("/TemplateWeb/Assets/jquery.min.js"); }
        }

        public static string AssetPopperJs
        {
            get { return ResourceLocator.GetResourceFile<BuiltInTemplates>("/TemplateWeb/Assets/popper.min.js"); }
        }

    }
}
