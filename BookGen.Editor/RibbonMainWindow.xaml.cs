//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Editor
{
    /// <summary>
    /// Interaction logic for RibbonMainWindow.xaml
    /// </summary>
    public partial class RibbonMainWindow
    {
        public RibbonMainWindow()
        {
            InitializeComponent();
        }

        private void RibbonWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Properties.Settings.Default.Save();
        }
    }
}
