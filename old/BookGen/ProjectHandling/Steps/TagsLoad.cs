//-----------------------------------------------------------------------------
// (c) 2022-224 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.ProjectHandling.Steps;

internal class TagsLoad : LoadStep
{
    public TagsLoad(LoadState state, ILogger log) : base(state, log)
    {
    }

    public override bool Execute()
    {
        if (_tagsJson.IsExisting)
        {
            var loaded = _tagsJson.DeserializeJson<Dictionary<string, string[]>>(Log);
            if (loaded != null)
            {
                State.Tags = loaded;
                return true;
            }
            else
            {
                Log.LogCritical("Invalid tags.json file.");
                return false;
            }
        }
        else
        {
            Log.LogWarning("tags.json not found, continuing with empty collection");
            return true;
        }
    }
}
