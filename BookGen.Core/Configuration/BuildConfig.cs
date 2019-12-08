//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections.Generic;

namespace BookGen.Core.Configuration
{
    public sealed class BuildConfig
    {
        [Doc("Output directory, relative to work directory")]
        public string OutPutDirectory { get; set; }

        [Doc("HTML template path, relative to work directory. If not specified built in template is used.", true)]
        public string TemplateFile { get; set; }

        [Doc("List of assets required by the template", true)]
        public List<Asset> TemplateAssets { get; set; }

        [Doc("CSS classes that will be aplied to generated html elements")]
        public StyleClasses StyleClasses { get; set; }

        [Doc("Additional template options", true, TypeAlias = typeof(Dictionary<string, string>))]
        public TemplateOptions TemplateOptions { get; set; }

        public BuildConfig()
        {
            OutPutDirectory = "Path to output directory";
            TemplateFile = "";
            TemplateAssets = new List<Asset>
            {
                new Asset
                {
                    Source = "",
                    Target = ""
                }
            };
            TemplateOptions = TemplateOptions.CreateDefaultOptions();
            StyleClasses = new StyleClasses();
        }

        public static BuildConfig CreateDefault(string? outdir = null)
        {
            var config = new BuildConfig();

            if (!string.IsNullOrEmpty(outdir))
                config.OutPutDirectory = outdir;

            return config;
        }
    }
}
