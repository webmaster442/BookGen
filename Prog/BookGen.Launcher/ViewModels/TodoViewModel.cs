//-----------------------------------------------------------------------------
// (c) 2023-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Threading.Tasks;

using BookGen.Settings;

namespace BookGen.Launcher.ViewModels;

internal sealed partial class TodoViewModel : ObservableObject
{
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
        _editorTitle = string.Empty;
        TodoItems = LoadList();
    }

    private BindingList<TodoItemModel> LoadList()
    {
        var manager = FileProvider.GetSettingsManager();
        try
        {
            TodoItemModel[]? items = manager.DeserializeAsync<TodoItemModel[]>(FileProvider.Keys.TodoItems).GetAwaiter().GetResult();
            if (items != null)
            {
                return new BindingList<TodoItemModel>(items);
            }
            return new BindingList<TodoItemModel>();
        }
        catch (Exception ex)
        {
            Dialog.ShowMessageBox(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return new BindingList<TodoItemModel>();
        }
    }

    private async Task SaveList()
    {
        var manager = FileProvider.GetSettingsManager();
        try
        {
            await manager.SerializeAsync(FileProvider.Keys.TodoItems, TodoItems.ToArray());
        }
        catch (Exception ex)
        {
            Dialog.ShowMessageBox(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
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
    private async Task Check(TodoItemModel? todoItemModel)
    {
        if (todoItemModel == null)
            return;

        todoItemModel.IsChecked = true;
        await SaveList();
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
    private async Task EditorOk()
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
        await SaveList();
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
