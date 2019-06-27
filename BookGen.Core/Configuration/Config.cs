//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections.ObjectModel;

namespace BookGen.Core.Configuration
{
    public class Config
    {
        public string OutputDir
        {
            get;
            set;
        }

        public string TOCFile
        {
            get;
            set;
        }

        public string ImageDir
        {
            get;
            set;
        }

        public string HostName
        {
            get;
            set;
        }

        public ObservableCollection<Asset> Assets
        {
            get;
            set;
        }

        public string Template
        {
            get;
            set;
        }

        public string EpubCss
        {
            get;
            set;
        }

        public string Index
        {
            get;
            set;
        }

        public int Version
        {
            get;
            set;
        }

        public bool LinksOutSideOfHostOpenNewTab
        {
            get;
            set;
        }

        public long InlineImageSizeLimit
        {
            get;
            set;
        }

        public StyleClasses StyleClasses
        {
            get;
            set;
        }

        public SearchSettings SearchOptions
        {
            get;
            set;
        }

        public Metadata Metadata
        {
            get;
            set;
        }

        public Precompile PrecompileHeader
        {
            get;
            set;
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
                Assets = new ObservableCollection<Asset>(),
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
