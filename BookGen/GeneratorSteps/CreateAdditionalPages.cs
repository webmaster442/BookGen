//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Contracts;
using BookGen.Core;
using BookGen.Core.Configuration;
using BookGen.Core.Contracts;
using BookGen.Domain;
using BookGen.Framework;
using BookGen.Utilities;
using System.Collections.Generic;
using System.IO;

namespace BookGen.GeneratorSteps
{
    internal class CreateAdditionalPages : ITemplatedStep
    {
        private readonly List<HeaderMenuItem> _menuItems;

        public Template Template { get; set; }
        public GeneratorContent Content { get; set; }

        public CreateAdditionalPages(List<HeaderMenuItem> menuItems)
        {
            _menuItems = menuItems;
        }

        public void RunStep(RuntimeSettings settings, ILog log)
        {
            log.Info("Creating additional pages...");
            foreach (var header in _menuItems)
            {
                Render(settings, header.Link, log);
                if (header.HasChilds)
                {
                    foreach (var subitem in header.SubItems)
                    {
                        Render(settings, subitem.Link, log);
                    }
                }
            }
        }

        private void Render(RuntimeSettings settings, string link, ILog log)
        {
            if (link.StartsWith("http://")
                || link.StartsWith("https://")
                || link.StartsWith("ftp://")
                || link.StartsWith("#"))
            {
                return;
            }

            var input = settings.SourceDirectory.Combine(link);

            log.Detail("processing markdown file: {0}", input);

            var output = settings.OutputDirectory.Combine(Path.ChangeExtension(link, ".html"));
            var inputContent = input.ReadFile();

            Content.Title = MarkdownUtils.GetTitle(inputContent);
            Content.Content = MarkdownUtils.Markdown2WebHTML(inputContent);
            var html = Template.ProcessTemplate(Content);
            output.WriteFile(html);
        }
    }
}
