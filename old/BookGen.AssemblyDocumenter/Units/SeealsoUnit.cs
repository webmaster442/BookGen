//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// Based on the work of Junle Li and the Vsxmd project
// https://github.com/lijunle/Vsxmd
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.AssemblyDocumenter.Units;

/// <summary>
/// Seealso unit.
/// </summary>
internal class SeealsoUnit : BaseUnit
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SeealsoUnit"/> class.
    /// </summary>
    /// <param name="element">The seealso XML element.</param>
    /// <exception cref="ArgumentException">Throw if XML element name is not <c>seealso</c>.</exception>
    internal SeealsoUnit(XElement element)
        : base(element, XmlElements.Seealso)
    {
    }

    /// <inheritdoc />
    public override IEnumerable<string> ToMarkdown()
    {
        yield return GetAttribute(XmlAttributes.Cref).ToReferenceLink();
    }

    /// <summary>
    /// Convert the seealso XML element to Markdown safely.
    /// If element is <value>null</value>, return empty string.
    /// </summary>
    /// <param name="elements">The seealso XML element list.</param>
    /// <returns>The generated Markdown.</returns>
    internal static IEnumerable<string> ToMarkdown(IEnumerable<XElement> elements)
    {
        if (!elements.Any())
        {
            return Enumerable.Empty<string>();
        }

        IEnumerable<string>? markdowns = elements
            .Select(element => new SeealsoUnit(element))
            .SelectMany(unit => unit.ToMarkdown());

        return new[]
        {
            "##### See Also",
            string.Join("\n", markdowns),
        };
    }
}
