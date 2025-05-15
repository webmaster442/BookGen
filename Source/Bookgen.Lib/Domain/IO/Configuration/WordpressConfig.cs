using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using Bookgen.Lib.Domain.Validation;

namespace Bookgen.Lib.Domain.IO.Configuration;

public sealed class WordpressConfig : OutputConfig
{
    [Description("When enabled, links that point outside of host are opened in a new tab")]
    [Required]
    public bool OpenLinksOutsideHostOnNewTab { get; init; }

    [Description("Deploy host name")]
    [NotNullOrWhiteSpace]
    public string DeployHost { get; set; }

    [Description("Wordpress item type")]
    [NotNullOrWhiteSpace]
    public string ItemType { get; init; }

    public WordpressConfig()
    {
        CssClasses = new CssClasses();
        DeployHost = string.Empty;
        Images = new ImageConfig();
        ItemType = string.Empty;
        OpenLinksOutsideHostOnNewTab = true;
    }
}