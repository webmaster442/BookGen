//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BookGen.Core.Configuration
{
    [Serializable]
    public sealed class TemplateOptions : Dictionary<string, string>
    {
        public const string CookieDisplayBannerEnabled = nameof(CookieDisplayBannerEnabled);
        public const string WordpressTargetHost = nameof(WordpressTargetHost);
        public const string WordpressAuthorDisplayName = nameof(WordpressAuthorDisplayName);
        public const string WordpressAuthorLastName = nameof(WordpressAuthorLastName);
        public const string WordpressAuthorFirstName = nameof(WordpressAuthorFirstName);
        public const string WordpressAuthorEmail = nameof(WordpressAuthorEmail);
        public const string WordpressAuthorLogin = nameof(WordpressAuthorLogin);
        public const string WordpressAuthorId = nameof(WordpressAuthorId);

        public TemplateOptions() : base()
        {
        }

        public TemplateOptions(IDictionary<string, string> dictionary) : base(dictionary)
        {
        }

        public TemplateOptions(IDictionary<string, string> dictionary, IEqualityComparer<string> comparer) : base(dictionary, comparer)
        {
        }

        public TemplateOptions(IEnumerable<KeyValuePair<string, string>> collection) : base(collection)
        {
        }

        public TemplateOptions(IEnumerable<KeyValuePair<string, string>> collection, IEqualityComparer<string> comparer) : base(collection, comparer)
        {
        }

        public TemplateOptions(IEqualityComparer<string> comparer) : base(comparer)
        {
        }

        public TemplateOptions(int capacity) : base(capacity)
        {
        }

        public TemplateOptions(int capacity, IEqualityComparer<string> comparer) : base(capacity, comparer)
        {
        }

        private TemplateOptions(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

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
