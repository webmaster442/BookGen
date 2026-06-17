using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;

using Bookgen.Lib.Domain.IO;
using Bookgen.Lib.Internals;

using BookGen.Vfs;

namespace Bookgen.Lib.Confighandling;

internal class AppSettingsAccessor
{
    private readonly AppSetting _appSetting;
    private readonly string _appSettingsFilePath;
    private readonly IWritableFileSystem _fileSystem;
    private readonly SerializedObjectValidator _validator;
    private readonly HashSet<string> _problematicProperties;

    private AppSetting? LoadSettings()
    {
        return _fileSystem.FileExists(_appSettingsFilePath) 
            ? _fileSystem.Deserialize<AppSetting>(_appSettingsFilePath)
            : null;
    }

    public AppSettingsAccessor(IWritableFileSystem fileSystem)
    {
        _fileSystem = fileSystem;
        _problematicProperties = new HashSet<string>();
        _appSettingsFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "bookgen.app.json");
        _validator = new SerializedObjectValidator(_fileSystem);
        _appSetting = LoadSettings() ?? AppSetting.CreateDefault();
        Validate();
    }

    private void Validate()
    {
        if (_appSetting == null)
            return;

        var issues = new List<ValidationResult>();
        _validator.ValidateSingleLevel(_appSetting, issues);

        _problematicProperties.Clear();
        foreach (var issue in issues)
        {
            foreach (var memberName in issue.MemberNames)
            {
                _problematicProperties.Add(memberName);
            }
        }
    }

    private static string GetPropertyName<T>(Expression<Func<AppSetting, T>> expression)
    {
        return expression.Body is MemberExpression memberExpression
            ? memberExpression.Member.Name
            : throw new ArgumentException("Invalid expression", nameof(expression));
    }

    public IEnumerable<(string setting, Type type)> KnownSettings
    {
        get
        {
            PropertyInfo[] properites = typeof(AppSetting).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo property in properites)
            {
                yield return (property.Name, property.PropertyType);
            }
        }
    }

    public T Get<T>(Func<AppSetting, T> selector)
    {
        return selector(_appSetting);
    }

    public void Set<T>(Action<AppSetting> setter)
    {
        setter.Invoke(_appSetting);
        Validate();
    }

    public bool IsSettingValid<T>(Expression<Func<AppSetting, T>> expression)
    {
        string propertyName = GetPropertyName(expression);
        return !_problematicProperties.Contains(propertyName);
    }
}
