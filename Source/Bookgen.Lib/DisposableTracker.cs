namespace BookGen.Lib;

internal sealed class DisposableTracker : IDisposable
{
    private readonly List<IDisposable> _items = new();
    private readonly Lock _lock = new();

    public void Dispose()
    {
        lock (_lock)
        {
            foreach (IDisposable item in _items)
            {
                item.Dispose();
            }
            _items.Clear();
        }
    }

    public void Track<T>(T item) where T : IDisposable
    {
        lock (_lock)
        {
            _items.Add(item);
        }
    }
}
