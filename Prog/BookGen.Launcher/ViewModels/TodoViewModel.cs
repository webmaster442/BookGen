using BookGen.Launcher.Infrastructure;

namespace BookGen.Launcher.ViewModels;
internal sealed partial class TodoViewModel : ObservableObject
{
    private readonly string _fileName;
    private readonly string _tempName;

    [ObservableProperty]
    private bool _sortByDate;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(DeleteCommand), nameof(EditCommand))]
    private TodoItemModel? _selectedItem;

    public BindingList<TodoItemModel> TodoItems { get; }

    public TodoViewModel()
    {
        _tempName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "todotemp.json");
        _fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "bookgenTodo.json");
        _sortByDate = true;
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
}
