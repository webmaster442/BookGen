//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Cli;

/// <summary>
/// Base class for command settings.
/// </summary>
public abstract class ArgumentsBase
{
    private class EmptyArgs : ArgumentsBase;

    public static readonly ArgumentsBase Empty = new EmptyArgs();

    /// <summary>
    /// Performs validation of the settings.
    /// </summary>
    /// <returns>The validation result.</returns>
    public virtual ValidationResult Validate(IValidationContext context)
    {
        return ValidationResult.Ok();
    }

    /// <summary>
    /// Called when validation is done. Allows a centralized
    /// place for modifications. Eg. Enforcing a file extension
    /// </summary>
    public virtual void ModifyAfterValidation()
    {
        //Empty
    }
}
