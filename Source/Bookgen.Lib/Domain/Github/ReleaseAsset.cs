using System.Text.Json.Serialization;

namespace Bookgen.Lib.Domain.Github;

public class ReleaseAsset
{
    [JsonPropertyName("browser_download_url")]
    public required Uri BrowserDownloadUrl { get; set; }

    [JsonPropertyName("content_type")]
    public required string ContentType { get; set; }

    [JsonPropertyName("created_at")]
    public required DateTimeOffset CreatedAt { get; set; }

    [JsonPropertyName("digest")]
    public required string Digest { get; set; }

    [JsonPropertyName("download_count")]
    public required long DownloadCount { get; set; }

    [JsonPropertyName("id")]
    public required long Id { get; set; }

    [JsonPropertyName("label")]
    public required string Label { get; set; }

    /// <summary>
    /// The file name of the asset.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("node_id")]
    public required string NodeId { get; set; }

    [JsonPropertyName("size")]
    public required long Size { get; set; }

    /// <summary>
    /// State of the release asset.
    /// </summary>
    [JsonPropertyName("state")]
    public required State State { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTimeOffset UpdatedAt { get; set; }

    [JsonPropertyName("url")]
    public required Uri Url { get; set; }
}
