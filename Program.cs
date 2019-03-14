//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Contracts;
using BookGen.Framework;
using BookGen.UserInterface;
using BookGen.Utilities;
using System;
using System.Windows;

namespace BookGen
{
    internal static class Program
    {
        public static ILog Log { get; private set; }

        [STAThread]
        public static void Main(string[] args)
        {
            Log = new Logger();
            var argumentParser = new GeneratorRunner(args, Log);
            if (argumentParser.IsGuiMode)
            {
                NativeMethods.HideConsole();
                var window = new MainView();
                window.DataContext = new MainViewModel();
                window.ShowDialog();
            }
            else
            {
                NativeMethods.ShowConsole();
                argumentParser.RunGenerator();
                Log.Dispose();
            }
        }
    }
}
