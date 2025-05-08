using System.Collections;

namespace Bookgen.Lib;

public sealed class EnvironmentStatus : ICollection<string>
{
    private readonly List<string> _issues;

    public EnvironmentStatus()
    {
        _issues = new List<string>();
    }

    public void Add(string message)
        => _issues.Add(message);

    public bool IsOk => _issues.Count < 1;

    public int Count => _issues.Count;

    public bool IsReadOnly => false;

    public IEnumerator<string> GetEnumerator()
        => _issues.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();

    public void Clear()
        => _issues.Clear();

    public bool Contains(string item)
        => _issues.Contains(item);

    public void CopyTo(string[] array, int arrayIndex)
        => _issues.CopyTo(array, arrayIndex);

    public bool Remove(string item)
        => _issues.Remove(item);
}
