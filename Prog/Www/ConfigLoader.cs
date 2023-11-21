//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

using BookGen.Domain.Www;
using BookGen.DomainServices;

namespace Www;

internal class ConfigLoader
{
    public WwwBang[] Bangs { get; }
    public WwwUrl[] Favorites { get; }

    public ConfigLoader()
    {
        var serializer = new XmlSerializer(typeof(WwwConfig));
        Bangs = Array.Empty<WwwBang>();
        Favorites = Array.Empty<WwwUrl>();
        if (File.Exists(FileProvider.GetWwwConfig()))
        {
            using (var file = File.OpenRead(FileProvider.GetWwwConfig()))
            {
                if (serializer.Deserialize(file) is WwwConfig config)
                {
                    Bangs = config.Bangs;
                    Favorites = config.Favorites;
                }
            }
        }
    }
}
