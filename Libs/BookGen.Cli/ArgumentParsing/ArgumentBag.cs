using BookGen.Cli.Annotations;
using System.Collections;

namespace BookGen.Cli.ArgumentParsing
{
    internal sealed class ArgumentBag : IEnumerable<string>
    {
        private readonly string?[] _arguments;

        public ArgumentBag(string[] args) 
        {
            _arguments = args;
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
            for (int i=0; i<_arguments.Length; i++)
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

        public string? GetArgument(ArgumentAttribute argument)
        {
            int notNullIndex = 0;
            for (int i = 0; i < _arguments.Length; i++)
            {
                if (_arguments[i] != null)
                {
                    notNullIndex++;
                }
                if (notNullIndex == argument.Index)
                {
                    return _arguments[i];
                }
            }
            return null;
        }
    }
}
