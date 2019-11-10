//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Bookgen.Template;
using BookGen.Core;
using BookGen.Core.Markdown;
using System;

namespace Md2HTML
{
    public sealed class Program
    {
        private Program() { }

        public static void Main(string[] args)
        {
            ArgumentParser parser = new ArgumentParser(args);

            Arguments parsed = new Arguments(
                parser.GetSwitchWithValue("-i", "--input"),
                parser.GetSwitchWithValue("-o", "--output"),
                parser.GetSwitchWithValue("-c", "--css"));

            if (!parsed.InputFile.IsExisting)
            {
                Error("Input file doesn't exist");
                PrintHelp();
                return;
            }
            else if (parser.WasHelpRequested())
            {
                PrintHelp();
                return;
            }

            try
            {
                string md = parsed.InputFile.ReadFile();
                string rendered = BuiltInTemplates.Print.Replace("[content]", MarkdownRenderers.Markdown2Preview(md, parsed.InputFile.GetDirectory()));
                parsed.OutputFile.WriteFile(rendered);

            }
            catch (Exception ex)
            {
                Error(ex.Message);
            }

        }

        private static void PrintHelp()
        {
            string helpstr = ResourceLocator.GetResourceFile<Program>("Help/Md2Html.txt");
            Console.WriteLine(helpstr);
        }

        private static void Error(string error)
        {
            Console.WriteLine("Error:");
            Console.WriteLine("-------------------------------------");
            Console.WriteLine(error);
        }
    }
}
