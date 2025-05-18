using System.ComponentModel.DataAnnotations;

namespace Bookgen.Lib.Domain.Validation;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
internal sealed class ValidUrlAttribute : ValidationAttribute
{
    public bool EndsWithSlash { get; set; }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not string @string)
            throw new InvalidOperationException($"{nameof(NotNullOrWhiteSpaceAttribute)} works with {typeof(string)} properties");

        var name = validationContext.MemberName ?? "";

        if (!Uri.TryCreate(@string, UriKind.Absolute, out _))
            return new ValidationResult("Value is not a valid URL", [name]);

        if (EndsWithSlash && !@string.EndsWith('/'))
            return new ValidationResult("Value is not a valid URL with trailing slash", [name]);

        return ValidationResult.Success;
    }
}
