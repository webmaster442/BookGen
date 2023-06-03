using System;

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

                using (var process =
                    new ProcessBuilder()
                    .SetProgram(AppDomain.CurrentDomain.BaseDirectory, Constants.DataFolder, Constants.BookGenLauncher)
                    .SetWorkDir(AppDomain.CurrentDomain.BaseDirectory, Constants.DataFolder)
                    .SetArguments(args)
                    .VerifyPaths()
                    .Build())
                {
                    process.Start();
                }
            });
        }
    }
}