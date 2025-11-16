//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

using BookGen.Vfs;

using Microsoft.Extensions.DependencyInjection;

namespace Bookgen.Lib.Domain.Validation;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
internal sealed class FileExistsAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var folder = validationContext.GetRequiredService<IReadOnlyFileSystem>();

        if (value is IEnumerable<string> files)
        {
            foreach (var file in files)
            {
                if (!folder.FileExists(file))
                    return new($"{file} file doesn't exist");
            }

            return ValidationResult.Success;
        }
        else if (value is string file)
        {
            if (!folder.FileExists(file))
                return new($"{file} file doesn't exist");

            return ValidationResult.Success;
        }

        throw new InvalidOperationException($"{nameof(NotNullOrWhiteSpaceAttribute)} works with {typeof(string)} properties");
    }
}
