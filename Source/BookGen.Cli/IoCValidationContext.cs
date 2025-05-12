//-----------------------------------------------------------------------------
// (c) 2023-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace BookGen.Cli;

public sealed class IoCValidationContext : IValidationContext
{
    private readonly IResolver _resolver;

    public IoCValidationContext(IResolver resolver)
    {
        _resolver = resolver;
    }

    public object? GetService(Type serviceType)
        => _resolver.Resolve(serviceType);

    public TType Resolve<TType>() 
        => (TType)_resolver.Resolve(typeof(TType));

    public ValidationResult ValidateWithAttributes(object @object)
    {
        List<System.ComponentModel.DataAnnotations.ValidationResult> results = new();

        var validationContext = new ValidationContext(instance: @object,
                                                      serviceProvider: this,
                                                      items: null);

        bool result = Validator.TryValidateObject(instance: @object,
                                                  validationContext: validationContext,
                                                  validationResults: results,
                                                  validateAllProperties: true);

        if (result)
            return ValidationResult.Ok();

        return ValidationResult.Error(string.Join(',', results.Select(x => x.ErrorMessage)));
    }
}