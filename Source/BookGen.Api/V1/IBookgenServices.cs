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
    IRenderer CreateRenderer(RendererOptions rendererOptions);
    /// <summary>
    /// Gets the output folder where the generated book files will be stored.
    /// </summary>
    IFileSystem OutputFolder { get; }
    /// <summary>
    /// Gets the asset source that provides access to the assets used by the Bookgen application.
    /// </summary>
    IAssetSource AssetSource { get; }

    /// <summary>
    /// Gets the logger that can be used to log messages with different severity levels.
    /// </summary>
    IPluginLogger Logger { get; }

    /// <summary>
    /// Gets the dynamic documentation service that provides access to the documentation of the Bookgen application.
    /// </summary>
    IDynamicDocumentation DynamicDocumentation { get; }
}
