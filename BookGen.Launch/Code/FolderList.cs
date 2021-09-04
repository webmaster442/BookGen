//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Gui.Wpf;
using BookGen.Launch.Properties;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.Json;

namespace BookGen.Launch.Code
{
    internal class FolderList : ViewModelBase
    {
        private readonly List<string> _elements;
        private string _filter;
        public BindingList<ItemViewModel> View { get; }

        public FolderList()
        {
            _elements = new List<string>();
            View = new BindingList<ItemViewModel>();
            _filter = string.Empty;

            if (!string.IsNullOrEmpty(Settings.Default.FolderListJson))
            {
                string[]? deserialized = JsonSerializer.Deserialize<string[]>(Settings.Default.FolderListJson);
                if (deserialized != null)
                {
                    _elements = new List<string>(deserialized);
                }
            }
            ApplyFilter();
        }

        public string Filter
        {
            get => _filter;
            set
            {
                if (_filter != value)
                {
                    _filter = value;
                    ApplyFilter();
                    RaisePropertyChanged(nameof(Filter));
                }
            }
        }

        public bool IsEmpty => View.Count < 1;
        public bool IsNotEmpty => View.Count > 0;

        public void Add(string item)
        {
            if (_elements.Contains(item))
            {
                _elements.Remove(item);
                _elements.Insert(0, item);
            }
            else
            {
                _elements.Insert(0, item);
            }
            ApplyFilter();
            SaveFolders();
        }

        public void Clear()
        {
            _elements.Clear();
            ApplyFilter();
            SaveFolders();
        }

        public void SaveFolders()
        {
            var text = JsonSerializer.Serialize(_elements);
            Settings.Default.FolderListJson = text;
            Settings.Default.Save();
            App.UpdateJumplist(_elements);
        }

        private void ApplyFilter()
        {
            if (!string.IsNullOrEmpty(Filter))
            {
                var subset = _elements.Where(x => x.Contains(Filter));
                CreateItems(subset);
            }
            else
            {
                CreateItems(_elements);
            }
            RaisePropertyChanged(nameof(IsEmpty));
            RaisePropertyChanged(nameof(IsNotEmpty));
        }

        private void CreateItems(IEnumerable<string> subset)
        {
            View.RaiseListChangedEvents = false;
            View.Clear();
            foreach (var item in subset)
            {
                View.Add(new ItemViewModel
                {
                    FullPath = item
                });
            }
            View.RaiseListChangedEvents = true;
            View.ResetBindings();
        }
    }
}
