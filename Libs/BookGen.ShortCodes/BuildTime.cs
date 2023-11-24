//-----------------------------------------------------------------------------
// (c) 2019-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.ShortCodes;

[Export(typeof(ITemplateShortCode))]
[BuiltInShortCode]
public sealed class BuildTime : ITemplateShortCode
{
    public string Tag => nameof(BuildTime);

    public bool CanCacheResult => false;

    public string Generate(IArguments arguments)
    {
        return DateTime.Now.ToString("yy-MM-dd hh:mm:ss");
    }
}
