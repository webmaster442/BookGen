using System.Reflection;

using BookGen.Gui;
using BookGen.ShortCodes;

namespace BookGen.Commands;

[CommandName("shortcodes")]
internal sealed class ShortCodesCommand : Command
{
    private readonly ILog _log;
    private readonly IAppSetting _appSetting;
    private readonly TimeProvider _timeProvider;

    public ShortCodesCommand(ILog log, IAppSetting appSetting, TimeProvider timeProvider)
    {
        _log = log;
        _appSetting = appSetting;
        _timeProvider = timeProvider;
    }

    public override int Execute(string[] context)
    {
        using (var loader = new ShortCodeLoader(_log, new RuntimeSettings(new DumyTags()), _appSetting, _timeProvider))
        {
            loader.LoadAll();
            List<string> help = new List<string>();
            foreach (var item in loader.Imports)
            {
                if (item.GetType().GetCustomAttribute<BuiltInShortCodeAttribute>()?.DisplayInHelp == false)
                    continue;

                help.AddRange(RenderShortCodeHelp(item));
            }
            HelpRenderer.RenderHelp(help);
        }

        return Constants.Succes;
    }

    private static IEnumerable<string> RenderShortCodeHelp(ITemplateShortCode item)
    {
        yield return $"# {item.Tag}";
        yield return $"  {item.HelpInfo.Description}";
        yield return "";

        if (item.HelpInfo.ArgumentInfos.Count > 0)
        {
            yield return "  Arguments:";
            foreach (var argumentinfo in item.HelpInfo.ArgumentInfos)
            {
                yield return $"    {argumentinfo.Name} ({Optionality(argumentinfo.Optional)})";
                yield return $"      {argumentinfo.Description}";
            }
            yield return "";
        }
    }

    private static string Optionality(bool optional)
        => optional ? "optional" : "required";

    private class DumyTags : ITagUtils
    {
        public int UniqueTagCount => 0;
        public int TotalTagCount => 0;
        public int FilesWithOutTags => 0;

        public ISet<string> GetTagsForFile(string file)
            => new HashSet<string>();

        public ISet<string> GetTagsForFiles(IEnumerable<string> files)
            => new HashSet<string>();

        public string GetUrlNiceName(string tag)
            => string.Empty;
    }
}
