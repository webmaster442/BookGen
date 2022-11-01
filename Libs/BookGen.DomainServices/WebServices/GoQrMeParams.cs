//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.DomainServices.WebServices
{
    public static class GoQrMeParams
    {
        public const string ApiUrl = "https://api.qrserver.com/v1/create-qr-code/";
        public const string DataParam = "data";
        public const int MaxDataLength = 900;
        public const string SizeParam = "size";
        public const int MinimumSize = 10;
        public const int MaximumSizer = 1000;
        public const string FormatParam = "format";
    }
}
