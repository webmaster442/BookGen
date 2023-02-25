namespace BookGen.Cli
{
    /// <summary>
    /// Base class for a command.
    /// </summary>
    /// <typeparam name="TArguments"></typeparam>
    public abstract class Command<TArguments> : ICommand
        where TArguments : ArgumentsBase
    {
        public abstract int Execute(TArguments arguments, string[] context);

        Task<int> ICommand.Execute(ArgumentsBase arguments, string[] context)
        {
            return Task.FromResult(Execute((TArguments)arguments, context));
        }

        public virtual SupportedOs SupportedOs
        {
            get { return SupportedOs.Windows | SupportedOs.Linux | SupportedOs.OsX; }
        }
    }


}
