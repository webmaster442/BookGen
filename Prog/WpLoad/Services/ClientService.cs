//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using WordPressPCL;

namespace WpLoad.Services
{
    internal static class ClientService
    {
        public static bool TryConfifgureConnection(string site, [NotNullWhen(true)] out WordPressClient? client)
        {
            if (SiteServices.TryReadSiteInfo(site, out SiteInfo? info))
            {
                if (info.DisableCertValidation)
                {
                    client = CreateUnvalidatedClient(info);
                }
                else
                {
                    client = CreateValidatedClient(info);
                }
                return true;
            }
            client = null;
            return false;
        }

        private static WordPressClient CreateUnvalidatedClient(SiteInfo info)
        {
            var httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
            {
                return true;
            };
            var httpClient = new HttpClient(httpClientHandler)
            {
                BaseAddress = new Uri(info.ApiEndPoint)
            };
            var client = new WordPressClient(httpClient, string.Empty);
            client.Auth.UseBasicAuth(info.Username, info.Password);
            return client;
        }

        private static WordPressClient CreateValidatedClient(SiteInfo info)
        {
            var client = new WordPressClient(info.ApiEndPoint, string.Empty);
            client.Auth.UseBasicAuth(info.Username, info.Password);
            return client;
        }
    }
}
