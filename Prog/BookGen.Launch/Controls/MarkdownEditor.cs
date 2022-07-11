//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Gui.Wpf;
using ICSharpCode.AvalonEdit;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

namespace BookGen.Launch.Controls
{
    internal sealed class MarkdownEditor : TextEditor
    {
        public ICommand WrapOnBothSidesCommand { get; }
        public ICommand WrapOnLeftNumbersCommand { get; }
        public ICommand WrapOnLeftCommand { get; }
        public ICommand InsertTextCommand { get; }
        public ICommand FontIncrease { get; }
        public ICommand FontDecrease { get; }
        public DelegateCommand UndoCommand { get; }
        public DelegateCommand RedoCommand { get; }

        private readonly List<double> _fontSizeTable = new() { 8.0, 9.0, 10.0, 11.0, 12.0, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };
        private int _selectedFontIndex;

        public bool IsDirty
        {
            get { return (bool)GetValue(IsDirtyProperty); }
            set { SetValue(IsDirtyProperty, value); }
        }

        public static readonly DependencyProperty IsDirtyProperty =
            DependencyProperty.Register("IsDirty", typeof(bool), typeof(MarkdownEditor), new PropertyMetadata(false));

        public MarkdownEditor()
        {
            _selectedFontIndex = _fontSizeTable.IndexOf(16.0);
            FontSize = _fontSizeTable[_selectedFontIndex];
            Options = new TextEditorOptions
            {
                AllowScrollBelowDocument = false,
                ConvertTabsToSpaces = true,
                EnableHyperlinks = true,
                EnableRectangularSelection = true
            };
            ShowLineNumbers = true;
            HorizontalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Auto;
            VerticalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Visible;
            WordWrap = true;
            TextChanged += OnTextChange;

            FontDecrease = new DelegateCommand((_) => ChangeFontSize(increase: false));
            FontIncrease = new DelegateCommand((_) => ChangeFontSize(increase: true));
            UndoCommand = new DelegateCommand(_ => Undo(), (_) => CanUndo);
            RedoCommand = new DelegateCommand(_ => Redo(), (_) => CanRedo);
            WrapOnBothSidesCommand = new EditCommand(this, wrapLeft: true, wrapRight: true);
            WrapOnLeftCommand = new EditCommand(this, wrapLeft: true, wrapRight: false);
            InsertTextCommand = new EditCommand(this, wrapLeft: false, wrapRight: false);
            WrapOnLeftNumbersCommand = new EditNumbersCommand(this, wrapLeft: true, wrapRight: false);
        }

        private void ChangeFontSize(bool increase)
        {
            if (increase)
            {
                int newIndex = _selectedFontIndex + 1;
                if (newIndex < _fontSizeTable.Count - 1)
                {
                    _selectedFontIndex = newIndex;
                    FontSize = _fontSizeTable[newIndex];
                }
            }
            else
            {
                int newIndex = _selectedFontIndex - 1;
                if (newIndex > -1)
                {
                    _selectedFontIndex = newIndex;
                    FontSize = _fontSizeTable[newIndex];
                }
            }
        }

        private void OnTextChange(object? sender, EventArgs e)
        {
            IsDirty = true;
            UndoCommand.RaiseCanExecuteChanged();
            RedoCommand.RaiseCanExecuteChanged();
        }
    }
}
