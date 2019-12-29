//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core.Contracts;

namespace BookGen.Contracts
{
    public interface IScript
    {
        string InvokeName { get; }
        string ScriptMain(IReadonlyRuntimeSettings runtimeSettings, ILog log);
    }
}
