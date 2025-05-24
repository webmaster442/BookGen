//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace Bookgen.Lib.Domain.IO.Legacy;

public sealed class BuildConfig
{
    public string OutPutDirectory { get; set; }

    public string TemplateFile { get; set; }

    public Asset[] TemplateAssets { get; set; }

    public StyleClasses StyleClasses { get; set; }

    public TemplateOptions TemplateOptions { get; set; }
    public ImageOptions ImageOptions { get; set; }

    public BuildConfig()
    {
        OutPutDirectory = "Path to output directory";
        TemplateFile = "";
        TemplateAssets =
        [
            new Asset
            {
                Source = "",
                Target = ""
            }
        ];
        TemplateOptions = TemplateOptions.CreateDefaultOptions();
        StyleClasses = new StyleClasses();
        ImageOptions = new ImageOptions();
    }

    public static BuildConfig CreateDefault(string outdir, long inlineSize)
    {
        var config = new BuildConfig();

        if (!string.IsNullOrEmpty(outdir))
            config.OutPutDirectory = outdir;

        config.ImageOptions.InlineImageSizeLimit = inlineSize;

        return config;
    }
}
