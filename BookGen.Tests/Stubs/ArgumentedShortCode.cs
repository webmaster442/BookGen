using BookGen.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookGen.Tests.Stubs
{
    class ArgumentedShortCode : ITemplateShortCode
    {
        public string Tag => "Arguments";

        public string Generate(IReadOnlyDictionary<string, string> arguments)
        {
            return arguments["parameter"];
        }
    }
}
