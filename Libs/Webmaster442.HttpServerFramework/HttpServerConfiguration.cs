// ------------------------------------------------------------------------------------------------
// Copyright (c) 2021-2022 Ruzsinszki Gábor
// This is free software under the terms of the MIT License. https://opensource.org/licenses/MIT
// -----------------------------------------------------------------------------------------------

using System.Net;
using Webmaster442.HttpServerFramework.Domain;

namespace Webmaster442.HttpServerFramework;

/// <summary>
/// Represents the HTTP server configuration
/// </summary>
public sealed class HttpServerConfiguration
{
    /// <summary>
    /// Listen port. If Not changed, the default is 8080
    /// </summary>
    public ushort Port { get; init; }

    /// <summary>
    /// Listen Adress. Default is Any.
    /// </summary>
    public IPAddress ListenAdress { get; init; }

    /// <summary>
    /// Set cutom error page for a given Error page.
    /// </summary>
    public Dictionary<HttpResponseCode, string> CustomErrorHandlers { get; init; }

    /// <summary>
    /// Maximum post data size. By default it's set to 25Mb.
    /// </summary>
    public int MaxPostSize { get; init; }

    /// <summary>
    /// If set to true, stack trace is printed to the output
    /// </summary>
    public bool DebugMode { get; init; }

    /// <summary>
    /// Maximum number of clients that are handled simultaniously. By default it's set to 10
    /// </summary>
    public int MaxClients { get; init; }

    /// <summary>
    /// Creates a new Instance of HttpServerConfiguration
    /// </summary>
    public HttpServerConfiguration()
    {
        DebugMode = false;
        Port = 8080;
        ListenAdress = IPAddress.Any;
        CustomErrorHandlers = new Dictionary<HttpResponseCode, string>();
        MaxPostSize = 25 * 1024 * 1024;
        MaxClients = 10;
    }
}
