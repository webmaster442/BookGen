//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// Based on the work of Junle Li and the Vsxmd project
// https://github.com/lijunle/Vsxmd
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.AssemblyDocumenter.Units;
using System.Collections.Generic;
using System.Linq;

namespace BookGen.AssemblyDocumenter
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
            yield return $"## Contents";
            yield return _memberUnits.Select(ToMarkdown).Join("\n");
        }

        private static string ToMarkdown(MemberUnit memberUnit) => $"{GetIndentation(memberUnit)}- {memberUnit.Link}";

        private static string GetIndentation(MemberUnit memberUnit) => memberUnit.Kind == MemberKind.Type ? string.Empty : "  ";
    }
}
