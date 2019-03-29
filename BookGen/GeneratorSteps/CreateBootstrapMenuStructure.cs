//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Contracts;
using BookGen.Core.Configuration;
using BookGen.Core.Contracts;
using BookGen.Domain;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BookGen.GeneratorSteps
{
    internal class CreateBootstrapMenuStructure : IGeneratorContentFillStep
    {
        private readonly List<HeaderMenuItem> _menuItems;

        public GeneratorContent Content { get; set; }

        public CreateBootstrapMenuStructure(List<HeaderMenuItem> items)
        {
            _menuItems = items;
        }

        public void RunStep(GeneratorSettings settings, ILog log)
        {
            log.Info("Creating menu for Additional pages...");
            StringBuilder buffer = new StringBuilder();

            foreach (var Menuitem in _menuItems)
            {
                if (Menuitem.HasChilds)
                    RenderSubmenus(settings, Menuitem, buffer);
                else
                    RenderSingleMenu(settings, Menuitem, buffer);
            }

            Content.AdditionalMenus = buffer.ToString();
        }

        private void RenderSingleMenu(GeneratorSettings settings, MenuItem menuitem, StringBuilder buffer)
        {
            buffer.AppendFormat("<li>{0}<a href=\"{1}\">{2}</a></li>\n", 
                                RenderFaIcon(menuitem.FontAwesomeIcon),
                                Relink(settings, menuitem.Link), 
                                menuitem.Title);
        }

        public string Relink(GeneratorSettings settings, string link)
        {
            if (link.StartsWith("http://")
                || link.StartsWith("https://")
                || link.StartsWith("ftp://")
                || link.StartsWith("#"))
            {
                return link;
            }

            var rewritten = Path.ChangeExtension(link, ".html").Replace("\\", "/");

            return $"{settings.Configruation.HostName}{rewritten}";

        }

        private string RenderFaIcon(string fontAwesomeIcon)
        {
            if (string.IsNullOrEmpty(fontAwesomeIcon))
                return string.Empty;

            return $"<i class=\"{fontAwesomeIcon}\"></i>";
        }

        private void RenderSubmenus(GeneratorSettings settings, HeaderMenuItem menuitem, StringBuilder buffer)
        {
            buffer.Append("<li class=\"dropdown\">\n");
            buffer.AppendFormat("{0}<a class=\"dropdown-toggle\" data-toggle=\"dropdown\" href=\"{1}\">{2}<span class=\"caret\"></span></a>\n",
                                RenderFaIcon(menuitem.FontAwesomeIcon), 
                                Relink(settings, menuitem.Link), 
                                menuitem.Title);


            buffer.Append("<ul class=\"dropdown-menu\">\n");
            foreach (var subutem in menuitem.SubItems)
            {
                RenderSingleMenu(settings, subutem, buffer);
            }
            buffer.Append("</ul>\n");
            buffer.Append("</li>\n");
        }
    }
}
