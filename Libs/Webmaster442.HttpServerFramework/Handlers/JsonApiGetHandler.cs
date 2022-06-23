// ------------------------------------------------------------------------------------------------
// Copyright (c) 2021-2022 Ruzsinszki Gábor
// This is free software under the terms of the MIT License. https://opensource.org/licenses/MIT
// -----------------------------------------------------------------------------------------------

using Webmaster442.HttpServerFramework.Domain;

namespace Webmaster442.HttpServerFramework.Handlers;

/// <summary>
/// Base class that can be used to implement JSON based REST API
/// This class is suitable for GET like requests
/// </summary>
/// <typeparam name="TResponse">Response object tpye</typeparam>
public abstract class JsonApiGetHandler<TResponse> : IRequestHandler
{
    /// <summary>
    /// API supported HTTP method
    /// </summary>
    public RequestMethod SupportedMethod { get; }

    /// <summary>
    /// Creates a new instance of JsonApiGetHandler
    /// </summary>
    /// <param name="supportedMethod">HTTP method supported by this handler</param>
    public JsonApiGetHandler(RequestMethod supportedMethod)
    {
        SupportedMethod = supportedMethod;
    }

    /// <inheritdoc/>
    public async Task<bool> Handle(IServerLog? log, HttpRequest request, HttpResponse response)
    {
        if (!CanProcess(request.Url))
        {
            return false;
        }

        bool result = await TryProcessRequest(log, request.Url, request.Parameters, out TResponse responseObject);

        if (result)
        {
            response.ContentType = "application/json";
            await response.WriteJson(responseObject);
        }

        return result;
    }

    /// <summary>
    /// Tries to process the request.
    /// If request can't be processed return false.
    /// </summary>
    /// <param name="log">Log object, that can be used to log</param>
    /// <param name="url">Request url</param>
    /// <param name="parameters">Request parameters</param>
    /// <param name="responseObject">Response object that will be JSON serialized</param>
    /// <returns>True, if processing was succesfull, false if not</returns>
    protected abstract Task<bool> TryProcessRequest(IServerLog? log, string url, IReadOnlyDictionary<string, string> parameters, out TResponse responseObject);

    /// <summary>
    /// Determines if the specified url can be processed by this handler
    /// </summary>
    /// <param name="url">url of the request</param>
    /// <returns>true, if request can be processed, false otherwise</returns>
    protected abstract bool CanProcess(string url);


}
