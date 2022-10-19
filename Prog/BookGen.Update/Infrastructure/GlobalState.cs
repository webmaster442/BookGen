using BookGen.Update.Dto;

namespace BookGen.Update.Infrastructure;

internal sealed class GlobalState
{
    public List<string> Issues { get; }
    public Release[] Releases { get; set; }
    public Release Latest { get; set; }
    public string TempFile { get; set; }
    public string TargetDir { get; }

    public GlobalState()
    {
        Issues = new List<string>();
        Releases = Array.Empty<Release>();
        Latest = new Release();
        TempFile = string.Empty;
        TargetDir = AppContext.BaseDirectory;
    }

    public void Cleanup()
    {
        if (File.Exists(TempFile))
        {
            File.Delete(TempFile);
        }
    }
}
