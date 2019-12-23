//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Core.Configuration;
using BookGen.Domain.ArgumentParsing;
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

            return result.ToString();
        }

        private static void DocumentActions(StringBuilder result)
        {
            Type actionType = typeof(ActionType);

            foreach (var action in Enum.GetNames(actionType).OrderBy(o => o))
            {
                result.Append("    ").AppendLine(action);
                var memberInfo = actionType.GetMember(action).FirstOrDefault();
                var desc = memberInfo?.GetCustomAttribute<DescriptionAttribute>();
                if (desc != null)
                {
                    result.Append("        ").AppendLine(desc.Description);
                }
                result.AppendLine();
            }
        }

        public static string DocumentConfiguration()
        {
            StringBuilder result = new StringBuilder(4096);
            result.AppendLine("Configuration properties for current version:");
            result.AppendLine();
            result.AppendLine();
            ClassDocumenter.DocumentType<Config>(out string properties, out string types);
            result.AppendLine(properties);
            result.AppendLine();
            result.AppendLine();
            result.AppendLine(types);

            return result.ToString();
        }
    }
}
