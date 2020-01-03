//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core.Contracts;
using System.Collections.Generic;

namespace BookGen.Tests.Environment
{
    public class TestScript : IScript
    {
        public string InvokeName => nameof(TestScript);

        public string ScriptMain(ILog log, IReadOnlyDictionary<string, string> arguments)
        {
            log.Detail("Executing test script");
            return "Hello, from test script!";
        }
    }
}
