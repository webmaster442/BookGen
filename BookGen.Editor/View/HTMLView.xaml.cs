//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace BookGen.Editor.View
{
    /// <summary>
    /// Interaction logic for HTMLView.xaml
    /// </summary>
    public partial class HTMLView : UserControl
    {
        public HTMLView()
        {
            InitializeComponent();
            Browser.Navigating += Browser_Navigating;
        }

        public void RenderHTML(string htmlstring)
        {
            if (string.IsNullOrEmpty(htmlstring)) return;
            Browser.NavigateToString(htmlstring);
        }

        public void RenderPartialHtml(string partialHtml)
        {
            if (string.IsNullOrEmpty(partialHtml)) return;

            StringBuilder full = new StringBuilder();

            full.Append(Properties.Resources.Preview);
            full.Append(partialHtml);
            full.Append("</body></html>");


            Browser.NavigateToString(full.ToString());
        }

        private void Browser_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            //Disable links
            if (e.Uri != null)
            {
                MessageBox.Show($"Link to: {e.Uri}", "Links disabled in preview", MessageBoxButton.OK, MessageBoxImage.Information);
                e.Cancel = true;
            }
        }
    }
}
