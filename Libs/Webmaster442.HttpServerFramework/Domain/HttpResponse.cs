// ------------------------------------------------------------------------------------------------
// Copyright (c) 2021-2022 Ruzsinszki Gábor
// This is free software under the terms of the MIT License. https://opensource.org/licenses/MIT
// -----------------------------------------------------------------------------------------------

using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Xml.Serialization;

namespace Webmaster442.HttpServerFramework.Domain;

/// <summary>
/// Represents a HTTP Response
/// </summary>
public sealed class HttpResponse : IDisposable
{
    private static readonly byte[] end = Encoding.UTF8.GetBytes("\n\n");

    private NetworkStream? _stream;

    /// <summary>
    /// Response code
    /// </summary>
    public HttpResponseCode ResponseCode { get; set; }

    /// <summary>
    /// Response content type
    /// </summary>
    public string ContentType { get; set; }

    /// <summary>
    /// Additional headers to send
    /// </summary>
    public Dictionary<string, string> AdditionalHeaders { get; }

    /// <summary>
    /// Creates a new instance of HttpResponse
    /// </summary>
    internal HttpResponse(NetworkStream networkStream)
    {
        _stream = networkStream;
        ContentType = "text/plain";
        AdditionalHeaders = new Dictionary<string, string>();
        ResponseCode = HttpResponseCode.Ok;
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

    private string PrepareHeaders(long contentLength)
    {
        StringBuilder headers = new StringBuilder();
        headers.Append("HTTP/1.1 ").Append((int)ResponseCode).AppendLine(" ResponseCode");
        headers.Append("Content-Length: ").Append(contentLength).AppendLine();
        headers.Append("Content-Type: ").AppendLine(ContentType);
        foreach (var header in AdditionalHeaders)
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
    public async ValueTask Write(string text)
    {
        var txt = Encoding.UTF8.GetBytes(text);
        var headers = Encoding.UTF8.GetBytes(PrepareHeaders(txt.Length));
        if (_stream != null)
        {
#pragma warning disable RCS1090 // Add call to 'ConfigureAwait' (or vice versa).
            await _stream.WriteAsync(headers);
            await _stream.WriteAsync(txt);
            await _stream.WriteAsync(end);
#pragma warning restore RCS1090 // Add call to 'ConfigureAwait' (or vice versa).
        }
    }

    /// <summary>
    /// Write binary data to the client
    /// </summary>
    /// <param name="data">a stream containing the data</param>
    /// <returns>an awaitable task</returns>
    public async ValueTask Write(Stream data)
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
            await _stream.WriteAsync(end);
#pragma warning restore RCS1090 // Add call to 'ConfigureAwait' (or vice versa).
        }
    }

    /// <summary>
    /// Write binary data to the client
    /// </summary>
    /// <param name="data">data to write</param>
    /// <returns>an awaitable task</returns>
    public async ValueTask Write(byte[] data)
    {
        var headers = Encoding.UTF8.GetBytes(PrepareHeaders(data.Length));
        if (_stream != null)
        {
#pragma warning disable RCS1090 // Add call to 'ConfigureAwait' (or vice versa).
            await _stream.WriteAsync(headers);
            await _stream.WriteAsync(data, 0, data.Length);
            await _stream.WriteAsync(end);
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
    public async ValueTask WriteJson<T>(T input, JsonSerializerOptions? options = null)
    {
        string serialized = JsonSerializer.Serialize(input, options);
        ContentType = "application/json";
        await Write(serialized);
    }
    /// <summary>
    /// Write data as XML
    /// </summary>
    /// <param name="serializer">XML serializer to use for the data</param>
    /// <param name="data">Data to write</param>
    /// <returns>an awaitable task</returns>
    public async ValueTask WriteXml(XmlSerializer serializer, object data)
    {
        ContentType = "application/xml";
        using (var ms = new MemoryStream())
        {
            serializer.Serialize(ms, data);
            await Write(ms);
        }
    }
}
