using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace BookGen.Domain.Github
{
    internal class Relese
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("published_at")]
        public DateTime? PublishDate { get; set; }

        [JsonPropertyName("prerelease")]
        public bool IsPreRelase { get; set; }

        [JsonPropertyName("draft")]
        public bool IsDraft { get; set; }

        [JsonPropertyName("assets")]
        public List<Asset>? Assets { get; set; }

        [JsonPropertyName("body")]
        public string? Body { get; set; }
    }
}
