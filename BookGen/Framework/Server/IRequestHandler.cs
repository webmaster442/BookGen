//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Net;

namespace BookGen.Framework.Server
{
    internal interface IRequestHandler
    {
        bool CanServe(string AbsoluteUri);
        void Serve(HttpListenerResponse response);
    }
}
