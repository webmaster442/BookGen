//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain;
using BookGen.Utilities;

namespace BookGen.Framework
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
            return _template.ReplaceTags(contents);
        }
    }
}
