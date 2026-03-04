//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Schema;
using System.Text.Json.Serialization;
using System.Text.Unicode;

using BookGen.Vfs.Internals;

namespace BookGen.Vfs;

public static class JsonOptions
{
    public readonly static JsonSerializerOptions SerializerOptions = new(JsonSerializerOptions.Default)
    {
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
        WriteIndented = true,
        Converters =
        {
            new JsonStringEnumConverter(),
            new CultureInfoJsonConverter(),
            new DateOnlyJsonConverter(),
            new TimeOnlyJsonConverter(),
            new IsoDateTimeOffsetJsonConverter(),
        }
    };

    public static readonly JsonSchemaExporterOptions ExporterOptions = new()
    {
        TransformSchemaNode = JsonSchemaTransformer.TransformSchemaNode
    };
}
