//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace BookGen.Core
{
    public sealed class ArgumentItem : IEquatable<ArgumentItem>
    {
        public string Switch { get; set; }
        public string Value { get; set; }

        public bool HasSwitch
        {
            get { return !string.IsNullOrWhiteSpace(Switch); }
        }

        public bool HasValue
        {
            get { return !string.IsNullOrWhiteSpace(Value); }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ArgumentItem);
        }

        public bool Equals(ArgumentItem other)
        {
            return other != null
                   && Switch == other.Switch
                   && Value == other.Value;
        }

        public override int GetHashCode()
        {
            var hashCode = 1656223395;
            hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(Switch);
            hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(Value);
            return hashCode;
        }
    }
}
