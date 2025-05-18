using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using Bookgen.Lib.Domain.Validation;

namespace Bookgen.Lib.Domain.IO.Configuration;

public sealed class StaticWebsiteConfig : OutputConfig
{
    [Description("When enabled, links that point outside of host are opened in a new tab")]
    [Required]
    public bool OpenLinksOutsideHostOnNewTab { get; init; }

    [Description("Deploy host name")]
    [ValidUrl(EndsWithSlash = true)]
    [NotNullOrWhiteSpace]
    public string DeployHost { get; set; }

    [Description("Files to copy to the output directory")]
    [FileExists]
    public List<string> CopyToOutput { get; init; }

    public StaticWebsiteConfig()
    {
        DeployHost = string.Empty;
        CssClasses = new CssClasses();
        Images = new ImageConfig();
        OpenLinksOutsideHostOnNewTab = false;
        CopyToOutput = new List<string>();
    }
}
