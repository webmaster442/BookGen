//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// Based on the work of Junle Li and the Vsxmd project
// https://github.com/lijunle/Vsxmd
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.AssemblyDocumenter.Units;

/// <summary>
/// Exception unit.
/// </summary>
internal class ExceptionUnit : BaseUnit
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ExceptionUnit"/> class.
    /// </summary>
    /// <param name="element">The exception XML element.</param>
    /// <exception cref="ArgumentException">Throw if XML element name is not <c>exception</c>.</exception>
    internal ExceptionUnit(XElement element) : base(element, XmlElements.Exception)
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
    /// Convert the exception XML element to Markdown safely.
    /// If element is <value>null</value>, return empty string.
    /// </summary>
    /// <param name="elements">The exception XML element list.</param>
    /// <returns>The generated Markdown.</returns>
    internal static IEnumerable<string> ToMarkdown(IEnumerable<XElement> elements)
    {
        if (!elements.Any())
        {
            return Enumerable.Empty<string>();
        }

        IEnumerable<string>? markdowns = elements
            .Select(element => new ExceptionUnit(element))
            .SelectMany(unit => unit.ToMarkdown());

        IEnumerable<string>? table = new[]
        {
            "| Name | Description |",
            "| ---- | ----------- |",
        }
        .Concat(markdowns);

        return new[]
        {
            "##### Exceptions",
            string.Join("\n", table),
        };
    }
}
