//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// Based on the work of Junle Li and the Vsxmd project
// https://github.com/lijunle/Vsxmd
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.AssemblyDocumenter.Units
{
    /// <summary>
    /// Permission unit.
    /// </summary>
    internal class PermissionUnit : BaseUnit
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionUnit"/> class.
        /// </summary>
        /// <param name="element">The permission XML element.</param>
        /// <exception cref="ArgumentException">Throw if XML element name is not <c>permission</c>.</exception>
        internal PermissionUnit(XElement element)
            : base(element, XmlElements.Permission)
        {
        }

        private string Name => GetAttribute(XmlAttributes.Cref).ToReferenceLink();

        private string Description => ElementContent;

        /// <inheritdoc />
        public override IEnumerable<string> ToMarkdown()
        {
            yield return $"| {Name} | {Description} |";
        }

        /// <summary>
        /// Convert the permission XML element to Markdown safely.
        /// If element is <value>null</value>, return empty string.
        /// </summary>
        /// <param name="elements">The permission XML element list.</param>
        /// <returns>The generated Markdown.</returns>
        internal static IEnumerable<string> ToMarkdown(IEnumerable<XElement> elements)
        {
            if (!elements.Any())
            {
                return Enumerable.Empty<string>();
            }

            IEnumerable<string>? markdowns = elements
                .Select(element => new PermissionUnit(element))
                .SelectMany(unit => unit.ToMarkdown());

            IEnumerable<string>? table = new[]
            {
                "| Name | Description |",
                "| ---- | ----------- |",
            }
            .Concat(markdowns);

            return new[]
            {
                "##### Permissions",
                string.Join("\n", table),
            };
        }
    }
}
