namespace Bookgen.Lib.Domain.IO.Configuration;

public sealed class FeedConfig : OutputConfig
{
    public FeedConfig()
    {
        Images = new ImageConfig();
        CssClasses = new CssClasses();
        PreRenderCode = true;
        DefaultTempate = BundledAssets.TemplateBlank;
    }
}
