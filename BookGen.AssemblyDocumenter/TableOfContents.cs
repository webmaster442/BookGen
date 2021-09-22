//-----------------------------------------------------------------------
// <copyright file="TableOfContents.cs" company="Junle Li">
//     Copyright (c) Junle Li. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using Vsxmd.Units;

namespace Vsxmd
{
    /// <summary>
    /// Table of contents.
    /// </summary>
    internal class TableOfContents : IUnit
    {
        private const string Href = "contents";

        private readonly IEnumerable<MemberUnit> _memberUnits;

        /// <summary>
        /// Initializes a new instance of the <see cref="TableOfContents"/> class.
        /// <para>It convert the table of contents from the <paramref name="memberUnits"/>.</para>
        /// </summary>
        /// <param name="memberUnits">The member unit list.</param>
        internal TableOfContents(IEnumerable<MemberUnit> memberUnits)
        {
            _memberUnits = memberUnits;
        }

        /// <summary>
        /// Gets the link pointing to the table of contents.
        /// </summary>
        /// <value>The link pointing to the table of contents.</value>
        internal static string Link => $"[=](#{Href} 'Back To Contents')";

        /// <summary>
        /// Convert the table of contents to Markdown syntax.
        /// </summary>
        /// <returns>The table of contents in Markdown syntax.</returns>
        public IEnumerable<string> ToMarkdown()
        {
            return new[]
{
                $"## Contents",
                _memberUnits.Select(ToMarkdown).Join("\n"),
            };
        }

        private static string ToMarkdown(MemberUnit memberUnit) => $"{GetIndentation(memberUnit)}- {memberUnit.Link}";

        private static string GetIndentation(MemberUnit memberUnit) => memberUnit.Kind == MemberKind.Type ? string.Empty : "  ";
    }
}
