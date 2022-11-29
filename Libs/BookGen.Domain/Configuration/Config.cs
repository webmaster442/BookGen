//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api.Configuration;
using System.Globalization;
using System.Text.Json.Serialization;

namespace BookGen.Domain.Configuration
{
    public sealed class Config : IReadOnlyConfig
    {
        [Doc("Table of contents Markdown file")]
        public string TOCFile
        {
            get;
            set;
        }

        [Doc("Image directory relative to workdir", IsOptional = true)]
        public string ImageDir
        {
            get;
            set;
        }

        [Doc("Scripts directory relative to workdir. Scipts can extend the functionality of the generator", IsOptional = true)]
        public string ScriptsDirectory
        {
            get;
            set;
        }

        [Doc("Publish host name. Must include protocoll (http or https) and must end with a /")]
        public string HostName
        {
            get;
            set;
        }

        [Doc("Index or first page.")]
        public string Index
        {
            get;
            set;
        }

        [Doc("Config file version. !DO NOT CHANGE!")]
        public int Version
        {
            get;
            set;
        }

        [Doc("If set to true, then links that point outside of the Hostme specifed will open in new tab")]
        public bool LinksOutSideOfHostOpenNewTab
        {
            get;
            set;
        }

        [Doc("Metadata information for output")]
        public Metadata Metadata
        {
            get;
            set;
        }

        [Doc("Web output configuration")]
        public BuildConfig TargetWeb
        {
            get;
            set;
        }

        [Doc("Printable HTML output configuration")]
        public BuildConfig TargetPrint
        {
            get;
            set;
        }

        [Doc("e-pub format output configuration")]
        public BuildConfig TargetEpub
        {
            get;
            set;
        }

        [Doc("Wordpress compatible xml output configuration")]
        public BuildConfig TargetWordpress
        {
            get;
            set;
        }

        [Doc("Postprocess html output configuration")]
        public BuildConfig TargetPostProcess
        {
            get;
            set;
        }

        [Doc("Translateable strings that can be used in the template", TypeAlias = typeof(Dictionary<string, string>))]
        public Translations Translations
        {
            get;
            set;
        }

        [Doc("Language of book represented in standard ISO 639-1 language code")]
        public CultureInfo BookLanguage { get; set; }

        [JsonIgnore]
        IReadOnlyMetadata IReadOnlyConfig.Metadata => Metadata;

        [JsonIgnore]
        IReadOnlyBuildConfig IReadOnlyConfig.TargetEpub => TargetEpub;

        [JsonIgnore]
        IReadOnlyBuildConfig IReadOnlyConfig.TargetPrint => TargetPrint;

        [JsonIgnore]
        IReadOnlyBuildConfig IReadOnlyConfig.TargetWeb => TargetWeb;

        [JsonIgnore]
        IReadOnlyBuildConfig IReadOnlyConfig.TargetWordpress => TargetWordpress;

        [JsonIgnore]
        IReadOnlyBuildConfig IReadOnlyConfig.TargetPostProcess => TargetPostProcess;

        [JsonIgnore]
        IReadOnlyTranslations IReadOnlyConfig.Translations => Translations;

        public Config()
        {
            TargetWeb = BuildConfig.CreateDefault("output/web", 64 * 1024);
            TargetEpub = BuildConfig.CreateDefault("output/epub", long.MaxValue);
            TargetPrint = BuildConfig.CreateDefault("output/print", 0);
            TargetWordpress = BuildConfig.CreateDefault("output/wordpress", long.MaxValue);
            TargetPostProcess = BuildConfig.CreateDefault("output/postproc", long.MaxValue);
            Translations = Translations.CreateDefault();
            Metadata = new Metadata();
            ImageDir = string.Empty;
            Index = string.Empty;
            TOCFile = string.Empty;
            HostName = string.Empty;
            ScriptsDirectory = string.Empty;
            BookLanguage = new CultureInfo("en-US");
        }

        public static Config CreateDefault(int version = 100)
        {
            var config = new Config
            {
                TOCFile = "Path of table of contents",
                Index = "Path of startup (index) file",
                ImageDir = "Path to images directory",
                HostName = "http://localhost:8080/",
                Version = version,
                LinksOutSideOfHostOpenNewTab = true,
            };

            config.AddBootStrapClassesForWeb();
            config.AddWordpressSettings();

            return config;
        }
    }
}
