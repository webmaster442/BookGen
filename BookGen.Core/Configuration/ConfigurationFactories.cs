//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Core.Configuration
{
    public static class ConfigurationFactories
    {
        public static TemplateOptions CreateWordpressOptions()
        {
            return new TemplateOptions
            {
                { TemplateOptions.WordpressTargetHost, "https://localhost/" },
                { TemplateOptions.WordpressAuthorDisplayName, "John Doe" },
                { TemplateOptions.WordpressAuthorLastName, "Doe" },
                { TemplateOptions.WordpressAuthorFirstName, "John" },
                { TemplateOptions.WordpressAuthorEmail, "john.doe@provider.com" },
                { TemplateOptions.WordpressAuthorLogin, "wploginuser" },
                { TemplateOptions.WordpressAuthorId, "1" },
            };
        }
    }
}
