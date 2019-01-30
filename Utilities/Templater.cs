using BookGen.Domain;
using System.Collections.Generic;
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

        public string ProcessTemplate(Dictionary<string, string> contents)
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
