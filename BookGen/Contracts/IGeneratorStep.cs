//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core.Contracts;
using BookGen.Domain;

namespace BookGen.Contracts
{
    internal interface IGeneratorStep
    {
        void RunStep(RuntimeSettings settings, ILog log);
    }
}
