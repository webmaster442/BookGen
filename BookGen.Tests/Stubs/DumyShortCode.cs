using BookGen.Core.Contracts;
using System.Collections.Generic;

namespace BookGen.Tests.Stubs
{
    public class DumyShortCode : ITemplateShortCode
    {
        public string Tag => "Dumy";

        public string Generate(IReadOnlyDictionary<string, string> arguments)
        {
            return "Genrated";
        }
    }
}
