//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.Linq;

namespace BookGen
{
    internal class ArgumentParser
    {
        private readonly string[] _args;

        public event EventHandler BuildWebsite;
        public event EventHandler BuildTestWebsite;
        public event EventHandler BuildPrintHtml;
        public event EventHandler CreateMenuJson;

        public ArgumentParser(string[] arguments)
        {
            _args = arguments;
        }

        public bool IsLogEnabled
        {
            get
            {
                return _args.Any(a => a == "--log" || a == "-l");
            }
        }

        public void RunArgumentSteps()
        {
            if (_args.Length == 0 || HelpRequested()) PrintHelp();
            foreach (var arg in _args)
            {
                switch (arg)
                {
                    case "createmenu":
                        CreateMenuJson?.Invoke(this, EventArgs.Empty);
                        return;
                    case "build":
                        BuildWebsite?.Invoke(this, EventArgs.Empty);
                        return;
                    case "test":
                        BuildTestWebsite?.Invoke(this, EventArgs.Empty);
                        return;
                    case "print":
                        BuildPrintHtml?.Invoke(this, EventArgs.Empty);
                        return;
                    default:
                        Console.WriteLine("Uable to process arguments.");
                        PrintHelp();
                        return;
                }
            }
        }

        private bool HelpRequested()
        {
            return _args.Length > 0 && _args.Any(a => a == "help" || a == "?");
        }

        private void PrintHelp()
        {
            Console.WriteLine("usage: bookgen.exe [command] [--log]");
            Console.WriteLine("Supported commands:");
            Console.WriteLine("  createmenu - Create menus.json file");
            Console.WriteLine("  build - Builds website");
            Console.WriteLine("  test - Builds a localy testable website & opens the test url in browser");
            Console.WriteLine("  print - Builds website optimized for printing");
            Console.WriteLine("  --log - enable logging");
        }
    }
}
