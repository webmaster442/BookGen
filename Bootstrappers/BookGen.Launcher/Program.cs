using System;
//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Bookgen.Win;

namespace BookGen.Launcher
{
    public static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {
            ExceptionHandler.Try(() =>
            {
                InstallVerify.ThrowIfNotExist();
                using (var process =
                    new ProcessBuilder()
                    .SetProgram(AppDomain.CurrentDomain.BaseDirectory, Constants.DataFolder, Constants.BookGen)
                    .SetWorkDir(AppDomain.CurrentDomain.BaseDirectory, Constants.DataFolder)
                    .SetArguments(args)
                    .Build())
                {
                    process.Start();
                }
            });
        }
    }
}