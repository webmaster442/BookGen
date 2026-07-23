//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Api.V1;

/// <summary>
/// Represents accessor for dynamically generated documentation, such as commands and schemas, in markdown format.
/// </summary>
public interface IDynamicDocumentation
{
    /// <summary>
    /// Gets the dynamically generated commands documentation in markdown format.
    /// </summary>
    /// <returns>The commands documentation in markdown format.</returns>
    string GetCommandsMarkdown();
    /// <summary>
    /// Gets the dynamically generated schemas documentation in markdown format.
    /// </summary>
    /// <returns>The schemas documentation in markdown format.</returns>    
    string GetSchemasMarkdown();
}
