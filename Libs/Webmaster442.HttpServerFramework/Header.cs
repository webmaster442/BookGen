// ------------------------------------------------------------------------------------------------
// Copyright (c) 2021-2022 Ruzsinszki Gábor
// This is free software under the terms of the MIT License. https://opensource.org/licenses/MIT
// -----------------------------------------------------------------------------------------------

namespace Webmaster442.HttpServerFramework;

/// <summary>
/// Lists all known HTTP headers
/// </summary>
public static class KnownHeaders
{
    /// <summary>
    /// User agent of browser
    /// </summary>
    public const string UserAgent = "User-Agent";
    /// <summary>
    /// Requested host name
    /// </summary>
    public const string Host = "Host";
    /// <summary>
    /// Accepted language
    /// </summary>
    public const string AcceptLanguage = "Accept-Language";
    /// <summary>
    /// Accepted encodings
    /// </summary>
    public const string AcceptEncoding = "Accept-Encoding";
    /// <summary>
    /// Connection type
    /// </summary>
    public const string Connection = "Connection";
    /// <summary>
    /// Content type
    /// </summary>
    public const string ContentType = "Content-Type";
    /// <summary>
    /// Content length
    /// </summary>
    public const string ContentLength = "Content-Length";
}
