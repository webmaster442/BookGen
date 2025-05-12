//-----------------------------------------------------------------------------
// (c) 2023-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Cli;

public interface IValidationContext : IServiceProvider
{
    TType Resolve<TType>();

    IFileSystem FileSystem => new FileSystem();

    ValidationResult ValidateWithAttributes(object @object);
}
