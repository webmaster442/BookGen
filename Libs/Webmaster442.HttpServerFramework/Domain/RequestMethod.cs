// ------------------------------------------------------------------------------------------------
// Copyright (c) 2021-2022 Ruzsinszki Gábor
// This is free software under the terms of the MIT License. https://opensource.org/licenses/MIT
// -----------------------------------------------------------------------------------------------

namespace Webmaster442.HttpServerFramework.Domain;

/// <summary>
/// HTTP Request method
/// </summary>
public enum RequestMethod
{
    /// <summary>
    /// The GET method requests a representation of the specified resource. Requests using GET should only retrieve data
    /// </summary>
    Get,
    /// <summary>
    /// The POST method is used to submit an entity to the specified resource, often causing a change in state or side effects on the server.
    /// </summary>
    Post,
    /// <summary>
    /// The HEAD method asks for a response identical to that of a GET request, but without the response body.
    /// </summary>
    Head,
    /// <summary>
    /// The PUT method replaces all current representations of the target resource with the request payload.
    /// </summary>
    Put,
    /// <summary>
    /// The DELETE method deletes the specified resource.
    /// </summary>
    Delete,
    /// <summary>
    /// The CONNECT method establishes a tunnel to the server identified by the target resource.
    /// </summary>
    Connect,
    /// <summary>
    /// The OPTIONS method is used to describe the communication options for the target resource.
    /// </summary>
    Options,
    /// <summary>
    /// The TRACE method performs a message loop-back test along the path to the target resource.
    /// </summary>
    Trace,
    /// <summary>
    /// The PATCH method is used to apply partial modifications to a resource.
    /// </summary>
    Patch,
}
