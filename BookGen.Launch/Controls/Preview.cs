//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Microsoft.Web.WebView2.Wpf;
using System.Threading.Tasks;
using System.Windows;

namespace BookGen.Launch.Controls
{
    internal class Preview : WebView2
    {
        public string HtmlText
        {
            get { return (string)GetValue(HtmlTextProperty); }
            set { SetValue(HtmlTextProperty, value); }
        }

        public static readonly DependencyProperty HtmlTextProperty =
            DependencyProperty.Register("HtmlText", typeof(string), typeof(Preview), new PropertyMetadata(string.Empty, OnHtmlChange));

        private static async void OnHtmlChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Preview preview &&
               await InializeIfNeeded(preview))
            {
                preview.CoreWebView2.NavigateToString(preview.HtmlText);
            }
        }

        private static async Task<bool> InializeIfNeeded(Preview preview)
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
