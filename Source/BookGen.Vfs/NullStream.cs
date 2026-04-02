//-----------------------------------------------------------------------------
// (c) 2019-2026 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Diagnostics;

namespace BookGen.Vfs;

public sealed class NullStream : Stream
{
    private long _length = long.MaxValue;

    public override bool CanRead => true;

    public override bool CanSeek => true;

    public override bool CanWrite => true;

    public override long Length => _length;

    public override long Position { get; set; }

    public override void Flush()
    {
        // do nothing
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        Array.Fill(buffer, (byte)0, offset, count);
        return count;
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        switch (origin)
        {
            case SeekOrigin.Begin:
                Position = offset;
                break;
            case SeekOrigin.Current:
                Position += offset;
                break;
            case SeekOrigin.End:
                Position -= offset;
                break;
            default:
                throw new UnreachableException($"Unknown {nameof(SeekOrigin)} value");
        }

        return Position;
    }

    public override void SetLength(long value)
    {
        _length = value;
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        // do nothing
    }
}
