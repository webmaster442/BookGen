using System;
using System.ComponentModel;
using System.Windows;

using BookGen.FormulaEdit.AppLogic;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BookGen.FormulaEdit;

internal partial class MainViewModel : ObservableObject
{
    private readonly IDialogs _dialogs;
    private bool _isDirty;

    [ObservableProperty]
    private int _selectedIndex;

    [ObservableProperty]
    private string _currentFormula;

    partial void OnSelectedIndexChanged(int value)
    {
        if (value > -1
            && value < Formulas.Count)
        {
            CurrentFormula = Formulas[value];
        }
    }

    partial void OnCurrentFormulaChanged(string value)
    {
        if (SelectedIndex > -1)
        {
            Formulas[SelectedIndex] = value;
            _isDirty = true;
        }
    }

    public BindingList<string> Formulas { get; }

    public MainViewModel(IDialogs dialogs)
    {
        Formulas = new BindingList<string>();
        _dialogs = dialogs;
        _selectedIndex = -1;
        _currentFormula = string.Empty;
    }

    [RelayCommand]
    public void New()
    {
        if (_isDirty && 
            _dialogs.Confirm("Do you want to save the current file?"))
        {
            Save();
        }
        Formulas.Clear();
        CurrentFormula = string.Empty;
        SelectedIndex = -1;
        OnPropertyChanged(nameof(Formulas));
        _isDirty = false;
    }

    [RelayCommand]
    public void Open()
    {
        var fileName = _dialogs.OpenFile();
        if (fileName != null)
        {
            try
            {
                Formulas.Update(FileManager.LoadFile(fileName));
                OnPropertyChanged(nameof(Formulas));
                if (Formulas.Count > 0)
                {
                    SelectedIndex = 0;
                }
                _isDirty = false;
            }
            catch (Exception ex)
            {
                _dialogs.Error(ex);
            }
        }
    }

    [RelayCommand]
    public void Save()
    {
        var fileName = _dialogs.SaveFile();
        if (fileName != null)
        {
            try
            {
                FileManager.SaveFile(fileName, Formulas);
                _isDirty = false;
            }
            catch (Exception ex)
            {
                _dialogs.Error(ex);
            }
        }
    }

    [RelayCommand]
    public void Exit()
    {
        Application.Current.Shutdown();
    }

    [RelayCommand]
    public void Add()
    {
        Formulas.Add(string.Empty);
        SelectedIndex = Formulas.Count - 1;
        _isDirty = true;
    }

    [RelayCommand]
    public void Delete()
    {
        Formulas.RemoveAt(SelectedIndex);
        SelectedIndex = -1;
        _isDirty = true;
    }

    [RelayCommand]
    public void Render(string arg)
    {

    }
}
