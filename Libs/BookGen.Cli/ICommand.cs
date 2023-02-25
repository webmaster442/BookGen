namespace BookGen.Cli
{
    public interface ICommand
    {
        Task<int> Execute(ArgumentsBase arguments, string[] context);
        SupportedOs SupportedOs { get; }
        string[] AutocompleteItems { get; }
    }
}
