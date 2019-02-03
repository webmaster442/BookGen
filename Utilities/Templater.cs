//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain;
using System.Text;

namespace BookGen.Utilities
{
    internal class Template
    {
        private string _template;

        public Template(FsPath templatePath)
        {
            _template = templatePath.ReadFile();
        }

        public string ProcessTemplate(GeneratorContent contents)
        {
            StringBuilder result = new StringBuilder(_template);
            foreach (var content in contents)
            {
                result.Replace($"[[{content.Key}]]", content.Value);
            }
            return result.ToString();
        }
    }
}
