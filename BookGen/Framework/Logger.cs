//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core.Contracts;
using System;
using System.IO;

namespace BookGen.Framework
{
    internal sealed class Logger : ILog
    {
        private StreamWriter _fileout;

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

            if (logLevel != LogLevel.Detail)
                Console.WriteLine("{0}: {1}", logLevel, text);

            _fileout?.WriteLine(line);
        }

        public void ConfigureFile(string path)
        {
            try
            {
                _fileout = File.CreateText(path);
            }
            catch (IOException)
            {
                _fileout = null;
                Console.WriteLine("Coudn't create log file: {0}", path);
            }
        }

        public void Dispose()
        {
            if (_fileout != null)
            {
                _fileout.Dispose();
                _fileout = null;
            }
        }
    }
}
