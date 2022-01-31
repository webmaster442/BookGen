//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Domain
{
    internal class StatisticsData
    {
        public long Chars { get; set; }
        public long Bytes { get; set; }
        public long Words { get; set; }
        public long ParagraphLines { get; set; }
        public long PageCountLines { get; set; }

        public override string ToString()
        {
            return $"Characters: {Chars}"
                + $"Total bytes: {Bytes}"
                + $"Words:       {Words}"
                + $"Paragraphs:  {ParagraphLines}"
                + $"Pages:       {PageCountLines / Constants.LinesPerPage}";
        }
    }
}
