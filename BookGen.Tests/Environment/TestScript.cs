//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using System.Collections.Generic;

namespace BookGen.Tests.Environment
{
    public class TestScript : IScript
    {
        public string InvokeName => nameof(TestScript);

        public string ScriptMain(IScriptHost host, IReadOnlyDictionary<string, string> arguments)
        {
            host.Log.Detail("Executing test script");
            return "Hello, from test script!";
        }
    }
}
