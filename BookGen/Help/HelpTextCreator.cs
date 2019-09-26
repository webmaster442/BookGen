//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Core.Configuration;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BookGen.Help
{
    internal static class HelpTextCreator
    {
        public static string GenerateHelpText()
        {
            StringBuilder result = new StringBuilder(4096);

            result.AppendLine(ResourceLocator.GetResourceFile<GeneratorRunner>("Resources/Help.txt"));
            DocumentActions(result);
            DocumentConfiguration(result);

            return result.ToString();
        }

        private static void DocumentActions(StringBuilder result)
        {
            Type actionType = typeof(ParsedOptions.ActionType);

            foreach (var action in Enum.GetNames(actionType).OrderBy(o => o))
            {
                result.Append("    ").AppendLine(action);
                var memberInfo = actionType.GetMember(action);
                var desc = memberInfo[0].GetCustomAttribute<DescriptionAttribute>();
                result.Append("      ").AppendLine(desc.Description);
                result.AppendLine();
            }
        }

        private static void DocumentConfiguration(StringBuilder result)
        {
            result.AppendLine("Configuration properties for current version:");
            result.AppendLine();
            result.AppendLine();
            ClassDocumenter.DocumentType<Config>(out string properties, out string types);
            result.AppendLine(properties);
            result.AppendLine();
            result.AppendLine();
            result.AppendLine(types);
        }
    }
}
