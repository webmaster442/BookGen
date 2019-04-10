using BookGen.Core.Configuration;
using BookGen.Core.Contracts;
using BookGen.Framework;

namespace BookGen
{
    internal class EpubBuilder : Generator
    {
        public EpubBuilder(string workdir, Config configuration, ILog log) : base(workdir, configuration, log)
        { 
            AddStep(new GeneratorSteps.CopyImagesDirectory(true, true));
        }
    }
}
