//-----------------------------------------------------------------------------
// (c) 2019-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Framework;
using BookGen.Framework.Scripts;
using BookGen.Interfaces;

namespace BookGen
{
    internal class WordpressBuilder : Builder
    {
        public WordpressBuilder(RuntimeSettings settings, ILog log, ShortCodeLoader loader, CsharpScriptHandler scriptHandler)
            : base(settings, log, loader, scriptHandler)
        {
            var session = new GeneratorSteps.Wordpress.Session();
            AddStep(new GeneratorSteps.CreateOutputDirectory());
            AddStep(new GeneratorSteps.ImageProcessor());
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
            return "<!--{content}-->";
        }
    }
}
