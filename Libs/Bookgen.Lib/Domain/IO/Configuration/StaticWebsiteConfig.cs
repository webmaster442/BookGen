using System.ComponentModel.DataAnnotations;

using Bookgen.Lib.Domain.Validation;

namespace Bookgen.Lib.Domain.IO.Configuration;

public sealed class StaticWebsiteConfig : OutputConfig
{
    [Required]
    public bool OpenLinksOutsideHostOnNewTab { get; init; }

    [NotNullOrWhiteSpace]
    public string DeployHost { get; set; }

    [FileExists]
    public List<string> AssetsToCopy { get; init; }

    public StaticWebsiteConfig()
    {
        DeployHost = string.Empty;
        CssClasses = new CssClasses();
        Images = new ImageConfig();
        OpenLinksOutsideHostOnNewTab = false;
        AssetsToCopy = new List<string>();
    }
}
