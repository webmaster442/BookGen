//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// Based on the work of Junle Li and the Vsxmd project
// https://github.com/lijunle/Vsxmd
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.AssemblyDocumenter.Units
{
    /// <summary>
    /// <see cref="IUnit"/> is wrapper to handle XML elements.
    /// </summary>
    internal interface IUnit
    {
        /// <summary>
        /// Represent the XML element content as Markdown syntax.
        /// </summary>
        /// <returns>The generated Markdown content.</returns>
        IEnumerable<string> ToMarkdown();
    }
}
