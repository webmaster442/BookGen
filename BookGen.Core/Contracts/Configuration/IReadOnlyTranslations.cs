//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections.Generic;

namespace BookGen.Core.Contracts.Configuration
{
    /// <summary>
    /// a Dictionary of key - value paris that can be used for translating
    /// </summary>
    public interface IReadOnlyTranslations: IReadOnlyDictionary<string, string>
    {
    }
}
