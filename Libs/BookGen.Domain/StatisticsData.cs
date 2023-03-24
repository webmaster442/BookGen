//-----------------------------------------------------------------------------
// (c) 2021-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Net.Http.Headers;

namespace BookGen.Domain
{
    public class StatisticsData
    {
        public long Chars { get; set; }
        public long Bytes { get; set; }
        public long Words { get; set; }
        public long ParagraphLines { get; set; }
        public long PageCountLines { get; set; }

        public IDictionary<string, long> ToTable()
        {
            return new Dictionary<string, long>
            {
                { "Characters", Chars },
                { "Total bytes", Bytes },
                { "Words", Words },
                { "Paragraphs", ParagraphLines },
                { "Pages", PageCountLines / Constants.LinesPerPage }
            };
        }
    }
}
