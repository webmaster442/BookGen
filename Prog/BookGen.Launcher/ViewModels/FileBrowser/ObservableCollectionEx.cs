﻿//-----------------------------------------------------------------------------
// (c) 2021-2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace BookGen.Launcher.ViewModels.FileBrowser;

internal sealed class ObservableCollectionEx<T> : ObservableCollection<T>
{
    public ObservableCollectionEx()
    {
    }

    public ObservableCollectionEx(IEnumerable<T> collection) : base(collection)
    {
    }

    public ObservableCollectionEx(List<T> list) : base(list)
    {
    }

    /// <summary> 
    /// Adds the elements of the specified collection to the end of the ObservableCollection(Of T). 
    /// </summary> 
    public void AddRange(IEnumerable<T> collection)
    {
        if (collection == null)
            throw new ArgumentNullException(nameof(collection));

        CheckReentrancy();

        foreach (T? i in collection)
        {
            Items.Add(i);
        }

        OnPropertyChanged(new PropertyChangedEventArgs("Count"));
        OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }
}
