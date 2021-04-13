﻿//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Contracts;
using BookGen.Domain.Shell;
using BookGen.Framework;
using BookGen.Utilities;
using System;
using System.Diagnostics;

namespace BookGen.Modules
{
    internal class EditModule : ModuleWithState
    {
        private readonly IAppSetting _settings;

        public EditModule(ProgramState currentState, IAppSetting appSetting) : base(currentState)
        {
            _settings = appSetting;
        }

        public override string ModuleCommand => "Edit";

        public override AutoCompleteItem AutoCompleteInfo => new AutoCompleteItem(ModuleCommand);

        public override bool Execute(string[] arguments)
        {
            if (arguments.Length != 1)
            {
                CurrentState.Log.Warning("No file name given");
                return false;
            }

            if (string.IsNullOrEmpty(_settings.EditorPath))
            {
                CurrentState.Log.Warning("No Editor configured");
                return false;
            }

            var file = System.IO.Path.GetFullPath(arguments[0]);

            if (!EditorHelper.IsSupportedFile(file))
            {
                CurrentState.Log.Warning("Unsupported file type");
                return false;
            }

            try
            {
                using Process p = new Process();
                p.StartInfo.FileName = _settings.EditorPath;
                p.StartInfo.Arguments = $"\"{file}\"";
                p.StartInfo.UseShellExecute = false;
                p.Start();
                return true;
            }
            catch (Exception ex)
            {
                CurrentState.Log.Critical(ex);
                return false;
            }
        }

        public override string GetHelp()
        {
            return HelpUtils.GetHelpForModule(nameof(EditModule));
        }
    }
}
