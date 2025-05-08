using System.ComponentModel.DataAnnotations;

namespace Bookgen.Lib.Domain.Validation;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
internal sealed class NotNullOrWhiteSpaceAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not string @string)
            throw new InvalidOperationException($"{nameof(NotNullOrWhiteSpaceAttribute)} works with {typeof(string)} properties");

        if (string.IsNullOrWhiteSpace(@string))
            return new($"{validationContext.MemberName} can't be null, empty or whitespace");

        return ValidationResult.Success;
    }
}
