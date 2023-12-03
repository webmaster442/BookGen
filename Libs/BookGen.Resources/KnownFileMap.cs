//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Resources
{
    internal static class KnownFileMap
    {
        public static readonly Dictionary<KnownFile, string> Map = new()
        {
            //JsCss
            { KnownFile.BootstrapMinCss, "/JsCss/bootstrap.min.css" },
            { KnownFile.BootstrapMinJs, "/JsCss/bootstrap.min.js" },
            { KnownFile.JqueryMinJs, "/JsCss/jquery.min.js" },
            { KnownFile.PageTocJs, "/JsCss/PageToc.js" },
            { KnownFile.PopperMinJs, "/JsCss/popper.min.js" },
            { KnownFile.PrismCss, "/JsCss/prism.css" },
            { KnownFile.PrismJs, "/JsCss/prism.js" },
            { KnownFile.PrismPrintCss, "/JsCss/prism.print.css" },
            { KnownFile.PreviewCss, "/JsCss/preview.css" },
            { KnownFile.SinglePageCss, "/JsCss/SinglePage.css"  },
            { KnownFile.EditCss, "/JsCss/edit.css" },

            //html files
            { KnownFile.CookieWarningHtml, "/Html/CookieWarning.html" },
            { KnownFile.IndexHtml, "/Html/Index.html" },
            { KnownFile.SearchformHtml, "/Html/Searchform.html" },
            { KnownFile.TemplateEpubHtml, "/Html/TemplateEpub.html" },
            { KnownFile.TemplatePrintHtml, "/Html/TemplatePrint.html" },
            { KnownFile.TemplateWebHtml, "/Html/TemplateWeb.html" },
            { KnownFile.TemplateSinglePageHtml, "/Html/TemplateSinglePage.html" },
            { KnownFile.PreviewHtml, "/Html/TemplatePreview.html" },
            { KnownFile.EditHtml, "/Html/TemplateEdit.html" },
            { KnownFile.TerminalRenderingHtml, "/Html/TerminalRendering.html" },

            //etc
            { KnownFile.IndexMd, "/Etc/index.md" },
            { KnownFile.SummaryMd, "/Etc/summary.md" },

            //Png
            { KnownFile.FaviconP, "Png/favicon-p.png" },
            { KnownFile.FaviconT, "Png/favicon-t.png" },
            { KnownFile.FaviconFs, "Png/favicon-fs.png" },

        };
    }
}
