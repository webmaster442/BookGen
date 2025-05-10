using System.Reflection;

namespace Bookgen.Lib.Templates;

public class ViewData
{
    /// <summary>
    /// HTML document title
    /// </summary>
    public required string Title { get; init; }

    /// <summary>
    /// HTML document content
    /// </summary>
    public required string Content { get; init; }

    public Dictionary<string, string> GetDataTable()
    {
        Dictionary<string, string> result = new();
        var properties = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (var property in properties)
        {
            result.Add(property.Name, property.GetValue(this)?.ToString() ?? "");
        }
        return result;
    }
}
