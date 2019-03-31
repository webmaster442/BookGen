//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Controls;

namespace BookGen.Editor.View
{
    class WebBrowserBinder
    {
        public static readonly DependencyProperty BindableSourceProperty =
            DependencyProperty.RegisterAttached("BindableSource", typeof(string), typeof(WebBrowserBinder), new UIPropertyMetadata(null, BindableSourcePropertyChanged));

        public static string GetBindableSource(DependencyObject obj)
        {
            return (string)obj.GetValue(BindableSourceProperty);
        }

        public static void SetBindableSource(DependencyObject obj, string value)
        {
            obj.SetValue(BindableSourceProperty, value);
        }

        public static void BindableSourcePropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            if (o is WebBrowser browser)
            {
                string uri = e.NewValue as string;
                browser.Source = !string.IsNullOrEmpty(uri) ? new Uri(uri) : null;
            }
        }

    }
}
