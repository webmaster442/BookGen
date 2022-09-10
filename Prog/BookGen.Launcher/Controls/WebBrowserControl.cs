using Microsoft.Web.WebView2.Wpf;
using System.Threading.Tasks;
using System.Windows;

namespace BookGen.Launcher.Controls
{
    internal class WebBrowserControl : WebView2
    {
        public string HtmlText
        {
            get { return (string)GetValue(HtmlTextProperty); }
            set { SetValue(HtmlTextProperty, value); }
        }

        public static readonly DependencyProperty HtmlTextProperty =
            DependencyProperty.Register("HtmlText", typeof(string), typeof(WebBrowserControl), new PropertyMetadata(string.Empty, OnHtmlChange));

        private static async void OnHtmlChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is WebBrowserControl preview &&
               await InializeIfNeeded(preview))
            {
                if (preview.HtmlText.IsUrl())
                    preview.CoreWebView2.Navigate(preview.HtmlText);
                else
                    preview.CoreWebView2.NavigateToString(preview.HtmlText);
            }
        }

        private static async Task<bool> InializeIfNeeded(WebBrowserControl preview)
        {
            if (preview.CoreWebView2 == null)
            {
                await preview.EnsureCoreWebView2Async();
                return true;
            }
            return true;
        }
    }
}
