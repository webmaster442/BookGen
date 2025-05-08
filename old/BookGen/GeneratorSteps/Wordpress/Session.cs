//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain.Wordpress;

namespace BookGen.GeneratorSteps.Wordpress;

internal sealed class Session
{
    public Channel CurrentChannel { get; set; }

    public Session()
    {
        CurrentChannel = new Channel();
    }
}
