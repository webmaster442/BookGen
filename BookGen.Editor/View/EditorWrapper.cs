//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Editor.Framework;
using BookGen.Editor.Services;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace BookGen.Editor.View
{
    internal class EditorWrapper: TextEditor, INotifyPropertyChanged
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
            DependencyProperty.Register("ShowColumnRuler", typeof(bool), typeof(EditorWrapper), new PropertyMetadata(false, ConfigureShow));

        private static void ConfigureShow(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is EditorWrapper editor)
                SetViewProperties(editor);
        }

        public ICommand WrapWithToken
        {
            get { return (ICommand)GetValue(WrapWithTokenProperty); }
            set { SetValue(WrapWithTokenProperty, value); }
        }

        public static readonly DependencyProperty WrapWithTokenProperty =
            DependencyProperty.Register("WrapWithToken", typeof(ICommand), typeof(EditorWrapper), new PropertyMetadata(null));

        public ICommand InsertToken
        {
            get { return (ICommand)GetValue(InsertTokenProperty); }
            set { SetValue(InsertTokenProperty, value); }
        }

        public static readonly DependencyProperty InsertTokenProperty =
            DependencyProperty.Register("InsertToken", typeof(ICommand), typeof(EditorWrapper), new PropertyMetadata(null));

        public ICommand UndoCommand
        {
            get { return (ICommand)GetValue(UndoCommandProperty); }
            set { SetValue(UndoCommandProperty, value); }
        }

        public static readonly DependencyProperty UndoCommandProperty =
            DependencyProperty.Register("UndoCommand", typeof(ICommand), typeof(EditorWrapper), new PropertyMetadata(null));

        public ICommand RedoCommand
        { 
            get { return (ICommand)GetValue(RedoCommandProperty); }
            set { SetValue(RedoCommandProperty, value); }
        }

        public static readonly DependencyProperty RedoCommandProperty =
            DependencyProperty.Register("RedoCommand", typeof(ICommand), typeof(EditorWrapper), new PropertyMetadata(null));

        public ICommand ListUnorderedCommand
        {
            get { return (ICommand)GetValue(ListUnorderedCommandProperty); }
            set { SetValue(ListUnorderedCommandProperty, value); }
        }

        public static readonly DependencyProperty ListUnorderedCommandProperty =
            DependencyProperty.Register("ListUnorderedCommand", typeof(ICommand), typeof(EditorWrapper), new PropertyMetadata(null));

        public ICommand ListOrderedCommand
        {
            get { return (ICommand)GetValue(ListOrderedCommandProperty); }
            set { SetValue(ListOrderedCommandProperty, value); }
        }

        public static readonly DependencyProperty ListOrderedCommandProperty =
            DependencyProperty.Register("ListOrderedCommand", typeof(ICommand), typeof(EditorWrapper), new PropertyMetadata(null));



        public event PropertyChangedEventHandler PropertyChanged;

        private static void SetViewProperties(EditorWrapper editor)
        {
            editor.Options.ShowEndOfLine = editor.ShowLineEndings;
            editor.Options.ShowSpaces = editor.ShowSpaces;
            editor.Options.ShowTabs = editor.ShowTabs;
            editor.Options.ShowColumnRuler = editor.ShowColumnRuler;
        }

        public EditorWrapper() : base()
        {
            SetViewProperties(this);
            SyntaxHighlighting = EditorServices.LoadHighlightingDefinition();
            TextArea.TextView.LinkTextForegroundBrush = new SolidColorBrush(Color.FromRgb(0x56, 0x9C, 0xD6));
            TextChanged += EditorWrapper_TextChanged;
            WrapWithToken = new EditorCommand(this, true);
            InsertToken = new EditorCommand(this, false);
            ListOrderedCommand = new EditorListCommand(this, true);
            ListUnorderedCommand = new EditorListCommand(this, false);
            UndoCommand = DelegateCommand.CreateCommand(OnUndo, OnCanUndo);
            RedoCommand = DelegateCommand.CreateCommand(OnRedo, OnCanRedo);
        }

        private void OnRedo(object obj)
        {
            if (CanRedo) Redo();
        }

        private bool OnCanRedo(object obj)
        {
            return CanRedo;
        }

        private bool OnCanUndo(object obj)
        {
            return CanUndo;
        }

        private void OnUndo(object obj)
        {
            if (CanUndo) Undo();
        }

        private void EditorWrapper_TextChanged(object sender, System.EventArgs e)
        {
            FirePropertyChange(nameof(Text));
            FirePropertyChange(nameof(CanUndo));
            FirePropertyChange(nameof(CanRedo));
        }

        private void FirePropertyChange([CallerMemberName]string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public void InsertstringAtCaretPos(string md)
        {
            Document.Replace(SelectionStart, SelectionLength, md, OffsetChangeMappingType.RemoveAndInsert);
        }
    }
}
