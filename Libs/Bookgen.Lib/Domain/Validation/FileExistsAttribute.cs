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

        if (value is string[] files)
        {
            foreach (var file in files)
            {
                if (!folder.Exists(file))
                    return new($"{file} file doesn't exist");
            }

            return ValidationResult.Success;
        }
        else if (value is string file)
        {
            if (!folder.Exists(file))
                return new($"{file} file doesn't exist");

            return ValidationResult.Success;
        }
        
        throw new InvalidOperationException($"{nameof(NotNullOrWhiteSpaceAttribute)} works with {typeof(string)} properties");
    }
}