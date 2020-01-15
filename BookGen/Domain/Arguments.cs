//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BookGen.Domain
{
    internal class Arguments : IArguments
    {
        private readonly Dictionary<string, string> _storage;

        public Arguments()
        {
            _storage = new Dictionary<string, string>();
        }

        public Arguments(Dictionary<string, string> results)
        {
            _storage = results;
        }

        public string this[int index]
        {
            get
            {
                int skip = index - 1 < 0 ? 0 : index - 1;
                var key = _storage.Keys.Skip(skip).Take(1).First();
                return _storage[key];
            }
        }

        public IEnumerable<string> ArgumentNames
        {
            get { return _storage.Keys; }
        }

        public int Count
        {
            get { return _storage.Count; }
        }

        public bool HasArgument(string name)
        {
            return _storage.ContainsKey(name.ToLower());
        }

        public T GetArgumentOrFallback<T>(string argument, T fallback) where T : IConvertible
        {
            var key = _storage.Keys.FirstOrDefault(k => string.Compare(k, argument, true) == 0);
            
            if (key == null)
                return fallback;

            return (T)Convert.ChangeType(_storage[key], typeof(T));
        }

        public T GetArgumentOrThrow<T>(string argument) where T : IConvertible
        {
            var key = _storage.Keys.FirstOrDefault(k => string.Compare(k, argument, true) == 0);
            
            if (key == null)
                throw new ArgumentException($"{argument} was not found");

            return (T)Convert.ChangeType(_storage[key], typeof(T));
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return _storage.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _storage.GetEnumerator();
        }
    }
}
