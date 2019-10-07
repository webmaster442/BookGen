﻿//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
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

        public void Critical(string format, params object[] args)
        {
            Log(LogLevel.Critical, format, args);
        }

        public void Detail(string format, params object[] args)
        {
            Log(LogLevel.Detail, format, args);
        }

        public void Info(string format, params object[] args)
        {
            Log(LogLevel.Info, format, args);
        }

        public void Warning(string format, params object[] args)
        {
            Log(LogLevel.Warning, format, args);
        }

        public void Warning(Exception ex)
        {
            Log(LogLevel.Warning, "{0}", ex.Message);
        }

        public void Critical(Exception ex)
        {
            Log(LogLevel.Critical, "{0}\r\n{1}", ex.Message, ex.StackTrace);
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
