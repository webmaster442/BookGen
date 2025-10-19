//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Vfs;

namespace BookGen.Cli;

public interface IValidationContext : IServiceProvider
{
    TType Resolve<TType>() where TType : notnull;

    IReadOnlyFileSystem FileSystem => new FileSystem();

    ValidationResult ValidateWithAttributes(object @object);
}
