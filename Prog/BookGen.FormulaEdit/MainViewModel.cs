using System.Collections.ObjectModel;
using System.Windows;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BookGen.FormulaEdit;

internal partial class MainViewModel : ObservableObject
{
    private readonly IDialogs _dialogs;

    [ObservableProperty]
    private string _currentFormula;

    public ObservableCollection<string> Formulas { get; }

    public MainViewModel(IDialogs dialogs)
    {
        Formulas = new ObservableCollection<string>();
        _dialogs = dialogs;
        _currentFormula = string.Empty;
    }

    [RelayCommand]
    public void Open()
    {

    }

    [RelayCommand]
    public void Save()
    {
    }

    [RelayCommand]
    public void Exit()
    {
        Application.Current.Shutdown();
    }

    [RelayCommand]
    public void Add()
    {

    }

    [RelayCommand]
    public void Delete()
    { 
    }

    [RelayCommand]
    public void Render(string arg)
    {

    }
}
