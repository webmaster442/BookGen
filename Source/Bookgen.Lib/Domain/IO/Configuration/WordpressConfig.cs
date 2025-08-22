//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

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
    [ValidUrl(EndsWithSlash = true)]
    public string DeployHost { get; set; }

    [Description("Wordpress item type. Usually page, post or docs for WeDocs")]
    [NotNullOrWhiteSpace]
    public string ItemType { get; init; }

    [Description("Enable or disable comments")]
    public bool AllowComments { get; init; }

    [Description("Wordpress tag category name. By default: post_tag")]
    [NotNullOrWhiteSpace]
    public string TagCategory { get; init; }

    public WordpressConfig()
    {
        DeployHost = string.Empty;
        ItemType = "docs";
        OpenLinksOutsideHostOnNewTab = true;
        TagCategory = "post_tag";
    }
}