namespace BookGen.Infrastructure.Terminal;

internal sealed class MenuItem
{
    private readonly string _icon;
    private readonly string _title;
    private readonly Action _action;

    public MenuItem(string icon, string title, Action action)
    {
        _icon = icon;
        _title = title;
        _action = action;
    }

    public override string ToString()
        => $"{_icon} {_title}";

    public void Execute()
        => _action.Invoke();
}
