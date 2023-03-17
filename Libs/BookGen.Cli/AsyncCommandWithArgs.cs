using BookGen.Cli.ArgumentParsing;

namespace BookGen.Cli
{
    /// <summary>
    /// Base class for async command
    /// </summary>
    /// <typeparam name="TArguments">Argument type</typeparam>
    public abstract class AsyncCommand<TArguments> : ICommand
        where TArguments : ArgumentsBase
    {
        public abstract Task<int> Execute(TArguments arguments, string[] context);

        Task<int> ICommand.Execute(ArgumentsBase arguments, string[] context)
        {
            return Execute((TArguments)arguments, context);
        }

        public virtual SupportedOs SupportedOs 
            => SupportedOs.Windows | SupportedOs.Linux | SupportedOs.OsX;
    }


}
