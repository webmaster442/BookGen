//-----------------------------------------------------------------------------
// (c) 2019-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.RenderEngine.Internals;

internal interface IInjectable
{
    void Inject(FunctionServices functionServices);
}
