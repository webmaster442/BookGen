using Bookgen.Lib.Domain.IO;

namespace Bookgen.Lib.Domain;

public sealed class SourceFile
{
    public required FrontMatter FrontMatter { get; init; }
    public required string Content { get; init; }
    public required DateTime LastModified { get; init; }
    public required string FileNameInToc { get; init; }
}
