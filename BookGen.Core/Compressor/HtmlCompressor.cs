//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Core.Compressor
{
    public static class HtmlCompressor
    {
        public static string CompressHtml(string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return string.Empty;
            }

            var compressor = new ZetaProducerHtmlCompressor.Internal.HtmlCompressor();
            compressor.setRemoveComments(true);
            compressor.setCssCompressor(new CssCompressor());
            compressor.setCompressCss(true);
            compressor.setJavaScriptCompressor(new JsCompressor());
            compressor.setCompressJavaScript(true);

            return compressor.compress(source);
        }
    }
}
