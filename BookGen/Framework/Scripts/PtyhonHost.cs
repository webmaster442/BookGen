//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Core;

namespace BookGen.Framework.Scripts
{
    internal class PtyhonHost : ProcessHost
    {
        public PtyhonHost(ILog log) : base(log)
        {
        }

        public override string ProcessFileName => ProcessInterop.AppendExecutableExtension("python");

        public override string ProcessArguments => string.Empty;

        public override string SerializeHostInfo(ScriptHost host)
        {
            return JsonInliner.InlinePython(nameof(host), host, _log);
        }
    }
}
