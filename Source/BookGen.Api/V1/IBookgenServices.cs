//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Api.V1;

/// <summary>
/// Represents services offered by the Bookgen application.
/// </summary>
public interface IBookgenServices
{
    /// <summary>
    /// Creates a renderer based on the provided renderer options.
    /// </summary>
    /// <param name="rendererOptions">The options to configure the renderer.</param>
    /// <returns>A renderer instance configured with the specified options.</returns>
    IRenderer Create(RendererOptions rendererOptions);
    /// <summary>
    /// Gets the output folder where the generated book files will be stored.
    /// </summary>
    IFileSystem OutputFolder { get; }
}
