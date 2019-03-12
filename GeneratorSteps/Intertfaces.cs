//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain;
using BookGen.Framework;
using NLog;

namespace BookGen.GeneratorSteps
{
    internal interface IGeneratorStep
    {
        void RunStep(GeneratorSettings settings, ILogger log);
    }

    internal interface IGeneratorContentFillStep: IGeneratorStep
    {
        GeneratorContent Content { get; set; }
    }

    internal interface ITemplatedStep : IGeneratorStep, IGeneratorContentFillStep
    {
        Template Template { get; set; }
    }
}
