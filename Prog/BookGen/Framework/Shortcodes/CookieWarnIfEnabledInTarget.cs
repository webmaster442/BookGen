using System.ComponentModel.Composition;
///-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain.Configuration;
using BookGen.Resources;

namespace BookGen.Framework.Shortcodes;

[Export(typeof(ITemplateShortCode))]
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
        Api.Configuration.IReadOnlyBuildConfig? currentconfig = _settings.CurrentBuildConfig;

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
