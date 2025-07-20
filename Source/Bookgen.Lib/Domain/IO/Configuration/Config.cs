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
    [Range(2000, int.MaxValue)]
    public int VersionTag { get; init; }

    [Description("Book title")]
    [NotNullOrWhiteSpace]
    [MinLength(1)]
    public string BookTitle { get; init; }

    [Description("Book language - 2 letter ISO 639 code")]
    [Iso639Language]
    public string Book2LetterISO639Language { get; init; }

    [Description("Book author")]
    [NotNullOrWhiteSpace]
    [MinLength(1)]
    public string BookAuthor { get; init; }

    [Description("Static website settings")]
    [Required]
    public StaticWebsiteConfig StaticWebsiteConfig { get; init; }

    [Description("Wordpress settings")]
    [Required]
    public WordpressConfig WordpressConfig { get; init; }

    [Description("Print settings")]
    [Required]
    public PrintConfig PrintConfig { get; init; }

    public const int CurrentVersionTag = 2007;

    public Config()
    {
        Book2LetterISO639Language = "en";
        BookTitle = string.Empty;
        BookAuthor = string.Empty;
        VersionTag = CurrentVersionTag;
        StaticWebsiteConfig = new StaticWebsiteConfig();
        PrintConfig = new PrintConfig();
        WordpressConfig = new WordpressConfig();
    }
}
