//-----------------------------------------------------------------------------
// (c) 2023-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Vfs;

namespace BookGen.Cli;

public interface IValidationContext : IServiceProvider
{
    TType Resolve<TType>();

    IReadOnlyFileSystem FileSystem => new FileSystem();

    ValidationResult ValidateWithAttributes(object @object);
}
