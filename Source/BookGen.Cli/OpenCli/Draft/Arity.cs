//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text.Json.Serialization;

namespace BookGen.Cli.OpenCli.Draft;

/// <summary>
/// The argument arity. Arity defines the minimum and maximum number of argument values
/// </summary>
public partial class Arity
{
    /// <summary>
    /// The maximum number of values allowed
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("maximum")]
    public long? Maximum
    {
        get => field;
        set
        {
            if (value < Minimum)
                throw new ArgumentOutOfRangeException(nameof(Maximum));

            field = value;
        }
    }

    /// <summary>
    /// The minimum number of values allowed
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("minimum")]
    public long Minimum
    {
        get => field;
        set
        {
            if (value < 1)
                throw new ArgumentOutOfRangeException(nameof(Minimum));

            field = value;
        }
    } = 1;
}
