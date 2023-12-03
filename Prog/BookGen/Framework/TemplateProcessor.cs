//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain.Configuration;
using BookGen.ShortCodes;

namespace BookGen.Framework;

internal sealed class TemplateProcessor : ITemplateProcessor
{
    private readonly Dictionary<string, string> _table;
    private readonly ShortCodeParser _parser;

    public string TemplateContent { get; set; }

    private static readonly HashSet<string> _protectedNames =
    [
        "toc", "title", "content", "host", "metadata", "precompiledheader",
    ];

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

    private List<ITemplateShortCode> CreateInternalsList()
    {
        var internals = new List<ITemplateShortCode>(_table.Count);
        foreach (KeyValuePair<string, string> item in _table)
        {
            internals.Add(CreateTagFromTableEntry(item));
        }
        return internals;
    }

    private DelegateShortCode CreateTagFromTableEntry(KeyValuePair<string, string> item)
    {
        return new DelegateShortCode(item.Key, (_) =>
        {
            if (item.Key == "content")
                return _parser.Parse(_table[item.Key]);
            else
                return _table[item.Key];
        });
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

    public void AddContent(string key, string value)
    {
        string keyToAdd = key.ToLower();
        if (_protectedNames.Contains(keyToAdd))
            throw new InvalidOperationException($"{keyToAdd} can't be set. Use designated property for this");

        if (_table.ContainsKey(keyToAdd))
        {
            _table[keyToAdd] = value;
        }
        else
        {
            _table.Add(keyToAdd, value);
            _parser.AddShortcodeToLookupIndex(CreateTagFromTableEntry(_table.First(x => x.Key == keyToAdd)));
        }
    }
}
