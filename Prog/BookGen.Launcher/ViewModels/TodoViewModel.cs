using BookGen.Launcher.Infrastructure;

using Microsoft.VisualBasic;

namespace BookGen.Launcher.ViewModels;
internal sealed partial class TodoViewModel : ObservableObject
{
    private readonly string _fileName;
    private readonly string _tempName;

    private bool _isNewItem;

    [ObservableProperty]
    private bool _sortByDate;

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
        _tempName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "todotemp.json");
        _fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "bookgenTodo.json");
        _sortByDate = true;
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

    private bool CanEditDelete(TodoItemModel? todoItemModel)
    {
        return todoItemModel != null;
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

    [RelayCommand(CanExecute = nameof(CanEditDelete))]
    private void Edit(TodoItemModel? todoItemModel)
    {
        if (todoItemModel == null)
            return;

        
    }

    [RelayCommand(CanExecute = nameof(CanEditDelete))]
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
