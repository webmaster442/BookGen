using BookGen.Framework;

namespace BookGen.ProjectHandling.Steps
{
    internal sealed class ConfigValidate : LoadStep
    {
        public ConfigValidate(LoadState state, ILog log) : base(state, log)
        {
        }


        public override bool Execute()
        {
            var validator = new ConfigValidator(State.Config, State.WorkDir);

            validator.Validate();

            if (!validator.IsValid)
            {
                Log.Critical("Errors found in configuration: ");
                foreach (string? error in validator.Errors)
                {
                    Log.Warning(error);
                }
                return false;
            }

            Log.Info("Config file doesn't contain any errors");
            return true;
        }

    }
}
