//-----------------------------------------------------------------------------
// (c) 2023-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections.Concurrent;
using System.Globalization;
using System.Text;

using BookGen.Domain;

using Microsoft.Extensions.Logging;

namespace BookGen.DomainServices;

public static class StatisticsCollector
{
    public static StatisticResult ComputeStatistics(IEnumerable<string> files, ILogger log)
    {
        ConcurrentBag<StatisticResult> results = new ConcurrentBag<StatisticResult>();
        Parallel.ForEach(files, file => results.Add(ComputeStatistics(file, log)));
        return results.Aggregate((sum, fileStat) => sum + fileStat);
    }

    public static StatisticResult ComputeStatistics(string file, ILogger log)
    {
        try
        {
            using (var reader = File.OpenText(file)) 
            {
                var statistics = new StatisticResult();
                string? line;
                while ((line = reader.ReadLine()) != null)
                {
                    statistics.Characters += new StringInfo(line).LengthInTextElements;
                    statistics.Words += line.Split(new char[] { '\t', '\n', }, StringSplitOptions.RemoveEmptyEntries).Length;
                    statistics.Bytes += Encoding.UTF8.GetBytes(line).LongLength;
                    statistics.Paragraphs += string.IsNullOrEmpty(line) ? 0 : 1;
                    statistics.LineCountForPrint += GetLineCountForPrint(line); 
                }
                return statistics;
            }
        }
        catch (Exception ex)
        {
            log.LogWarning(ex, "Reading of file file {file} has failed, skipping in stat...", file);
            return new StatisticResult();
        }
    }

    private static int GetLineCountForPrint(string line)
    {
        return line.Length < 80 ? 1 : (int)Math.Ceiling(line.Length / 80.0);
    }
}
