using BookGen.Api;
using System.Collections.Generic;

namespace Script
{
    public class Script1 : IScript
    {
        public string InvokeName => "Script1";

        public string ScriptMain(IScriptHost host, IReadOnlyDictionary<string, string> arguments)
        {
            host.Log.Detail("Executing Script1...");
            return string.Empty;
        }
    }
}
