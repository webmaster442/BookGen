using System.Text.Json.Serialization;

namespace BookGen.Domain.Github
{
    internal class Asset
    {
        [JsonPropertyName("content_type")]
        public string? ContentType { get; set; }

        [JsonPropertyName("browser_download_url")]
        public string? DownloadUrl { get; set; }

        [JsonPropertyName("size")]
        public long Size { get; set; }
    }
}
