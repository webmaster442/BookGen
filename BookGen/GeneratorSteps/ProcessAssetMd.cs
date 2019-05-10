//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Contracts;
using BookGen.Core;
using BookGen.Core.Contracts;
using BookGen.Core.Markdown;
using BookGen.Domain;
using BookGen.Framework;
using BookGen.Utilities;

namespace BookGen.GeneratorSteps
{
    class ProcessAssetMd : ITemplatedStep
    {
        public GeneratorContent Content { get; set; }
        public Template Template { get; set; }

        public void RunStep(RuntimeSettings settings, ILog log)
        {
            log.Info("Generating Asset md files...");
            foreach (var asset in settings.Configruation.Assets)
            {
                FsPath source = settings.SourceDirectory.Combine(asset.Source);
                FsPath target = settings.OutputDirectory.Combine(asset.Target);

                if (source.IsExisting &&
                    source.Extension == ".md" &&
                    target.Extension != ".html" &&
                    target.Extension != ".htm")
                {
                    log.Detail("Processing file: {0}", source);
                    var inputContent = source.ReadFile();


                    Content.Title = MarkdownUtils.GetTitle(inputContent);
                    Content.Content = MarkdownRenderers.Markdown2WebHTML(inputContent, settings);
                    Content.Metadata = string.Empty;

                    var html = Template.ProcessTemplate(Content);
                    target.WriteFile(html);
                }
            }
        }
    }
}
