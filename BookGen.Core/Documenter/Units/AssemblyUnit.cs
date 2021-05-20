//-----------------------------------------------------------------------
// <copyright file="AssemblyUnit.cs" company="Junle Li">
//     Copyright (c) Junle Li. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace BookGen.Core.Documenter.Units
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
        internal AssemblyUnit(XElement element)
            : base(element, "assembly")
        {
        }

        private string AssemblyName => GetChild("name")?.Value ?? string.Empty;

        /// <inheritdoc />
        public override IEnumerable<string> ToMarkdown()
        {
            yield return $"{Href.ToAnchor()}# {AssemblyName}";
        }
    }
}
