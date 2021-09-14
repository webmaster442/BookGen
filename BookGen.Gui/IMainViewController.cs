//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Gui
{
    public interface IMainViewController : IControllerBase
    {
        public string WorkDir { get; }
        string CurrentHelpText { get; set; }
        string[] HelpItemSource { get; }
        int SelectedHelpIndex { get; set; }

        public void ValidateConfig();
        public void CleanOutDir();
        public void BuildTest();
        public void BuildRelease();
        public void BuildPrint();
        public void BuildEpub();
        public void BuildWordpress();
        void SelectedHelpItemChange(int index);
    }
}
