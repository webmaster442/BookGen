using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

using Bookgen.Lib.Domain.Validation;

namespace Bookgen.Lib.Domain.IO.Configuration;

public sealed class Config
{
    [JsonPropertyName("$schema")]
    public string Schema => "bookgen.schema.json";

    [Description("Configuration version")]
    [Range(0, int.MaxValue)]
    public int VersionTag { get; init; }

    [Description("Table of contents file")]
    [FileExists]
    public string TocFile { get; init; }

    [Description("Book title")]
    [NotNullOrWhiteSpace]
    public string BookTitle { get; init; }

    [Description("Output folder")]
    [NotNullOrWhiteSpace]
    public string OutputFolder { get; init; }

    [Description("Static website settings")]
    [Required]
    public StaticWebsiteConfig StaticWebsiteConfig { get; init; }

    [Description("Wordpress settings")]
    [Required]
    public WordpressConfig WordpressConfig { get; init; }

    [Description("Print settings")]
    [Required]
    public PrintConfig PrintConfig { get; init; }

    public const int CurrentVersionTag = 250506;

    public Config()
    {
        BookTitle = string.Empty;
        VersionTag = CurrentVersionTag;
        TocFile = FileNameConstants.TableOfContents;
        StaticWebsiteConfig = new StaticWebsiteConfig();
        PrintConfig = new PrintConfig();
        WordpressConfig = new WordpressConfig();
        OutputFolder = string.Empty;
    }
}
