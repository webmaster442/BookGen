//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Lib.Domain.IO;

using BookGen.Api.V1;

namespace BookGen.PluginInfrastructure.V1;

internal sealed class DocumentFrontMatter : IDocumentFrontMatter
{
    private DocumentFrontMatter(string title,
                               IReadOnlyList<string> tags,
                               string? template,
                               IReadOnlyDictionary<string, string> additionalData)
    {
        Title = title;
        Tags = tags;
        Template = template;
        AdditionalData = additionalData;
    }

    public string Title { get; }
    public IReadOnlyList<string> Tags { get; }
    public string? Template { get; }
    public IReadOnlyDictionary<string, string> AdditionalData { get; }

    internal static IDocumentFrontMatter CreateFrom(FrontMatter frontMatter)
    {
        return new DocumentFrontMatter(
            frontMatter.Title,
            frontMatter.TagArray,
            frontMatter.Template,
            frontMatter.Data ?? new Dictionary<string, string>());
    }
}
