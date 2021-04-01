//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Domain
{
    internal class StatisticsData
    {
        //Google How many chars is one page
        public const int CharsPerA4Page = 3000;

        public long Chars { get; set; }
        public long Bytes { get; set; }
        public long Words { get; set; }
        public long Lines { get; set; }
        public double Pages { get; set; }
    }
}
