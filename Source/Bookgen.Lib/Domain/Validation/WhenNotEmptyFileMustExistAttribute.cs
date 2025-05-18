using System.ComponentModel.DataAnnotations;

using BookGen.Vfs;
using Microsoft.Extensions.DependencyInjection;

namespace Bookgen.Lib.Domain.Validation;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
internal sealed class WhenNotEmptyFileMustExistAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var folder = validationContext.GetRequiredService<IReadOnlyFileSystem>();

        if (value is not string @string)
            throw new InvalidOperationException($"{nameof(NotNullOrWhiteSpaceAttribute)} works with {typeof(string)} properties");

        var name = validationContext.MemberName ?? "";

        if (!string.IsNullOrEmpty(@string) && !folder.FileExists(@string))
        {
            return new ValidationResult($"{@string} file doesn't exist", [name]);
        }

        return ValidationResult.Success;
    }
}