using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace BookGen.Launcher.ViewModels.FileBrowser
{
    internal class ObservableCollectionEx<T> : ObservableCollection<T>
    {
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
}
