//-----------------------------------------------------------------------------
// (c) 2020-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Interfaces;

namespace BookGen.ShortCodes;

[Export(typeof(ITemplateShortCode))]
[BuiltInShortCode]
public sealed class NodeJs : ScriptProcess, ITemplateShortCode
{
    private readonly IAppSetting _appSetting;

    [ImportingConstructor]
    public NodeJs(ILog log, IAppSetting appSetting) : base(log)
    {
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
}
