//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Linq.Expressions;

using BookGen.Lib.Domain.IO;

namespace BookGen.Lib.AppSettings;

public interface IReadOnlyAppSettings
{
    IEnumerable<(string setting, Type type)> KnownSettings { get; }
    T Get<T>(Func<AppSetting, T> selector);
    object? Get(string settingName);
    bool IsSettingValid(string settingName, out IReadOnlyList<string> issues);
    bool IsSettingValid<T>(Expression<Func<AppSetting, T>> expression, out IReadOnlyList<string> issues);

}
