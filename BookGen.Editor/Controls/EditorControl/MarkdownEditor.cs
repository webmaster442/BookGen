//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Editor.Infrastructure;
using BookGen.Editor.ServiceContracts;
using GalaSoft.MvvmLight.Command;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using NHunspell;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace BookGen.Editor.Controls
{
    internal sealed class MarkdownEditor : TextEditor, IMarkdownEditor
    {
        public bool ShowTabs
        {
            get { return (bool)GetValue(ShowTabsProperty); }
            set { SetValue(ShowTabsProperty, value); }
        }

        public static readonly DependencyProperty ShowTabsProperty =
            DependencyProperty.Register("ShowTabs", typeof(bool), typeof(MarkdownEditor), new PropertyMetadata(false, ConfigureShow));

        public bool ShowSpaces
        {
            get { return (bool)GetValue(ShowSpacesProperty); }
            set { SetValue(ShowSpacesProperty, value); }
        }

        public static readonly DependencyProperty ShowSpacesProperty =
            DependencyProperty.Register("ShowSpaces", typeof(bool), typeof(MarkdownEditor), new PropertyMetadata(false, ConfigureShow));

        public bool ShowLineEndings
        {
            get { return (bool)GetValue(ShowLineEndingsProperty); }
            set { SetValue(ShowLineEndingsProperty, value); }
        }

        public static readonly DependencyProperty ShowLineEndingsProperty =
            DependencyProperty.Register("ShowLineEndings", typeof(bool), typeof(MarkdownEditor), new PropertyMetadata(false, ConfigureShow));

        public bool ShowColumnRuler
        {
            get { return (bool)GetValue(ShowColumnRulerProperty); }
            set { SetValue(ShowColumnRulerProperty, value); }
        }

        public static readonly DependencyProperty ShowColumnRulerProperty =
            DependencyProperty.Register("ShowColumnRuler", typeof(bool), typeof(MarkdownEditor), new PropertyMetadata(false, ConfigureShow));

        public bool ScrollBelowDocument
        {
            get { return (bool)GetValue(ScrollBelowDocumentProperty); }
            set { SetValue(ScrollBelowDocumentProperty, value); }
        }

        public static readonly DependencyProperty ScrollBelowDocumentProperty =
            DependencyProperty.Register("ScrollBelowDocument", typeof(bool), typeof(MarkdownEditor), new PropertyMetadata(true, ConfigureShow));

        private static void ConfigureShow(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MarkdownEditor editor)
                SetViewProperties(editor);
        }

        public ICommand WrapWithToken
        {
            get { return (ICommand)GetValue(WrapWithTokenProperty); }
            set { SetValue(WrapWithTokenProperty, value); }
        }

        public static readonly DependencyProperty WrapWithTokenProperty =
            DependencyProperty.Register("WrapWithToken", typeof(ICommand), typeof(MarkdownEditor), new PropertyMetadata(null));

        public ICommand InsertToken
        {
            get { return (ICommand)GetValue(InsertTokenProperty); }
            set { SetValue(InsertTokenProperty, value); }
        }

        public static readonly DependencyProperty InsertTokenProperty =
            DependencyProperty.Register("InsertToken", typeof(ICommand), typeof(MarkdownEditor), new PropertyMetadata(null));

        public ICommand UndoCommand
        {
            get { return (ICommand)GetValue(UndoCommandProperty); }
            set { SetValue(UndoCommandProperty, value); }
        }

        public static readonly DependencyProperty UndoCommandProperty =
            DependencyProperty.Register("UndoCommand", typeof(ICommand), typeof(MarkdownEditor), new PropertyMetadata(null));

        public ICommand RedoCommand
        {
            get { return (ICommand)GetValue(RedoCommandProperty); }
            set { SetValue(RedoCommandProperty, value); }
        }

        public static readonly DependencyProperty RedoCommandProperty =
            DependencyProperty.Register("RedoCommand", typeof(ICommand), typeof(MarkdownEditor), new PropertyMetadata(null));

        public ICommand ListUnorderedCommand
        {
            get { return (ICommand)GetValue(ListUnorderedCommandProperty); }
            set { SetValue(ListUnorderedCommandProperty, value); }
        }

        public static readonly DependencyProperty ListUnorderedCommandProperty =
            DependencyProperty.Register("ListUnorderedCommand", typeof(ICommand), typeof(MarkdownEditor), new PropertyMetadata(null));

        public ICommand ListOrderedCommand
        {
            get { return (ICommand)GetValue(ListOrderedCommandProperty); }
            set { SetValue(ListOrderedCommandProperty, value); }
        }

        public static readonly DependencyProperty ListOrderedCommandProperty =
            DependencyProperty.Register("ListOrderedCommand", typeof(ICommand), typeof(MarkdownEditor), new PropertyMetadata(null));

        public bool SpellCheckEnabled
        {
            get { return (bool)GetValue(SpellCheckEnabledProperty); }
            set { SetValue(SpellCheckEnabledProperty, value); }
        }

        public static readonly DependencyProperty SpellCheckEnabledProperty =
            DependencyProperty.Register("SpellCheckEnabled", typeof(bool), typeof(MarkdownEditor), new PropertyMetadata(false, SpellEnabled));

        private static void SpellEnabled(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MarkdownEditor editor)
                editor.ToggleSpeelCheck();
        }

        public int SpellingErrors
        {
            get { return (int)GetValue(SpellingErrorsProperty); }
            set { SetValue(SpellingErrorsProperty, value); }
        }

        public static readonly DependencyProperty SpellingErrorsProperty =
            DependencyProperty.Register("SpellingErrors", typeof(int), typeof(MarkdownEditor), new PropertyMetadata(0));



        public int Chars
        {
            get { return Document.TextLength; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private static void SetViewProperties(MarkdownEditor editor)
        {
            editor.Options.ShowEndOfLine = editor.ShowLineEndings;
            editor.Options.ShowSpaces = editor.ShowSpaces;
            editor.Options.ShowTabs = editor.ShowTabs;
            editor.Options.ShowColumnRuler = editor.ShowColumnRuler;
            editor.Options.AllowScrollBelowDocument = editor.ScrollBelowDocument;
        }

        private SpellCheck _spellCheck;
        private Hunspell _hunspell;

        public INHunspellServices NHunspellServices { get; set; }
        public IDialogService DialogService { get; set; }

        IDocument IMarkdownEditor.Document
        {
            get { return Document; }
        }

        public MarkdownEditor() : base()
        {
            SetViewProperties(this);
            SyntaxHighlighting = LoadHighlightingDefinition();
            TextArea.TextView.LinkTextForegroundBrush = new SolidColorBrush(Color.FromRgb(0x56, 0x9C, 0xD6));
            ContextMenuOpening += EditorWrapper_ContextMenuOpening;
            TextChanged += EditorWrapper_TextChanged;
            WrapWithToken = new EditorCommand(this, true);
            InsertToken = new EditorCommand(this, false);
            ListOrderedCommand = new EditorListCommand(this, true);
            ListUnorderedCommand = new EditorListCommand(this, false);
            UndoCommand = new RelayCommand<object>(OnUndo, OnCanUndo);
            RedoCommand = new RelayCommand<object>(OnRedo, OnCanRedo);
            TextArea.TextView.VisualLinesChanged += TextView_VisualLinesChanged;
            Options.AllowToggleOverstrikeMode = true;

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                ToggleSpeelCheck();
            }
        }

        public void ToggleSpeelCheck()
        {
            if (_hunspell != null)
            {
                _hunspell.Dispose();
                _hunspell = null;
            }
            _spellCheck?.Invalidate();
            _spellCheck = null;
            SpellingErrors = 0;

            if (SpellCheckEnabled 
                && NHunspellServices?.CreateConfiguredHunspell(NHunspellServices?.GetCurrentLanguage(), out _hunspell) == true)
            {
                _spellCheck = new SpellCheck(TextArea.TextView, _hunspell);
                SpellingErrors = _spellCheck.DoSpellCheck();
            }
        }

        private IHighlightingDefinition LoadHighlightingDefinition()
        {
            const string resourceName = "BookGen.Editor.Controls.EditorControl.MardkownSyntax.xshd";
            var type = typeof(MarkdownEditor);
            using var stream = type.Assembly.GetManifestResourceStream(resourceName);
            using var reader = new System.Xml.XmlTextReader(stream);
            return HighlightingLoader.Load(reader, HighlightingManager.Instance);
        }

        private void TextView_VisualLinesChanged(object sender, EventArgs e)
        {
            if (_spellCheck == null) return;
            if (CaretOffset == Document.TextLength) return;
            SpellingErrors = _spellCheck.DoSpellCheck();
        }

        private string GetWord(out int start, out int end)
        {
            var mousePosition = this.GetPositionFromPoint(Mouse.GetPosition(this));

            if (mousePosition == null)
            {
                start = -1;
                end = -1;
                return string.Empty;
            }

            var line = mousePosition.Value.Line;
            var column = mousePosition.Value.Column;
            var offset = Document.GetOffset(line, column);

            if (offset >= Document.TextLength)
                offset--;

            int offsetStart = TextUtilities.GetNextCaretPosition(Document, offset, LogicalDirection.Backward, CaretPositioningMode.WordBorder);
            int offsetEnd = TextUtilities.GetNextCaretPosition(Document, offset, LogicalDirection.Forward, CaretPositioningMode.WordBorder);

            if (offsetEnd == -1 || offsetStart == -1)
            {
                start = offsetStart;
                end = offsetEnd;
                return string.Empty;
            }

            var currentChar = Document.GetText(offset, 1);

            if (string.IsNullOrWhiteSpace(currentChar))
            {
                start = -1;
                end = -1;
                return string.Empty;
            }

            start = offsetStart;
            end = offsetEnd;
            return Document.GetText(offsetStart, offsetEnd - offsetStart);
        }

        private void EditorWrapper_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            var word = GetWord(out int start, out int end);
            ContextMenu = null;
            if (string.IsNullOrEmpty(word)) return;

            if (!_spellCheck.Spell(word))
            {
                ContextMenu = new ContextMenu();
                var suggestions = _spellCheck.Suggest(word);
                if (suggestions.Count < 1)
                {
                    var item = new MenuItem { Header = "Unknown word", FontWeight = FontWeights.Bold };
                    ContextMenu.Items.Add(item);
                    return;
                }
                foreach (string corrected in suggestions)
                {
                    var item = new MenuItem { Header = corrected, FontWeight = FontWeights.Bold };
                    item.Tag = new Tuple<int, int>(start, end);
                    item.Click += ReplaceWithCorrectWord;
                    ContextMenu.Items.Add(item);
                }
            }
        }

        private void ReplaceWithCorrectWord(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem item)
            {
                var seg = item.Tag as Tuple<int, int>;
                int len = seg.Item2 - seg.Item1;
                Document.Replace(seg.Item1, len, item.Header.ToString());
            }
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
            FirePropertyChange(nameof(LineCount));
            FirePropertyChange(nameof(Chars));
            (UndoCommand as RelayCommand<object>)?.RaiseCanExecuteChanged();
            (RedoCommand as RelayCommand<object>)?.RaiseCanExecuteChanged();
        }

        private void FirePropertyChange([CallerMemberName]string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public void InsertstringAtCaretPos(string md)
        {
            Document.Replace(SelectionStart, SelectionLength, md, OffsetChangeMappingType.RemoveAndInsert);
        }

        public void Dispose()
        {
            if (_hunspell != null)
            {
                _hunspell.Dispose();
                _hunspell = null;
            }
        }

        public void ShowFindDialog()
        {
            FindReplaceDialog.ShowForFind(this);
        }

        public void ShowReplaceDialog()
        {
            FindReplaceDialog.ShowForReplace(this);
        }

        ~MarkdownEditor()
        {
            Dispose();
        }
    }
}
