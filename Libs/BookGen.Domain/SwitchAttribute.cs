//-----------------------------------------------------------------------------
// (c) 2020-2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Domain
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class SwitchAttribute : Attribute, IEquatable<SwitchAttribute?>
    {
        public string LongName { get; }
        public string ShortName { get; }

        public bool Required { get; }

        public SwitchAttribute(string shortName, string longName, bool required = false)
        {
            LongName = longName;
            ShortName = shortName;
            Required = required;
        }

        public bool NamingGood => !ShortName.StartsWith("-") && !LongName.StartsWith("--");

        public override bool Equals(object? obj)
        {
            return Equals(obj as SwitchAttribute);
        }

        public bool Equals(SwitchAttribute? other)
        {
            return other != null &&
                   base.Equals(other) &&
                   LongName == other.LongName &&
                   ShortName == other.ShortName &&
                   Required == other.Required;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), LongName, ShortName, Required);
        }

        public static bool operator ==(SwitchAttribute? left, SwitchAttribute? right)
        {
            return EqualityComparer<SwitchAttribute>.Default.Equals(left, right);
        }

        public static bool operator !=(SwitchAttribute? left, SwitchAttribute? right)
        {
            return !(left == right);
        }
    }
}
