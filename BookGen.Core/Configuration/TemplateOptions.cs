﻿//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace BookGen.Core.Configuration
{
    public class TemplateOptions : Dictionary<string, string>
    {
        public const string CookieDisplayBannerEnabled = nameof(CookieDisplayBannerEnabled);
        public const string WordpressTargetHost = nameof(WordpressTargetHost);
        public const string WordpressAuthorDisplayName = nameof(WordpressAuthorDisplayName);
        public const string WordpressAuthorLastName = nameof(WordpressAuthorLastName);
        public const string WordpressAuthorFirstName = nameof(WordpressAuthorFirstName);
        public const string WordpressAuthorEmail = nameof(WordpressAuthorEmail);
        public const string WordpressAuthorLogin = nameof(WordpressAuthorLogin);
        public const string WordpressAuthorId = nameof(WordpressAuthorId);

        internal static TemplateOptions CreateDefaultOptions()
        {
            return new TemplateOptions
            {
                { CookieDisplayBannerEnabled, "true" }
            };
        }

        public bool TryGetOption<T>(string setting, out T value)
        {
            if (!ContainsKey(setting))
            {
                value = default(T);
                return false;
            }

            try
            {
                value = (T)Convert.ChangeType(this[setting], typeof(T));
                return true;
            }
            catch(Exception)
            {
                value = default(T);
                return false;
            }

        }
    }
}