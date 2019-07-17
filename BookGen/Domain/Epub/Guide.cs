//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections.Generic;
using System.Xml.Serialization;

namespace BookGen.Domain.Epub
{
    [XmlRoot(ElementName = "guide", Namespace = "http://www.idpf.org/2007/opf")]
    public class Guide
    {
        [XmlElement(ElementName = "reference", Namespace = "http://www.idpf.org/2007/opf")]
        public List<Reference> Reference { get; set; }
    }
}
