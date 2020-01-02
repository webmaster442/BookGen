//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Core.Contracts.Configuration
{
    public interface IReadOnlyTemplateOptions
    {
        bool TryGetOption<T>(string setting, out T value) where T : struct;
    }
}
