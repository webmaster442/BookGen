using BookGen.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookGen.ProjectHandling.Steps
{
    internal class LoadTags : LoadStep
    {
        public LoadTags(LoadState state, ILog log) : base(state, log)
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
