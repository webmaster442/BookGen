// ------------------------------------------------------------------------------------------------
// Copyright (c) 2021-2022 Ruzsinszki Gábor
// This is free software under the terms of the MIT License. https://opensource.org/licenses/MIT
// -----------------------------------------------------------------------------------------------

using System.Text.Json;
using Webmaster442.HttpServerFramework.Domain;

namespace Webmaster442.HttpServerFramework.Handlers
{
    /// <summary>
    /// Base class that can be used to implement JSON based REST API
    /// This class is suitable for post like requests
    /// </summary>
    /// <typeparam name="TRequest">Request object type</typeparam>
    /// <typeparam name="TResponse">Response object tpye</typeparam>
    public abstract class JsonPostApiHandler<TRequest, TResponse> : IRequestHandler 
        where TRequest: class, new()
        where TResponse: class, new()
    {
        /// <summary>
        /// API supported HTTP method
        /// </summary>
        protected RequestMethod SupportedMethod { get; }

        /// <summary>
        /// Url of the API
        /// </summary>
        protected string Url { get; }

        /// <summary>
        /// Creates a new instance of JsonApiPostHandler
        /// </summary>
        /// <param name="supportedMethod">HTTP method supported by this handler</param>
        /// <param name="url">Url of the API</param>
        public JsonPostApiHandler(RequestMethod supportedMethod, string url)
        {
            SupportedMethod = supportedMethod;
            Url = url;
        }

        /// <inheritdoc/>
        public async Task<bool> Handle(IServerLog? log, HttpRequest request, HttpResponse response)
        {
            if (request.Url != Url)
            {
                return false;
            }

            var requestObject = JsonSerializer.Deserialize<TRequest>(request.RequestContent);
            if (requestObject == null)
            {
                log?.Warning("Request deserialize failed");
                throw new ServerException(HttpResponseCode.InternalServerError);
            }
            bool result = await TryProcessRequest(log, requestObject, out TResponse responseObject);

            if (result)
            {
                await response.WriteJson(responseObject);
            }

            return result;
        }

        /// <summary>
        /// Tries to process the request.
        /// If request can't be processed return false.
        /// </summary>
        /// <param name="log">Log object, that can be used to log</param>
        /// <param name="requestObject">Request JSON deserialized object</param>
        /// <param name="responseObject">Response object that will be JSON serialized</param>
        /// <returns>True, if processing was succesfull, false if not</returns>
        protected abstract Task<bool> TryProcessRequest(IServerLog? log, TRequest requestObject, out TResponse responseObject);
    }
}
