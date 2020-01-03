//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core.Contracts;
using System;
using System.Collections.Generic;

namespace BookGen.Editor.Infrastructure
{
    public interface IMemoryLog: ILog
    {
        List<string> Items { get; }
    }

    internal class MemoryLog : IMemoryLog
    {
        public List<string> Items { get; }

        public MemoryLog()
        {
            Items = new List<string>(200);
        }

        public void Log(LogLevel logLevel, string format, params object[] args)
        {
            string text = string.Format(format, args);
            string line = string.Format("{0} | {1} | {2}", DateTime.Now.ToShortTimeString(), logLevel, text);

            if (Items.Count > 200)
            {
                Items.RemoveRange(0, 100);
            }
            Items.Add(line);

        }
    }
}
