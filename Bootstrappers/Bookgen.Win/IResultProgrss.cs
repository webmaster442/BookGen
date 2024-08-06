namespace Bookgen.Win
{
    public interface IResultProgrss : ICountProgress
    {
        void ReportFailed(string fileName);
    }
}
