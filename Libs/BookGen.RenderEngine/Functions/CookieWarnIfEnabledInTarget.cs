﻿//-----------------------------------------------------------------------------
// (c) 2019-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain.Configuration;
using BookGen.Interfaces;
using BookGen.Interfaces.Configuration;
using BookGen.RenderEngine.Internals;
using BookGen.Resources;

using Microsoft.Extensions.Logging;

namespace BookGen.RenderEngine.Functions;

internal sealed class CookieWarnIfEnabledInTarget : Function, IInjectable
{
    private ILogger _log = null!;
    private IReadonlyRuntimeSettings _settings = null!;

    public override string Execute(FunctionArguments arguments)
    {
        IReadOnlyBuildConfig? currentconfig = _settings.CurrentBuildConfig;

        if (currentconfig.TemplateOptions.TryGetOption(TemplateOptions.CookieDisplayBannerEnabled, out bool value) && value)
        {
            _log.LogDebug("Cookies enabled for current target. Generating Code...");
            return ResourceHandler.GetFile(KnownFile.CookieWarningHtml);
        }
        else
        {
            _log.LogDebug("Cookies not enalbed for current target.");
            return string.Empty;
        }
    }

    public void Inject(FunctionServices functionServices)
    {
        _log = functionServices.Log;
        _settings = functionServices.RuntimeSettings;
    }

    protected override FunctionInfo GetInformation()
    {
        return new FunctionInfo
        {
            Description = "Internal cookie banner",
            Name = "CookieWarnIfEnabledInTarget",
            ArgumentInfos = Array.Empty<Internals.ArgumentInfo>(),
        };
    }
}
