//-----------------------------------------------------------------------------
// (c) 2019-2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

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

    internal static MenuItem GroupHeader(string title)
        => new(string.Empty, title, () => Task.FromResult(0));
}
