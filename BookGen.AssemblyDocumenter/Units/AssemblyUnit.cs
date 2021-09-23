//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// Based on the work of Junle Li and the Vsxmd project
// https://github.com/lijunle/Vsxmd
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace BookGen.AssemblyDocumenter.Units
{
    /// <summary>
    /// Assembly unit.
    /// </summary>
    internal class AssemblyUnit : BaseUnit
    {
        private const string Href = "assembly";

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyUnit"/> class.
        /// </summary>
        /// <param name="element">The assembly XML element.</param>
        /// <exception cref="ArgumentException">Throw if XML element name is not <c>assembly</c>.</exception>
        internal AssemblyUnit(XElement element): base(element, XmlElements.Assembly)
        {
        }

        private string AssemblyName => GetChild(XmlElements.Name)?.Value ?? string.Empty;

        /// <inheritdoc />
        public override IEnumerable<string> ToMarkdown()
        {
            yield return $"{Href.ToAnchor()}# {AssemblyName}";
        }
    }
}
