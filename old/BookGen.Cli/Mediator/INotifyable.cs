//-----------------------------------------------------------------------------
// (c) 2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Cli.Mediator;

public interface INotifyable<in T> : IMediatable
{
    void OnNotify(T message);
}
