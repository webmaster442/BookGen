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
        public const string SizeParam = "size";
        public const string FormatParam = "format";
    }

    public static class MathVercelParams
    {
        public const string ApiUrl = "https://math.vercel.app";
        public const string FromPram = "from";
    }
}
