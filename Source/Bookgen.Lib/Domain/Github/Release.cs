using System.Text.Json.Serialization;

namespace Bookgen.Lib.Domain.Github;

/// <summary>
/// A release.
/// </summary>
public class Release
{
    [JsonPropertyName("assets")]
    public required ReleaseAsset[] Assets { get; set; }
}
