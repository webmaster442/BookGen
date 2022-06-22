﻿//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

namespace BookGen.Domain.Wordpress
{
    [XmlRoot(ElementName = "postmeta", Namespace = "http://wordpress.org/export/1.2/")]
    public class Postmeta
    {
        [XmlElement(ElementName = "meta_key", Namespace = "http://wordpress.org/export/1.2/")]
        public CData? Meta_key { get; set; }
        [XmlElement(ElementName = "meta_value", Namespace = "http://wordpress.org/export/1.2/")]
        public CData? Meta_value { get; set; }
    }
}
