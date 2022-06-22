//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// Based on the work of Junle Li and the Vsxmd project
// https://github.com/lijunle/Vsxmd
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.AssemblyDocumenter.Units
{
    /// <summary>
    /// Example unit.
    /// </summary>
    internal class ExampleUnit : BaseUnit
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExampleUnit"/> class.
        /// </summary>
        /// <param name="element">The example XML element.</param>
        /// <exception cref="ArgumentException">Throw if XML element name is not <c>example</c>.</exception>
        internal ExampleUnit(XElement element) : base(element, XmlElements.Example)
        {
        }

        /// <inheritdoc />
        public override IEnumerable<string> ToMarkdown()
        {
            yield return $"##### Example";
            yield return $"{ElementContent}";
        }

        /// <summary>
        /// Convert the example XML element to Markdown safely.
        /// If element is <value>null</value>, return empty string.
        /// </summary>
        /// <param name="element">The example XML element.</param>
        /// <returns>The generated Markdown.</returns>
        internal static IEnumerable<string> ToMarkdown(XElement? element)
        {
            if (element != null)
            {
                return new ExampleUnit(element).ToMarkdown();
            }
            else
            {
                return Enumerable.Empty<string>();
            }
        }
    }
}
