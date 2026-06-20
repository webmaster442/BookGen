//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Reflection;

namespace Bookgen.Lib.Rendering.Templates;

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

    /// <summary>
    /// Host url for links and images
    /// </summary>
    public required string Host { get; init; }

    /// <summary>
    /// Last modified date of the document
    /// </summary>
    public required DateTime LastModified { get; init; }

    public Dictionary<string, string> AdditionalData { get; init; } = new();

    public Dictionary<string, string> GetDataTable(StringComparer comparer)
    {
        Dictionary<string, string> result = new(comparer);
        IEnumerable<PropertyInfo> properties = GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.Name != nameof(AdditionalData));

        foreach (PropertyInfo? property in properties)
        {
            result.Add(property.Name, property.GetValue(this)?.ToString() ?? "");
        }

        foreach (KeyValuePair<string, string> kvp in AdditionalData)
        {
            result.Add(kvp.Key, kvp.Value);
        }

        return result;
    }
}
