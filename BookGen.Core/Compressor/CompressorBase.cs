//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using NUglify;
using System;
using ZetaProducerHtmlCompressor.Internal;

namespace BookGen.Core.Compressor
{
    internal abstract class CompressorBase : ICompressor
    {
        public abstract string Compress(string source);

        protected static string ProcessCompress(UglifyResult result)
        {
            if (result.HasErrors)
            {
                throw new InvalidOperationException(string.Join('\n', result.Errors));
            }
            return result.Code;
        }

        string ICompressor.compress(string source)
        {
            return Compress(source);
        }
    }
}
