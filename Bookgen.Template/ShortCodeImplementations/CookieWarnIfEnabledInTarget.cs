﻿using BookGen.Api;
///-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core.Configuration;
using BookGen.Core.Contracts;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace Bookgen.Template.ShortCodeImplementations
{
    [Export(typeof(ITemplateShortCode))]
    public class CookieWarnIfEnabledInTarget : ITemplateShortCode
    {
        private readonly ILog _log;
        private readonly IReadonlyRuntimeSettings _settings;

        public string Tag => nameof(CookieWarnIfEnabledInTarget);

        [ImportingConstructor]
        public CookieWarnIfEnabledInTarget(ILog log, IReadonlyRuntimeSettings settings)
        {
            _log = log;
            _settings = settings;
        }

        public string Generate(IReadOnlyDictionary<string, string> arguments)
        {
            var currentconfig = _settings.CurrentBuildConfig;

            if (currentconfig.TemplateOptions.TryGetOption(TemplateOptions.CookieDisplayBannerEnabled, out bool value) && value)
            {
                _log.Detail("Cookies enabled for current target. Generating Code...");
                return BuiltInTemplates.CookieWarningCode;
            }
            else
            {
                _log.Detail("Cookies not enalbed for current target.");
                return string.Empty;
            }

        }
    }
}
