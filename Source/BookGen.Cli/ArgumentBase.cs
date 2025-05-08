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
    /// <summary>
    /// Performs validation of the settings.
    /// </summary>
    /// <returns>The validation result.</returns>
    public virtual ValidationResult Validate()
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
