//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using System;
using System.Text;
using System.Text.Json;

namespace BookGen.Core
{
    public static class JsonInliner
    {
        public static string InlineJs<T>(string variableName, T obj, ILog log) where T: class
        {
            try
            {
                var json = JsonSerializer.Serialize<T>(obj);
                return $"const {variableName} = JSON.parse('{json}');";

            }
            catch (Exception ex)
            {
                log.Warning("InlineJs failed. type: {1}", typeof(T));
                log.Detail(ex.Message);
                return string.Empty;
            }
        }

        public static string InlinePython<T>(string variableName, T obj, ILog log) where T : class
        {
            try
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
            catch (Exception ex)
            {
                log.Warning("InlinePython failed. type: {1}", typeof(T));
                log.Detail(ex.Message);
                return string.Empty;
            }
        }
    }
}
