//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen
{
    internal abstract class BaseModule
    {
        public abstract string ModuleCommand { get; }
        public abstract bool Execute(string[] arguments);
        public abstract string GetHelp();
    }
}
