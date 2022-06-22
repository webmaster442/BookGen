//-----------------------------------------------------------------------------
// (c) 2021-2022 Ruzsinszki Gábor
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
            return $"Characters:  {Chars:n0}\r\n"
                 + $"Total bytes: {Bytes:n0}\r\n"
                 + $"Words:       {Words:n0}\r\n"
                 + $"Paragraphs:  {ParagraphLines:n0}\r\n"
                 + $"Pages:       {PageCountLines / Constants.LinesPerPage:n0}";
        }
    }
}
