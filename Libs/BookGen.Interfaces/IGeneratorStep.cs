//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;

namespace BookGen.Interfaces
{
    internal interface IGeneratorStep
    {
        void RunStep(IReadonlyRuntimeSettings settings, ILog log);
    }
}
