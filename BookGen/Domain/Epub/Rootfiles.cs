//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

namespace BookGen.Domain.Epub
{
    [XmlRoot(ElementName = "rootfiles")]
    public class Rootfiles
    {
        [XmlElement(ElementName = "rootfile")]
        public Rootfile Rootfile { get; set; }
    }
}
