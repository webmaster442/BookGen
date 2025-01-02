//-----------------------------------------------------------------------------
// (c) 2019-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Microsoft.Extensions.Logging;

namespace BookGen.Interfaces
{
    public interface IGeneratorStep
    {
        void RunStep(IReadonlyRuntimeSettings settings, ILogger log);
    }
}
