//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text;
using System.Text.Json;

namespace BookGen.Core
{
    public static class JsonInliner
    {
        public static string InlineJs<T>(string variableName, T obj) where T : class
        {
            var json = JsonSerializer.Serialize<T>(obj);
            return $"const {variableName} = JSON.parse('{json}');";
        }

        public static string InlinePhp<T>(string variableName, T obj) where T : class
        {
            var json = JsonSerializer.Serialize<T>(obj);
            var phpbuilder = new StringBuilder(4096);
            phpbuilder.AppendLine("<?php");
            phpbuilder.AppendFormat("${0} = json_decode(\"{1}\", false);", variableName, json);
            phpbuilder.AppendLine("?>");
            return phpbuilder.ToString();
        }


        public static string InlinePython<T>(string variableName, T obj) where T : class
        {
            var json = JsonSerializer.Serialize<T>(obj);
            var pythonBuilder = new StringBuilder(4096);
            pythonBuilder.AppendLine("import json");
            pythonBuilder.AppendLine("class Deserialize(object):");
            pythonBuilder.AppendLine("    def __init__(self, j):");
            pythonBuilder.AppendLine("          self.__dict__ = json.loads(j)");
            pythonBuilder.AppendFormat("{0} = Deserialize('{1}')\r\n", variableName, json);
            return pythonBuilder.ToString();
        }
    }
}
