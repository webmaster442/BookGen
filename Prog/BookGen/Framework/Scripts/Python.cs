//-----------------------------------------------------------------------------
// (c) 2020-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Interfaces;
using System.ComponentModel.Composition;

namespace BookGen.Framework.Scripts
{
    [Export(typeof(ITemplateShortCode))]
    internal class Python : ScriptProcess, ITemplateShortCode
    {
        private readonly IReadonlyRuntimeSettings _settings;
        private readonly IAppSetting _appSetting;

        [ImportingConstructor]
        public Python(ILog log, IReadonlyRuntimeSettings settings, IAppSetting appSetting) : base(log)
        {
            _settings = settings;
            _appSetting = appSetting;
        }

        public string Tag => "Python";

        public bool CanCacheResult => false;

        public string Generate(IArguments arguments)
        {
            string? file = arguments.GetArgumentOrThrow<string>("file");
            _log.Info("Trying to execute Python script: {0} ...", file);

            return ExecuteScriptProcess("python", _appSetting.PythonPath, file, _appSetting.PythonTimeout);
        }

        protected override void SerializeHostInfo(string scriptPath)
        {
            var hostinfo = new FsPath(scriptPath, "hostinfo.php");
            var host = new ScriptHost(_settings, _log);
            string? content = JsonInliner.InlinePython(nameof(host), host);
            hostinfo.WriteFile(_log, content);
        }
    }
}
