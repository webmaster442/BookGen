namespace BookGen.Infrastructure.Terminal;

internal sealed class MenuItem
{
    private readonly string _icon;
    private readonly string _title;
    private readonly Func<Task<int>> _action;

    public MenuItem(string icon, string title, Func<Task<int>> action)
    {
        _icon = icon;
        _title = title;
        _action = action;
    }

    public override string ToString()
        => $"{_icon} {_title}";

    public async Task<int> ExecuteAsync()
        => await _action.Invoke();
}
