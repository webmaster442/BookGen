using System.Text.Json.Serialization;

namespace Bookgen.Lib.Domain.Github;

/// <summary>
/// State of the release asset.
/// </summary>
public enum State
{
    [JsonStringEnumMemberName("open")]
    Open,
    [JsonStringEnumMemberName("uploaded")]
    Uploaded
};