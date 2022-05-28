//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// Based on the work of Junle Li and the Vsxmd project
// https://github.com/lijunle/Vsxmd
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.AssemblyDocumenter.Units;
using BookGen.Core;

namespace BookGen.AssemblyDocumenter
{
    /// <inheritdoc/>
    public class XmlDocumenter : IConverter
    {
        private readonly XDocument _document;

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlDocumenter"/> class.
        /// </summary>
        /// <param name="document">The XML document.</param>
        public XmlDocumenter(FsPath sourceFile)
        {
            using (var stream = sourceFile.OpenStream())
            {
                _document = XDocument.Load(stream);
            }
        }

        /// <inheritdoc/>
        public string ToMarkdown()
        {
            if (_document.Root != null)
            {
                return
                    ToUnits(_document.Root)
                    .SelectMany(x => x.ToMarkdown())
                    .Join("\n\n")
                    .Suffix("\n");
            }
            return string.Empty;
        }

        private static IEnumerable<IUnit> ToUnits(XElement docElement)
        {
            // member units
            var memberUnits = docElement
                ?.Element(XmlElements.Members)
                ?.Elements(XmlElements.Member)
                .Select(element => new MemberUnit(element))
                .Where(member => member.Kind != MemberKind.NotSupported)
                .GroupBy(unit => unit.TypeName)
                .Select(MemberUnit.ComplementType)
                .SelectMany(group => group)
                .OrderBy(member => member, MemberUnit.Comparer) ?? Enumerable.Empty<MemberUnit>();

            // table of contents
            var tableOfContents = new TableOfContents(memberUnits);

            var result = new List<IUnit>();

            var asm = docElement?.Element(XmlElements.Assembly);
            if (asm != null)
            {
                result.Add(new AssemblyUnit(asm));
            }
            result.Add(tableOfContents);
            result.AddRange(memberUnits);

            return result;
        }
    }
}
