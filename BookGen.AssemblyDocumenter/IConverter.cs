//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// Based on the work of Junle Li and the Vsxmd project
// https://github.com/lijunle/Vsxmd
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace Vsxmd
{
    /// <summary>
    /// Converter for XML document to Markdown syntax conversion.
    /// </summary>
    public interface IConverter
    {
        /// <summary>
        /// Convert to Markdown syntax.
        /// </summary>
        /// <returns>The generated Markdown content.</returns>
        string ToMarkdown();
    }
}
