using System.ComponentModel.DataAnnotations;

using Bookgen.Lib.VFS;

namespace Bookgen.Lib.Internals;

internal static class SerializedObjectValidator
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


    public static bool Validate<T>(T @object, IReadOnlyFolder folder, ICollection<string> issues) where T : class
    {

        ValidationServiceProvider provider = new();
        provider.Add<IReadOnlyFolder>(folder);

        ValidationContext context = new ValidationContext(instance: @object,
                                                          serviceProvider: provider,
                                                          items: null);

        var validationResults = new List<ValidationResult>();

        bool result = Validator.TryValidateObject(instance: @object,
                                                  validationContext: context,
                                                  validationResults: validationResults,
                                                  validateAllProperties: true);

        if (!result)
        {
            foreach (var validationResult in validationResults)
            {
                foreach (var memberName in validationResult.MemberNames)
                {
                    issues.Add($"{memberName}: {validationResult.ErrorMessage}");
                }
            }
        }

        return result;
    }
}
