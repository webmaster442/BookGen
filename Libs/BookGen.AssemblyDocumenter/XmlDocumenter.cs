//-----------------------------------------------------------------------------
// (c) 2021-2023 Ruzsinszki Gábor
// Based on the work of Junle Li and the Vsxmd project
// https://github.com/lijunle/Vsxmd
// This code is licensed under MIT license (see LICENSE for details)
//--------------------------------------------------------------------------

using System.Reflection;

using BookGen.DomainServices;
using BookGen.Interfaces;

namespace BookGen.AssemblyDocumenter;

public sealed class XmlDocumenter : IConverter
{
    private readonly Converter _converter;

    /// <summary>
    /// Initializes a new instance of the <see cref="XmlDocumenter"/> class.
    /// </summary>
    /// <param name="xml">the xml file</param>
    /// <param name="assembly">The assembly file</param>
    public XmlDocumenter(FsPath xml, FsPath assembly)
    {
        using (var stream = xml.OpenStream())
        {
            Assembly loadedAssembly = Assembly.LoadFrom(assembly.ToString());
            _converter = new Converter(XDocument.Load(stream), loadedAssembly);
        }
    }

    /// <inheritdoc/>
    public string ToMarkdown(ConverterSettings settings)
    {
        return _converter.ToMarkdown(settings);
    }
}
