//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace Bookgen.Lib.AppSettings;

public interface IAppSettings : IReadOnlyAppSettings
{
    void Save();
    void Set(string setting, string value);
}
