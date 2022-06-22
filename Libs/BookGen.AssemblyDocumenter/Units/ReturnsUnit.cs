//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// Based on the work of Junle Li and the Vsxmd project
// https://github.com/lijunle/Vsxmd
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.AssemblyDocumenter.Units
{
    /// <summary>
    /// Returns unit.
    /// </summary>
    internal class ReturnsUnit : BaseUnit
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReturnsUnit"/> class.
        /// </summary>
        /// <param name="element">The returns XML element.</param>
        /// <exception cref="ArgumentException">Throw if XML element name is not <c>returns</c>.</exception>
        internal ReturnsUnit(XElement element)
            : base(element, XmlElements.Returns)
        {
        }

        /// <inheritdoc />
        public override IEnumerable<string> ToMarkdown()
        {
            yield return "##### Returns";
            yield return ElementContent;
        }

        /// <summary>
        /// Convert the returns XML element to Markdown safely.
        /// If element is <value>null</value>, return empty string.
        /// </summary>
        /// <param name="element">The returns XML element.</param>
        /// <returns>The generated Markdown.</returns>
        internal static IEnumerable<string> ToMarkdown(XElement? element)
        {
            if (element != null)
            {
                return new ReturnsUnit(element).ToMarkdown();
            }
            else
            {
                return Enumerable.Empty<string>();
            }
        }
    }
}
