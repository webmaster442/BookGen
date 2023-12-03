///-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain.Configuration;
using BookGen.Interfaces;
using BookGen.Interfaces.Configuration;
using BookGen.Resources;

namespace BookGen.ShortCodes;

[Export(typeof(ITemplateShortCode))]
[BuiltInShortCode]
public sealed class CookieWarnIfEnabledInTarget : ITemplateShortCode
{
    private readonly ILog _log;
    private readonly IReadonlyRuntimeSettings _settings;

    public string Tag => nameof(CookieWarnIfEnabledInTarget);

    public bool CanCacheResult => true;

    [ImportingConstructor]
    public CookieWarnIfEnabledInTarget(ILog log, IReadonlyRuntimeSettings settings)
    {
        _log = log;
        _settings = settings;
    }

    public string Generate(IArguments arguments)
    {
        IReadOnlyBuildConfig? currentconfig = _settings.CurrentBuildConfig;

        if (currentconfig.TemplateOptions.TryGetOption(TemplateOptions.CookieDisplayBannerEnabled, out bool value) && value)
        {
            _log.Detail("Cookies enabled for current target. Generating Code...");
            return ResourceHandler.GetFile(KnownFile.CookieWarningHtml);
        }
        else
        {
            _log.Detail("Cookies not enalbed for current target.");
            return string.Empty;
        }
    }
}
