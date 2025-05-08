//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Numerics;
using System.Reflection;

namespace BookGen.Domain;

public sealed class StatisticResult : 
    IAdditionOperators<StatisticResult, StatisticResult, StatisticResult>
{
    public int Words { get; set; }
    public int Characters { get; set; }
    public int Paragraphs { get; set; }
    public long Bytes { get; set; }
    public int LineCountForPrint { get; set; }

    public static StatisticResult operator +(StatisticResult left, StatisticResult right)
    {
        return new StatisticResult
        {
            Characters = left.Characters + right.Characters,
            Paragraphs = left.Paragraphs + right.Paragraphs,
            Words = left.Words + right.Words,
            Bytes = left.Bytes + right.Bytes,
            LineCountForPrint = left.LineCountForPrint + right.LineCountForPrint
        };
    }

    public IDictionary<string, object> ToTable()
    {
        return GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .ToDictionary(x => x.Name, x => x.GetValue(this) ?? "null");
    }
}
