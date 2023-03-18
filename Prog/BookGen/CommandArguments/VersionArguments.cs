using BookGen.Cli;
using BookGen.Cli.Annotations;

namespace BookGen.CommandArguments
{
    internal class VersionArguments : ArgumentsBase
    {
        [Switch("bd", "builddate")]
        public bool BuildDate { get; set; }
        
        [Switch("api", "apiversion")]
        public bool ApiVersion { get; set; }

        public bool IsDefault => !BuildDate && !ApiVersion;
    }
}
