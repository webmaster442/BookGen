//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core.Contracts;
using System;
using System.Text;

namespace BookGen.Framework
{
    public class EventedLogger : ILog
    {
        private StringBuilder _builder;

        public event EventHandler LogWritten;

        public string LogText
        {
            get { return _builder.ToString(); }
        }

        public int Lines
        {
            get;
            private set;
        }

        public EventedLogger()
        {
            _builder = new StringBuilder();
            Lines = 0;
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
            string line = string.Format("{0}|{1}|{2}", DateTime.Now, logLevel, text);

            _builder.Append(line);
            Lines++;
            LogWritten?.Invoke(this, EventArgs.Empty);
        }
    }
}
