//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;

using BookGen.FormulaEdit.AppLogic;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BookGen.FormulaEdit;

internal partial class MainViewModel : ObservableObject
{
    private readonly IDialogs _dialogs;
    private readonly DocumentState _documentState;

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
        SaveAsCommand.NotifyCanExecuteChanged();
        RenderCurrentCommand.NotifyCanExecuteChanged();
        RenderAllCommand.NotifyCanExecuteChanged();
    }

    partial void OnCurrentFormulaChanged(string value)
    {
        if (SelectedIndex > -1)
        {
            int tempIndex = SelectedIndex;
            Formulas[SelectedIndex] = value;
            SelectedIndex = tempIndex;
            _documentState.Modifified();
        }
    }

    public BindingList<string> Formulas { get; }

    public MainViewModel(IDialogs dialogs)
    {
        _documentState = new();
        Formulas = [];
        _dialogs = dialogs;
        _selectedIndex = -1;
        _currentFormula = string.Empty;
    }

    [RelayCommand]
    public async Task New()
    {
        if (_documentState.IsDirty &&
            await _dialogs.Confirm("Do you want to save the current file?"))
        {
            await Save();
            _documentState.NewCreated();
        }
        Formulas.Clear();
        CurrentFormula = string.Empty;
        SelectedIndex = -1;
        OnPropertyChanged(nameof(Formulas));
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
                _documentState.Opened(fileName);
            }
            catch (Exception ex)
            {
                _dialogs.Error(ex);
            }
        }
    }

    [RelayCommand(CanExecute = nameof(HasItems))]
    public async Task Save()
    {
        if (_documentState.HasFileName)
        {
            try
            {
                FileManager.SaveFile(_documentState.CurrentFileName, Formulas);
                _documentState.Saved();
            }
            catch (Exception ex)
            {
                await _dialogs.Error(ex);
            }
        }
        else
        {
            await SaveAs();
        }
    }

    [RelayCommand(CanExecute = nameof(HasItems))]
    public async Task SaveAs()
    {
        var fileName = _dialogs.SaveFile("formulas");
        if (fileName != null)
        {
            try
            {
                FileManager.SaveFile(fileName, Formulas);
                _documentState.SavedAs(fileName);
            }
            catch (Exception ex)
            {
                await _dialogs.Error(ex);
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
        _documentState.Modifified();
    }

    [RelayCommand(CanExecute = nameof(HasSelection))]
    public void Delete()
    {
        Formulas.RemoveAt(SelectedIndex);
        SelectedIndex = -1;
        _documentState.Modifified();
    }

    [RelayCommand(CanExecute = nameof(HasSelection))]
    public async Task RenderCurrent(RenderFormat renderFormat)
    {
        try
        {
            var fileName = _dialogs.SaveFile(renderFormat.GetExtension());
            if (fileName != null)
            {
                Renderer.RenderTo(CurrentFormula, fileName, renderFormat);
            }
        }
        catch (Exception ex)
        {
            await _dialogs.Error(ex);
        }
    }

    [RelayCommand(CanExecute = nameof(HasItems))]
    public async Task RenderAll(RenderFormat renderFormat)
    {
        var dialogData = await _dialogs.ExportDialog();
        if (dialogData != null)
        {
            try
            {
                if (string.IsNullOrEmpty(dialogData.Value.baseName))
                {
                    await _dialogs.Error(new InvalidOperationException("Base name cannot be empty"));
                    return;
                }
                Renderer.RenderAllTo(dialogData.Value.folder, dialogData.Value.baseName, renderFormat, Formulas);
            }
            catch (Exception ex)
            {
                await _dialogs.Error(ex);
            }
        }
    }

    public async Task Closing()
    {
        if (_documentState.IsDirty &&
            await _dialogs.Confirm("Do you want to save the current file?"))
        {
            await Save();
        }
        Environment.Exit(0);
    }
}
