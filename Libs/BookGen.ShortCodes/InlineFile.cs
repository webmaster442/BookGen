//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Framework.Shortcodes;

[Export(typeof(ITemplateShortCode))]
public sealed class InlineFile : ITemplateShortCode
{
    private readonly ILog _log;

    public string Tag => nameof(InlineFile);

    public bool CanCacheResult => true;

    [ImportingConstructor]
    public InlineFile(ILog log)
    {
        _log = log;
    }

    public string Generate(IArguments arguments)
    {
        string? name = arguments.GetArgumentOrThrow<string>("file");

        _log.Detail("Inlineing {0}...", name);

        return File.ReadAllText(name);
    }
}
