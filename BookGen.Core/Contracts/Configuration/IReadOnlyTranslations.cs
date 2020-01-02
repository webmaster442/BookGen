//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections.Generic;

namespace BookGen.Core.Contracts.Configuration
{
    public interface IReadOnlyTranslations: IReadOnlyDictionary<string, string>
    {
    }
}
