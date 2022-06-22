//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace BookGen.Core
{
    internal class CultureInfoConverter : JsonConverter<CultureInfo>, IYamlTypeConverter
    {
        #region JSON
        public override CultureInfo? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return new CultureInfo(reader.GetString()!);
        }

        public override void Write(Utf8JsonWriter writer, CultureInfo value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.Name);
        }
        #endregion

        #region YAML
        public bool Accepts(Type type)
        {
            return type == typeof(CultureInfo);
        }

        public object? ReadYaml(IParser parser, Type type)
        {
            if (parser.Current is YamlDotNet.Core.Events.Scalar scalar)
            {
                return new CultureInfo(scalar.Value);
            }
            return null;
        }

        public void WriteYaml(IEmitter emitter, object? value, Type type)
        {
            if (value is CultureInfo cultureInfo)
            {
                emitter.Emit(new YamlDotNet.Core.Events.Scalar(cultureInfo.Name));
            }
        }
        #endregion
    }
}
