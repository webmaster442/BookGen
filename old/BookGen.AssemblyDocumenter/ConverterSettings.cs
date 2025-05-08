namespace BookGen.AssemblyDocumenter;

/// <summary>
/// Contains settings for the conversion process.
/// </summary>
public sealed class ConverterSettings
{
    /// <summary>
    /// Gets or sets a value indicating whether internal members should be skipped in the documentation.
    /// </summary>
    public bool ShouldSkipInternal { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether members marked with
    /// <see cref="System.ComponentModel.EditorBrowsableState.Never"/> should be skipped in the documentation.
    /// </summary>
    public bool ShouldSkipNonBrowsable { get; set; }
}
