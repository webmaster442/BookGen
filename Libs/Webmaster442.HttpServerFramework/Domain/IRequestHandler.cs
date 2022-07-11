// ------------------------------------------------------------------------------------------------
// Copyright (c) 2021-2022 Ruzsinszki Gábor
// This is free software under the terms of the MIT License. https://opensource.org/licenses/MIT
// -----------------------------------------------------------------------------------------------

using System.Threading.Tasks;

namespace Webmaster442.HttpServerFramework.Domain;

/// <summary>
/// Request handler interface
/// </summary>
public interface IRequestHandler
{
    /// <summary>
    /// Handle a request
    /// </summary>
    /// <param name="log">Logger interface</param>
    /// <param name="request">Request</param>
    /// <param name="response">Response to the request</param>
    /// <returns>true, if the request is handled</returns>
    Task<bool> Handle(IServerLog? log, HttpRequest request, HttpResponse response);
}
