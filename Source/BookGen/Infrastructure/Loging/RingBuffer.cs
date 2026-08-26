namespace BookGen.Infrastructure.Loging;

internal sealed class RingBuffer<T>
{
    private readonly T[] _buffer;
    private readonly Lock _lock;
    private int _head;
    private int _tail;
    private int _count;

    public RingBuffer(int capacity)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(capacity, 0);
        _lock = new Lock();
        _buffer = new T[capacity];
        _head = 0;
        _tail = 0;
        _count = 0;
    }

    public void Add(T item)
    {
        lock (_lock)
        {
            _buffer[_tail] = item;
            _tail = (_tail + 1) % _buffer.Length;
            if (_count < _buffer.Length)
            {
                _count++;
            }
            else
            {
                _head = (_head + 1) % _buffer.Length; // Overwrite the oldest item
            }
        }
    }

    public T[] ToArray()
    {
        lock (_lock)
        {
            var array = new T[_count];
            for (int i = 0; i < _count; i++)
            {
                array[i] = _buffer[(_head + i) % _buffer.Length];
            }
            return array;
        }
    }
}
