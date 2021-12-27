//-----------------------------------------------------------------------------
// (c) 2020-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Domain;
using BookGen.Domain.Shell;
using BookGen.Utilities;
using System;
using System.IO;

namespace BookGen.Framework
{
    internal abstract class ModuleBase
    {
        public abstract string ModuleCommand { get; }
        public abstract ModuleRunResult Execute(string[] arguments);

        public virtual string GetHelp()
        {
            return HelpUtils.GetHelpForModule(GetType().Name);
        }

        public virtual SupportedOs SupportedOs
        {
            get { return SupportedOs.Windows | SupportedOs.Linux | SupportedOs.OsX; }
        }

        public virtual AutoCompleteItem AutoCompleteInfo
        {
            get => new(ModuleCommand);
        }
    }
}
