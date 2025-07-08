namespace BookGen.Tooldownloaders;

internal interface IDownloadUi : IProgress<long>
{
    void BeginNew(string message, long maximum);
    void Error(string message);
}
