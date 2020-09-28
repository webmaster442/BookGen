//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Core.Configuration;
using BookGen.Domain.ArgumentParsing;
using BookGen.Framework;
using BookGen.Resources;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BookGen.Utilities
{
    internal static class HelpUtils
    {
        public static string GetGeneralHelp()
        {
            return ResourceHandler.GetResourceFile<GeneratorRunner>("Resources/Help.General.txt");
        }

        public static string GetHelpForModule(string moduleClass)
        {
            return ResourceHandler.GetResourceFile<GeneratorRunner>($"Resources/Help.{moduleClass}.txt");
        }

        public static void DocumentActions<T>(StringBuilder result) where T: Enum
        {
            Type actionType = typeof(T);

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
