//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// Based on the work of Junle Li and the Vsxmd project
// https://github.com/lijunle/Vsxmd
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Reflection;

using BookGen.AssemblyDocumenter.Reflection;
using BookGen.AssemblyDocumenter.Units;

namespace BookGen.AssemblyDocumenter
{
    /// <inheritdoc/>
    public class Converter : IConverter
    {
        private readonly AssemblyReflector? assembly;
        private readonly XDocument _document;

        /// <summary>
        /// Initializes a new instance of the <see cref="Converter"/> class.
        /// </summary>
        /// <param name="document">The XML document.</param>
        public Converter(XDocument document) :this(document, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Converter"/> class.
        /// </summary>
        /// <param name="document">The XML document.</param>
        /// <param name="assembly">The owning assembly, or null if unknown.</param>
        public Converter(XDocument document, Assembly? assembly)
        {
            this.assembly = new AssemblyReflector(assembly);
            _document = document;
        }

        /// <inheritdoc/>
        public string ToMarkdown(ConverterSettings settings)
        {
            if (_document.Root != null)
            {
                return
                    ToUnits(_document.Root, this.assembly, settings)
                    .SelectMany(x => x.ToMarkdown())
                    .Join("\n\n")
                    .Suffix("\n");
            }
            return string.Empty;
        }

        private static IEnumerable<IUnit> ToUnits(XElement docElement, AssemblyReflector? assembly, ConverterSettings settings)
        {
            // member units
            IEnumerable<MemberUnit>? memberUnits = docElement
                ?.Element(XmlElements.Members)
                ?.Elements(XmlElements.Member)
                .Select(element => new MemberUnit(element, assembly))
                .Where(member => member.Kind != MemberKind.NotSupported && !member.ShouldSkipMember(settings))
                .GroupBy(unit => unit.TypeName)
                .Select(group => MemberUnit.ComplementType(group, assembly))
                .SelectMany(group => group)
                .OrderBy(member => member, MemberUnit.Comparer) ?? Enumerable.Empty<MemberUnit>();

            // table of contents
            var tableOfContents = new TableOfContents(memberUnits);

            var result = new List<IUnit>();

            XElement? asm = docElement?.Element(XmlElements.Assembly);
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
