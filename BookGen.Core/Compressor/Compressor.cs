//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using ZetaProducerHtmlCompressor.Internal;

namespace BookGen.Core.Compressor
{
    public static class Compressor
    {
        public static string CompressHtml(string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return string.Empty;
            }

            HtmlCompressor compressor = new HtmlCompressor();
            compressor.setRemoveComments(true);
            compressor.setCssCompressor(new CssCompressor());
            compressor.setCompressCss(true);
            compressor.setJavaScriptCompressor(new JsCompressor());
            compressor.setCompressJavaScript(true);

            return compressor.compress(source);
        }
    }
}
