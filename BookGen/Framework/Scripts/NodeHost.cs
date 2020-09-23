//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Contracts;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace BookGen.Framework.Scripts
{
    internal class NodeHost
    {
        private readonly ILog _log;
        private readonly IAppSetting _appSettings;

        public NodeHost(ILog log, IAppSetting appSettings)
        {
            _log = log;
           _appSettings = appSettings;
        }

        internal static string EncodeForCmdLine(string arg)
        {
            var stringBuilder = new StringBuilder(arg.Length);
            int slashSequenceLength = 0;
            for (int i = 0; i < arg.Length; i++)
            {
                char currentChar = arg[i];

                if (currentChar == '\\')
                {
                    slashSequenceLength++;

                    // If the last character in the argument is \, it must be escaped, together with any \ that immediately preceed it.
                    // This prevents situations like: SomeExecutable.exe "SomeArg\", where the quote meant to demarcate the end of the
                    // argument gets escaped.
                    if (i == arg.Length - 1)
                    {
                        for (int j = 0; j < slashSequenceLength; j++)
                        {
                            stringBuilder.
                                Append('\\').
                                Append('\\');
                        }
                    }
                }
                else if (currentChar == '"')
                {
                    // Every \ or sequence of \ that preceed a " must be escaped.
                    for (int j = 0; j < slashSequenceLength; j++)
                    {
                        stringBuilder.
                            Append('\\').
                            Append('\\');
                    }
                    slashSequenceLength = 0;

                    stringBuilder.
                        Append('\\').
                        Append('"');
                }
                else
                {
                    for (int j = 0; j < slashSequenceLength; j++)
                    {
                        stringBuilder.Append('\\');
                    }
                    slashSequenceLength = 0;

                    stringBuilder.Append(currentChar);
                }
            }

            return stringBuilder.ToString();
        }


        public async Task<string> Evaluate(string script)
        {
            string program = ProcessInterop.AppendExecutableExtension("node");
            string? programPath = ProcessInterop.ResolveProgramFullPath(program, _appSettings.NodeJsPath);

            string output = "";

            using (var process = new Process())
            {
                process.StartInfo = new ProcessStartInfo
                {
                    CreateNoWindow = true,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    FileName = programPath,
                    Arguments = $"-e \"{EncodeForCmdLine(script)}\""
                };

                process.Start();
                output = await process.StandardOutput.ReadToEndAsync();
                process.WaitForExit(_appSettings.NodeJsTimeout * 1000);

            }
            return output;
        }
    }
}
