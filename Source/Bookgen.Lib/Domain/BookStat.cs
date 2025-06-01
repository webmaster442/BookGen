namespace Bookgen.Lib.Domain;

public sealed class BookStat
{
    public Dictionary<string, int> FileCountsByExtension { get; } = new Dictionary<string, int>(StringComparer.InvariantCultureIgnoreCase);
    public Dictionary<string, long> FileSizeByExtension { get; } = new Dictionary<string, long>(StringComparer.InvariantCultureIgnoreCase);
    public Dictionary<string, long> ChapterSizes { get; } = new Dictionary<string, long>();
    public long TotalSize { get; set; }
    public long WordCount { get; set; }
    public long CharacterCount { get; set; }
    public long LineCount { get; set; }
}
