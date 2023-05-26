//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

using Bang.Model;

namespace Bang.Internals;
internal static class BangLoader
{
    public static Bangs Load()
    {
        using var xmlData = typeof(BangLoader).Assembly.GetManifestResourceStream("Bang.BangData.xml");
        if (xmlData != null)
        {
            var serializer = new XmlSerializer(typeof(BangLoader));
            if (serializer.Deserialize(xmlData) is Bangs bangs)
            {
                return bangs;
            }
        }
        return new Bangs();
    }
}
