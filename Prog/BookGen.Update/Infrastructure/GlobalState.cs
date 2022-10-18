using BookGen.Update.Dto;

namespace BookGen.Update.Infrastructure;

internal sealed class GlobalState
{
    public List<string> Issues { get; }
    public Release[] Releases { get; set; }
    public Release Latest { get; set; }

    public GlobalState()
    {
        Issues = new List<string>();
        Releases = Array.Empty<Release>();
        Latest = new Release();
    }
}
