//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text.Json.Serialization;

using BookGen.Lib.Domain.IO;
using BookGen.Lib.Domain.IO.Configuration;
using BookGen.Lib.Domain.PostProcess;
using BookGen.Lib.Domain.VsCode;

namespace BookGen.Lib;

public sealed partial class SerializerContexts
{
    [JsonSerializable(typeof(TableOfContents))]
    public sealed partial class TableOfContentsContext : JsonSerializerContext;

    [JsonSerializable(typeof(Config))]
    public sealed partial class ConfigContext : JsonSerializerContext;
    
    [JsonSerializable(typeof(VsCodeTasks))]
    public sealed partial class VsCodeTaskContext : JsonSerializerContext;

    [JsonSerializable(typeof(RecommendedExtensions))]
    public sealed partial class RecommendedExtensionsContext : JsonSerializerContext;


    [JsonSerializable(typeof(Domain.IO.Legacy.Config))]
    public sealed partial class LegacyConfigContext : JsonSerializerContext;

    [JsonSerializable(typeof(Dictionary<string, string[]>))]
    public sealed partial class LegacyTagsContext : JsonSerializerContext;

    [JsonSerializable(typeof(PostProcessExport))]
    public sealed partial class PostProcessExportContext : JsonSerializerContext;
}
