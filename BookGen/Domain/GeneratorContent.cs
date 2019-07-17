//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;

namespace BookGen.Domain
{
    public class GeneratorContent : IEnumerable<KeyValuePair<string, string>>
    {
        private Dictionary<string, string> _table;

        public string Content
        {
            get { return _table["content"]; }
            set { _table["content"] = value; }
        }

        public string Title
        {
            get { return _table["title"]; }
            set { _table["title"] = value; }
        }

        public string TableOfContents
        {
            get { return _table["toc"]; }
            set { _table["toc"] = value; }
        }

        public string AdditionalMenus
        {
            get { return _table["menus"]; }
            set { _table["menus"] = value; }
        }

        public string Metadata
        {
            get { return _table["metadata"]; }
            set { _table["metadata"] = value; }
        }

        public string HostUrl
        {
            get { return _table["host"]; }
        }

        public string BuildTime
        {
            get
            {
                _table["buildtime"] = DateTime.Now.ToString();
                return _table["buildtime"];
            }
        }

        public string PrecompiledHeader
        {
            get { return _table["precompiledheader"]; }
            set { _table["precompiledheader"] = value; }
        }

        public GeneratorContent(Config cfg)
        {
            _table = new Dictionary<string, string>
            {
                { "toc", string.Empty },
                { "title", string.Empty },
                { "content", string.Empty },
                { "menus", string.Empty },
                { "host", cfg.HostName },
                { "buildtime", DateTime.Now.ToString() },
                { "metadata", string.Empty },
                { "precompiledheader", string.Empty }
            };
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return _table.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _table.GetEnumerator();
        }
    }
}
