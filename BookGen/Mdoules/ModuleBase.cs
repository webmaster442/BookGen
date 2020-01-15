//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Core;

namespace BookGen.Mdoules
{
    internal abstract class ModuleBase
    {
        protected ProgramState CurrentState { get; }

        public abstract string ModuleCommand { get; }

        protected ModuleBase(ProgramState currentState)
        {
            CurrentState = currentState;
        }

        public abstract bool Execute(ArgumentParser tokenizedArguments);

        public virtual void Abort() 
        {
            //empty behaviour by default
        }
    }
}
