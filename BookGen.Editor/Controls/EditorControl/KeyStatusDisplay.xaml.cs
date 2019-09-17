//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BookGen.Editor.Controls
{
    /// <summary>
    /// Interaction logic for KeyStatusDisplay.xaml
    /// </summary>
    internal partial class KeyStatusDisplay : UserControl
    {
        public KeyStatusDisplay()
        {
            InitializeComponent();
        }

        public MarkdownEditor Editor
        {
            get { return (MarkdownEditor)GetValue(EditorProperty); }
            set { SetValue(EditorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Editor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EditorProperty =
            DependencyProperty.Register("Editor", typeof(MarkdownEditor), typeof(KeyStatusDisplay), new PropertyMetadata(null, EditorChange));

        private static void EditorChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is KeyStatusDisplay sender)
            {
                if (sender.Editor != null)
                    sender.Editor.PropertyChanged -= sender.EditorPropertyChange;
                if (e.NewValue != null)
                    sender.Editor.PropertyChanged += sender.EditorPropertyChange;
            }
        }

        private void EditorPropertyChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MarkdownEditor.Text))
            {
                if (Editor.TextArea.OverstrikeMode)
                    OvertypeMode.Text = "Overtype";
                else
                    OvertypeMode.Text = "Insert";
            }
        }
    }
}
