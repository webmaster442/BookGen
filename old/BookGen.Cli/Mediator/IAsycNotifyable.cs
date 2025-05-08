//-----------------------------------------------------------------------------
// (c) 2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Cli.Mediator;

public interface IAsycNotifyable<in T> : IMediatable
{
    Task OnNotifyAsync(T message);
}
