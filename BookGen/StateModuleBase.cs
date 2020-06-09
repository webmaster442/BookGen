//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain.Shell;

namespace BookGen
{
    internal abstract class StateModuleBase : BaseModule
    {
        protected ProgramState CurrentState { get; }

        public abstract AutoCompleteItem AutoCompleteInfo { get; }

        protected StateModuleBase(ProgramState currentState)
        {
            CurrentState = currentState;
        }

        public abstract string GetHelp();

        public virtual void Abort()
        {
            //empty behaviour by default
        }
    }
}
