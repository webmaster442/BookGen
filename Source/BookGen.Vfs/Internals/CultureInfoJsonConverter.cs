//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BookGen.Vfs.Internals;

internal sealed class CultureInfoJsonConverter : JsonConverter<CultureInfo>
{
    public override CultureInfo? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return new CultureInfo(reader.GetString()!);
    }

    public override void Write(Utf8JsonWriter writer, CultureInfo value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.Name);
    }
}
