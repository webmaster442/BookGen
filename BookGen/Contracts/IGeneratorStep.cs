//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain;
using BookGen.Core.Contracts;

namespace BookGen.Contracts
{
    internal interface IGeneratorStep
    {
        void RunStep(GeneratorSettings settings, ILog log);
    }
}
