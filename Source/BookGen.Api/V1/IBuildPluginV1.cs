//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Api.V1;

/// <summary>
/// Represents a plugin that can be used during the build process of a book.
/// A single assembly must contain only one implementation of this interface. 
/// If multiple implementations are found, an exception will be thrown.
/// </summary>
public interface IBuildPluginV1
{
    /// <summary>
    /// Builds the book using the provided IBook instance.
    /// </summary>
    /// <param name="book">The book to build.</param>
    /// <param name="bookgenServices">The services offered by the Bookgen application.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous build operation. The task result contains a boolean indicating whether the build was successful.</returns>
    Task<bool> Build(IBook book, IBookgenServices bookgenServices, CancellationToken cancellationToken);
}
