//-----------------------------------------------------------------------------
// (c) 2022-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.ProjectHandling.Steps;

internal sealed class ConfigValidate : LoadStep
{
    public ConfigValidate(LoadState state, ILogger log) : base(state, log)
    {
    }

    public override bool Execute()
    {
        var validator = new ConfigValidator(State.Config!, State.WorkDir);

        validator.Validate();

        if (!validator.IsValid)
        {
            Log.LogCritical("Errors found in configuration: ");
            foreach (string? error in validator.Errors)
            {
                Log.LogWarning("Config error: {error}", error);
            }
            return false;
        }

        Log.LogInformation("Config file doesn't contain any errors");
        return true;
    }
}
