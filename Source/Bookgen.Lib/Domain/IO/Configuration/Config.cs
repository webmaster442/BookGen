using System.ComponentModel.DataAnnotations;

using Bookgen.Lib.Domain.Validation;

namespace Bookgen.Lib.Domain.IO.Configuration;

public sealed class Config
{
    [Range(0, int.MaxValue)]
    public int VersionTag { get; init; }

    [FileExists]
    public string TocFile { get; init; }

    [NotNullOrWhiteSpace]
    public string OutputFolder { get; init; }

    [Required]
    public StaticWebsiteConfig StaticWebsiteConfig { get; init; }

    [Required]
    public WordpressConfig WordpressConfig { get; init; }

    [Required]
    public PrintConfig PrintConfig { get; init; }

    public const int CurrentVersionTag = 250506;

    public Config()
    {
        VersionTag = CurrentVersionTag;
        TocFile = string.Empty;
        StaticWebsiteConfig = new StaticWebsiteConfig();
        PrintConfig = new PrintConfig();
        WordpressConfig = new WordpressConfig();
        OutputFolder = string.Empty;
    }
}
