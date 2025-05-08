//-----------------------------------------------------------------------------
// (c) 2025 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Cli.Mediator;

public interface IMediator
{
    void Register(IMediatable mediatable);
    void Unregister(IMediatable mediatable);
    void Notify<T>(T message);
    Task NotifyAsync<T>(T message);
}
