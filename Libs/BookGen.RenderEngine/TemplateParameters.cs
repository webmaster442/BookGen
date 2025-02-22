//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.RenderEngine;

public sealed class TemplateParameters
{
    private readonly Dictionary<string, string> _parameters;
    private readonly HashSet<string> _protectedParameters;

    private void AddProtecteds()
    {
        foreach (var param in _protectedParameters)
        {
            _parameters.Add(param, string.Empty);
        }
    }

    public TemplateParameters()
    {
        _protectedParameters = ["toc", "tochtml", "title", "content", "host", "metadata", "precompiledheader"];
        _parameters = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
        AddProtecteds();
    }

    public string Toc
    {
        get => _parameters["toc"];
        set => _parameters["toc"] = value;
    }

    public string TocHtml
    {
        get => _parameters["tochtml"];
        set => _parameters["tochtml"] = value;
    }

    public string Title
    {
        get => _parameters["title"];
        set => _parameters["title"] = value;
    }
    public string Content
    {
        get => _parameters["content"];
        set => _parameters["content"] = value;
    }

    public string Host
    {
        get => _parameters["host"];
        set => _parameters["host"] = value;
    }

    public string Metadata
    {
        get => _parameters["metadata"];
        set => _parameters["metadata"] = value;
    }

    public string PrecompiledHeader
    {
        get => _parameters["precompiledheader"];
        set => _parameters["precompiledheader"] = value;
    }

    public void Add(string key, string value) 
        => _parameters.TryAdd(key, value);

    public bool TryGetValue(string key, out string? value)
        => _parameters.TryGetValue(key, out value);

    public void Clear()
    {
        _parameters.Clear();
        AddProtecteds();
    }

    public void Remove(string key)
    {
        if (_protectedParameters.Contains(key))
        {
            _parameters[key] = string.Empty;
        }
    }
}
