//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Bookgen.Lib.Domain.Epub;
using Bookgen.Lib.Domain.IO.Configuration;

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

    public record class ChapterItem(string Title, string FileName);

    public Dictionary<string, string> ImagesData { get; }

    public PackageSpine Spine { get; }

    public Dictionary<string, List<ChapterItem>> TocData { get; }

    public Guid BookId { get; }

    public EpubState()
    {
        BookId = Guid.CreateVersion7();
        TocData = new();
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
                Href = "content/nav.xhtml",
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
