//-----------------------------------------------------------------------------
// (c) 2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.DomainServices.Markdown.Scripting;
internal sealed class NotSupportedReader : TextReader
{
    private const string Message = "Console Inputs is not supported in scripting";

    public override int Read() 
        => throw new NotSupportedException(Message);

    public override int Read(char[] buffer, int index, int count)
        => throw new NotSupportedException(Message);

    public override int Read(Span<char> buffer)
        => throw new NotSupportedException(Message);

    public override Task<int> ReadAsync(char[] buffer, int index, int count)
        => throw new NotSupportedException(Message);

    public override ValueTask<int> ReadAsync(Memory<char> buffer, CancellationToken cancellationToken = default)
        => throw new NotSupportedException(Message);

    public override int ReadBlock(char[] buffer, int index, int count)
        => throw new NotSupportedException(Message);

    public override int ReadBlock(Span<char> buffer)
        => throw new NotSupportedException(Message);

    public override Task<int> ReadBlockAsync(char[] buffer, int index, int count)
        => throw new NotSupportedException(Message);

    public override ValueTask<int> ReadBlockAsync(Memory<char> buffer, CancellationToken cancellationToken = default)
        => throw new NotSupportedException(Message);

    public override string? ReadLine()
        => throw new NotSupportedException(Message);

    public override Task<string?> ReadLineAsync()
        => throw new NotSupportedException(Message);

    public override ValueTask<string?> ReadLineAsync(CancellationToken cancellationToken)
        => throw new NotSupportedException(Message);

    public override string ReadToEnd()
        => throw new NotSupportedException(Message);

    public override Task<string> ReadToEndAsync()
        => throw new NotSupportedException(Message);

    public override Task<string> ReadToEndAsync(CancellationToken cancellationToken)
        => throw new NotSupportedException(Message);
}
