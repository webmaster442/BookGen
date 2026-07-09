//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Lib.Rendering.Templates;

public sealed class TemplateEngineOptions
{
    public bool EmitNullString { get; set; } = true;
    public TimeProvider TimeProvider { get; set; } = TimeProvider.System;
    public StringComparer PropertyNameComparer { get; set; } = StringComparer.Ordinal;
}
