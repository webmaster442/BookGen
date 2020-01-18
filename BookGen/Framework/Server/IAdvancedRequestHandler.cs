//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using System.Net;

namespace BookGen.Framework.Server
{
    public interface IAdvancedRequestHandler: IRequestHandler
    {
        void Serve(HttpListenerRequest request, HttpListenerResponse response, ILog log);
    }
}
