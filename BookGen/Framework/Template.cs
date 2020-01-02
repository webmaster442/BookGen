//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Bookgen.Template.ShortCodeImplementations;
using BookGen.Contracts;
using BookGen.Core.Configuration;
using BookGen.Core.Contracts;
using System;
using System.Collections.Generic;

namespace BookGen.Framework
{
    internal class Template : IContent
    {
        private readonly Dictionary<string, string> _table;
        private readonly ShortCodeParser _parser;
        private readonly Config _configuration;

        public string TemplateContent { get; set; }

        public Template(Config cfg, ShortCodeParser shortCodeParser)
        {
            _configuration = cfg;
            _table = new Dictionary<string, string>
            {
                { "toc", string.Empty },
                { "title", string.Empty },
                { "content", string.Empty },
                { "host", cfg.HostName },
                { "metadata", string.Empty },
                { "precompiledheader", string.Empty }
            };
            TemplateContent = string.Empty;
            _parser = shortCodeParser;
            _parser.AddShortcodesToLookupIndex(CreateInternalsList());
        }

        private IList<ITemplateShortCode> CreateInternalsList()
        {
            List<ITemplateShortCode> internals = new List<ITemplateShortCode>(_table.Count);
            foreach (var item in _table)
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
