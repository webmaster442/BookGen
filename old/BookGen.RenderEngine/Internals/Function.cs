//-----------------------------------------------------------------------------
// (c) 2019-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.RenderEngine.Internals;

internal abstract class Function
{
    public FunctionInfo Information { get; }

    public Function()
    {
        Information = GetInformation();
    }

    protected abstract FunctionInfo GetInformation();

    public abstract string Execute(FunctionArguments arguments);
}
