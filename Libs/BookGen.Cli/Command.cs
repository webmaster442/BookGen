﻿namespace BookGen.Cli
{
    /// <summary>
    /// Base class for a command without settings.
    /// </summary>
    public abstract class Command : ICommand
    {
        public abstract int Execute(string[] context);

        Task<int> ICommand.Execute(ArgumentsBase arguments, string[] context)
        {
            return Task.FromResult(Execute(context));
        }

        public virtual SupportedOs SupportedOs
            => SupportedOs.Windows | SupportedOs.Linux | SupportedOs.OsX;

        public virtual string[] AutocompleteItems
            => Array.Empty<string>();
    }
}