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
        public ArgumentItem(string switchValue)
        {
            Switch = switchValue;
            Value = string.Empty;
        }

        public ArgumentItem(string switchValue, string value)
        {
            Switch = switchValue;
            Value = value;
        }


        public string Switch { get; }
        public string Value { get; }

        public bool IsStandaloneSwitch
        {
            get
            {
                return !string.IsNullOrWhiteSpace(Switch)
                    && string.IsNullOrWhiteSpace(Value);
            }
        }

        public bool IsArgumentedSwitch
        {
            get
            {
                return
                    !string.IsNullOrWhiteSpace(Switch)
                    && !string.IsNullOrWhiteSpace(Value);
            }
        }

        public bool IsValue
        {
            get
            {
                return string.IsNullOrWhiteSpace(Switch)
                    && !string.IsNullOrWhiteSpace(Value);
            }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as ArgumentItem);
        }

        public bool Equals(ArgumentItem? other)
        {
            return 
                  Switch == other?.Switch
                  && Value == other?.Value;
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
