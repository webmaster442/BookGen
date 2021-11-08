namespace BookGen.ShellHelper.Domain
{
    internal record GitStatus
    {
        public int OutGoingCommits { get; set; }
        public int IncommingCommits { get; set; }
        public string BranchName { get; set; }
        public string LastCommitId { get; set; }

        public GitStatus()
        {
            BranchName = string.Empty;
            LastCommitId = string.Empty;
        }

    }
}
