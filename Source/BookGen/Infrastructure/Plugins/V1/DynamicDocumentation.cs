using BookGen.Api.V1;

namespace BookGen.Infrastructure.Plugins.V1;

internal sealed class DynamicDocumentation(IDynamicDocumentGenerator dynamicDocumentGenerator) : IDynamicDocumentation
{
    public string GetCommandsMarkdown()
        => dynamicDocumentGenerator.GenerateCommandsDocument();

    public string GetSchemasMarkdown()
        => dynamicDocumentGenerator.GenerateSchemasDocument();
}
