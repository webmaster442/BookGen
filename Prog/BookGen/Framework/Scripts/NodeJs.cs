//-----------------------------------------------------------------------------
// (c) 2020-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel.Composition;

namespace BookGen.Framework.Scripts;

[Export(typeof(ITemplateShortCode))]
internal class NodeJs : ScriptProcess, ITemplateShortCode
{
    private readonly IReadonlyRuntimeSettings _settings;
    private readonly IAppSetting _appSetting;

    [ImportingConstructor]
    public NodeJs(ILog log, IReadonlyRuntimeSettings settings, IAppSetting appSetting) : base(log)
    {
        _settings = settings;
        _appSetting = appSetting;
    }

    public string Tag => "NodeJs";

    public bool CanCacheResult => false;

    public string Generate(IArguments arguments)
    {
        string? file = arguments.GetArgumentOrThrow<string>("file");
        _log.Info("Trying to execute NoseJs script: {0} ...", file);

        return ExecuteScriptProcess("node", _appSetting.NodeJsPath, file, _appSetting.NodeJsTimeout);
    }

    protected override void SerializeHostInfo(string scriptPath)
    {
        var hostinfo = new FsPath(scriptPath, "hostinfo.js");
        var host = new ScriptHost(_settings, _log);
        string content = JsonInliner.InlineJs(nameof(host), host);
        hostinfo.WriteFile(_log, content);
    }
}
