//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Core.Configuration
{
    public class Config : ConfigurationBase
    {
        private string _OutputDir;
        private string _TOCFile;
        private string _ImageDir;
        private string _HostName;
        private string _AssetsDir;
        private string _Template;
        private string _EpubCss;
        private string _Index;
        private int _Version;
        private bool _LinksOutSideOfHostOpenNewTab;
        private long _InlineImageSizeLimit;
        private StyleClasses _StyleClasses;
        private SearchSettings _SearchOptions;
        private Metadata _Metadata;
        private Precompile _PrecompileHeader;

        public string OutputDir
        {
            get => _OutputDir;
            set => SetValue(ref _OutputDir, value);
        }

        public string TOCFile
        {
            get => _TOCFile;
            set => SetValue(ref _TOCFile, value);
        }

        public string ImageDir
        {
            get => _ImageDir;
            set => SetValue(ref _ImageDir, value);
        }

        public string HostName
        {
            get => _HostName;
            set => SetValue(ref _HostName, value);
        }

        public string AssetsDir
        {
            get => _AssetsDir;
            set => SetValue(ref _AssetsDir, value);
        }

        public string Template
        {
            get => _Template;
            set => SetValue(ref _Template, value);
        }

        public string EpubCss
        {
            get => _EpubCss;
            set => SetValue(ref _EpubCss, value);
        }

        public string Index
        {
            get => _Index;
            set => SetValue(ref _Index, value);
        }

        public int Version
        {
            get => _Version;
            set => SetValue(ref _Version, value);
        }

        public bool LinksOutSideOfHostOpenNewTab
        {
            get => _LinksOutSideOfHostOpenNewTab;
            set => SetValue(ref _LinksOutSideOfHostOpenNewTab, value);
        }

        public long InlineImageSizeLimit
        {
            get => _InlineImageSizeLimit;
            set => SetValue(ref _InlineImageSizeLimit, value);
        }

        public StyleClasses StyleClasses
        {
            get => _StyleClasses;
            set => SetValue(ref _StyleClasses, value);
        }

        public SearchSettings SearchOptions
        {
            get => _SearchOptions;
            set => SetValue(ref _SearchOptions, value);
        }

        public Metadata Metadata
        {
            get => _Metadata;
            set => SetValue(ref _Metadata, value);
        }

        public Precompile PrecompileHeader
        {
            get => _PrecompileHeader;
            set => SetValue(ref _PrecompileHeader, value);
        }

        public static Config CreateDefault()
        {
            return new Config
            {
                OutputDir = "Path to output directory",
                TOCFile = "Path of table of contents",
                Index = "Path of startup (index) file",
                ImageDir = "Path to images directory",
                HostName = "http://localhost:8080/",
                AssetsDir = "Path to static assets required by template or null",
                Template = "Path of template file",
                EpubCss = "Path to epub css file",
                StyleClasses = new StyleClasses(),
                SearchOptions = SearchSettings.CreateDefault(),
                Metadata = Metadata.CreateDefault(),
                PrecompileHeader = Precompile.CreateDefault(),
                Version = 100,
                LinksOutSideOfHostOpenNewTab = true,
                InlineImageSizeLimit = 50 * 1024
            };
        }
    }
}
