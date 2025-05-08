//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel;
using System.Reflection;

using BookGen.CommandArguments;

namespace BookGen.Infrastructure;
internal static class HelpCallbacks
{
    public static string DocumentBuildActions()
    {
        var result = new StringBuilder(1024);
        DocumentActions<BuildAction>(result);
        return result.ToString();
    }

    private static void DocumentActions<T>(StringBuilder result) where T : Enum
    {
        Type actionType = typeof(T);

        foreach (string? action in Enum.GetNames(actionType).OrderBy(o => o))
        {
            result.Append("    ").AppendLine(action);
            MemberInfo? memberInfo = actionType.GetMember(action).FirstOrDefault();
            DescriptionAttribute? desc = memberInfo?.GetCustomAttribute<DescriptionAttribute>();
            if (desc != null)
            {
                result.Append("        ").AppendLine(desc.Description);
            }
            result.AppendLine();
        }
    }
}
