using BookGen.Interfaces;

namespace BookGen.ShortCodes;

[Export(typeof(ITemplateShortCode))]
[BuiltInShortCode]
public sealed class Php : ScriptProcess, ITemplateShortCode
{
    private readonly IAppSetting _appSetting;

    [ImportingConstructor]
    public Php(ILog log, IAppSetting appSetting) : base(log)
    {
        _appSetting = appSetting;
    }

    public string Tag => "Php";

    public bool CanCacheResult => false;

    public string Generate(IArguments arguments)
    {
        string? file = arguments.GetArgumentOrThrow<string>("file");
        _log.Info("Trying to execute PHP CGI script: {0} ...", file);

        return ExecuteScriptProcess("php-cgi", _appSetting.PhpPath, file, _appSetting.PhpTimeout);
    }
}