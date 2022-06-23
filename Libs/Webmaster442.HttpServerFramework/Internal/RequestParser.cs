// ------------------------------------------------------------------------------------------------
// Copyright (c) 2021-2022 Ruzsinszki Gábor
// This is free software under the terms of the MIT License. https://opensource.org/licenses/MIT
// -----------------------------------------------------------------------------------------------

using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Webmaster442.HttpServerFramework.Domain;

namespace Webmaster442.HttpServerFramework.Internal;

internal class RequestParser
{
    private static readonly Regex splitter = new(@"\?|\&");
    private static readonly Regex headerSplitter = new(@":\s");
    private readonly int _maxPayload;

    public RequestParser(int maxPayload = 1024 * 1024 * 25)
    {
        _maxPayload = maxPayload;
    }

    public HttpRequest ParseRequest(Stream stream)
    {
        string? line = null;
        int lineNumber = 0;

        RequestMethod method = RequestMethod.Get;
        var urlParameters = new Dictionary<string, string>();
        var headers = new Dictionary<string, string>();
        string url = string.Empty;
        string version = string.Empty;
        byte[] data = Array.Empty<byte>();
        int size = 0;

        using (var reader = new StreamReader(stream, leaveOpen: true))
        {
            do
            {
                line = reader.ReadLine();
                if (string.IsNullOrEmpty(line))
                {
                    break;
                }
                if (lineNumber == 0)
                {
                    var methodUrlVersion = line.Split(' ');
                    if (methodUrlVersion.Length != 3)
                    {
                        throw new ServerException(HttpResponseCode.BadRequest);
                    }
                    if (!Enum.TryParse<RequestMethod>(methodUrlVersion[0], true, out method))
                        throw new ServerException(HttpResponseCode.BadRequest);

                    if (methodUrlVersion[1].Length > ushort.MaxValue)
                        throw new ServerException(HttpResponseCode.UriTooLong);

                    url = ExtractUrlAndParameters(methodUrlVersion[1], urlParameters);
                    version = methodUrlVersion[2];

                    if (version != "HTTP/1.0" && version != "HTTP/1.1")
                        throw new ServerException(HttpResponseCode.HTTPVersionNotSupported);

                }
                else
                {
                    ProcessHeaderKeyValue(line, headers);
                }
                ++lineNumber;
            }
            while (!string.IsNullOrEmpty(line));

            if (method == RequestMethod.Post)
            {
                if (!headers.ContainsKey(KnownHeaders.ContentLength))
                    throw new ServerException(HttpResponseCode.LengthRequired);

                try
                {
                    checked
                    {
                        size = int.Parse(headers[KnownHeaders.ContentLength]);
                        if (size > _maxPayload)
                            throw new ServerException(HttpResponseCode.PayloadTooLarge);
                    }
                }
                catch (OverflowException)
                {
                    throw new ServerException(HttpResponseCode.PayloadTooLarge);
                }

                int read = 0;
                int totaldone = 0;
                var buffer = new char[4096];
                data = new byte[size];
                do
                {
                    read = reader.Read(buffer, 0, buffer.Length);
                    byte[] converted = Encoding.UTF8.GetBytes(buffer, 0, read);
                    Array.Copy(converted, 0, data, totaldone, read);
                    totaldone += read;
                }
                while (read > 0 && totaldone < size);
            }
        }


        return new HttpRequest
        {
            Version = version,
            Url = url,
            Method = method,
            Parameters = urlParameters,
            Headers = headers,
            RequestSize = size,
            RequestContent = data
        };
    }

    private static void ProcessHeaderKeyValue(string line, Dictionary<string, string> headers)
    {
        if (string.IsNullOrEmpty(line)) return;

        var keyvalue = headerSplitter.Split(line);
        if (keyvalue.Length == 2)
        {
            headers.Add(keyvalue[0], keyvalue[1]);
        }
    }

    private static string ExtractUrlAndParameters(string location, Dictionary<string, string> urlParameters)
    {
        location = HttpUtility.UrlDecode(location);

        if (location.Contains('&')
            || location.Contains('?')
            || location.Contains('='))
        {
            var parts = splitter.Split(location);

            for (int i = 1; i < parts.Length; i++)
            {
                var keyvalue = parts[i].Split('=');
                if (keyvalue.Length == 2)
                {
                    urlParameters.Add(keyvalue[0], keyvalue[1]);
                }
            }
            return parts[0];
        }
        else
        {
            return location;
        }
    }
}
