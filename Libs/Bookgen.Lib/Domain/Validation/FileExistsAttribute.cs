using System.ComponentModel.DataAnnotations;

using Bookgen.Lib.VFS;

using Microsoft.Extensions.DependencyInjection;

namespace Bookgen.Lib.Domain.Validation;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
internal sealed class FileExistsAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var folder = validationContext.GetRequiredService<IFolder>();

        if (value is not string @string)
            throw new InvalidOperationException($"{nameof(NotNullOrWhiteSpaceAttribute)} works with {typeof(string)} properties");

        if (!folder.Exists(@string))
            return new($"{@string} file doesn't exist");

        return ValidationResult.Success;
    }
}