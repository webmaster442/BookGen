//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api.Configuration;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BookGen.Core.Configuration
{
    public sealed class BuildConfig: IReadOnlyBuildConfig
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

        [Doc("Image processing options for this build configuration")]
        public ImageOptions ImageOptions { get; set; }

        [JsonIgnore]
        IReadOnlyList<IReadOnlyAsset> IReadOnlyBuildConfig.TemplateAssets => TemplateAssets;

        [JsonIgnore]
        IReadOnylStyleClasses IReadOnlyBuildConfig.StyleClasses => StyleClasses;

        [JsonIgnore]
        IReadOnlyTemplateOptions IReadOnlyBuildConfig.TemplateOptions => TemplateOptions;

        [JsonIgnore]
        IReadonlyImageOptions IReadOnlyBuildConfig.ImageOptions => ImageOptions;

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
            ImageOptions = new ImageOptions();
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
