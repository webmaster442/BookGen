//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using System.Net;

namespace BookGen.Framework.Server
{
    internal interface ISimpleRequestHandler: IRequestHandler
    {
        void Serve(string AbsoluteUri, HttpListenerResponse response, ILog log);
    }
}
