using System.ComponentModel;

namespace BookGen.CommandArguments;

public enum BuildAction
{
    [Description("Build Test Static Website & Run test server")]
    Test,
    [Description("Build Printable HTML document")]
    BuildPrint,
    [Description("Build Releaseable Static Website")]
    BuildWeb,
    [Description("Build Ebup 3.2")]
    BuildEpub,
    [Description("Build Wordpress Export XML file")]
    BuildWordpress,
    [Description("Build html files for processing with external program(s)")]
    BuildPostprocess,
    [Description("Clean output folders")]
    Clean,
    [Description("Validates the bookgen.json configuration file")]
    ValidateConfig,
}
