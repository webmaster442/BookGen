//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Shellprog;

internal class CommandNameProvider
{
    private IEnumerable<string>? _commandNames;

    public IEnumerable<string> CommandNames
    { 
        get
        {
            return _commandNames == null
                ? throw new InvalidOperationException("Provider hasn't been setup correctly") 
                : _commandNames;
        }
        set
        {
            _commandNames = value;
        }
    }
}
