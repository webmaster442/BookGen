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
    /// Typeparam unit.
    /// </summary>
    internal class TypeparamUnit : BaseUnit
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeparamUnit"/> class.
        /// </summary>
        /// <param name="element">The typeparam XML element.</param>
        /// <exception cref="ArgumentException">Throw if XML element name is not <c>typeparam</c>.</exception>
        internal TypeparamUnit(XElement element)
            : base(element, XmlElements.Typeparam)
        {
        }

        private string Name => GetAttribute(XmlAttributes.Name);

        private string Description => ElementContent;

        /// <inheritdoc />
        public override IEnumerable<string> ToMarkdown()
        {
            yield return $"| {Name} | {Description} |";
        }

        /// <summary>
        /// Convert the param XML element to Markdown safely.
        /// If element is <value>null</value>, return empty string.
        /// </summary>
        /// <param name="elements">The param XML element list.</param>
        /// <returns>The generated Markdown.</returns>
        internal static IEnumerable<string> ToMarkdown(IEnumerable<XElement> elements)
        {
            if (!elements.Any())
            {
                return Enumerable.Empty<string>();
            }

            var markdowns = elements
                .Select(element => new TypeparamUnit(element))
                .SelectMany(unit => unit.ToMarkdown());

            var table = new[]
            {
                "| Name | Description |",
                "| ---- | ----------- |",
            }
            .Concat(markdowns);

            return new[]
            {
                "##### Generic Types",
                string.Join("\n", table),
            };
        }
    }
}
