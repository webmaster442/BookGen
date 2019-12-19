//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Domain.ArgumentParsing;
using System;
using System.Linq;

namespace BookGen
{
    internal class ProgramArgumentHandler
    {
        private readonly ArgumentParser _arguments;

        public event EventHandler? DoHelp;
        public event EventHandler? DoConfigHelp;
        public event EventHandler<GuiParameters>? DoGui;
        public event EventHandler<BuildParameters>? DoBuild;
        public event EventHandler<UpdateParameters>? DoUpdate;

        public ProgramArgumentHandler(string[] args)
        {
            _arguments = new ArgumentParser(args);
        }

        private bool GetBuildParameters(out BuildParameters buildParameters)
        {
            buildParameters = new BuildParameters
            {
                NoWaitForExit = _arguments.GetSwitch("n", "nowait"),
                Verbose = _arguments.GetSwitch("v", "verbose")
            };


            var dir = _arguments.GetSwitchWithValue("d", "dir");

            if (!string.IsNullOrEmpty(dir))
                buildParameters.WorkDir = dir;

            var action = _arguments.GetSwitchWithValue("a", "action");

            bool result = Enum.TryParse(action, true, out ActionType parsedAction);

            buildParameters.Action = parsedAction;
            return result;

        }

        private GuiParameters GetGuiParameters()
        {
            var guiParams = new GuiParameters
            {
                Verbose = _arguments.GetSwitch("v", "verbose")
            };

            var dir = _arguments.GetSwitchWithValue("d", "dir");

            if (!string.IsNullOrEmpty(dir))
                guiParams.WorkDir = dir;

            return guiParams;
        }

        private UpdateParameters GetUpdateParameters()
        {
            return new UpdateParameters
            {
                Prerelease = _arguments.GetSwitch("p", "prerelease"),
                SearchOnly = _arguments.GetSwitch("s", "searchonly")
            };
        }

        public void Parse()
        {
            string command = _arguments.GetValues().FirstOrDefault() ?? string.Empty;
            if (!Enum.TryParse(command, true, out Command subcommand))
            {
                DoHelp?.Invoke(this, EventArgs.Empty);
                return;
            }

            if (subcommand == Command.Gui)
            {
                DoGui?.Invoke(this, GetGuiParameters());
            }
            else if (subcommand == Command.Update)
            {
                DoUpdate?.Invoke(this, GetUpdateParameters());
            }
            else if (subcommand == Command.Build 
                     && GetBuildParameters(out BuildParameters buildParams))
            {
                DoBuild?.Invoke(this, buildParams);
            }
            else if (subcommand == Command.ConfigHelp)
            {
                DoConfigHelp?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                DoHelp?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
