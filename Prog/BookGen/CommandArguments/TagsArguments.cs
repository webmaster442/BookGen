namespace BookGen.CommandArguments
{
    public sealed class TagsArguments : BookGenArgumentBase
    {
        [Switch("a", "auto", false)]
        public bool AutoGenerateTags { get; set; }

        [Switch("k", "keywordcount")]
        public int AutoKeyWordCount { get; set; }

        public TagsArguments()
        {
            AutoGenerateTags = false;
            AutoKeyWordCount = 10;
        }
    }
}
