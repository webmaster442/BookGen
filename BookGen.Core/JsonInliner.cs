//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using System;
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
    }
}
