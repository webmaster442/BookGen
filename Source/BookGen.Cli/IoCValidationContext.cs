//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

using Microsoft.Extensions.DependencyInjection;

namespace BookGen.Cli;

public sealed class IoCValidationContext : IValidationContext
{
    private readonly IServiceProvider _serviceProvider;

    public IoCValidationContext(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public object? GetService(Type serviceType)
        => _serviceProvider.GetService(serviceType);

    public TType Resolve<TType>() where TType : notnull
        => _serviceProvider.GetRequiredService<TType>();

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