using Bookgen.Lib.Domain.Epub;

using BookGen.Vfs;

namespace Bookgen.Lib.Pipeline.Epub;

internal sealed class EpubState
{
    public IZipBuilder EpubFile
    {
        get => field ?? throw new InvalidOperationException($"{nameof(Init)} wasn't called"); 
        private set => field = value;
    }

    public List<PackageItem> Items { get; }

    public EpubState()
    {
        Items = new List<PackageItem>
        {
            new PackageItem
            {
                Id = "ncx",
                Href = "toc.ncx",
                Mediatype = "application/x-dtbncx+xml",
            },
            new PackageItem
            {
                Id = "nav",
                Href = "nav.xhtml",
                Mediatype = "application/xhtml+xml",
                Properties = "nav",
            }
        };
    }

    public void Init(IZipBuilder zipBuilder)
        => EpubFile = zipBuilder;

    public void Close()
        => EpubFile.Dispose();
}
