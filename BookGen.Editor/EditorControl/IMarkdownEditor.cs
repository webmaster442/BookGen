﻿//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Editor.ServiceContracts;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace BookGen.Editor.EditorControl
{
    internal interface IMarkdownEditor: INotifyPropertyChanged, IDisposable
    {
        bool ShowTabs { get; set; }
        bool ShowSpaces { get; set; }
        bool ShowLineEndings { get; set; }
        bool ShowColumnRuler { get; set; }
        bool ScrollBelowDocument { get; set; }
        ICommand WrapWithToken { get; }
        ICommand InsertToken { get; }
        ICommand UndoCommand { get; }
        ICommand RedoCommand { get; }
        ICommand ListUnorderedCommand { get; }
        ICommand ListOrderedCommand { get; }
        ICommand ConfigureSpelling { get; }

        INHunspellServices NHunspellServices { get; set; }
        IDialogService DialogService { get; set; }
    }
}