//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api.Configuration;
using System.Runtime.Serialization;

namespace BookGen.Domain.Configuration
{
    [Serializable]
    public sealed class TemplateOptions : Dictionary<string, string>, IReadOnlyTemplateOptions
    {
        public const string CookieDisplayBannerEnabled = nameof(CookieDisplayBannerEnabled);
        public const string WordpressTargetHost = nameof(WordpressTargetHost);
        public const string WordpressAuthorDisplayName = nameof(WordpressAuthorDisplayName);
        public const string WordpressAuthorLastName = nameof(WordpressAuthorLastName);
        public const string WordpressAuthorFirstName = nameof(WordpressAuthorFirstName);
        public const string WordpressAuthorEmail = nameof(WordpressAuthorEmail);
        public const string WordpressAuthorLogin = nameof(WordpressAuthorLogin);
        public const string WordpressAuthorId = nameof(WordpressAuthorId);
        public const string WordpressItemType = nameof(WordpressItemType);
        public const string WordpressCreateParent = nameof(WordpressCreateParent);
        public const string WordpresCreateParentTitle = nameof(WordpresCreateParentTitle);
        public const string WordpressCreateFillerPages = nameof(WordpressCreateFillerPages);
        public const string WordpressCommentStatus = nameof(WordpressCommentStatus);
        public const string WordpressSkipPageTitle = nameof(WordpressSkipPageTitle);
        public const string WordpressTagCategory = nameof(WordpressTagCategory);

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

        internal static TemplateOptions CreateDefaultOptions()
        {
            return new TemplateOptions
            {
                { CookieDisplayBannerEnabled, "true" }
            };
        }

        public bool TryGetOption<T>(string setting, out T value) where T : struct
        {
            if (!ContainsKey(setting))
            {
                value = default;
                return false;
            }

            try
            {
                value = (T)Convert.ChangeType(this[setting], typeof(T));
                return true;
            }
            catch (Exception)
            {
                value = default;
                return false;
            }

        }
    }
}
