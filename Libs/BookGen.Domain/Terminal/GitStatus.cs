namespace BookGen.Domain.Terminal;

internal record GitStatus
{
    public int OutGoingCommits { get; init; }
    public int IncommingCommits { get; init; }
    public string BranchName { get; init; }
    public string LastCommitId { get; init; }
    public int NotCommitedChanges { get; init; }

    public GitStatus()
    {
        BranchName = string.Empty;
        LastCommitId = string.Empty;
    }

}
