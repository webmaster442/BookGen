using BookGen.Vfs;

namespace Bookgen.Lib.Pipeline.Epub;

internal sealed class EpubState
{
    public IZipBuilder EpubFile
    {
        get => field ?? throw new InvalidOperationException($"{nameof(Init)} wasn't called"); 
        private set => field = value;
    }

    public void Init(IZipBuilder zipBuilder)
        => EpubFile = zipBuilder;

    public void Close()
        => EpubFile.Dispose();
}
