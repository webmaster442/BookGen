using Bookgen.Lib.Domain.Epub;

using BookGen.Vfs;

namespace Bookgen.Lib.Pipeline.Epub;

internal sealed class EpubState
{
    public IZipBuilder EpubFile
    {
        get => field ?? throw new InvalidOperationException($"{nameof(Initialize)} wasn't called"); 
        private set => field = value;
    }

    public List<PackageItem> PackageItems { get; }

    public Dictionary<string, string> ImagesData { get; }

    public PackageSpine Spine { get;} 

    public EpubState()
    {
        ImagesData = new Dictionary<string, string>();
        PackageItems = new List<PackageItem>
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
        Spine = new PackageSpine
        {
            Itemref = new List<PackageSpineItemref>(),
            Toc = "ncx"
        };
    }

    public void Initialize(IZipBuilder zipBuilder)
        => EpubFile = zipBuilder;

    public void Deinitialize()
        => EpubFile.Dispose();
}
