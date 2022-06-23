﻿// ------------------------------------------------------------------------------------------------
// Copyright (c) 2021-2022 Ruzsinszki Gábor
// This is free software under the terms of the MIT License. https://opensource.org/licenses/MIT
// -----------------------------------------------------------------------------------------------

namespace Webmaster442.HttpServerFramework.Domain;

/// <summary>
/// Represents available HTTP response codes
/// Source: https://developer.mozilla.org/en-US/docs/Web/HTTP/Status#information_responses
/// detailed: https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/[number]
/// </summary>
public enum HttpResponseCode
{
    /// <summary>
    /// This interim response indicates that everything so far is OK and that the client should continue the request, or ignore the response if the request is already finished.
    /// </summary>
    Continue = 100,
    /// <summary>
    /// This code is sent in response to an Upgrade request header from the client, and indicates the protocol the server is switching to.
    /// </summary>
    SwitchingProtocol = 101,
    /// <summary>
    /// This status code is primarily intended to be used with the Link header, letting the user agent start preloading resources while the server prepares a response.
    /// </summary>
    EarlyHints = 103,
    /// <summary>
    /// he request has succeeded. The meaning of the success depends on the HTTP method:
    /// GET: The resource has been fetched and is transmitted in the message body.
    /// HEAD: The entity headers are in the message body.
    /// PUT or POST: The resource describing the result of the action is transmitted in the message body.
    /// TRACE: The message body contains the request message as received by the server.
    /// </summary>
    Ok = 200,
    /// <summary>
    /// The request has succeeded and a new resource has been created as a result. This is typically the response sent after POST requests, or some PUT requests.
    /// </summary>
    Created = 201,
    /// <summary>
    /// The request has been received but not yet acted upon. It is noncommittal, since there is no way in HTTP to later send an asynchronous response indicating the outcome of the request. It is intended for cases where another process or server handles the request, or for batch processing.
    /// </summary>
    Accepted = 202,
    /// <summary>
    /// This response code means the returned meta-information is not exactly the same as is available from the origin server, but is collected from a local or a third-party copy. This is mostly used for mirrors or backups of another resource. Except for that specific case, the "200 OK" response is preferred to this status.
    /// </summary>
    NonAuthoritativeInformation = 203,
    /// <summary>
    /// There is no content to send for this request, but the headers may be useful. The user-agent may update its cached headers for this resource with the new ones.
    /// </summary>
    NoContent = 204,
    /// <summary>
    /// Tells the user-agent to reset the document which sent this request.
    /// </summary>
    ResetContent = 205,
    /// <summary>
    /// This response code is used when the Range header is sent from the client to request only part of a resource.
    /// </summary>
    PartialContent = 206,
    /// <summary>
    /// Conveys information about multiple resources, for situations where multiple status codes might be appropriate.
    /// </summary>
    MultiStatus = 207,
    /// <summary>
    /// Used inside a dav:propstat response element to avoid repeatedly enumerating the internal members of multiple bindings to the same collection.
    /// </summary>
    AlreadyReported = 208,
    /// <summary>
    /// The server has fulfilled a GET request for the resource, and the response is a representation of the result of one or more instance-manipulations applied to the current instance.
    /// </summary>
    IMUsed = 226,
    /// <summary>
    /// The request has more than one possible response. The user-agent or user should choose one of them. (There is no standardized way of choosing one of the responses, but HTML links to the possibilities are recommended so the user can pick.)
    /// </summary>
    MultipleChoice = 300,
    /// <summary>
    /// The URL of the requested resource has been changed permanently. The new URL is given in the response.
    /// </summary>
    MovedPermanently = 301,
    /// <summary>
    /// This response code means that the URI of requested resource has been changed temporarily. Further changes in the URI might be made in the future. Therefore, this same URI should be used by the client in future requests.
    /// </summary>
    Found = 302,
    /// <summary>
    /// The server sent this response to direct the client to get the requested resource at another URI with a GET request.
    /// </summary>
    SeeOther = 303,
    /// <summary>
    /// This is used for caching purposes. It tells the client that the response has not been modified, so the client can continue to use the same cached version of the response.
    /// </summary>
    NotModified = 304,
    /// <summary>
    /// Defined in a previous version of the HTTP specification to indicate that a requested response must be accessed by a proxy. It has been deprecated due to security concerns regarding in-band configuration of a proxy.
    /// </summary>
    UseProxy = 305,
    /// <summary>
    /// The server sends this response to direct the client to get the requested resource at another URI with same method that was used in the prior request. This has the same semantics as the 302 Found HTTP response code, with the exception that the user agent must not change the HTTP method used: If a POST was used in the first request, a POST must be used in the second request.
    /// </summary>
    TemporaryRedirect = 307,
    /// <summary>
    /// This means that the resource is now permanently located at another URI, specified by the Location: HTTP Response header. This has the same semantics as the 301 Moved Permanently HTTP response code, with the exception that the user agent must not change the HTTP method used: If a POST was used in the first request, a POST must be used in the second request.
    /// </summary>
    PermanentRedirect = 308,
    /// <summary>
    /// The server could not understand the request due to invalid syntax.
    /// </summary>
    BadRequest = 400,
    /// <summary>
    /// Although the HTTP standard specifies "unauthorized", semantically this response means "unauthenticated". That is, the client must authenticate itself to get the requested response.
    /// </summary>
    Unauthorized = 401,
    /// <summary>
    /// This response code is reserved for future use. The initial aim for creating this code was using it for digital payment systems, however this status code is used very rarely and no standard convention exists.
    /// </summary>
    PaymentRequired = 402,
    /// <summary>
    /// The client does not have access rights to the content; that is, it is unauthorized, so the server is refusing to give the requested resource. Unlike 401, the client's identity is known to the server.
    /// </summary>
    Forbidden = 403,
    /// <summary>
    /// The server can not find the requested resource. In the browser, this means the URL is not recognized. In an API, this can also mean that the endpoint is valid but the resource itself does not exist. Servers may also send this response instead of 403 to hide the existence of a resource from an unauthorized client. This response code is probably the most famous one due to its frequent occurrence on the web.
    /// </summary>
    NotFound = 404,
    /// <summary>
    /// The request method is known by the server but has been disabled and cannot be used. For example, an API may forbid DELETE-ing a resource. The two mandatory methods, GET and HEAD, must never be disabled and should not return this error code.
    /// </summary>
    MethodNotAllowed = 405,
    /// <summary>
    /// This response is sent when the web server, after performing server-driven content negotiation, doesn't find any content that conforms to the criteria given by the user agent.
    /// </summary>
    NotAcceptable = 406,
    /// <summary>
    /// This is similar to 401 but authentication is needed to be done by a proxy.
    /// </summary>
    ProxyAuthenticationRequired = 407,
    /// <summary>
    /// This response is sent on an idle connection by some servers, even without any previous request by the client. It means that the server would like to shut down this unused connection. This response is used much more since some browsers, like Chrome, Firefox 27+, or IE9, use HTTP pre-connection mechanisms to speed up surfing. Also note that some servers merely shut down the connection without sending this message.
    /// </summary>
    RequestTimeout = 408,
    /// <summary>
    /// This response is sent when a request conflicts with the current state of the server.
    /// </summary>
    Conflict = 409,
    /// <summary>
    /// This response is sent when the requested content has been permanently deleted from server, with no forwarding address. Clients are expected to remove their caches and links to the resource. The HTTP specification intends this status code to be used for "limited-time, promotional services". APIs should not feel compelled to indicate resources that have been deleted with this status code.
    /// </summary>
    Gone = 410,
    /// <summary>
    /// Server rejected the request because the Content-Length header field is not defined and the server requires it.
    /// </summary>
    LengthRequired = 411,
    /// <summary>
    /// The client has indicated preconditions in its headers which the server does not meet.
    /// </summary>
    PreconditionFailed = 412,
    /// <summary>
    /// Request entity is larger than limits defined by server; the server might close the connection or return an Retry-After header field.
    /// </summary>
    PayloadTooLarge = 413,
    /// <summary>
    /// The URI requested by the client is longer than the server is willing to interpret.
    /// </summary>
    UriTooLong = 414,
    /// <summary>
    /// The media format of the requested data is not supported by the server, so the server is rejecting the request.
    /// </summary>
    UnsupportedMediaType = 415,
    /// <summary>
    /// The range specified by the Range header field in the request can't be fulfilled; it's possible that the range is outside the size of the target URI's data.
    /// </summary>
    RangeNotSatisfiable = 416,
    /// <summary>
    /// This response code means the expectation indicated by the Expect request header field can't be met by the server.
    /// </summary>
    ExpectationFailed = 417,
    /// <summary>
    /// The server refuses the attempt to brew coffee with a teapot.
    /// </summary>
    IamaTeapot = 418,
    /// <summary>
    /// The request was directed at a server that is not able to produce a response. This can be sent by a server that is not configured to produce responses for the combination of scheme and authority that are included in the request URI.
    /// </summary>
    MisdirectedRequest = 421,
    /// <summary>
    /// The request was well-formed but was unable to be followed due to semantic errors.
    /// </summary>
    UnprocessableEntity = 422,
    /// <summary>
    /// The resource that is being accessed is locked.
    /// </summary>
    Locked = 423,
    /// <summary>
    /// The request failed due to failure of a previous request.
    /// </summary>
    FailedDependency = 424,
    /// <summary>
    /// Indicates that the server is unwilling to risk processing a request that might be replayed.
    /// </summary>
    TooEarly = 425,
    /// <summary>
    /// The server refuses to perform the request using the current protocol but might be willing to do so after the client upgrades to a different protocol. The server sends an Upgrade header in a 426 response to indicate the required protocol(s).
    /// </summary>
    UpgradeRequired = 426,
    /// <summary>
    /// The origin server requires the request to be conditional. This response is intended to prevent the 'lost update' problem, where a client GETs a resource's state, modifies it, and PUTs it back to the server, when meanwhile a third party has modified the state on the server, leading to a conflict.
    /// </summary>
    PreconditionRequired = 428,
    /// <summary>
    /// The user has sent too many requests in a given amount of time ("rate limiting").
    /// </summary>
    TooManyRequests = 429,
    /// <summary>
    /// The server is unwilling to process the request because its header fields are too large. The request may be resubmitted after reducing the size of the request header fields.
    /// </summary>
    RequestHeaderFieldsTooLarge = 431,
    /// <summary>
    /// The user-agent requested a resource that cannot legally be provided, such as a web page censored by a government.
    /// </summary>
    UnavailableForLegalReasons = 451,
    /// <summary>
    /// The server has encountered a situation it doesn't know how to handle.
    /// </summary>
    InternalServerError = 500,
    /// <summary>
    /// The request method is not supported by the server and cannot be handled. The only methods that servers are required to support (and therefore that must not return this code) are GET and HEAD.
    /// </summary>
    NotImplemented = 501,
    /// <summary>
    /// This error response means that the server, while working as a gateway to get a response needed to handle the request, got an invalid response.
    /// </summary>
    BadGateway = 502,
    /// <summary>
    /// The server is not ready to handle the request. Common causes are a server that is down for maintenance or that is overloaded. Note that together with this response, a user-friendly page explaining the problem should be sent. This responses should be used for temporary conditions and the Retry-After: HTTP header should, if possible, contain the estimated time before the recovery of the service. The webmaster must also take care about the caching-related headers that are sent along with this response, as these temporary condition responses should usually not be cached.
    /// </summary>
    ServiceUnavailable = 503,
    /// <summary>
    /// This error response is given when the server is acting as a gateway and cannot get a response in time.
    /// </summary>
    GatewayTimeout = 504,
    /// <summary>
    /// The HTTP version used in the request is not supported by the server.
    /// </summary>
    HTTPVersionNotSupported = 505,
    /// <summary>
    /// The server has an internal configuration error: the chosen variant resource is configured to engage in transparent content negotiation itself, and is therefore not a proper end point in the negotiation process.
    /// </summary>
    VariantAlsoNegotiates = 506,
    /// <summary>
    /// The method could not be performed on the resource because the server is unable to store the representation needed to successfully complete the request.
    /// </summary>
    InsufficientStorage = 507,
    /// <summary>
    /// The server detected an infinite loop while processing the request.
    /// </summary>
    LoopDetected = 508,
    /// <summary>
    /// Further extensions to the request are required for the server to fulfill it.
    /// </summary>
    NotExtended = 510,
    /// <summary>
    /// The 511 status code indicates that the client needs to authenticate to gain network access.
    /// </summary>
    NetworkAuthenticationRequired = 511
}
