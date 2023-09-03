namespace BookGen.Launcher.ViewModels;
public sealed class TodoItemModel : ObservableObject
{
    private bool _isChecked;
    private string _title;
    private DateOnly? _dueDate;

    public TodoItemModel()
    {
        _title = string.Empty;
    }

    public bool IsChecked 
    { 
        get => _isChecked; 
        set => SetProperty(ref _isChecked, value);
    }

    public DateOnly? DueDate
    { 
        get => _dueDate;
        set => SetProperty(ref _dueDate, value);
    }

    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }
}
