//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;

using Bookgen.Lib.Domain.IO;
using Bookgen.Lib.Internals;

using BookGen.Vfs;

namespace Bookgen.Lib.AppSettings;

public sealed class AppSettings : IAppSettings
{
    private readonly AppSetting _appSetting;
    private readonly string _appSettingsFilePath;
    private readonly IWritableFileSystem _fileSystem;
    private readonly SerializedObjectValidator _validator;
    private readonly Dictionary<string, List<string>> _problematicProperties;
    private readonly PropertyInfo[] _properites;

    private AppSetting? LoadSettings()
    {
        return _fileSystem.FileExists(_appSettingsFilePath)
            ? _fileSystem.Deserialize<AppSetting>(_appSettingsFilePath)
            : null;
    }

    private void Validate()
    {
        if (_appSetting == null)
            return;

        var issues = new List<ValidationResult>();
        _validator.ValidateSingleLevel(_appSetting, issues);

        _problematicProperties.Clear();

        foreach (ValidationResult issue in issues)
        {
            foreach (string memberName in issue.MemberNames)
            {
                if (!_problematicProperties.TryGetValue(memberName, out List<string>? value))
                {
                    value = [];
                    _problematicProperties[memberName] = value;
                }
                value.Add(issue.ErrorMessage ?? "Unknown error");
            }
        }
    }

    private static string GetPropertyName<T>(Expression<Func<AppSetting, T>> expression)
    {
        return expression.Body is MemberExpression memberExpression
            ? memberExpression.Member.Name
            : throw new ArgumentException("Invalid expression", nameof(expression));
    }

    public AppSettings(IWritableFileSystem fileSystem)
    {
        _properites = typeof(AppSetting).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        _fileSystem = fileSystem;
        _problematicProperties = new Dictionary<string, List<string>>();
        _appSettingsFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "bookgen.app.json");
        _validator = new SerializedObjectValidator(_fileSystem);
        _appSetting = LoadSettings() ?? AppSetting.CreateDefault();
        Validate();
    }

    public IEnumerable<(string setting, Type type)> KnownSettings
    {
        get
        {
            foreach (PropertyInfo property in _properites)
            {
                yield return (property.Name, property.PropertyType);
            }
        }
    }

    public T Get<T>(Func<AppSetting, T> selector)
        => selector(_appSetting);

    public object? Get(string settingName)
    {
        var propery = _properites.First(x => x.Name == settingName);
        return propery.GetValue(_appSetting);
    }

    public bool IsSettingValid(string settingName, out IReadOnlyList<string> issues)
    {
        bool ret = _problematicProperties.TryGetValue(settingName, out List<string>? issueList);
        issues = issueList ?? [];
        return ret;
    }

    public bool IsSettingValid<T>(Expression<Func<AppSetting, T>> expression, out IReadOnlyList<string> issues)
    {
        string propertyName = GetPropertyName(expression);
        if (_problematicProperties.TryGetValue(propertyName, out List<string>? value))
        {
            issues = value;
            return false;
        }
        issues = [];
        return true;
    }

    public void Set(string setting, string value)
    {
        PropertyInfo property = _properites.First(x => x.Name == setting);
        object convertedValue = Convert.ChangeType(value, property.PropertyType);
        property.SetValue(_appSetting, convertedValue);
        Validate();
    }

    public void Save() 
        => _fileSystem.Serialize<AppSetting>(_appSettingsFilePath, _appSetting);
}
