using System.ComponentModel.DataAnnotations;

using Bookgen.Lib.Domain.Validation;

namespace Bookgen.Lib.Domain.IO.Configuration;

public sealed class WordpressConfig : OutputConfig
{
    [Required]
    public bool OpenLinksOutsideHostOnNewTab { get; init; }

    [NotNullOrWhiteSpace]
    public string DeployHost { get; set; }

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