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

namespace Vsxmd.Units
{
    /// <summary>
    /// Remarks unit.
    /// </summary>
    internal class RemarksUnit : BaseUnit
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RemarksUnit"/> class.
        /// </summary>
        /// <param name="element">The remarks XML element.</param>
        /// <exception cref="ArgumentException">Throw if XML element name is not <c>remarks</c>.</exception>
        internal RemarksUnit(XElement element)
            : base(element, XmlElements.Remarks)
        {
        }

        /// <inheritdoc />
        public override IEnumerable<string> ToMarkdown()
        {
            yield return "##### Remarks";
            yield return ElementContent;
        }

        /// <summary>
        /// Convert the remarks XML element to Markdown safely.
        /// If element is <value>null</value>, return empty string.
        /// </summary>
        /// <param name="element">The remarks XML element.</param>
        /// <returns>The generated Markdown.</returns>
        internal static IEnumerable<string> ToMarkdown(XElement? element)
        {
            if (element != null)
            {
                return new RemarksUnit(element).ToMarkdown();
            }
            else
            {
                return Enumerable.Empty<string>();
            }
        }
    }
}
