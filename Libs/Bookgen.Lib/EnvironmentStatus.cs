using System.Collections;

namespace Bookgen.Lib;

public sealed class EnvironmentStatus : IEnumerable<string>
{
    private readonly List<string> _issues;

    public EnvironmentStatus()
    {
        _issues = new List<string>();
    }

    public void AddIssue(string message)
        => _issues.Add(message);

    public bool IsOk => _issues.Count < 1;

    public IEnumerator<string> GetEnumerator() 
        => _issues.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
}
