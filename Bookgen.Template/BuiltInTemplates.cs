//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;

namespace Bookgen.Template
{
    public abstract class BuiltInTemplates
    {
        public static string Epub
        {
            get { return ResourceLocator.GetResourceFile<BuiltInTemplates>("TemplateEpub/TemplateEpub.html"); }
        }

        public static string Print
        {
            get { return ResourceLocator.GetResourceFile<BuiltInTemplates>("TemplatePrint/TemplatePrint.html"); }
        }

        public static string TemplateWeb
        {
            get { return ResourceLocator.GetResourceFile<BuiltInTemplates>("TemplatePrint/TemplateWeb.html"); }
        }

        public static string Searchform
        {
            get { return ResourceLocator.GetResourceFile<BuiltInTemplates>("TemplatePrint/Searchform.html"); }
        }
        public static string AssetPrismCss
        {
            get { return ResourceLocator.GetResourceFile<BuiltInTemplates>("TemplatePrint/Assets/prism.css"); }
        }

        public static string AssetPrismJs
        {
            get { return ResourceLocator.GetResourceFile<BuiltInTemplates>("TemplatePrint/Assets/prism.js"); }
        }
    }
}
