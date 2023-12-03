namespace BookGen.Api;

/// <summary>
/// Class describing help information for a shortcode
/// </summary>
public sealed class ShortCodeInfo
{
    /// <summary>
    /// Shortcode description
    /// </summary>
    public required string Description { get; init; }

    /// <summary>
    /// Argument informations
    /// </summary>
    public required IReadOnlyList<ArgumentInfo> ArgumentInfos { get; init; }

    /// <summary>
    /// Empty info
    /// </summary>
    public static readonly ShortCodeInfo Empty = new()
    {
        Description = string.Empty,
        ArgumentInfos = Array.Empty<ArgumentInfo>(),
    };
}
