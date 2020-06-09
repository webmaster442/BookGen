//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;

namespace BookGen
{
    internal abstract class BaseModule
    {
        public abstract string ModuleCommand { get; }
        public abstract bool Execute(ArgumentParser tokenizedArguments);
    }
}
