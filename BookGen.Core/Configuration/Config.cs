//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api.Configuration;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BookGen.Core.Configuration
{
    public sealed class Config : IReadOnlyConfig
    {
        [Doc("Table of contents Markdown file")]
        public string TOCFile
        {
            get;
            set;
        }

        [Doc("Image directory relative to workdir")]
        public string ImageDir
        {
            get;
            set;
        }

        [Doc("Scripts directory. Scipts can extend the functionality of the generator")]
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

        [Doc("Inline images as base64 that are less then this size in bytes. 0 = inlines all files")]
        public long InlineImageSizeLimit
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

        [Doc("Translateable strings that can be used in the template", TypeAlias = typeof(Dictionary<string, string>))]
        public Translations Translations
        {
            get;
            set;
        }

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
        IReadOnlyTranslations IReadOnlyConfig.Translations => Translations;

        public Config()
        {
            Translations = new Translations();
            TargetWordpress = new BuildConfig();
            TargetEpub = new BuildConfig();
            TargetPrint = new BuildConfig();
            TargetWeb = new BuildConfig();
            Metadata = new Metadata();
            ImageDir = string.Empty;
            Index = string.Empty;
            TOCFile = string.Empty;
            HostName = string.Empty;
            ScriptsDirectory = string.Empty;
        }

        public static Config CreateDefault(int version = 100)
        {
            var config = new Config
            {
                TargetWeb = BuildConfig.CreateDefault("output/web"),
                TargetEpub = BuildConfig.CreateDefault("output/epub"),
                TargetPrint = BuildConfig.CreateDefault("output/print"),
                TargetWordpress = BuildConfig.CreateDefault("output/wordpress"),
                Translations = Translations.CreateDefault(),
                TOCFile = "Path of table of contents",
                Index = "Path of startup (index) file",
                ImageDir = "Path to images directory",
                HostName = "http://localhost:8080/",
                Metadata = Metadata.CreateDefault(),
                Version = version,
                LinksOutSideOfHostOpenNewTab = true,
                InlineImageSizeLimit = 50 * 1024
            };

            config.AddBootStrapClassesForWeb();
            config.AddWordpressSettings();

            return config;
        }
    }
}
