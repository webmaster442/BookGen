namespace BookGen.Infrastructure;

internal interface IDynamicDocumentGenerator
{
    string GenerateSchemasDocument();
    string GenerateCommandsDocument();
}
