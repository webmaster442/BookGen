namespace BookGen.Cli
{
    /// <summary>
    /// Base class for async command without settings.
    /// </summary>
    public abstract class AsyncCommand : ICommand
    {
        public abstract Task<int> Execute(string[] context);

        Task<int> ICommand.Execute(ArgumentsBase arguments, string[] context)
        {
            return Execute(context);
        }

        public virtual SupportedOs SupportedOs
            => SupportedOs.Windows | SupportedOs.Linux | SupportedOs.OsX;
    }
}
