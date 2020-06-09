//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;
using BookGen.Domain.Shell;

namespace BookGen.Modules
{
    internal abstract class ModuleBase
    {
        protected ProgramState CurrentState { get; }

        public abstract string ModuleCommand { get; }

        public abstract AutoCompleteItem AutoCompleteInfo { get; }

        protected ModuleBase(ProgramState currentState)
        {
            CurrentState = currentState;
        }

        public abstract bool Execute(ArgumentParser tokenizedArguments);

        public abstract string GetHelp();

        public virtual void Abort() 
        {
            //empty behaviour by default
        }
    }
}
