//-----------------------------------------------------------------------------
// (c) 2020-2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Interfaces.Configuration
{
    /// <summary>
    /// a Dictionary of key - value paris that can be used for translating
    /// </summary>
    public interface IReadOnlyTranslations : IReadOnlyDictionary<string, string>
    {
    }
}
