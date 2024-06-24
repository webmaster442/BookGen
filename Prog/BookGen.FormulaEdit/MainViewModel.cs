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
        UpdateCommands();
    }

    private void UpdateCommands()
    {
        DeleteCommand.NotifyCanExecuteChanged();
        AddCommand.NotifyCanExecuteChanged();
        NewCommand.NotifyCanExecuteChanged();
        SaveCommand.NotifyCanExecuteChanged();
        RenderCurrentCommand.NotifyCanExecuteChanged();
        RenderAllCommand.NotifyCanExecuteChanged();
    }

    partial void OnCurrentFormulaChanged(string value)
    {
        if (SelectedIndex > -1)
        {
            int tempIndex = SelectedIndex;
            Formulas[SelectedIndex] = value;
            _isDirty = true;
            SelectedIndex = tempIndex;
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

    [RelayCommand(CanExecute = nameof(HasItems))]
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

    public bool HasSelection => SelectedIndex > -1;

    public bool HasItems => Formulas.Count > 0;

    [RelayCommand]
    public void Add()
    {
        Formulas.Add(string.Empty);
        SelectedIndex = Formulas.Count - 1;
        _isDirty = true;
    }

    [RelayCommand(CanExecute = nameof(HasSelection))]
    public void Delete()
    {
        Formulas.RemoveAt(SelectedIndex);
        SelectedIndex = -1;
        _isDirty = true;
    }

    [RelayCommand(CanExecute = nameof(HasSelection))]
    public void RenderCurrent(string arg)
    {

    }

    [RelayCommand(CanExecute = nameof(HasItems))]
    public void RenderAll(string arg)
    {

    }
}
