//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen
{
    public class ParsedOptions
    {
        public enum ActionType
        {
            Test,
            BuildPrint,
            BuildWeb,
            BuildEpub,
            BuildWordpress,
            Clean,
            Initialize,
            ValidateConfig,
        }

        public ActionType? Action { get; set; }
        public bool GuiReqested { get; set; }
        public bool ShowHelp { get; set; }
        public bool VerboseLog { get; set; }
        public string WorkingDirectory { get; set; }
    }
}
