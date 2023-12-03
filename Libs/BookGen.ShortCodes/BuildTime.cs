//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.ShortCodes;

[Export(typeof(ITemplateShortCode))]
[BuiltInShortCode]
public sealed class BuildTime : ITemplateShortCode
{
    private readonly TimeProvider _timeProvider;

    [ImportingConstructor]
    public BuildTime(TimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
    }

    public string Tag => nameof(BuildTime);

    public bool CanCacheResult => false;

    public ShortCodeInfo HelpInfo => new()
    {
        Description = "Inserts the current build time",
        ArgumentInfos = Array.Empty<ArgumentInfo>(),
    };

    public string Generate(IArguments arguments)
    {
        return _timeProvider.GetLocalNow().ToString("yy-MM-dd hh:mm:ss");
    }
}
