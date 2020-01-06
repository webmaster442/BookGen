//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Api
{
    public interface IScriptHost
    {
        string SourceDirectory { get; }
        string TargetDirectory { get; }
    }
}
