﻿// ------------------------------------------------------------------------------------------------
// Copyright (c) 2021-2023 Ruzsinszki Gábor
// This is free software under the terms of the MIT License. https://opensource.org/licenses/MIT
// -----------------------------------------------------------------------------------------------

using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Xml.Serialization;

using Webmaster442.HttpServerFramework.Internal;

namespace Webmaster442.HttpServerFramework.Domain;

/// <summary>
/// Represents a HTTP Response
/// </summary>
public sealed class HttpResponse : IDisposable
{
    private NetworkStream? _stream;
    private readonly byte[] _end;

    /// <summary>
    /// Response code
    /// </summary>
    public HttpResponseCode ResponseCode { get; set; }

    /// <summary>
    /// Response content type
    /// </summary>
    public string ContentType { get; set; }

    /// <summary>
    /// Contains a date and time when the origin server believes the resource was last modified.
    /// </summary>
    public DateTime LastModified { get; set; }

    /// <summary>
    /// Additional headers to send
    /// </summary>
    public Dictionary<string, string> Headers { get; }

    /// <summary>
    /// Creates a new instance of HttpResponse
    /// </summary>
    internal HttpResponse(NetworkStream networkStream)
    {
        _stream = networkStream;
        ContentType = "text/plain";
        Headers = new Dictionary<string, string>();
        ResponseCode = HttpResponseCode.Ok;
        LastModified = DateTime.UtcNow;

        _end = "\n\n"u8.ToArray();
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        if (_stream != null)
        {
            _stream.Dispose();
            _stream = null;
        }
    }
    
    private void SetHeader(string name, object value)
    {
        string valueStr = value.ToString() ?? string.Empty;
        if (Headers.ContainsKey(name))
            Headers[name] = valueStr;
        else
            Headers.Add(name, valueStr);
    }

    private string PrepareHeaders(long contentLength)
    {
        StringBuilder headers = new();
        headers.Append("HTTP/1.1 ").Append((int)ResponseCode).AppendLine($" {ResponseCode}");
        SetHeader("Content-Length", contentLength);
        SetHeader("Content-Type", ContentType);
        SetHeader("Date", DateTime.UtcNow.ToHeaderFormat());
        SetHeader("Last-Modified", LastModified.ToHeaderFormat());
        foreach (var header in Headers)
        {
            headers.AppendLine($"{header.Key}: {header.Value}");
        }
        headers.AppendLine();
        return headers.ToString();
    }

    /// <summary>
    /// Write text to the client
    /// </summary>
    /// <param name="text">Text to write</param>
    /// <returns>an awaitable task</returns>
    public async ValueTask WriteAsync(string text)
    {
        var txt = Encoding.UTF8.GetBytes(text);
        var headers = Encoding.UTF8.GetBytes(PrepareHeaders(txt.Length+ _end.Length));
        if (_stream != null)
        {
#pragma warning disable RCS1090 // Add call to 'ConfigureAwait' (or vice versa).
            await _stream.WriteAsync(headers);
            await _stream.WriteAsync(txt);
            await _stream.WriteAsync(_end);
#pragma warning restore RCS1090 // Add call to 'ConfigureAwait' (or vice versa).
        }
    }

    /// <summary>
    /// Write binary data to the client
    /// </summary>
    /// <param name="data">a stream containing the data</param>
    /// <returns>an awaitable task</returns>
    public async ValueTask WriteAsync(Stream data)
    {
        var headers = Encoding.UTF8.GetBytes(PrepareHeaders(data.Length));
        if (_stream != null)
        {
#pragma warning disable RCS1090 // Add call to 'ConfigureAwait' (or vice versa).
            await _stream.WriteAsync(headers);

            byte[] buffer = new byte[4096];
            int read = 0;
            do
            {
                read = data.Read(buffer, 0, buffer.Length);
                await _stream.WriteAsync(buffer, 0, read);
            }
            while (read > 0);
            await _stream.WriteAsync(_end);
#pragma warning restore RCS1090 // Add call to 'ConfigureAwait' (or vice versa).
        }
    }

    /// <summary>
    /// Write binary data to the client
    /// </summary>
    /// <param name="data">data to write</param>
    /// <returns>an awaitable task</returns>
    public async ValueTask WriteAsync(byte[] data)
    {
        var headers = Encoding.UTF8.GetBytes(PrepareHeaders(data.Length));
        if (_stream != null)
        {
#pragma warning disable RCS1090 // Add call to 'ConfigureAwait' (or vice versa).
            await _stream.WriteAsync(headers);
            await _stream.WriteAsync(data, 0, data.Length);
            await _stream.WriteAsync(_end);
#pragma warning restore RCS1090 // Add call to 'ConfigureAwait' (or vice versa).
        }
    }

    /// <summary>
    /// Write a type as JSON string
    /// </summary>
    /// <typeparam name="T">Type parameter</typeparam>
    /// <param name="input">input object</param>
    /// <param name="options">Serializer options</param>
    /// <returns>an awaitable task</returns>
    public async ValueTask WriteJsonAsync<T>(T input, JsonSerializerOptions? options = null)
    {
        string serialized = JsonSerializer.Serialize(input, options);
        ContentType = "application/json";
        await WriteAsync(serialized);
    }
    /// <summary>
    /// Write data as XML
    /// </summary>
    /// <param name="serializer">XML serializer to use for the data</param>
    /// <param name="data">Data to write</param>
    /// <returns>an awaitable task</returns>
    public async ValueTask WriteXmlAsync(XmlSerializer serializer, object data)
    {
        ContentType = "application/xml";
        using (var ms = new MemoryStream())
        {
            serializer.Serialize(ms, data);
            await WriteAsync(ms);
        }
    }
}
