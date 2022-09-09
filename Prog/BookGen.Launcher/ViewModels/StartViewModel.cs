using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace BookGen.Launcher.ViewModels
{
    internal class StartViewModel : ObservableObject
    {
        private List<string> _elements;
        private string _filter;
        private readonly string _fileName;

        public string Version { get; }

        public BindingList<ItemViewModel> View { get; }

        public StartViewModel()
        {
            _elements = new List<string>();
            _fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "bookgenlauncher.json");
            View = new BindingList<ItemViewModel>();
            _filter = string.Empty;
            Version = GetVersion();
            LoadFolderList();
        }

        private void LoadFolderList()
        {
            string? json = ReadFile();

            if (!string.IsNullOrEmpty(json))
            {
                string[]? deserialized = JsonSerializer.Deserialize<string[]>(json);
                if (deserialized != null)
                {
                    _elements = new List<string>(deserialized);
                }
            }

            string[] arguments = Environment.GetCommandLineArgs();
            if (arguments.Length == 2
                && Directory.Exists(arguments[1]))
            {
                _elements.Add(arguments[1]);
            }

            App.UpdateJumplist(_elements);
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
                    OnPropertyChanged(nameof(Filter));
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
            string? text = JsonSerializer.Serialize(_elements);
            WriteFile(text);
            App.UpdateJumplist(_elements);
        }

        internal void Remove(string folder)
        {
            _elements.Remove(folder);
            ApplyFilter();
            SaveFolders();
        }

        private void ApplyFilter()
        {
            if (!string.IsNullOrEmpty(Filter))
            {
                IEnumerable<string>? subset = _elements.Where(x => x.Contains(Filter));
                CreateItems(subset);
            }
            else
            {
                CreateItems(_elements);
            }
            OnPropertyChanged(nameof(IsEmpty));
            OnPropertyChanged(nameof(IsNotEmpty));
        }

        private void CreateItems(IEnumerable<string> subset)
        {
            View.RaiseListChangedEvents = false;
            View.Clear();
            foreach (string? item in subset)
            {
                View.Add(new ItemViewModel
                {
                    FullPath = item
                });
            }
            View.RaiseListChangedEvents = true;
            View.ResetBindings();
        }

        private string GetVersion()
        {
            System.Reflection.AssemblyName? name = typeof(App).Assembly.GetName();
            return name?.Version?.ToString() ?? "Couldn't get version";
        }

        private string ReadFile()
        {
            if (File.Exists(_fileName))
            {
                try
                {
                    return File.ReadAllText(_fileName);
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }
            return string.Empty;
        }

        private void WriteFile(string content)
        {
            try
            {
                File.WriteAllText(_fileName, content);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}
