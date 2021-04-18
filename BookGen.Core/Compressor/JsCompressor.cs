//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using NUglify;

namespace BookGen.Core.Compressor
{
    internal class JsCompressor : CompressorBase
    {
        public override string Compress(string source)
        {
            UglifyResult ugly = Uglify.Js(source);
            return ProcessCompress(ugly);
        }
    }
}
