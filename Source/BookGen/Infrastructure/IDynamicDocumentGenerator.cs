//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Infrastructure;

internal interface IDynamicDocumentGenerator
{
    string GenerateSchemasDocument();
    string GenerateCommandsDocument();
}
