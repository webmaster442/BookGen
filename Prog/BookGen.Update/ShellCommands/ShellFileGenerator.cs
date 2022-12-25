//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Update.Infrastructure;
using System.Diagnostics;
using System.Text;

namespace BookGen.Update.ShellCommands
{
    internal sealed class ShellFileGenerator
    {
        private readonly List<IShellCommand> _commands;
        private bool _finished;
        private const string BashHeader = "#!/bin/bash";

        public ShellFileGenerator()
        {
            _finished = false;
            _commands = new List<IShellCommand>
            {
                new EchoCommand
                {
                    Message = "Finishing update. Waiting for Bookgen to be closed..."
                },
                new WaitCommand
                {
                    Seconds = 5
                }
            };
        }

        internal enum ShellType
        {
            Bash,
            Powershell
        }

        public void Finish(string version)
        {
            if (_finished)
                throw new InvalidOperationException($"{nameof(Finish)} has been called");

            _commands.Add(new EchoCommand
            {
                Message = $"Bookgen updated to version {version}"
            });
            _commands.Add(new DeleteScriptCommand());
            _finished = true;
        }

        public void AddFiles(IEnumerable<(string source, string destination)> files)
        {
            if (_finished)
                throw new InvalidOperationException($"{nameof(AddFiles)} can't be called after {nameof(Finish)} has been called");

            _commands.AddRange(files.Select(f => new MoveCommand
            {
                Source = f.source,
                Target = f.destination
            }));
        }

        public string Generate(ShellType type)
        {
            StringBuilder script = new(4096);

            if (type == ShellType.Bash)
            {
                script.Append(BashHeader);
                script.Append('\n');
                foreach (IShellCommand command in _commands)
                {
                    script.Append(command.ToBash());
                    script.Append('\n');
                }
                return script.ToString();
            }
            else if (type == ShellType.Powershell)
            {
                foreach (IShellCommand command in _commands)
                {
                    script.Append(command.ToPowerShell());
                    script.Append("\r\n");
                }
                return script.ToString();
            }
            throw new UnreachableException("Unknown enum value");
        }

    }
}
