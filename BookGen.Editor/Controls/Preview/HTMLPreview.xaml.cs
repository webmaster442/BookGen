//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;
using System.Windows;
using System.Windows.Controls;

namespace BookGen.Editor.Controls
{
    /// <summary>
    /// Interaction logic for HTMLPreview.xaml
    /// </summary>
    public partial class HTMLPreview : UserControl
    {
        public HTMLPreview()
        {
            InitializeComponent();
            Browser.NavigationStarting += Browser_NavigationStarting;
        }

        public string Markdown
        {
            get { return (string)GetValue(MarkdownProperty); }
            set { SetValue(MarkdownProperty, value); }
        }
        public static readonly DependencyProperty MarkdownProperty =
            DependencyProperty.Register("Markdown", typeof(string), typeof(HTMLPreview), new PropertyMetadata(HtmlChanged));

        private static void HtmlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is HTMLPreview preview)
                preview.DoUpdate();
        }

#pragma warning disable S3168 // "async" methods should not return "void"
        private async void DoUpdate()
        {
            Progress.Visibility = Visibility.Visible;
            string html = await PreviewRender.RenderPreviewForMd(Markdown);
            Dispatcher.Invoke(() =>
            {
                Browser.NavigateToString(html);
                Progress.Visibility = Visibility.Collapsed;
            });
        }
#pragma warning restore S3168 // "async" methods should not return "void"

        private void Browser_NavigationStarting(object sender, WebViewControlNavigationStartingEventArgs e)
        {
            //Disable links
            if (e.Uri != null && e.Uri.AbsoluteUri != "about:blank")
            {
                MessageBox.Show($"Link to: {e.Uri}", "Links disabled in preview", MessageBoxButton.OK, MessageBoxImage.Information);
                e.Cancel = true;
            }
        }
    }
}
