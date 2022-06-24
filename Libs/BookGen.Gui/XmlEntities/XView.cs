﻿//-----------------------------------------------------------------------------
// (c) 2020-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Xml.Serialization;

namespace BookGen.Gui.XmlEntities
{
    public abstract record XView
    {
        [XmlAttribute]
        public int Left { get; set; }

        [XmlAttribute]
        public int Top { get; set; }

        [XmlAttribute]
        public WidthHandling WidthHandling { get; set; }

        [XmlAttribute]
        public float Width { get; set; }

        public XView()
        {
            Width = float.NaN;
            WidthHandling = WidthHandling.Auto;
        }
    }
}