//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections.Generic;

namespace BookGen.Core.Contracts.Configuration
{
    public interface IReadOnlyBuildConfig
    {
        string OutPutDirectory { get; }
        string TemplateFile { get; }
        IReadOnlyList<IReadOnlyAsset> TemplateAssets { get; }
        IReadOnylStyleClasses StyleClasses { get; }
        IReadOnlyTemplateOptions TemplateOptions { get; }
    }
}
