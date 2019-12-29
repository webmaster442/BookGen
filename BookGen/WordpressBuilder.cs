//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Core.Configuration;
using BookGen.Core.Contracts;
using BookGen.Framework;

namespace BookGen
{
    internal class WordpressBuilder : Builder
    {
        public WordpressBuilder(string workdir, Config configuration, ILog log) : base(workdir, configuration, log, configuration.TargetWordpress)
        {
            var session = new GeneratorSteps.Wordpress.Session();
            AddStep(new GeneratorSteps.CreateOutputDirectory());
            AddStep(new GeneratorSteps.CopyImagesDirectory(true, true));
            AddStep(new GeneratorSteps.Wordpress.CreateWpChannel(session));
            AddStep(new GeneratorSteps.Wordpress.CreateWpPages(session));
            AddStep(new GeneratorSteps.Wordpress.WriteExportXmlFile(session));
        }

        protected override FsPath ConfigureOutputDirectory(FsPath workingDirectory)
        {
            return workingDirectory.Combine(Settings.Configuration.TargetWordpress.OutPutDirectory);
        }

        protected override string ConfigureTemplateContent()
        {
            return "[content]";
        }
    }
}
