namespace BookGen.Utilities
{
    internal class TagUtils
    {
        private readonly IDictionary<string, string[]> _tags;

        public TagUtils(IDictionary<string, string[]> tags)
        {
            _tags = tags;
        }

        public string[] GetTags()
        {
            return _tags
                .SelectMany(x => x.Value)
                .Distinct()
                .ToArray();
        }
    }
}
