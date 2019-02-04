//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain;
using BookGen.Utilities;
using System;

namespace BookGen.GeneratorSteps
{
    internal class CreateOutputDirectory : IGeneratorStep
    {
        public void RunStep(GeneratorSettings settings)
        {
            Console.WriteLine("Creating output directory...");
            settings.OutputDirectory.CreateDir();
        }
    }
}
