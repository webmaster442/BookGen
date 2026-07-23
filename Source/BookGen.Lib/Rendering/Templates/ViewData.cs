//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Reflection;

namespace BookGen.Lib.Rendering.Templates;

public class ViewData : IDictionaryConvertible, ICanProvideRoughSize
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

    public int CalculateRoughSize()
    {
        return Title.Length
            + Content.Length
            + Host.Length
            + AdditionalData.Sum(kvp => kvp.Key.Length + kvp.Value.Length);
    }

    public Dictionary<string, object?> ToDictionary(StringComparer comparer)
    {
        Dictionary<string, object?> result = new(comparer);
        IEnumerable<PropertyInfo> properties = GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.Name != nameof(AdditionalData));

        foreach (PropertyInfo? property in properties)
        {
            result.Add(property.Name, property.GetValue(this));
        }

        foreach (KeyValuePair<string, string> kvp in AdditionalData)
        {
            result.Add(kvp.Key, kvp.Value);
        }

        return result;
    }
}
