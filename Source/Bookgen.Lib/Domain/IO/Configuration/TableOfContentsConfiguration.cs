using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bookgen.Lib.Domain.IO.Configuration;

public sealed class TableOfContentsConfiguration
{
    [Required]
    [Description("The ID of the container element for the table of contents.")]
    public string ContainerId { get; init; }

    [Description("The HTML element type for the container of the table of contents.")]
    public ContainerElement ContainerElement { get; init; }

    [Required]
    [Description("The CSS class for the container element of the table of contents.")]
    public string ContainerClass { get; init; }

    [Description("The HTML element type for the container of each chapter in the table of contents.")]
    public ContainerElement ChapterContainer { get; init; }

    [Description("The HTML element type for the items in the table of contents.")]
    public ItemContainer ItemContainer { get; init; }

    public TableOfContentsConfiguration()
    {
        ContainerId = "tableofcontents";
        ContainerElement = ContainerElement.Nav;
        ContainerClass = string.Empty;
        ChapterContainer = ContainerElement.Section;
        ItemContainer = ItemContainer.UnorderedList;
    }
}