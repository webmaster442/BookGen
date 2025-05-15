using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Bookgen.Lib;

public static class YamlSerializerFactory
{
    public static IDeserializer CreateDeserializer()
    {
        return new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .IgnoreUnmatchedProperties()
            .Build();
    }

    public static ISerializer CreateSerializer()
    {
        return new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .WithYamlFormatter(YamlFormatter.Default)
            .Build();
    }
}
