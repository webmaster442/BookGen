//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Api.Configuration;
using BookGen.Core.Contracts;

namespace BookGen.Framework.Scripts
{
    internal class ScriptHost : IScriptHost
    {
        public string SourceDirectory { get; }

        public IReadOnlyConfig Configuration { get; }

        public IReadOnlyBuildConfig CurrentBuildConfig { get; }

        public ILog Log { get; }

        public ScriptHost(IReadonlyRuntimeSettings runtimeSettings, ILog log)
        {
            SourceDirectory = runtimeSettings.SourceDirectory.ToString();
            Configuration = runtimeSettings.Configuration;
            CurrentBuildConfig = runtimeSettings.CurrentBuildConfig;
            Log = log;
        }
    }
}
