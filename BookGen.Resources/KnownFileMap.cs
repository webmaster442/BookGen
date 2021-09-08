//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections.Generic;

namespace BookGen.Resources
{
    internal static class KnownFileMap
    {
        public static readonly Dictionary<KnownFile, string> Map = new Dictionary<KnownFile, string>
        {
            //JsCss
            { KnownFile.BootstrapMinCss, "/JsCss/bootstrap.min.css" },
            { KnownFile.BootstrapMinJs, "/JsCss/bootstrap.min.js" },
            { KnownFile.JqueryMinJs, "/JsCss/jquery.min.js" },
            { KnownFile.PageTocJs, "/JsCss/PageToc.js" },
            { KnownFile.PopperMinJs, "/JsCss/popper.min.js" },
            { KnownFile.PrismCss, "/JsCss/prism.css" },
            { KnownFile.PrismJs, "/JsCss/prism.js" },
            { KnownFile.TurbolinksJs, "/JsCss/turbolinks.js" },
            { KnownFile.PreviewCss, "/JsCss/preview.css" },

            //html files
            { KnownFile.CookieWarningHtml, "/Html/CookieWarning.html" },
            { KnownFile.IndexHtml, "/Html/Index.html" },
            { KnownFile.SearchformHtml, "/Html/Searchform.html" },
            { KnownFile.TemplateEpubHtml, "/Html/TemplateEpub.html" },
            { KnownFile.TemplatePrintHtml, "/Html/TemplatePrint.html" },
            { KnownFile.TemplateWebHtml, "/Html/TemplateWeb.html" },
            { KnownFile.TemplateSinglePageHtml, "/Html/TemplateSinglePage.html" },
            { KnownFile.PreviewHtml, "/Html/TemplatePreview.html" },

            //etc
            { KnownFile.IndexMd, "/Etc/index.md" },
            { KnownFile.ScriptTemplateCs, "/Etc/ScriptTemplate.cs" },
            { KnownFile.SummaryMd, "/Etc/summary.md" },

        };
    }
}
