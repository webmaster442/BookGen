//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

using BookGen.Domain;
using BookGen.Domain.Formulas;

namespace BookGen.FormulaEdit.AppLogic;

internal static class FileManager
{
    public static IEnumerable<string> LoadFile(string path)
    {
        XmlSerializer xs = new XmlSerializer(typeof(Formulas));
        using var f = File.OpenRead(path);
        if (xs.Deserialize(f) is Formulas result)
        {
            return result.Items.Select(i => i.Value);
        }
        throw new InvalidDataException("File load failed");
    }

    public static void SaveFile(string path, IEnumerable<string> lines)
    {
        XmlSerializer xs = new XmlSerializer(typeof(Formulas));
        using var f = File.Create(path);
        xs.Serialize(f, new Formulas
        {
            Items = lines.Select(line => new CData(line)).ToArray()
        });
    }
}
