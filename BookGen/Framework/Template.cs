//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Domain;
using BookGen.Utilities;

namespace BookGen.Framework
{
    internal class Template: 
    {
        private readonly string _content;



        public Template(string content)
        {
            _content = content;
        }

        public string Render()
        {
            return string.Empty;
        }
    }
}
