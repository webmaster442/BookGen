//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BookGen.Tests.SystemTests
{
    public class SystemTestLog : ILog
    {
        private List<string> _logContents;

        public SystemTestLog()
        {
            _logContents = new List<string>();
        }

        public void Log(LogLevel logLevel, string format, params object[] args)
        {
            if (_logContents.Count > 100)
            {
                _logContents = _logContents.TakeLast(10).ToList();
            }

            string text = string.Format(format, args);
            string line = string.Format("{0} | {1} | {2}", DateTime.Now.ToShortTimeString(), logLevel, text);

            _logContents.Add(line);

        }

        public string FullContent => string.Join("\n", _logContents);

        public LogLevel LogLevel { get; set; }

        public override string ToString()
        {
            return string.Join("\n", _logContents.TakeLast(10));
        }

        public void PrintLine(string str)
        {
            _logContents.Add(str);
        }

        public void PrintLine(object obj)
        {
            foreach (var property in obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (property != null)
                {
                    var value = property.GetValue(obj)?.ToString() ?? "null";
                    _logContents.Add($"{property.Name}: {value}");
                }
            }
        }
    }
}
