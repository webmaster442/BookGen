using System.ComponentModel;

namespace Bookgen.Lib.Domain.IO.Configuration;

public sealed class MarkdownLintSettings
{
    [Description("Heading levels should only increment by one level at a time")]
    public bool Md001 { get; set; } = true;
}
