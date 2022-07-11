//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain.Configuration;
using BookGen.Framework.Shortcodes;
using BookGen.Interfaces;

namespace BookGen.Framework
{
    internal class TemplateProcessor : ITemplateProcessor
    {
        private readonly Dictionary<string, string> _table;
        private readonly ShortCodeParser _parser;

        public string TemplateContent { get; set; }

        public TemplateProcessor(Config cfg, ShortCodeParser shortCodeParser, StaticTemplateContent? staticContent = null)
        {
            _table = new Dictionary<string, string>
            {
                { "toc",  staticContent == null ? string.Empty : staticContent.TableOfContents },
                { "title", staticContent == null ? string.Empty : staticContent.Title },
                { "content", staticContent == null ? string.Empty : staticContent.Content },
                { "host", cfg.HostName },
                { "metadata", staticContent == null ? string.Empty : staticContent.Metadata },
                { "precompiledheader", staticContent == null ? string.Empty : staticContent.PrecompiledHeader }
            };
            TemplateContent = string.Empty;
            _parser = shortCodeParser;
            _parser.AddShortcodesToLookupIndex(CreateInternalsList());
        }

        private IList<ITemplateShortCode> CreateInternalsList()
        {
            var internals = new List<ITemplateShortCode>(_table.Count);
            foreach (KeyValuePair<string, string> item in _table)
            {
                internals.Add(new DelegateShortCode(item.Key, (_) =>
                {
                    if (item.Key == "content")
                        return _parser.Parse(_table[item.Key]);
                    else
                        return _table[item.Key];
                }));
            }
            return internals;
        }

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

        public string Metadata
        {
            get { return _table["metadata"]; }
            set { _table["metadata"] = value; }
        }

        public string HostUrl
        {
            get { return _table["host"]; }
        }

        public string PrecompiledHeader
        {
            get { return _table["precompiledheader"]; }
            set { _table["precompiledheader"] = value; }
        }

        public string Render()
        {
            if (TemplateContent == null)
                throw new InvalidOperationException("Can't generate while TemplateContent is null");

            return _parser.Parse(TemplateContent);
        }
    }
}
