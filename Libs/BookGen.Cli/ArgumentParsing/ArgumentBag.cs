﻿//-----------------------------------------------------------------------------
// (c) 2023-2024 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections;

using BookGen.Cli.Annotations;

namespace BookGen.Cli.ArgumentParsing;

internal sealed class ArgumentBag : IEnumerable<string>
{
    private readonly string?[] _arguments;

    public ArgumentBag(IReadOnlyList<string> args)
    {
        _arguments = args.ToArray();
    }

    public IEnumerator<string> GetEnumerator()
    {
        foreach (var argument in _arguments)
        {
            if (argument != null)
                yield return argument;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public bool GetSwitch(SwitchAttribute @switch)
    {
        for (int i = 0; i < _arguments.Length; i++)
        {
            if (_arguments[i] == $"-{@switch.ShortName}"
                || _arguments[i] == $"--{@switch.LongName}")
            {
                _arguments[i] = null;
                return true;
            }
        }
        return false;
    }

    public string? GetSwitchValue(SwitchAttribute @switch)
    {
        for (int i = 0; i < _arguments.Length; i++)
        {
            if (_arguments[i] == $"-{@switch.ShortName}"
                || _arguments[i] == $"--{@switch.LongName}")
            {
                int nextIndex = i + 1;
                if (nextIndex < _arguments.Length)
                {
                    string? returnValue = _arguments[nextIndex];
                    _arguments[i] = null;
                    _arguments[nextIndex] = null;
                    return returnValue;
                }
            }
        }
        return null;
    }

    public string[] GetSwitchValues(SwitchAttribute @switch)
    {
        List<string> values = new();
        for (int i = 0; i < _arguments.Length; i++)
        {
            if (_arguments[i] == $"-{@switch.ShortName}"
                || _arguments[i] == $"--{@switch.LongName}")
            {
                int nextIndex = i + 1;
                if (nextIndex < _arguments.Length)
                {
                    string? returnValue = _arguments[nextIndex];
                    _arguments[i] = null;
                    _arguments[nextIndex] = null;
                    values.Add(returnValue ?? string.Empty);
                }
            }
        }
        return values.ToArray();
    }

    public string? GetArgument(ArgumentAttribute argument)
    {
        int notNullIndex = -1;
        for (int i = 0; i < _arguments.Length; i++)
        {
            if (_arguments[i] != null)
            {
                notNullIndex++;
            }
            if (notNullIndex == argument.Index)
            {
                string? returnValue = _arguments[i];
                _arguments[i] = null;
                return returnValue;
            }
        }
        return null;
    }

    public IEnumerable<string> GetNotProcessed()
    {
        foreach (var item in _arguments)
        {
            if (item != null)
            {
                yield return item;
            }
        }
    }
}
