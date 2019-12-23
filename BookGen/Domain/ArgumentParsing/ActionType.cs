//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel;

namespace BookGen.Domain.ArgumentParsing
{
    internal enum ActionType
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
        [Description("Clean output folders")]
        Clean,
        [Description("Initialize dir as BookGen project")]
        Initialize,
        [Description("Validates the bookgen.json configuration file")]
        ValidateConfig,
    }
}
