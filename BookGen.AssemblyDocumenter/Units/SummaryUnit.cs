//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// Based on the work of Junle Li and the Vsxmd project
// https://github.com/lijunle/Vsxmd
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace BookGen.AssemblyDocumenter.Units
{
    /// <summary>
    /// Summary unit.
    /// </summary>
    internal class SummaryUnit : BaseUnit
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SummaryUnit"/> class.
        /// </summary>
        /// <param name="element">The summary XML element.</param>
        /// <exception cref="ArgumentException">Throw if XML element name is not <c>summary</c>.</exception>
        internal SummaryUnit(XElement element)
            : base(element, XmlElements.Summary)
        {
        }

        /// <inheritdoc />
        public override IEnumerable<string> ToMarkdown()
        {
            yield return "##### Summary";
            yield return ElementContent;
        }

        /// <summary>
        /// Convert the summary XML element to Markdown safely.
        /// If element is <value>null</value>, return empty string.
        /// </summary>
        /// <param name="element">The summary XML element.</param>
        /// <returns>The generated Markdown.</returns>
        internal static IEnumerable<string> ToMarkdown(XElement? element)
        {
            if (element != null)
            {
                return new SummaryUnit(element).ToMarkdown();
            }
            else
            {
                return Enumerable.Empty<string>();
            }
        }
    }
}
