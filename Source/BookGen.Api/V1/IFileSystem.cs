//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Api.V1;

/// <summary>
/// Represents a file system abstraction for reading and writing files.
/// </summary>
public interface IFileSystem
{
    /// <summary>
    /// Writes the specified content to a text file at the given relative path.
    /// </summary>
    /// <param name="relativePath">The relative path to the file.</param>
    /// <param name="content">The content to write to the file.</param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    Task WriteTextFile(string relativePath, string content);

    /// <summary>
    /// Checks if a file exists at the specified relative path.
    /// </summary>
    /// <param name="relativePath">The relative path to the file.</param>
    /// <returns>True if the file exists; otherwise, false.</returns>
    bool FileExists(string relativePath);
}
