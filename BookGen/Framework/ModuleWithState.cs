//-----------------------------------------------------------------------------
// (c) 2020-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Framework
{
    internal abstract class ModuleWithState : ModuleBase
    {
        protected ProgramState CurrentState { get; }

        protected ModuleWithState(ProgramState currentState)
        {
            CurrentState = currentState;
        }

        public virtual void Abort()
        {
            //empty behaviour by default
        }
    }
}
