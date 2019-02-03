//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace BookGen.Domain
{
    public class GeneratorContent: IEnumerable<KeyValuePair<string, string>>
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

        public string HostUrl
        {
            get { return _table["host"]; }
        }

        public string AssetsUrl
        {
            get { return _table["assets"]; }
        }

        public GeneratorContent(Config cfg)
        {
            _table = new Dictionary<string, string>
            {
                { "toc", string.Empty },
                { "title", string.Empty },
                { "content", string.Empty },
                { "host", cfg.HostName },
                { "assets", Path.Combine(cfg.HostName, cfg.AssetsDir) }
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
