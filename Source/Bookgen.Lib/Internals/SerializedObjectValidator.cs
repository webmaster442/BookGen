using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;

using BookGen.Vfs;

using YamlDotNet.Serialization.BufferedDeserialization;

namespace Bookgen.Lib.Internals;

internal class SerializedObjectValidator
{
    internal sealed class ValidationServiceProvider : IServiceProvider
    {
        public Dictionary<Type, object> _instances;

        public ValidationServiceProvider()
        {
            _instances = new Dictionary<Type, object>();
        }

        public void Add<TType>(TType value) where TType : class
        {
            _instances.Add(typeof(TType), value);
        }

        public object? GetService(Type serviceType)
            => _instances[serviceType];
    }

    private readonly ValidationServiceProvider _provider;

    public SerializedObjectValidator(IReadOnlyFileSystem readOnlyFileSystem)
    {
        _provider = new();
        _provider.Add<IReadOnlyFileSystem>(readOnlyFileSystem);
    }

    public bool Validate<T>(T @object, ICollection<string> issues) where T : class
    {
        static void AddIssues(ICollection<string> target, string prefix, IEnumerable<ValidationResult> results)
        {
            foreach (var validationResult in results)
            {
                var names = string.Join(',', validationResult.MemberNames);

                if (!string.IsNullOrEmpty(prefix))
                    names = string.Join(',', validationResult.MemberNames.Select(n => $"{prefix}.{n}"));

                target.Add($"{names}: {validationResult.ErrorMessage}");
            }
        }

        ValidationContext context = new ValidationContext(instance: @object,
                                                          serviceProvider: _provider,
                                                          items: null);

        var validationResults = new List<ValidationResult>();

        bool result = Validator.TryValidateObject(instance: @object,
                                                  validationContext: context,
                                                  validationResults: validationResults,
                                                  validateAllProperties: true);

        bool returnvalue = true;

        if (!result)
        {
            returnvalue = false;
            AddIssues(issues, "", validationResults);
        }

        var properties = @object
            .GetType()
            .GetProperties()
            .Where(p => p.CanRead 
                     && p.GetIndexParameters().Length == 0
                     && !p.PropertyType.IsValueType
                     && p.PropertyType != typeof(string));

        validationResults.Clear();

        foreach (var property in properties)
        {
            var value = property.GetValue(@object);
            if (value == null) continue;

            bool propresult = Validator.TryValidateObject(instance: value,
                                                          validationContext: new ValidationContext(value, _provider, null),
                                                          validationResults: validationResults,
                                                          validateAllProperties: true);

            if (!propresult)
            {
                returnvalue = false;
                AddIssues(issues, property.Name, validationResults);
                validationResults.Clear();
            }
        }

        return returnvalue;
    }
}