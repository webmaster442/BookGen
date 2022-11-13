namespace BookGen.ProjectHandling.Steps
{
    internal class TagsLoad : LoadStep
    {
        public TagsLoad(LoadState state, ILog log) : base(state, log)
        {
        }

        public override bool Execute()
        {
            if(_tagsJson.IsExisting)
            {
                var loaded = _tagsJson.DeserializeJson<Dictionary<string, string[]>>(Log);
                if (loaded != null)
                {
                    State.Tags = loaded;
                    return true;
                }
                else
                {
                    Log.Critical("Invalid tags.json file.");
                    return false;
                }
            }
            else
            {
                Log.Warning("tags.json not found, continuing with empty collection");
                return true;
            }
        }
    }
}
