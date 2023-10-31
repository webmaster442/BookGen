//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.DomainServices;
using BookGen.Launcher.Infrastructure;

namespace BookGen.Launcher.ViewModels;

internal sealed partial class TodoViewModel : ObservableObject
{
    private readonly string _fileName;
    private readonly string _tempName;

    private bool _isNewItem;
    private int _editedIndex;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(DeleteCommand), nameof(EditCommand))]
    private TodoItemModel? _selectedItem;

    [ObservableProperty]
    private bool _editorVisible;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(EditorOkCommand))]
    private string _editorTitle;

    [ObservableProperty]
    private DateOnly? _editorDate;

    public BindingList<TodoItemModel> TodoItems { get; }

    public TodoViewModel()
    {
        _tempName = FileProvider.GetLauncherTodoTempFile();
        _fileName = FileProvider.GetLauncherTodoFile();
        _editorTitle = string.Empty;
        TodoItems = LoadList();
    }

    private BindingList<TodoItemModel> LoadList()
    {
        if (FilleManagement.ReadJson(_fileName, out TodoItemModel[]? items))
        {
            return new BindingList<TodoItemModel>(items);
        }
        return new BindingList<TodoItemModel>();
    }

    private void SaveList()
    {
        FilleManagement.WriteJson(_tempName, _fileName, TodoItems);
    }

    private static bool CanDelete(TodoItemModel? todoItemModel)
    {
        return todoItemModel != null;
    }

    private static bool CanEdit(TodoItemModel? todoItemModel)
    {
        return todoItemModel != null 
            && todoItemModel.IsChecked == false;
    }

    [RelayCommand]
    private void Check(TodoItemModel? todoItemModel)
    {
        if (todoItemModel == null)
            return;

        todoItemModel.IsChecked = true;
        SaveList();
    }

    [RelayCommand]
    private void Add()
    {
        _isNewItem = true;
        EditorTitle = string.Empty;
        EditorDate = null;
        EditorVisible = true;
    }

    [RelayCommand(CanExecute = nameof(CanEdit))]
    private void Edit(TodoItemModel? todoItemModel)
    {
        if (todoItemModel == null)
            return;

        _isNewItem = false;
        _editedIndex = TodoItems.IndexOf(todoItemModel);

        EditorTitle = todoItemModel.Title;
        EditorDate = todoItemModel.DueDate;
        EditorVisible = true;
    }

    [RelayCommand(CanExecute = nameof(CanDelete))]
    private void Delete(TodoItemModel? todoItemModel)
    {
        if (todoItemModel == null)
            return;

        TodoItems.Remove(todoItemModel);
    }

    private bool CanEditorOk()
    {
        return !string.IsNullOrEmpty(EditorTitle);
    }

    [RelayCommand(CanExecute = nameof(CanEditorOk))]
    private void EditorOk()
    {
        if (_isNewItem)
        {
            var item = new TodoItemModel
            {
                DueDate = EditorDate,
                Title = EditorTitle,
            };
            TodoItems.Add(item);
        }
        else
        {
            TodoItems[_editedIndex].Title = EditorTitle;
            TodoItems[_editedIndex].DueDate = EditorDate;
        }
        SaveList();
        EditorVisible = false;
    }

    [RelayCommand]
    private void EditorCancel()
    {
        EditorDate = null;
        EditorTitle = string.Empty;
        EditorVisible = false;
    }
}
