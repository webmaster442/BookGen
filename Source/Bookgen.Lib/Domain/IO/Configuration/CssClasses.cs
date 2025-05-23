using System.ComponentModel;

namespace Bookgen.Lib.Domain.IO.Configuration;

public sealed class CssClasses
{
    [Description("css classes aplied to <h1> element")]
    public string H1 { get; init; } = string.Empty;

    [Description("css classes aplied to <h2> element")]
    public string H2 { get; init; } = string.Empty;

    [Description("css classes aplied to <h3> element")]
    public string H3 { get; init; } = string.Empty;

    [Description("css classes aplied to <img> element")]
    public string Img { get; init; } = string.Empty;

    [Description("css classes aplied to <table> element")]
    public string Table { get; init; } = string.Empty;

    [Description("css classes aplied to <bockquote> element")]
    public string Blockquote { get; init; } = string.Empty;

    [Description("css classes aplied to <figure> element")]
    public string Figure { get; init; } = string.Empty;

    [Description("css classes aplied to <figcaption> element")]
    public string FigureCaption { get; init; } = string.Empty;

    [Description("css classes aplied to <a> element")]
    public string Link { get; init; } = string.Empty;

    [Description("css classes aplied to <ol> element")]
    public string Ol { get; init; } = string.Empty;

    [Description("css classes aplied to <ul> element")]
    public string Ul { get; init; } = string.Empty;

    [Description("css classes aplied to <li> element")]
    public string Li { get; init; } = string.Empty;
}
