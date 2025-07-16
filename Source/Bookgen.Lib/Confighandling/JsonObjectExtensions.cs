using System.Runtime.CompilerServices;
using System.Text.Json.Nodes;

namespace Bookgen.Lib.Confighandling;

internal static class JsonObjectExtensions
{
    public static JsonObject GetSubObjectOrThrow(this JsonObject jsonObject, string propertyName)
    {
        if (jsonObject.TryGetPropertyValue(propertyName, out var subObjectNode) &&
            subObjectNode is JsonObject subObject)
        {
            return subObject;
        }
        throw new InvalidOperationException($"Property '{propertyName}' not found or is not an object.");
    }

    public static JsonArray GetSubArrayOrThrow(this JsonObject jsonObject, string propertyName)
    {
        if (jsonObject.TryGetPropertyValue(propertyName, out var subArrayNode) &&
            subArrayNode is JsonArray subArray)
        {
            return subArray;
        }
        throw new InvalidOperationException($"Property '{propertyName}' not found or is not an array.");
    }
}