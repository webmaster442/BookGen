using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Schema;
using System.Text.Json.Serialization.Metadata;

namespace BookGen.Vfs.Internals;
internal static class JsonSchemaTransformer
{
    private const string SchemaDescriptionProperty = "description";
    private const string SchemaMinimumProperty = "minimum";
    private const string SchemaMaximumProperty = "maximum";
    private const string SchemaMinItemsProperty = "minItems";
    private const string SchemaMaxItemsProperty = "maxItems";
    private const string SchemaPatternProperty = "pattern";
    private const string SchemaMinLengthProperty = "minLength";
    private const string SchemaMaxLengthProperty = "maxLength";

    public static JsonNode TransformSchemaNode(JsonSchemaExporterContext context, JsonNode node)
    {
        ICustomAttributeProvider? attributeProvider = context.PropertyInfo is not null
         ? context.PropertyInfo.AttributeProvider
         : context.TypeInfo.Type;

        if (node is JsonObject obj
            && obj.TryGetPropertyValue("properties", out JsonNode? propertiesNode) 
            && propertiesNode is JsonObject properties)
        {
            var requiredArray = new JsonArray();
            foreach (var property in properties)
            {
                requiredArray.Add(property.Key);
            }
            obj["required"] = requiredArray;
        }

        AddDecription(node, GetAttribute<DescriptionAttribute>(attributeProvider));
        AddRange(node, GetAttribute<RangeAttribute>(attributeProvider), context.PropertyInfo);
        AddRegex(node, GetAttribute<RegularExpressionAttribute>(attributeProvider)?.Pattern, context.PropertyInfo);
        AddLength(node, GetAttribute<MinLengthAttribute>(attributeProvider)?.Length, context.PropertyInfo, minLength: true);
        AddLength(node, GetAttribute<MaxLengthAttribute>(attributeProvider)?.Length, context.PropertyInfo, minLength: false);

        return node;
    }

    private static T? GetAttribute<T>(ICustomAttributeProvider? attributeProvider)
    {
        if (attributeProvider == null)
            return default;

        return attributeProvider.GetCustomAttributes(inherit: true).OfType<T>().FirstOrDefault();
    }

    private static void AddDecription(JsonNode node, DescriptionAttribute? description)
    {
        if (description == null)
            return;

        if (node is not JsonObject jObj)
        {
            JsonValueKind valueKind = node.GetValueKind();
            Debug.Assert(valueKind is JsonValueKind.True or JsonValueKind.False);
            node = jObj = new JsonObject();
            if (valueKind is JsonValueKind.False)
            {
                jObj.Add("not", true);
            }
        }

        jObj.Add(SchemaDescriptionProperty, description.Description);
    }

    private static void AddRange(JsonNode node, RangeAttribute? rangeAttribute, JsonPropertyInfo? propertyInfo)
    {
        static bool IsNumeric(Type? propertyType)
        {
            return propertyType == typeof(int)
                || propertyType == typeof(uint)
                || propertyType == typeof(long)
                || propertyType == typeof(ulong)
                || propertyType == typeof(short)
                || propertyType == typeof(ushort)
                || propertyType == typeof(byte)
                || propertyType == typeof(sbyte)
                || propertyType == typeof(double)
                || propertyType == typeof(float)
                || propertyType == typeof(decimal);
        }

        static JsonNode? ToNumber(object value)
        {
            return value switch
            {
                int integer => (JsonNode)integer,
                double dbl => (JsonNode)dbl,
                _ => throw new ArgumentException($"Unsupported numeric type: {value.GetType()}", nameof(value)),
            };
        }

        if (rangeAttribute == null || node is not JsonObject jObj)
            return;

        if (IsNumeric(propertyInfo?.PropertyType))
        {
            jObj.Add(SchemaMinimumProperty, ToNumber(rangeAttribute.Minimum));
            jObj.Add(SchemaMaximumProperty, ToNumber(rangeAttribute.Maximum));
        }
    }

    private static void AddRegex(JsonNode node, string? regexPattern, JsonPropertyInfo? propertyInfo)
    {
        if (regexPattern == null || node is not JsonObject jObj)
            return;

        if (propertyInfo?.PropertyType == typeof(string))
        {
            jObj.Add(SchemaPatternProperty, regexPattern);
        }
    }

    private static void AddLength(JsonNode node, int? length, JsonPropertyInfo? propertyInfo, bool minLength)
    {
        static bool IsCollection(Type? propertyType)
        {
            return propertyType?.IsArray == true
                || propertyType?.IsAssignableTo(typeof(System.Collections.IEnumerable)) == true;
        }

        if (length == null || node is not JsonObject jObj)
            return;

        if (propertyInfo?.PropertyType == typeof(string))
        {
            if (minLength)
                jObj.Add(SchemaMinLengthProperty, length.Value);
            else
                jObj.Add(SchemaMaxLengthProperty, length.Value);
        }
        else if (IsCollection(propertyInfo?.PropertyType))
        {
            if (minLength)
                jObj.Add(SchemaMinItemsProperty, length.Value);
            else
                jObj.Add(SchemaMaxItemsProperty, length.Value);
        }

    }
}
