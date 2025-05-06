using System.ComponentModel.DataAnnotations;

using Bookgen.Lib.Domain.Validation;

namespace Bookgen.Lib.Domain.IO.Configuration;

public sealed class Config
{
    [Range(0, int.MaxValue)]
    public required int VersionTag { get; init; }

    [FileExists]
    public required string TocFile { get; init; }

    public required StaticWebsiteConfig StaticWebsiteConfig { get; init; }

    public required WordpressConfig WordpressConfig { get; init; }

    public required PrintConfig PrintConfig { get; init; }

    public static Config GetDefault()
    {
        return new Config
        {
            TocFile = string.Empty,
            VersionTag = 250506,
            StaticWebsiteConfig = new StaticWebsiteConfig(),
            PrintConfig = new PrintConfig(),
            WordpressConfig = new WordpressConfig(),
        };
    }
}
