//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.Json.Serialization;

namespace Bookgen.Lib.Domain.IO.Legacy;

public sealed class Config
{
    [Required]
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

    public string ScriptsDirectory
    {
        get;
        set;
    }

    public string HostName
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

    public Metadata Metadata
    {
        get;
        set;
    }

    public BuildConfig TargetWeb
    {
        get;
        set;
    }

    public BuildConfig TargetPrint
    {
        get;
        set;
    }

    public BuildConfig TargetEpub
    {
        get;
        set;
    }

    public BuildConfig TargetWordpress
    {
        get;
        set;
    }

    public BuildConfig TargetPostProcess
    {
        get;
        set;
    }

    public Translations Translations
    {
        get;
        set;
    }

    [Required]
    public CultureInfo BookLanguage { get; set; }

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

        return config;
    }
}
