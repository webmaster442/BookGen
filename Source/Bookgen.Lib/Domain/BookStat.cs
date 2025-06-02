namespace Bookgen.Lib.Domain;

public sealed class BookStat
{
    public Dictionary<string, double> FileCountsByExtension { get; }
    public Dictionary<string, double> FileSizeByExtension { get; }
    public Dictionary<string, double> ChapterSizes { get; }
    public long TotalSize { get; set; }
    public long WordCount { get; set; }
    public long CharacterCount { get; set; }
    public long LineCount { get; set; }

    public BookStat()
    {
        FileCountsByExtension = new Dictionary<string, double>(StringComparer.InvariantCultureIgnoreCase);
        FileSizeByExtension = new Dictionary<string, double>(StringComparer.InvariantCultureIgnoreCase);
        ChapterSizes = new Dictionary<string, double>();
    }
}
