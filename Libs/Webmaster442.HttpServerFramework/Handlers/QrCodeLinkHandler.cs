// ------------------------------------------------------------------------------------------------
// Copyright (c) 2023 Ruzsinszki Gábor
// This is free software under the terms of the MIT License. https://opensource.org/licenses/MIT
// -----------------------------------------------------------------------------------------------

using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Web;

using Webmaster442.HttpServerFramework.Domain;
using Webmaster442.HttpServerFramework.Internal;

namespace Webmaster442.HttpServerFramework.Handlers;

/// <summary>
/// Serves a HTML page with QR code
/// </summary>
public class QrCodeLinkHandler : IRequestHandler
{
    private readonly ushort _port;
    private readonly IPAddress _address;

    /// <summary>
    /// Creates a new instance of QrCodeLinkHandler
    /// </summary>
    /// <param name="configuration">current server configuration</param>
    public QrCodeLinkHandler(HttpServerConfiguration configuration)
    {
        var comparer = new IPAdressComparer();
        _port = configuration.Port;
        _address = comparer.Equals(configuration.ListenAdress, IPAddress.Any) ? GetLocalNetworkAdress() : configuration.ListenAdress;
    }

    private IPAddress GetLocalNetworkAdress()
    {
        foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
        {
            if ((item.NetworkInterfaceType == NetworkInterfaceType.Wireless80211
                || item.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                && item.OperationalStatus == OperationalStatus.Up)
            {
                return item.GetIPProperties()
                    .UnicastAddresses
                    .Where(x => x.Address.AddressFamily == AddressFamily.InterNetwork)
                    .Select(x => x.Address)
                    .FirstOrDefault() ?? IPAddress.Loopback;
            }
        }
        return IPAddress.Loopback;
    }

    /// <inheritdoc/>
    public async Task<bool> Handle(IServerLog? log, HttpRequest request, HttpResponse response)
    {
        if (request.Method != RequestMethod.Get
            || request.Url != "/qrcodelink")
        {
            return false;
        }

        const string title = "Link to this server";
        HtmlBuilder builder = new HtmlBuilder(title);

        builder
            .AppendCss("#content { margin-left: 30px; }")
            .AppendHeader(1, title)
            .AppendHr()
            .AppendBeginElement(Element.Div, "content")
            .AppendParagraph("Scan this QR code to connect to this server")
            .AppendBeginElement(Element.P)
            .AppendLink("/", "Continue to server...")
            .AppendEndElement(Element.P)
            .AppendLineBreak();

        string data = $"http://{_address}:{_port}";
        string query = $"https://api.qrserver.com/v1/create-qr-code/?data={HttpUtility.UrlEncode(data)}&size=300x300";
        
        builder
            .AppendImage(query, "Local server")
            .AppendParagraph($"Or letenatively use this url: {data}")
            .AppendParagraph("Note: This only works if the server and the device you are using is on the same network")
            .AppendEndElement(Element.Div);
        
        response.ContentType = "text/html; charset=utf8";
        response.ResponseCode = HttpResponseCode.Ok;

        await response.WriteAsync(builder.ToString());

        return true;
    }
}
