//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.IO.Compression;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BookGen.Infrastructure.Plugins;

internal sealed class PackageManifest : IValidatableObject
{
    [JsonPropertyName("entryAssembly")]
    public required string EntryAssembly { get; init; }

    [JsonPropertyName("author")]
    public required string Author { get; init; }

    [JsonPropertyName("description")]
    public required string Description { get; init; }
    
    [JsonPropertyName("apiVersion")]
    public required string ApiVersion { get; init; }
    
    [JsonPropertyName("url")]
    public string? Url { get; init; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrWhiteSpace(EntryAssembly))
        {
            yield return new ValidationResult("EntryAssembly cannot be null or whitespace.", [nameof(EntryAssembly)]);
        }
        string extension = Path.GetExtension(EntryAssembly);
        if (!string.Equals(extension, ".dll", StringComparison.OrdinalIgnoreCase))
        {
            yield return new ValidationResult("EntryAssembly must have a .dll extension.", [nameof(EntryAssembly)]);
        }
        if (string.IsNullOrWhiteSpace(Author))
        {
            yield return new ValidationResult("Author cannot be null or whitespace.", [nameof(Author)]);
        }
        if (string.IsNullOrWhiteSpace(Description))
        {
            yield return new ValidationResult("Description cannot be null or whitespace.", [nameof(Description)]);
        }

        if (string.IsNullOrWhiteSpace(ApiVersion))
        {
            yield return new ValidationResult("ApiVersion cannot be null or whitespace.", [nameof(ApiVersion)]);
        }
        if (!Version.TryParse(ApiVersion, out Version? version))
        {
            yield return new ValidationResult("ApiVersion must be a valid version string.", [nameof(ApiVersion)]);
        }
        if (version?.Major != 1)
        {
            yield return new ValidationResult("ApiVersion must be a major version of 1.", [nameof(ApiVersion)]);
        }
    }
}
