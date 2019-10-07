//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Core.Configuration
{
    public static class ConfigurationFactories
    {
        private static TemplateOptions CreateWordpressOptions()
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

        private static StyleClasses CreateBootstrapClasses()
        {
            return new StyleClasses()
            {
                Heading1 = string.Empty,
                Heading2 = string.Empty,
                Heading3 = string.Empty,
                Image = "img-fluid mx-auto rounded",
                Table = "table",
                Blockquote = "blockquote",
                Figure = "figure",
                FigureCaption = "figure-caption",
                Link = string.Empty,
                OrderedList = string.Empty,
                UnorederedList = string.Empty,
                ListItem = string.Empty,
            };
        }

        public static Config AddBootStrapClassesForWeb(this Config input)
        {
            input.TargetWeb.StyleClasses = CreateBootstrapClasses();
            return input;
        }

        public static Config AddWordpressSettings(this Config input)
        {
            input.TargetWordpress.TemplateOptions = CreateWordpressOptions();
            return input;
        }
    }
}
