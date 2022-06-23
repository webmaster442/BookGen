//-----------------------------------------------------------------------------
// (c) 2020-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain.Shell;
using BookGen.Resources;

namespace BookGen.Framework
{
    internal abstract class ModuleBase
    {
        public abstract string ModuleCommand { get; }
        public abstract ModuleRunResult Execute(string[] arguments);

        public virtual string GetHelp()
        {
            return ResourceHandler.GetResourceFile<GeneratorRunner>($"Resources/Help.{GetType().Name}.txt");
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
