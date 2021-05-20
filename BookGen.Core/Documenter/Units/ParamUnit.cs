//-----------------------------------------------------------------------
// <copyright file="ParamUnit.cs" company="Junle Li">
//     Copyright (c) Junle Li. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace BookGen.Core.Documenter.Units
{
    /// <summary>
    /// Param unit.
    /// </summary>
    internal class ParamUnit : BaseUnit
    {
        private readonly string _paramType;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParamUnit"/> class.
        /// </summary>
        /// <param name="element">The param XML element.</param>
        /// <param name="paramType">The parameter type corresponding to the param XML element.</param>
        /// <exception cref="ArgumentException">Throw if XML element name is not <c>param</c>.</exception>
        internal ParamUnit(XElement element, string paramType)
            : base(element, "param")
        {
            _paramType = paramType;
        }

        private string Name => GetAttribute("name");

        private string Description => ElementContent;

        /// <inheritdoc />
        public override IEnumerable<string> ToMarkdown() =>
            new[]
            {
                $"| {Name} | {_paramType.ToReferenceLink()} | {Description} |",
            };

        /// <summary>
        /// Convert the param XML element to Markdown safely.
        /// </summary>
        /// <param name="elements">The param XML element list.</param>
        /// <param name="paramTypes">The paramater type names.</param>
        /// <param name="memberKind">The member kind of the parent element.</param>
        /// <returns>The generated Markdown.</returns>
        /// <remarks>
        /// When the parameter (a.k.a <paramref name="elements"/>) list is empty:
        /// <para>If parent element kind is <see cref="MemberKind.Constructor"/> or <see cref="MemberKind.Method"/>, it returns a hint about "no parameters".</para>
        /// <para>If parent element kind is not the value mentioned above, it returns an empty string.</para>
        /// </remarks>
        internal static IEnumerable<string> ToMarkdown(
            IEnumerable<XElement> elements,
            IEnumerable<string> paramTypes,
            MemberKind memberKind)
        {
            if (!elements.Any())
            {
                return
                    memberKind != MemberKind.Constructor &&
                    memberKind != MemberKind.Method
                    ? Enumerable.Empty<string>()
                    : new[]
                    {
                        "##### Parameters",
                        $"This {memberKind.ToLowerString()} has no parameters.",
                    };
            }

            var markdowns = elements
                .Zip(paramTypes, (element, type) => new ParamUnit(element, type))
                .SelectMany(unit => unit.ToMarkdown());

            var table = new[]
            {
                "| Name | Type | Description |",
                "| ---- | ---- | ----------- |",
            }
            .Concat(markdowns);

            return new[]
            {
                "##### Parameters",
                string.Join("\n", table),
            };
        }
    }
}
