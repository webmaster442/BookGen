//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Editor.Infrastructure;
using BookGen.Editor.ServiceContracts;
using System;
using System.Diagnostics;
using System.IO;

namespace BookGen.Editor.Services
{
    public class BookGenServices : IBookGenServices
    {
        private const string EpubCommand = "BuildEpub";
        private const string WordpressCommand = "BuildWordpress";
        private const string PrintCommand = "BuildPrint";
        private const string TestCommand = "Test";
        private const string WebCommand = "BuildWeb";

        private const string CleanCommand = "Clean";
        private const string InitCommand = "Initialize";


        private void RunBookGen(string command)
        {
            using (var process = new Process())
            {
                process.StartInfo.FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BookGen.exe");
                process.StartInfo.Arguments = $"-d \"{EditorSessionManager.CurrentSession.DictionaryPath}\" -a {command}";
                process.StartInfo.UseShellExecute = false;
                process.Start();
            }
        }

        public void BuildEpub()
        {
            RunBookGen(EpubCommand);
        }

        public void BuildTestWebsite()
        {
            RunBookGen(TestCommand);
        }

        public void BuildWebsite()
        {
            RunBookGen(WebCommand);
        }

        public void BuildWordpress()
        {
            RunBookGen(WordpressCommand);
        }

        public void BuildPrint()
        {
            RunBookGen(PrintCommand);
        }

        public void Clean()
        {
            RunBookGen(CleanCommand);
        }

        public void Init()
        {
            RunBookGen(InitCommand);
        }
    }
}
