//-----------------------------------------------------------------------------
// (c) 2020-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.DomainServices;
using BookGen.Interfaces;
using System.ComponentModel.Composition;

namespace BookGen.Framework.Scripts
{
    [Export(typeof(ITemplateShortCode))]
    internal class Php : ScriptProcess, ITemplateShortCode
    {
        private readonly IReadonlyRuntimeSettings _settings;
        private readonly IAppSetting _appSetting;

        [ImportingConstructor]
        public Php(ILog log, IReadonlyRuntimeSettings settings, IAppSetting appSetting) : base(log)
        {
            _settings = settings;
            _appSetting = appSetting;
        }

        public string Tag => "Php";

        public bool CanCacheResult => false;

        public string Generate(IArguments arguments)
        {
            var file = arguments.GetArgumentOrThrow<string>("file");
            _log.Info("Trying to execute PHP CGI script: {0} ...", file);

            return ExecuteScriptProcess("php-cgi", _appSetting.PhpPath, file, _appSetting.PhpTimeout);
        }

        protected override void SerializeHostInfo(string scriptPath)
        {
            FsPath hostinfo = new FsPath(scriptPath, "hostinfo.php");
            ScriptHost host = new ScriptHost(_settings, _log);
            var content = JsonInliner.InlinePhp(nameof(host), host);
            hostinfo.WriteFile(_log, content);
        }
    }
}
