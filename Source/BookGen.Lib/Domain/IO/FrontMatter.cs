//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel;

using BookGen.Lib.Domain.Validation;

using YamlDotNet.Serialization;

namespace BookGen.Lib.Domain.IO;

public sealed class FrontMatter
{
    [YamlMember(Alias = "title")]
    [NotNullOrWhiteSpace]
    [Description("Document title")]
    public required string Title { get; init; }

    [YamlMember(Alias = "tags")]
    [NotNullOrWhiteSpace]
    [Description("A comma seperrated list of tags")]
    public required string Tags { get; init; }

    [YamlMember(Alias = "template")]
    [Description("Template file to use. If empty, default template is used")]
    public string? Template { get; init; }

    [YamlMember(Alias = "data")]
    [Description("Additional data that can be used during rendering")]
    public Dictionary<string, string>? Data { get; init; }

    [YamlIgnore]
    public string[] TagArray
        => Tags.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToArray();
}
