//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using ICSharpCode.AvalonEdit;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace BookGen.Editor.View
{
    public class EditorWrapper: TextEditor, INotifyPropertyChanged
    {
        public bool ShowTabs
        {
            get { return (bool)GetValue(ShowTabsProperty); }
            set { SetValue(ShowTabsProperty, value); }
        }

        public static readonly DependencyProperty ShowTabsProperty =
            DependencyProperty.Register("ShowTabs", typeof(bool), typeof(EditorWrapper), new PropertyMetadata(false, ConfigureShow));

        public bool ShowSpaces
        {
            get { return (bool)GetValue(ShowSpacesProperty); }
            set { SetValue(ShowSpacesProperty, value); }
        }

        public static readonly DependencyProperty ShowSpacesProperty =
            DependencyProperty.Register("ShowSpaces", typeof(bool), typeof(EditorWrapper), new PropertyMetadata(false, ConfigureShow));

        public bool ShowLineEndings
        {
            get { return (bool)GetValue(ShowLineEndingsProperty); }
            set { SetValue(ShowLineEndingsProperty, value); }
        }

        public static readonly DependencyProperty ShowLineEndingsProperty =
            DependencyProperty.Register("ShowLineEndings", typeof(bool), typeof(EditorWrapper), new PropertyMetadata(false, ConfigureShow));

        public bool ShowColumnRuler
        {
            get { return (bool)GetValue(ShowColumnRulerProperty); }
            set { SetValue(ShowColumnRulerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowColumnRuler.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowColumnRulerProperty =
            DependencyProperty.Register("ShowColumnRuler", typeof(bool), typeof(EditorWrapper), new PropertyMetadata(true, ConfigureShow));

        public event PropertyChangedEventHandler PropertyChanged;

        private static void ConfigureShow(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is EditorWrapper editor)
                SetViewProperties(editor);
        }

        private static void SetViewProperties(EditorWrapper editor)
        {
            editor.Options.ShowEndOfLine = editor.ShowLineEndings;
            editor.Options.ShowSpaces = editor.ShowSpaces;
            editor.Options.ShowTabs = editor.ShowTabs;
            editor.Options.ShowColumnRuler = editor.ShowColumnRuler;
        }

        public EditorWrapper(): base()
        {
            SetViewProperties(this);
            TextChanged += EditorWrapper_TextChanged;
        }

        private void EditorWrapper_TextChanged(object sender, System.EventArgs e)
        {
            FirePropertyChange("Text");
        }

        private void FirePropertyChange([CallerMemberName]string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
