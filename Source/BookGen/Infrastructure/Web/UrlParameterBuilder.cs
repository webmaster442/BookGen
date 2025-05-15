//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Web;

namespace BookGen.Infrastructure.Web;

public sealed class UrlParameterBuilder
{
    private readonly string _baseUrl;
    private readonly Dictionary<string, string> _parameters;

    public UrlParameterBuilder(string baseUrl)
    {
        if (baseUrl.EndsWith('/'))
            _baseUrl = baseUrl[..^1];
        else
            _baseUrl = baseUrl;
        _parameters = new Dictionary<string, string>();
    }

    public void AddParameter(string name, string value)
    {
        string urlSafeName = HttpUtility.UrlEncode(name);
        string urlSafeValue = HttpUtility.UrlEncode(value);
        if (_parameters.ContainsKey(urlSafeName))
        {
            _parameters[urlSafeName] = urlSafeValue;
        }
        else
        {
            _parameters.Add(urlSafeName, urlSafeValue);
        }
    }

    public void RemoveParameter(string name)
    {
        string urlSafeName = HttpUtility.UrlEncode(name);
        _parameters.Remove(urlSafeName);
    }

    public void Clear()
    {
        _parameters.Clear();
    }

    public Uri Build()
    {
        if (_parameters.Count > 0)
        {
            string joinedParams = string.Join('&', _parameters.Select(x => $"{x.Key}={x.Value}"));
            return new Uri($"{_baseUrl}?{joinedParams}");
        }
        return new Uri(_baseUrl);
    }
}
