//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Linq.Expressions;

using BookGen.Lib.AppSettings;
using BookGen.Lib.Domain.IO;

namespace Bookgen.Tests;

internal class TestAppSettings : IReadOnlyAppSettings
{
    public IEnumerable<(string setting, Type type)> KnownSettings
        => Enumerable.Empty<(string setting, Type type)>();

    public T Get<T>(Func<AppSetting, T> selector)
    {
        return default(T)!;
    }

    public object? Get(string settingName)
    {
        return null;
    }

    public bool IsSettingValid(string settingName, out IReadOnlyList<string> issues)
    {
        issues = Array.Empty<string>();
        return true;
    }

    public bool IsSettingValid<T>(Expression<Func<AppSetting, T>> expression, out IReadOnlyList<string> issues)
    {
        issues = Array.Empty<string>();
        return true;
    }
}
