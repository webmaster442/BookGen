//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// Based on the work of Junle Li and the Vsxmd project
// https://github.com/lijunle/Vsxmd
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.AssemblyDocumenter
{
    /// <summary>
    /// Converter for XML document to Markdown syntax conversion.
    /// </summary>
    public interface IConverter
    {
        /// <summary>
        /// Convert to Markdown syntax.
        /// </summary>
        /// <param name="settings">The settings to use during the conversion.</param>
        /// <returns>The generated Markdown content.</returns>
        string ToMarkdown(ConverterSettings settings);
    }
}
