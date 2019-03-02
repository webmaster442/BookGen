//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;

namespace BookGen
{
    internal class ArgumentParser
    {
        private readonly string[] _args;

        public event EventHandler BuildWebsite;
        public event EventHandler BuildTestWebsite;
        public event EventHandler BuildPrintHtml;

        public ArgumentParser(string[] arguments)
        {
            _args = arguments;
        }

        public void ParseArguments()
        {
            if (_args.Length == 0 || HelpRequested()) PrintHelp();
            switch (_args[0])
            {
                case "build":
                    BuildWebsite?.Invoke(this, EventArgs.Empty);
                    break;
                case "test":
                    BuildTestWebsite?.Invoke(this, EventArgs.Empty);
                    break;
                case "print":
                    BuildPrintHtml?.Invoke(this, EventArgs.Empty);
                    break;
            }
        }

        private bool HelpRequested()
        {
            return (_args.Length > 0) && (string.Equals(_args[0], "help", StringComparison.OrdinalIgnoreCase) || _args[0] == "?");
        }

        private void PrintHelp()
        {
            Console.WriteLine("usage: bookgen.exe [command]");
            Console.WriteLine("Supported commands:");
            Console.WriteLine("  build - Builds website");
            Console.WriteLine("  test - Builds a localy testable website & opens the test url in browser");
            Console.WriteLine("  print - Builds website optimized for printing");
        }
    }
}
