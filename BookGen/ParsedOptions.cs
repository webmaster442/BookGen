//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel;

namespace BookGen
{
    public class ParsedOptions
    {
        public enum ActionType
        {
            [Description("Build Test Static Website & Run test server")]
            Test,
            [Description("Build Printable HTML")]
            BuildPrint,
            [Description("Build Releaseable Static Website")]
            BuildWeb,
            [Description("Build Ebup")]
            BuildEpub,
            [Description("Build Wordpress Export file")]
            BuildWordpress,
            [Description("Clean output folders")]
            Clean,
            [Description("Initialize dir as BookGen project")]
            Initialize,
            [Description("Validates the bookgen.json configuration file")]
            ValidateConfig,
        }

        public ActionType? Action { get; set; }
        public bool GuiReqested { get; set; }
        public bool ShowHelp { get; set; }
        public bool VerboseLog { get; set; }
        public string WorkingDirectory { get; set; }

        public ParsedOptions()
        {
            WorkingDirectory = string.Empty;
        }
    }
}
