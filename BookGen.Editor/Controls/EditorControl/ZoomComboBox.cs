//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace BookGen.Editor.Controls
{
    internal class ZoomComboBox: ComboBox
    {
        public ObservableCollection<int> _items;
        private double _basefontSize;

        public ZoomComboBox()
        {
            _items = new ObservableCollection<int>(new int[] 
            {
                20, 50, 75, 100, 125, 150, 170, 200, 300, 400, 500
            });
            ItemsSource = _items;
            SelectionChanged += ZoomComboBox_SelectionChanged;
            SelectedIndex = _items.IndexOf(100);
        }

        private void ZoomComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ScaleFont();
        }

        private void ScaleFont()
        {
            if (SelectedIndex > -1 && Editor != null)
            {
                Editor.FontSize = _basefontSize * (_items[SelectedIndex] / 100.0);
            }
        }

        internal void ChangeByDelta(int delta)
        {
            if (delta > 0 
                && SelectedIndex + 1 < _items.Count)
            {
                SelectedIndex++;
            }
            if (delta < 0 
                && SelectedIndex - 1 > -1)
            {
                SelectedIndex--;
            }
        }

        public MarkdownEditor Editor
        {
            get { return (MarkdownEditor)GetValue(EditorProperty); }
            set { SetValue(EditorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Editor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EditorProperty =
            DependencyProperty.Register("Editor", typeof(MarkdownEditor), typeof(ZoomComboBox), new PropertyMetadata(null, EditorBound));

        private static void EditorBound(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ZoomComboBox comboBox)
            {
                comboBox._basefontSize = comboBox.Editor.FontSize;
                comboBox.ScaleFont();
            }
        }
    }
}
